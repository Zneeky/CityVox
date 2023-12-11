import {
  LayoutContainer,
  LayoutRoot,
  SIDE_NAV_WIDTH,
} from "../pages/home-page";
import { TopNav } from "../components/navigation/top-nav";
import { SideNav } from "../components/navigation/side-nav";
import EditIssueForm from "../components/edit-issue-form";
import { InfIssueTypes, InfIssueStatusTypes } from "../utils/consts";
import { useCallback, useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { Box, Typography, Alert, AlertTitle } from "@mui/material";
import { GetInfIssue } from "../utils/api";
import { useParams } from "react-router-dom";
import { useSelector } from "react-redux/es/hooks/useSelector";
import MapViewIssue from "../components/map/map-view-issue";

const EditInfIssue = () => {
  const { infIssueId } = useParams();
  const location = useLocation();
  const pathname = location.pathname;
  const [openNav, setOpenNav] = useState(false);
  const [infIssue, setInfIssue] = useState(null);
  const appUser = useSelector((state) => state.user);

  const [loading, setLoading] = useState(true);

  const handlePathnameChange = useCallback(() => {
    if (openNav) {
      setOpenNav(false);
    }
  }, [openNav]);

  useEffect(() => {
    const FetchIssue = async () => {
      try {
        const response = await GetInfIssue(infIssueId);
        setInfIssue(response);
      } catch (error) {
        console.error("Failed to fetch infIsue:", error);
      } finally {
        setLoading(false); // Set loading to false once fetching is done
      }
    };

    FetchIssue();
    handlePathnameChange();
  }, [pathname, infIssueId]);

  if (loading) {
    return <div>Loading...</div>; // You can replace this with a spinner or any loading component
  }

  if (
    appUser.username === infIssue?.CreatorUsername ||
    appUser.role === "Admin"
  ) {
    return (
      <>
        <TopNav onNavOpen={() => setOpenNav(true)} />
        <SideNav onClose={() => setOpenNav(false)} open={openNav} />
        <LayoutRoot>
          <LayoutContainer>
            <Typography p="1.6rem" mt="0.5rem" variant="h4">
              Edit Infrastructure Issue{" "}
            </Typography>
            <Box
              mt="0.5rem"
              p="0 1.6rem 1.6rem 1.6rem"
              width="100%"
              height={700}
              overflow="auto"
              display="flex"
            >
              <Box pr="1em">
                <EditIssueForm
                  type={"infIssue"}
                  issueTypes={InfIssueTypes}
                  statusTypes={InfIssueStatusTypes}
                  issue={infIssue}
                />
              </Box>
              <MapViewIssue issue={infIssue} type={"infIssue"} />
            </Box>
          </LayoutContainer>
        </LayoutRoot>
      </>
    );
  } else {
    return (
      <Alert severity="error">
        <AlertTitle>Error</AlertTitle>
        This is an error alert — <strong>UNAUTHORIZED!</strong>
      </Alert>
    );
  }
};

export default EditInfIssue;
