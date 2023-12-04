import { useCallback, useState } from 'react';
import {
    Box,
    Button,
    Card,
    CardActions,
    CardContent,
    CardHeader,
    Divider,
    TextField,
    Unstable_Grid2 as Grid,
    Typography,
    Snackbar,
    SvgIcon
} from '@mui/material';
import { Formik, Field, Form, ErrorMessage } from 'formik';
import * as yup from 'yup';
import Dropzone from 'react-dropzone';
import { useSelector } from 'react-redux/es/hooks/useSelector';
import { UpdateCurrentUser } from '../../utils/api';
import { useNavigate } from 'react-router-dom';
import FlexBetween from '../styling/flex-between';
import PencilIcon from '@heroicons/react/24/outline/PencilIcon';
import { uploadToCloudinary } from '../../utils/api';

const updateValidationSchema = yup.object().shape({
    username: yup.string().required("required"),
    fName: yup.string().required("required"),
    lName: yup.string().required("required"),
    //password: yup.string().required("required"),
    email: yup.string().email("invalid email").required("required"),
    pfp: yup.string().optional(),
});

const AccountProfileDetails = () => {

    const appUser = useSelector((state) => state.user)
    const navigate = useNavigate();
    const [openSnackbar, setOpenSnackbar] = useState(false);

    const updateUserProfile = async (values) => {
        if (values.pfp && values.pfp !== appUser.pfp) {
            const imgUrl = await uploadToCloudinary(values.pfp);
            console.log("profile picture URL: " + imgUrl);
            values.pfp = imgUrl;
        }
        const updateUserDto = {
            Id: appUser.id,
            Username: values.username,
            FirstName: values.fName,
            LastName: values.lName,
            Email: values.email,
            ProfilePictureUrl: values.pfp
        }
    }; 

    return (
        <Card>
            <CardContent>
                <Formik
                    initialValues={appUser} // Assuming the initial values match the current user's profile
                    validationSchema={updateValidationSchema}
                    onSubmit={updateUserProfile}
                >
                    {({ values,
                        errors,
                        touched,
                        isSubmitting,
                        setFieldValue }) => (
                        <Form>
                            {/* ... your other components ... */}

                            <Field name="username">
                                {({ field }) => (
                                    <TextField
                                        fullWidth
                                        label="Username"
                                        {...field}
                                        helperText={<ErrorMessage name="username" />}
                                        error={Boolean(errors.username && touched.username)}
                                    />
                                )}
                            </Field>

                            <Field name="fName">
                                {({ field }) => (
                                    <TextField
                                        fullWidth
                                        label="First Name"
                                        {...field}
                                        helperText={<ErrorMessage name="fName" />}
                                        error={Boolean(errors.fName && touched.fName)}
                                    />
                                )}
                            </Field>

                            <Field name="lName">
                                {({ field }) => (
                                    <TextField
                                        fullWidth
                                        label="Last Name"
                                        {...field}
                                        helperText={<ErrorMessage name="lName" />}
                                        error={Boolean(errors.lName && touched.lName)}
                                    />
                                )}
                            </Field>

                            <Field name="email">
                                {({ field }) => (
                                    <TextField
                                        fullWidth
                                        label="Email"
                                        {...field}
                                        helperText={<ErrorMessage name="email" />}
                                        error={Boolean(errors.email && touched.email)}
                                    />
                                )}
                            </Field>

                            <Dropzone
                                onDrop={(acceptedFiles) =>
                                    setFieldValue("pfp", acceptedFiles[0])
                                }
                            /* ... other dropzone props ... */
                            >
                                {({ getRootProps, getInputProps }) => (
                                    <div {...getRootProps()}>
                                        <input {...getInputProps()} />
                                        {!values.pfp ? (
                                            <Button >
                                                <Typography variant='text' color="primary">Upload Profile Picture</Typography>
                                            </Button>
                                        ) : (
                                            <FlexBetween>
                                                <Typography ml="5px" variant='subtitle2' color="primary">{values.pfp && values.pfp.name ? values.pfp.name : "change current photo"}</Typography>
                                                <SvgIcon>
                                                    {" "}
                                                    <PencilIcon />
                                                </SvgIcon>
                                            </FlexBetween>
                                        )}
                                    </div>
                                )}
                            </Dropzone>

                            <Box display="flex" justifyContent="flex-end">
                                <Button type="submit" disabled={isSubmitting} sx={{ mt: "1rem" }}>
                                    Update Profile
                                </Button>
                            </Box>

                        </Form>
                    )}
                </Formik>
                <Snackbar
                    open={openSnackbar}
                    autoHideDuration={6000}
                    onClose={() => setOpenSnackbar(false)}
                    message="Profile updated successfully!"
                    action={
                        <Button color="secondary" size="small" onClick={() => setOpenSnackbar(false)}>
                            Close
                        </Button>
                    }
                />
            </CardContent>
        </Card>
    )
}

export default AccountProfileDetails;