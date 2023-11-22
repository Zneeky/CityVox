import { useState } from "react";
import { Formik } from "formik";
import { uploadToCloudinary } from "../../utils/api";
import * as yup from "yup"; //client validation library
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
  Link as LinkMui,
  SvgIcon,
} from "@mui/material";
import FlexBetween from "../../components/styling/flex-between";
import Dropzone from "react-dropzone";
import useAuth from "../../hooks/use-auth";
import {
  Link as LinkRouter,
  useNavigate,
  useLocation,
  redirect,
} from "react-router-dom";
import { RegisterUser } from "../../utils/api";
import PencilIcon from "@heroicons/react/24/outline/PencilIcon";

const registerSchema = yup.object().shape({
  username: yup.string().required("required"),
  firstName: yup.string().required("required"),
  lastName: yup.string().required("required"),
  password: yup.string().required("required"),
  email: yup.string().email("invalid email").required("required"),
  profilePictureUrl: yup.string().optional(),
});

const initialValuesRegister = {
  username: "",
  firstName: "",
  lastName: "",
  password: "",
  email: "",
  profilePictureUrl: "",
};

const RegisterForm = () => {

  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || "/auth/login";


  const [isLoading, setIsLoading] = useState(false);
  const [failText, setFailText] = useState("");
  const { palette } = useTheme();
  const isNonMobile = useMediaQuery("(min-width:600px)");

  const register = async (values, onSubmitProps) => {
    return new Promise(async (resolve, reject) => {
      try {
        if (values.profilePictureUrl) {
          const imgUrl = await uploadToCloudinary(values.profilePictureUrl);
          console.log("profile picture URL: " + imgUrl);
          values.profilePictureUrl = imgUrl;
        }
        const response = await RegisterUser(values, onSubmitProps);
        if (response.status === 200) {
          console.log("Success:", response);
          resolve(response);
        } else {
          reject(new Error("Registration failed with status: " + response.status));
        }
      } catch (error) {
        alert("Registration failed");
        reject(error);
      } finally {
        setIsLoading(false);
      }
    });
  };

  const handleFormSubmit = async (values, onSubmitProps) => {
    setIsLoading(true);
    try {
      await register(values, onSubmitProps);
      alert("Registration successful! Please check your email to confirm your account.");
      navigate("/auth/login");
    } catch (error) {
      console.error("Registration failed:", error);
      alert("Registration failed. Please try again.");
    }
  };

  return (
    <Formik
      onSubmit={handleFormSubmit}
      initialValues={initialValuesRegister}
      validationSchema={registerSchema}
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
            Register
          </Typography>
          <Typography color="text.secondary" variant="h6" sx={{ mb: "1rem" }}>
            Already have an account? &nbsp;
            <LinkMui href="/auth/login" underline="hover" variant="h6">
              Log in
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
              label="Username"
              onBlur={handleBlur}
              onChange={handleChange}
              value={values.username}
              name="username"
              error={Boolean(touched.username) && Boolean(errors.username)}
              helperText={touched.username && errors.username}
              sx={{ gridColumn: "span 4" }}
            />
            <TextField
              label="First Name"
              onBlur={handleBlur}
              onChange={handleChange}
              value={values.firstName}
              name="firstName"
              error={Boolean(touched.firstName) && Boolean(errors.firstName)}
              helperText={touched.firstName && errors.firstName}
              sx={{ gridColumn: "span 2" }}
            />
            <TextField
              label="Last Name"
              onBlur={handleBlur}
              onChange={handleChange}
              value={values.lastName}
              name="lastName"
              error={Boolean(touched.lastName) && Boolean(errors.lastName)}
              helperText={touched.lastName && errors.lastName}
              sx={{ gridColumn: "span 2" }}
            />
            {/*<TextField
              label="Birth Date"
              type="date"
              defaultValue="2000-01-01"
              onBlur={handleBlur}
              onChange={handleChange}
              name="birthDate"
              error={Boolean(touched.birthDate) && Boolean(errors.birthDate)}
              sx={{ gridColumn: "span 4" }}
            />*/}
            <Box
              gridColumn="span 4"
              border={`1px solid ${palette.static.medium}`}
              borderRadius="5px"
              p="1rem"
            >
              <Dropzone
                acceptedFiles=".jpg,.jpeg,.png"
                multiple={false}
                onDrop={(acceptedFiles) =>
                  setFieldValue("profilePictureUrl", acceptedFiles[0])
                }
              >
                {({ getRootProps, getInputProps }) => (
                  <Box
                    {...getRootProps()}
                    border={`2px dashed ${palette.primary.main}`}
                    p="1rem"
                    sx={{ "&:hover": { cursor: "pointer" } }}
                  >
                    <input {...getInputProps()} />
                    {!values.profilePictureUrl ? (
                      <p>Add profile pic here</p>
                    ) : (
                      <FlexBetween>
                        <Typography>{values.profilePictureUrl.name}</Typography>
                        <SvgIcon>
                          {" "}
                          <PencilIcon />
                        </SvgIcon>
                      </FlexBetween>
                    )}
                  </Box>
                )}
              </Dropzone>
            </Box>
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
              REGISTER
            </Button>
          </Box>
        </form>
      )}
    </Formik>
  );
};

export default RegisterForm;
