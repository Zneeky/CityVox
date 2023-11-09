import { Box, Typography, useTheme, useMediaQuery } from "@mui/material";
import React, { useState, useEffect, useRef } from "react";
import LoginForm from "./LoginForm";

const Login = () => {
  const theme = useTheme();
  const isNonMobileScreens = useMediaQuery("(min-width:1000px)");

  return (
    <Box>
      <Box
        width={isNonMobileScreens ? "50%" : "93%"}
        p="2rem"
        m="2rem auto"
        borderRadius="1.5rem"
        backgroundColor={theme.palette.background.primary}
      >
        <Typography variant="h2">Welcome to CityVox!</Typography>
      </Box>
      <Box
        width={isNonMobileScreens ? "50%" : "93%"}
        p="2rem"
        m="2rem auto"
        borderRadius="1.5rem"
        backgroundColor={theme.palette.background.alt}
      >
        <LoginForm />
      </Box>
    </Box>
  );
};
export default Login;
