import { Box, Select, MenuItem, Container } from "@mui/material";
import { SideNav } from "../../components/navigation/side-nav";
import { TopNav } from "../../components/navigation/top-nav";
import { LayoutContainer, LayoutRoot } from "../Home";
import { useCallback, useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import RequestLayout from "../../components/request/request-layout";

const Requests = () => {
  const location = useLocation();
  const pathname = location.pathname;
  const [openNav, setOpenNav] = useState(false);
  const [type, setType] = useState("reports");

  const handlePathnameChange = useCallback(() => {
    if (openNav) {
      setOpenNav(false);
    }
  }, [openNav]);

  const handleTypeChange = (event) => {
    setType(event.target.value);
  };

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
          <Box>
              <Container maxWidth="xl">
              <Select value={type} onChange={handleTypeChange}>
                <MenuItem value={"reports"}>Reports</MenuItem>
                <MenuItem value={"emergencies"}>Emergencies</MenuItem>
                <MenuItem value={"infrastructure_issues"}>
                  Infrastructure Issues
                </MenuItem>
              </Select>
              </Container>
            <RequestLayout type={type} />
          </Box>
        </LayoutContainer>
      </LayoutRoot>
    </>
  );
};

export default Requests;
