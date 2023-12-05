import {
  LayoutContainer,
  LayoutRoot,
  SIDE_NAV_WIDTH,
} from "../pages/home-page";
import { TopNav } from "../components/navigation/top-nav";
import { SideNav } from "../components/navigation/side-nav";
import EditIssueForm from "../components/edit-issue-form";
import { ReportTypes, ReportStatusTypes } from "../utils/consts";
import { useCallback, useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { Box, Typography, Alert, AlertTitle } from "@mui/material";
import { GetReport } from "../utils/api";
import { useParams } from "react-router-dom";
import { useSelector } from "react-redux/es/hooks/useSelector";
import MapViewIssue from "../components/map/map-view-issue";

const EditReport = () => {
  const { reportId } = useParams();
  const location = useLocation();
  const pathname = location.pathname;
  const [openNav, setOpenNav] = useState(false);
  const [report, setReport] = useState(null);
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
        const response = await GetReport(reportId);
        setReport(response);
      } catch (error) {
        console.error("Failed to fetch report:", error);
      } finally {
        setLoading(false); // Set loading to false once fetching is done
      }
    };

    FetchIssue();
    handlePathnameChange();
  }, [pathname, reportId]);

  if (loading) {
    return <div>Loading...</div>; // You can replace this with a spinner or any loading component
  }

  if (
    appUser.username === report?.CreatorUsername ||
    appUser.role === "admin"
  ) {
    return (
      <>
        <TopNav onNavOpen={() => setOpenNav(true)} />
        <SideNav onClose={() => setOpenNav(false)} open={openNav} />
        <LayoutRoot>
          <LayoutContainer>
            <Typography p="1.6rem" mt="0.5rem" variant="h4">
              Edit Report{" "}
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
                  type={"report"}
                  issueTypes={ReportTypes}
                  statusTypes={ReportStatusTypes}
                  issue={report}
                />
              </Box>
              <MapViewIssue issue={report} type={"report"} />
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

export default EditReport;
