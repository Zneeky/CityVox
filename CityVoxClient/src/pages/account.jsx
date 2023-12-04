import { Box, Container, Stack, Typography, Unstable_Grid2 as Grid } from '@mui/material';
import AccountProfile from '../components/widget/account-profile';
import AccountProfileDetails from '../components/widget/account-profile-details';
import { LayoutRoot, LayoutContainer, SIDE_NAV_WIDTH } from "./home-page";
import { useCallback, useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { TopNav } from "../components/navigation/top-nav";
import { SideNav } from "../components/navigation/side-nav";

const Account = () => {
    const location = useLocation();
    const pathname = location.pathname;
    const [openNav, setOpenNav] = useState(false);

    const handlePathnameChange = useCallback(() => {
        if (openNav) {
            setOpenNav(false);
        }
    }, [openNav]);

    useEffect(
        () => {
            handlePathnameChange();
        },
        // eslint-disable-next-line react-hooks/exhaustive-deps
        [pathname]
    );

    return (
        <>
            <TopNav onNavOpen={() => setOpenNav(true)} />
            <SideNav onClose={() => setOpenNav(false)} open={openNav} />
            <LayoutRoot>
                <LayoutContainer>
                    <Box
                        component="main"
                        sx={{
                            flexGrow: 1,
                            py: 8
                        }}
                    >
                        <Container maxWidth="lg">
                            <Stack spacing={3}>
                                <div>
                                    <Typography variant="h4">
                                        Account
                                    </Typography>
                                </div>
                                <div>
                                    <Grid
                                        container
                                        spacing={3}
                                    >
                                        <Grid
                                            xs={12}
                                            md={6}
                                            lg={4}
                                        >
                                            <AccountProfile />
                                        </Grid>
                                        <Grid
                                            xs={12}
                                            md={6}
                                            lg={8}
                                        >
                                            <AccountProfileDetails />
                                        </Grid>
                                    </Grid>
                                </div>
                            </Stack>
                        </Container>
                    </Box>
                </LayoutContainer>
            </LayoutRoot>
        </>
    )
}

export default Account; 