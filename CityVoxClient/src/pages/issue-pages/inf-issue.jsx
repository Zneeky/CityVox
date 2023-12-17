import { LayoutContainer, LayoutRoot, SIDE_NAV_WIDTH } from "../home-page";
import { TopNav } from "../../components/navigation/top-nav";
import { SideNav } from "../../components/navigation/side-nav";
import { useCallback, useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { Box, Typography } from "@mui/material";
import { GetInfIssue } from "../../utils/api";
import { useParams } from "react-router-dom";
import { useSelector } from "react-redux/es/hooks/useSelector";
import MapViewIssue from "../../components/map/map-view-issue";
import IssuePresent from "../../components/issue-present";

const InfIssue = () => {
  const { infIssueId } = useParams();
  const location = useLocation();
  const pathname = location.pathname;
  const [openNav, setOpenNav] = useState(false);
  const [infIssue, setInfIssue] = useState(null);
  const token = useSelector((state) => state.user.accessToken);


  const handlePathnameChange = useCallback(() => {
    if (openNav) {
      setOpenNav(false);
    }

  }, [openNav]);

  useEffect(
    () => {
      const FetchIssue = async () => {
        const response = await GetInfIssue(infIssueId);
        setInfIssue(response);
      }

      FetchIssue();
      handlePathnameChange();
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [pathname, infIssueId]
  );
  return (
    <>
      <TopNav onNavOpen={() => setOpenNav(true)} />
      <SideNav onClose={() => setOpenNav(false)} open={openNav} />
      <LayoutRoot>
        <LayoutContainer>
          <Typography p="1.6rem" mt="0.5rem" variant="h4">Infrastructure Issue </Typography>
          <Box mt="0.5rem" p="0 1.6rem 1.6rem 1.6rem" width="100%" height={750} overflow="auto" display="flex" >
            <Box pr="1em" minWidth="500px">
              {infIssue && <IssuePresent issue={infIssue} type={"infrastructure_issues"}/>}
            </Box>
            <MapViewIssue issue={infIssue} type={"infIssue"} />
          </Box>
        </LayoutContainer>
      </LayoutRoot>
    </>
  )
}

export default InfIssue;