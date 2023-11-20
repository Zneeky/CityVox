import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux/es/hooks/useSelector";
import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { GetMunicipalities } from "../../utils/api";

/**
 * MunicipalityDropdown component is a dropdown menu for selecting municipalities based on a given region.
 *
 * @component
 * @param {Object} props - Component props
 * @param {string} props.regionId - The ID of the region for which municipalities are fetched
 * @param {Function} props.onChange - Callback function to handle municipality selection change
 * @returns {JSX.Element} - Rendered MunicipalityDropdown component
 */
const MunicipalityDropdown = ({ regionId, onChange }) => {
  // State to store the list of municipalities
  const [municipalities, setMunicipalities] = useState([]);

  // State to store the currently selected municipality
  const [selectedMunicipality, setSelectedMunicipality] = useState(null);

  // Access the user's token from the Redux store using the useSelector hook
  const token = useSelector((state) => state.user.accessToken);

  /**
   * useEffect hook to fetch municipalities when the component mounts or when the regionId changes.
   * Updates the municipalities state with the fetched data.
   */
  useEffect(() => {
    // Check if regionId is provided before making the API call
    if (regionId === null || regionId === undefined) return;

    // Fetch municipalities based on the regionId
    GetMunicipalities(token, regionId).then((response) => {
      setMunicipalities(response.$values);
    });
  }, [regionId, token]);

  /**
   * Event handler for municipality selection change.
   * Updates the selectedMunicipality state and calls the provided onChange callback.
   *
   * @param {Object} event - The change event object
   */
  const handleChange = (event) => {
    // Extract the selected municipality ID from the event
    const selectedMunicipalityId = event.target.value;

    // Find the municipality object in the list based on the ID
    const selectedMunicipality = municipalities.find(
      (municipality) => municipality.Id === selectedMunicipalityId
    );

    // Set the selectedMunicipality state to the found municipality object
    setSelectedMunicipality(selectedMunicipality);

    // Pass both the municipality id and the OpenStreetMap id to the callback
    onChange({
      municipalityId: selectedMunicipality.Id,
      osmId: selectedMunicipality.OpenStreetMapCode,
    });
  };

  /**
   * Rendered JSX structure of the MunicipalityDropdown component.
   * Uses Material-UI components (FormControl, InputLabel, Select, MenuItem).
   */
  return (
    <FormControl fullWidth variant="filled">
      <InputLabel id="municipality-label">Municipality</InputLabel>
      <Select
        labelId="municipality-label"
        value={selectedMunicipality ? selectedMunicipality.Id : ""} // Update value to handle null state
        onChange={handleChange}
      >
        {/* Maps through the municipalities and creates a MenuItem for each */}
        {municipalities.map((municipality) => (
          <MenuItem key={municipality.Id} value={municipality.Id}>
            {municipality.MunicipalityName}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default MunicipalityDropdown;
