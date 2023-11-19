import { Box, useMediaQuery, Grid, AppBar, Tabs, Tab, Paper, Button, IconButton, SvgIcon, Typography } from "@mui/material";
import { LayoutRoot, LayoutContainer, SIDE_NAV_WIDTH } from "../Home"
import { TopNav } from "../../components/navigation/TopNav";
import { useCallback, useEffect, useState } from "react";
import { SideNav } from "../../components/navigation/SideNav";
import RegionDropdown from "../../components/dropdown/RegionDropdown";
const PostPage = () => {

    const [openNav, setOpenNav] = useState(false);
    const [selectedRegion, setSelectedRegion] = useState(null);

    const handleRegionChange = (selectedRegionId) => {
        setSelectedRegion(selectedRegionId);
    }

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
            </Grid>
            </LayoutContainer>
         </LayoutRoot>
        </>
    )
    }
    export default PostPage;