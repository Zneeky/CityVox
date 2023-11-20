import { Box, useMediaQuery, Grid, AppBar, Tabs, Tab, Paper, Button, IconButton, SvgIcon, Typography } from "@mui/material";
import { LayoutRoot, LayoutContainer, SIDE_NAV_WIDTH } from "../Home"
import { TopNav } from "../../components/navigation/TopNav";
import { useCallback, useEffect, useState } from "react";
import { SideNav } from "../../components/navigation/SideNav";
import RegionDropdown from "../../components/dropdown/RegionDropdown";
import MunicipalityDropdown from "../../components/dropdown/MunicipalityDropdown";
import PencilSquareIcon from "@heroicons/react/24/outline/PencilSquareIcon";
import CreatePost from '../../components/widget/CreatePost';
const PostPage = () => {

    const [openNav, setOpenNav] = useState(false);
    const [selectedRegion, setSelectedRegion] = useState(null);
    const [setSelectedMunicipality] = useState(null);
    const [openCreatePost, setOpenCreatePost] = useState(false);


    const handleRegionChange = (selectedRegionId) => {
        setSelectedRegion(selectedRegionId);
    };

    const handleOpenCreatePost = () => {
        setOpenCreatePost(true);
    };
    
    const handleCloseCreatePost = () => {
        setOpenCreatePost(false);
    };

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
            </LayoutContainer>
         </LayoutRoot>
        </>
    )
    }
    export default PostPage;