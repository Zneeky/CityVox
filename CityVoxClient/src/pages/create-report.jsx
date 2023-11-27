// Importing necessary components and hooks from external libraries and files
import { TopNav } from "../components/navigation/top-nav";
import { useCallback, useEffect, useState } from "react";
import { useSelector } from "react-redux/es/hooks/useSelector";
import { SideNav } from "../components/navigation/side-nav";
import { useLocation } from "react-router-dom";
import RegionDropdown from "../components/dropdown/region-dropdown";
import MunicipalityDropdown from "../components/dropdown/municipality-dropdown";
import { Grid, Box, Typography } from "@mui/material";
import { LayoutRoot, LayoutContainer, SIDE_NAV_WIDTH } from "./Home";
import IssueForm from "../components/issue-form";
import MapViewIssueCreate from "../components/map/map-view-issue-create";
import { CreateReport as CreateReportAPICall } from "../utils/api";
import { ReportTypes } from "../utils/consts";
import { uploadToCloudinary } from "../utils/api";

// Main component for creating a report
const CreateReport = () => {
  // Accessing the current user information from the Redux store
  const appUser = useSelector((state) => state.user);

  // State to manage the selected region and municipality
  const [selectedRegion, setSelectedRegion] = useState(null);
  const [selectedMunicipality, setSelectedMunicipality] = useState(null);

  // State to manage the form data for creating a report
  const [formData, setFormData] = useState({
    latitude: "",
    longitude: "",
    title: "",
    description: "",
    imageUrl: null,
    address: "",
    issueType: "",
    municipalityId: "",
    creatorId: appUser.id,
  });

  // Accessing location information from the React Router
  const location = useLocation();
  const pathname = location.pathname;

  // State to manage the side navigation bar
  const [openNav, setOpenNav] = useState(false);

  // Function to handle the form submission for creating a report
  const handleFormSubmit = async (formData) => {
    // Make API call to upload image to Cloudinary, if an image is provided
    if (formData.imageUrl) {
      const imgUrl = await uploadToCloudinary(formData.imageUrl);
      formData.imageUrl = imgUrl;
    }
    // Make API call to create a report with the form data
    const res = await CreateReportAPICall(appUser.accessToken, formData);

    // Show an alert with the report submission confirmation
    alert(`Your report has been submited for investigation! ID:${res}`);
    console.log(formData);
  };

  // Function to handle the change in the selected region
  const handleRegionChange = (selectedRegionId) => {
    setSelectedRegion(selectedRegionId);
  };

  // Callback function to handle changes in the pathname
  const handlePathnameChange = useCallback(() => {
    if (openNav) {
      setOpenNav(false);
    }
  }, [openNav]);

  // Effect to handle pathname changes and update form data when the selected municipality changes
  useEffect(
    () => {
      handlePathnameChange();
      if (selectedMunicipality) {
        setFormData((prevData) => ({
          ...prevData,
          municipalityId: selectedMunicipality.municipalityId, // or selectedMunicipality.id if municipalityId doesn't exist
        }));
      }
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [pathname, selectedMunicipality]
  );

  // JSX structure for rendering the CreateReport component
  return (
    <>
      <TopNav onNavOpen={() => setOpenNav(true)} />
      <SideNav onClose={() => setOpenNav(false)} open={openNav} />
      <LayoutRoot>
        <LayoutContainer>
          <Grid
            container
            spacing={2}
            style={{ justifyContent: "flex-start" }}
            pl="1.5rem"
          >
            <Grid item xs={12} sm={2} style={{ flexBasis: "33.33%" }}>
              <RegionDropdown onChange={handleRegionChange} />
            </Grid>
            <Grid item xs={12} sm={2} style={{ flexBasis: "33.33%" }}>
              <MunicipalityDropdown
                regionId={selectedRegion}
                onChange={setSelectedMunicipality}
              />
            </Grid>
          </Grid>
          <Typography p={"1.6rem"} variant="h4">
            Create a report request
          </Typography>
          <Box
            mt="1rem"
            p="1.6rem"
            width="100%"
            height={700}
            overflow="auto"
            display="flex"
          >
            <Box sx={{ pr: "1rem" }}>
              <IssueForm
                issueTypes={ReportTypes}
                formData={formData}
                setFormData={setFormData}
                onFormSubmit={handleFormSubmit}
              />
            </Box>
            {/* MapViewIssueCreate component for displaying the map */}
            <MapViewIssueCreate
              selectedMunicipalityId={selectedMunicipality?.municipalityId}
              osmId={selectedMunicipality?.osmId}
              formData={formData}
              setFormData={setFormData}
              type={"report"}
            />
          </Box>
        </LayoutContainer>
      </LayoutRoot>
    </>
  );
};

// Export the CreateReport component as the default export
export default CreateReport;
