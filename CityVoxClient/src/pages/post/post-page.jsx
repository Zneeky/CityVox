import { Box, useMediaQuery, Grid, AppBar, Tabs, Tab, Paper, Button, IconButton, SvgIcon, Typography } from "@mui/material";
import { LayoutRoot, LayoutContainer, SIDE_NAV_WIDTH } from "../home-page";
import { TopNav } from "../../components/navigation/top-nav";
import { useCallback, useEffect, useState } from "react";
import { useSelector } from "react-redux/es/hooks/useSelector";
import { SideNav } from "../../components/navigation/side-nav";
import { useLocation } from "react-router-dom";
import RegionDropdown from "../../components/dropdown/region-dropdown";
import MunicipalityDropdown from "../../components/dropdown/municipality-dropdown";
import PostsSection from "../../components/styling/posts-section";
import CreatePost from '../../components/widget/create-post';
import PencilSquareIcon from "@heroicons/react/24/outline/PencilSquareIcon";

const PostPage = () => {
    const appUser = useSelector((state) => state.user);

    const [selectedRegion, setSelectedRegion] = useState(null);
    const [selectedMunicipality, setSelectedMunicipality] = useState(null);
    const [tabIndex, setTabIndex] = useState(0);
    const [openCreatePost, setOpenCreatePost] = useState(false);

    const location = useLocation();
    const pathname = location.pathname;
    const [openNav, setOpenNav] = useState(false);

    const handleOpenCreatePost = () => {
        setOpenCreatePost(true);
    };

    const handleCloseCreatePost = () => {
        setOpenCreatePost(false);
    };

    const handleRegionChange = (selectedRegionId) => {
        setSelectedRegion(selectedRegionId);
    }

    const handleChangeTab = (event, newValue) => {
        setTabIndex(newValue);
    };

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
                    <Grid container spacing={2} style={{ justifyContent: "center" }} pl="1.5rem">
                        <Grid item xs={12} sm={2} >
                            <RegionDropdown onChange={handleRegionChange} />
                        </Grid>
                        <Grid item xs={12} sm={2} >
                            <MunicipalityDropdown
                                regionId={selectedRegion}
                                onChange={setSelectedMunicipality}
                            />
                        </Grid>
                    </Grid>
                    <Box display="flex" justifyContent="center">
                        <IconButton sx={{
                            width: "2rem", '&:hover': {

                            }
                        }} variant="contained" color="primary" onClick={handleOpenCreatePost}>
                            <SvgIcon>
                                <PencilSquareIcon />
                            </SvgIcon>
                        </IconButton>
                    </Box>
                    <CreatePost open={openCreatePost} handleClose={handleCloseCreatePost} />
                    <Box sx={{ width: "80%", m: "1rem auto" }}>
                        <AppBar position="static" sx={{ backgroundColor: "inherit", alignItems: "center" }}>
                            <Tabs value={tabIndex} onChange={handleChangeTab}>
                                <Tab label="For You" />
                                <Tab label="Official" />
                            </Tabs>
                        </AppBar>
                        <Box
                            sx={{
                                width: {
                                    xs: '100%',
                                    sm: '90%',
                                    md: '50%',
                                },
                                m: {
                                    xs: "0 auto",
                                    sm: "1rem auto",
                                }
                            }}
                        >
                            <PostsSection type={tabIndex} municipalityId={selectedMunicipality?.municipalityId} />
                        </Box>
                    </Box>
                </LayoutContainer>
            </LayoutRoot>

        </>
    )
}

export default PostPage;