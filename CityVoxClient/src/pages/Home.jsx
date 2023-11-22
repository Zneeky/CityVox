// Import necessary components and libraries
import { TopNav } from "../components/navigation/TopNav"; // Top navigation bar component
import { useCallback, useEffect, useState } from "react"; // React hooks for state and side effects
import { styled } from "@mui/material/styles"; // Styling utility from Material-UI
import { useSelector } from "react-redux/es/hooks/useSelector"; // Redux hook for accessing the store
import { SideNav } from "../components/navigation/SideNav"; // Side navigation bar component
import { useLocation } from "react-router-dom"; // React Router hook for location information
import MapView from "../components/MapView"; // Map view component
import RegionDropdown from "../components/dropdown/RegionDropdown"; // Dropdown for selecting regions
import MunicipalityDropdown from "../components/dropdown/MunicipalityDropdown"; // Dropdown for selecting municipalities
import { Grid, Box } from "@mui/material"; // Material-UI components for layout

// Define the width of the side navigation bar
export const SIDE_NAV_WIDTH = 280;

// Styled components for layout styling
export const LayoutRoot = styled("div")(({ theme }) => ({
  display: "flex",
  flex: "1 1 auto",
  maxWidth: "100%",
  [theme.breakpoints.up("lg")]: {
    paddingLeft: SIDE_NAV_WIDTH,
  },
}));

export const LayoutContainer = styled("div")({
  display: "flex",
  flex: "1 1 auto",
  flexDirection: "column",
  width: "100%",
});

// Home component definition
const Home = () => {
  // Retrieve user information from the Redux store
  const appUser = useSelector((state) => state.user);

  // State variables for region, municipality, and navigation bar state
  const [selectedRegion, setSelectedRegion] = useState(null);
  const [selectedMunicipality, setSelectedMunicipality] = useState(null);
  const [openNav, setOpenNav] = useState(false);

  // Get the current location and pathname using React Router
  const location = useLocation();
  const pathname = location.pathname;

  // Handle region change
  const handleRegionChange = (selectedRegionId) => {
    setSelectedRegion(selectedRegionId);
  };

  // Callback to handle closing the navigation bar when the pathname changes
  const handlePathnameChange = useCallback(() => {
    if (openNav) {
      setOpenNav(false);
    }
  }, [openNav]);

  // Effect to handle pathname changes
  useEffect(() => {
    handlePathnameChange();
  }, [pathname]);

  // Render the components
  return (
    <>
      {/* Top navigation bar with a callback to open the side navigation */}
      <TopNav onNavOpen={() => setOpenNav(true)} />

      {/* Side navigation bar with props for closing and visibility */}
      <SideNav onClose={() => setOpenNav(false)} open={openNav} />

      {/* Root layout component */}
      <LayoutRoot>
        {/* Container for the main content */}
        <LayoutContainer>
          {/* Grid layout for region and municipality dropdowns */}
          <Grid
            container
            spacing={2}
            style={{ justifyContent: "flex-start" }}
            pl="1.5rem"
          >
            {/* Region dropdown */}
            <Grid item xs={12} sm={2} style={{ flexBasis: "33.33%" }}>
              <RegionDropdown onChange={handleRegionChange} />
            </Grid>

            {/* Municipality dropdown */}
            <Grid item xs={12} sm={2} style={{ flexBasis: "33.33%" }}>
              <MunicipalityDropdown
                regionId={selectedRegion}
                onChange={setSelectedMunicipality}
              />
            </Grid>
          </Grid>

          {/* Box component for map view */}
          <Box mt="1rem" p="1rem" width="100%" height={750} overflow="auto">
            {/* Map view component with props */}
            <MapView
              selectedMunicipalityId={selectedMunicipality?.municipalityId}
              osmId={selectedMunicipality?.osmId}
              token={appUser.accessToken}
            />
          </Box>
        </LayoutContainer>
      </LayoutRoot>
    </>
  );
};

// Export the Home component as the default export
export default Home;
