import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { FormControl, InputLabel, Select, MenuItem } from '@mui/material';
import { GetRegions } from '../../utils/api';
import { useSelector } from 'react-redux';

const RegionDropdown = ({ onChange }) => {
    const [regions, setRegions] = useState([]);
    const [selectedRegion, setSelectedRegion] = useState(''); // Added this state

    useEffect(() => {
      GetRegions()
        .then(response => {
          setRegions(response);
        });
    }, []);
  
    const handleChange = event => {
      const selectedRegionId = event.target.value;
      setSelectedRegion(selectedRegionId);
      onChange(selectedRegionId);
    };
  
    return (
      <FormControl fullWidth variant="filled">
        <InputLabel id="region-label">Region</InputLabel>
        <Select
          labelId="region-label"
          value={selectedRegion} // Add this line
          onChange={handleChange}
        >
          {regions?.$values?.map(region => (
            <MenuItem key={region.Id} value={region.Id}>
              {region.RegionName}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    );
  };
  
  export default RegionDropdown;
  
