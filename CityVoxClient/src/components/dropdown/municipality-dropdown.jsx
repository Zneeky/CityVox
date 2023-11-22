import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux/es/hooks/useSelector';
import axios from 'axios';
import { FormControl, InputLabel, Select, MenuItem, Box } from '@mui/material';
import { GetMunicipalities } from '../../utils/api';

const MunicipalityDropdown = ({ regionId, onChange }) => {
  const [municipalities, setMunicipalities] = useState([]);
  const [selectedMunicipality, setSelectedMunicipality] = useState(null); // New state
  const token = useSelector(state => state.user.accessToken);

  useEffect(() => {
    if (regionId===null || regionId===undefined) return;

    GetMunicipalities(token,regionId)
      .then(response => {
        setMunicipalities(response.$values);
      });
  }, [regionId, token]);

  const handleChange = event => {
    const selectedMunicipalityId = event.target.value;
    const selectedMunicipality = municipalities.find(municipality => municipality.Id === selectedMunicipalityId);

    setSelectedMunicipality(selectedMunicipality);  // Set the selectedMunicipality state to the found municipality object

    // Pass both the municipality id and the OpenStreetMap id to the callback
    onChange({municipalityId: selectedMunicipality.Id, osmId: selectedMunicipality.OpenStreetMapCode});
  };

  return (
    <FormControl fullWidth variant="filled">
      <InputLabel id="municipality-label">Municipality</InputLabel>
      <Select
        labelId="municipality-label"
        value={selectedMunicipality ? selectedMunicipality.Id : ''} // Update value to handle null state
        onChange={handleChange}
      >
        {municipalities.map(municipality => (
          <MenuItem key={municipality.Id} value={municipality.Id}>
            {municipality.MunicipalityName}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default MunicipalityDropdown;
