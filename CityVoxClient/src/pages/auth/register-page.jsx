import { Box, Typography, useTheme, useMediaQuery, Card } from "@mui/material";
import React, { useState, useEffect, useRef } from "react";
import RegisterForm from "./register-form"

const Register = () => {
  const theme = useTheme();
  const isNonMobileScreens = useMediaQuery("(min-width:1000px)");

  return (
    <Box>
      <Box
        width="100%"
        backgroundColor={theme.palette.background.alt}
        p="0.5rem 6%"
        textAlign="center"
      >
        <Card sx={{width:"100px" , m:"0 auto"}}>
          <Box component="img" src="/images/megaphone.png" alt="CityVox Logo" sx={{ p: "12px 20px 12px 20px" }} />
        </Card>
      </Box>

      <Box
        width={isNonMobileScreens ? "50%" : "93%"}
        p="2rem"
        m="2rem auto"
        borderRadius="1.5rem"
        backgroundColor={theme.palette.background.primary}
      >
        <Typography variant="h2">Welcome to CityVox!</Typography>
        <Typography fontWeight="500" variant="h5" sx={{ mb: "1.5rem" }}>
          Are you ready to make a difference in your community?
        </Typography>
      </Box>
      <Box
        width={isNonMobileScreens ? "50%" : "93%"}
        p="2rem"
        m="2rem auto"
        borderRadius="1.5rem"
        backgroundColor={theme.palette.background.alt}
      >
        <RegisterForm />
      </Box>
    </Box>
  )
}

export default Register;