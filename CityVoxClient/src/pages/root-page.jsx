import React from 'react';
import { Button, Grid, Typography, Paper, Box, Card, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
//"/images/megaphone.png"

const Root = () => {
    const navigate = useNavigate();
    const { palette } = useTheme();
    const handleSignUp = () => {
        navigate('/auth/register');
    }

    return (
        <>
            <Grid container sx={{ height: '100%' }}>

                {/* Left side */}
                <Grid item xs={12} md={6} container direction="column" justifyContent="center" alignItems="center">
                    {/* Uncomment and replace the below line with your logo image path */}
                    <Box display="flex" sx={{ position: { md: "absolute" }, top: "40px", m: "2rem" }}>
                        <Card>
                            <Box component="img" src="/images/megaphone.png" alt="CityVox Logo" sx={{ p: "12px 20px 12px 20px" }} />
                        </Card>
                        <Typography variant='h5' sx={{ p: "12px", mt: "20px" }}>CityVox</Typography>
                    </Box>

                    <Card sx={{ m: "2rem" }}>
                        <Box p="30px">
                            <Typography variant='h6' color="primary.main">NEW PLATFORM</Typography>
                            <Box maxWidth="400px">
                                <Typography variant="h2" sx={{ mb: 1, fontWeight: 700 }}>
                                    Empower Your Local Community.
                                </Typography>
                            </Box>
                        </Box>
                    </Card>
                    <Box maxWidth="400px">
                        <Typography variant="subtitle1" sx={{ mb: 5, color: 'text.secondary' }}>
                            With CityVox, voice your concerns, stay informed, and shape the future of your neighborhood.
                        </Typography>
                    </Box>
                    <Button variant="contained" color="primary" size="large" onClick={handleSignUp}>
                        Sign Up Now
                    </Button>
                </Grid>

                {/* Right side */}
                <Grid item xs={12} md={6} sx={{
                    pl: {
                        md: 5
                    }

                }}
                    container direction="column" justifyContent="center" alignItems="center"
                >
                    <Button variant="text" color="primary" size="large" onClick={handleSignUp} sx={{ position: { md: "absolute" }, top: { md: "60px" }, }}>
                        Get Started
                    </Button>
                    <Box maxWidth="600px">
                        <Card>
                            <Box component="img" src="/images/promo_girl_apps.png" />
                        </Card>
                    </Box>
                </Grid>

            </Grid>
        </>
    );
}

export default Root;