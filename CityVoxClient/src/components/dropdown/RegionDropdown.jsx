import React, { useEffect, useState } from "react";
import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { GetRegions } from "../utils/api";
import { useSelector } from "react-redux";

/**
 * RegionDropdown component is a dropdown menu for selecting regions.
 *
 * @component
 * @param {Object} props - Component props
 * @param {Function} props.onChange - Callback function to handle region selection change
 * @returns {JSX.Element} - Rendered RegionDropdown component
 */
const RegionDropdown = ({ onChange }) => {
  // State to store the list of regions
  const [regions, setRegions] = useState([]);

  // State to store the currently selected region
  const [selectedRegion, setSelectedRegion] = useState("");

  // Access the user's token from the Redux store using the useSelector hook
  const token = useSelector((state) => state.user.accessToken);

  /**
   * useEffect hook to fetch regions when the component mounts or when the token changes.
   * Updates the regions state with the fetched data.
   */
  useEffect(() => {
    GetRegions(token).then((response) => {
      setRegions(response);
    });
  }, [token]);

  /**
   * Event handler for region selection change.
   * Updates the selectedRegion state and calls the provided onChange callback.
   *
   * @param {Object} event - The change event object
   */
  const handleChange = (event) => {
    const selectedRegionId = event.target.value;
    setSelectedRegion(selectedRegionId);
    onChange(selectedRegionId);
  };

  /**
   * Rendered JSX structure of the RegionDropdown component.
   * Uses Material-UI components (FormControl, InputLabel, Select, MenuItem).
   */
  return (
    <FormControl fullWidth variant="filled">
      <InputLabel id="region-label">Region</InputLabel>
      <Select
        labelId="region-label"
        value={selectedRegion} // Specifies the currently selected region
        onChange={handleChange} // Calls handleChange on selection change
      >
        {/* Maps through the regions and creates a MenuItem for each */}
        {regions?.$values?.map((region) => (
          <MenuItem key={region.Id} value={region.Id}>
            {region.RegionName}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default RegionDropdown;
