// Import necessary React components and hooks
import React, { useEffect, useState, useRef } from "react";
import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
import { useSelector } from "react-redux/es/hooks/useSelector";
import { Box, Typography } from "@mui/material";

// Component to move the map view to the location of a specific issue
const MoveToIssueLocation = ({ issue }) => {
  // Access the map instance using the useMap hook
  const map = useMap();

  // Effect to fly to the location of the specified issue when it changes
  useEffect(() => {
    if (issue != null) {
      map.flyTo([issue.Latitude, issue.Longitude]);
    }
  }, [issue, map]);

  // This component doesn't render anything.
  return null;
};

// Function to create a colored marker icon based on the provided color
const createColoredIcon = (color) => {
  // Return a Leaflet icon object with the specified color
  return L.icon({
    iconUrl: `https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-${color}.png`,
    shadowUrl:
      "https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png",
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41],
  });
};

// Main MapViewIssue component to display the map with a marker for a specific issue
const MapViewIssue = ({ issue, type }) => {
  // ...other states and useEffects
  const [center, setCenter] = useState([42, 23]); // Default center coordinates Sofia, Bulgaria
  const mapRef = useRef();
  const zoom = 13;
  const token = useSelector((state) => state.user.accessToken);

  // Function to get the marker color based on the issue type
  const getStatusColor = () => {
    if (type === "report") {
      return `blue`; // Red
    } else if (type === "emergency") {
      return `red`;
    } else if (type === "infIssue") {
      return `yellow`;
    }
  };

  return (
    // Container for the Leaflet map
    <MapContainer
      ref={mapRef}
      center={center}
      zoom={zoom}
      style={{ width: "100%" }}
      // Callback when the map is created
      whenCreated={(mapInstance) => {
        mapRef.current = mapInstance;
      }}
    >
      {/* OpenStreetMap tile layer */}
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
      />
      {/* Component to move the map view to the location of the specified issue */}
      <MoveToIssueLocation issue={issue} />
      {/* Marker for the specified issue */}
      {issue && (
        <Marker
          position={[issue.Latitude, issue.Longitude]}
          key={issue.Id}
          // Colored marker icon based on the issue type
          icon={createColoredIcon(getStatusColor(type))}
        >
          {/* Popup with details about the issue */}
          <Popup minWidth={90}>
            <Box sx={{ maxHeight: '500px', overflowY: 'auto' }}>
              <Typography variant="h6">{issue.Title}</Typography>
              <Typography variant="body1">{issue.Description}</Typography>
              <Typography variant="body4" color="text.secondary">
                Municipality: {issue.Municipality}
                <br />
                Coordinates: {issue.Latitude}, {issue.Longitude}
                <br />
                Address: {issue.Address}
              </Typography>
              <Typography variant="body2">Type: {issue.Type}</Typography>
              <Typography variant="body2">
                Submitted By: {issue.CreatorUsername}
              </Typography>
            </Box>
          </Popup>
        </Marker>
      )}
    </MapContainer>
  );
};

// Export the MapViewIssue component as the default export
export default MapViewIssue;
