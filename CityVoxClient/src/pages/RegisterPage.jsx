import { useRouter } from 'next/navigation';
import { Formik } from "formik";
import axios from "axios";
import * as Yup from 'yup';
import { Box, Button, Link, Stack, TextField, Typography } from '@mui/material';
import { useAuth } from 'src/hooks/use-auth';
import { Layout as AuthLayout } from 'src/layouts/auth/layout';

const registerSchema = yup.object().shape({
    username: yup.string().required("required"),
    firstName: yup.string().required("required"),
    lastName: yup.string().required("required"),
    password: yup.string().required("required"),
    email: yup.string().email("invalid email").required("required"),
    profilePicture: yup.string().optional(),
});

const initialValuesRegister = {
    username: "",
    firstName: "",
    lastName: "",
    password: "",
    email: "",
    profilePicture: "",
  };



const RegisterPage = () =>{

    return(
        <Formik
          onS
        >

        </Formik>
    )
}

export default RegisterPage