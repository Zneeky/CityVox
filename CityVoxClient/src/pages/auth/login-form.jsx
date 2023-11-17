import { useState } from "react";
import { Formik } from "formik";
import * as yup from "yup"; //client validation library
import { useDispatch } from "react-redux";
import {
  Box,
  Button,
  Stack,
  TextField,
  Typography,
  Backdrop,
  CircularProgress,
  useTheme,
  useMediaQuery,
  Link as LinkMui
} from "@mui/material";
import {  Link as LinkRouter,useNavigate, useLocation, redirect } from "react-router-dom";
import { LoginUser } from "../../utils/api";
import { setLogin } from "../../redux";

const LoginSchema = yup.object().shape({
  password: yup.string().required("required"),
  email: yup.string().email("invalid email").required("required"),
});

const initialValuesLogin = {
  email: "",
  password: "",
};

let appUser = {
  accessToken: null,
  username: null,
  email : null,
  fName : null,
  lName : null,
  pfp: null,
  role: null,
  id: null,
}

const LoginForm = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || "/home";

  const [isLoading, setIsLoading] = useState(false);
  const [failText, setFailText] = useState("");
  const { palette } = useTheme();
  const isNonMobile = useMediaQuery("(min-width:600px)");

  const Login = async (values, onSubmitProps) => {
    return new Promise(async (resolve, reject) => {
      try {
        const response = await LoginUser(values, onSubmitProps);
        if (response.status === 200) {
          console.log('Success');
           let currentUser = { ...appUser };

           currentUser.accessToken = response?.data?.AccessToken;
           currentUser.username = response?.data?.Username;
           currentUser.email = response?.data?.Email;
           currentUser.fName = response?.data?.FirstName;
           currentUser.lName = response?.data?.LastName;
           currentUser.pfp = response?.data?.ProfilePicture;
           currentUser.role = response?.data?.Role;
           currentUser.id = response?.data?.Id;

          dispatch(
            setLogin({
              user: currentUser,
            })
          );
          resolve(response);
        } else {
          reject(new Error("Login failed with status: " + response.status));
        }
      } catch (error) {
        alert("Login failed");
        reject(error);
      } finally {
        setIsLoading(false);
      }
    });
  };

  const handleFormSubmit = async (values, onSubmitProps) => {
    setIsLoading(true);
    try {
      await Login(values, onSubmitProps);
      navigate(from, {replace : true});
    } catch (error) {
      console.error("Login failed:", error);
    }
  };

  return (
    <Formik
      onSubmit={handleFormSubmit}
      initialValues={initialValuesLogin}
      validationSchema={LoginSchema}
    >
      {({
        values,
        errors,
        touched,
        handleBlur,
        handleChange,
        handleSubmit,
        setFieldValue,
        resetForm,
      }) => (
        <form onSubmit={handleSubmit}>
          <Backdrop
            open={isLoading}
            sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
          >
            <CircularProgress color="inherit" />
          </Backdrop>
          <Typography fontWeight="500" variant="h3" sx={{ mb: "0.3rem" }}>
            Login
          </Typography>
          <Typography
                color="text.secondary"
                variant="h6"
                sx={{ mb: "1rem" }}
              >
                Don't have an account?
                &nbsp;
                <LinkMui
                  href="/auth/register"
                  underline="hover"
                  variant="h6"
                >
                  Register
                </LinkMui>
          </Typography>
          <Typography color="red">{failText}</Typography>
          <Box
            display="grid"
            gap="30px"
            gridTemplateColumns="repeat(4, minmax(0,1fr))"
            sx={{
              "& > div": { gridColumn: isNonMobile ? undefined : "span 4" },
            }}
          >
            <TextField
              label="Email"
              onBlur={handleBlur}
              onChange={handleChange}
              value={values.email}
              name="email"
              error={Boolean(touched.email) && Boolean(errors.email)}
              helperText={touched.email && errors.email}
              sx={{ gridColumn: "span 2" }}
            />
            <TextField
              label="Password"
              type="password"
              onBlur={handleBlur}
              onChange={handleChange}
              value={values.password}
              name="password"
              error={Boolean(touched.password) && Boolean(errors.password)}
              helperText={touched.password && errors.password}
              sx={{ gridColumn: "span 2" }}
            />
          </Box>
          <Box>
            <Button
              fullWidth
              type="submit"
              sx={{
                m: "2rem 0",
                p: "1rem",
                backgroundColor: palette.primary.main,
                color: palette.background.default,
                "&:hover": { color: palette.primary.main },
              }}
            >
              Login
            </Button>
          </Box>
        </form>
      )}
    </Formik>
  );
};

export default LoginForm;