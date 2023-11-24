// Importing necessary React and third-party libraries
import React, { useEffect, useState, useRef } from "react";
import {
  MapContainer,
  TileLayer,
  GeoJSON,
  Marker,
  Popup,
  useMap,
} from "react-leaflet";
import {
  MunicipalityBoundaries,
  GetReportsByMunicipality,
  GetEmergenciesByMunicipality,
  GetInfIssuesByMunicipality,
} from "../utils/api"; // Importing API functions for data fetching
import osmtogeojson from "osmtogeojson";
import L from "leaflet"; // Importing Leaflet for creating colored icons
import { useSelector } from "react-redux/es/hooks/useSelector"; // Importing Redux hook for accessing the store
import { Button, Typography, Box } from "@mui/material"; // Importing Material-UI components
import { useNavigate } from "react-router-dom"; // Importing React Router hook for navigation

// Function to define the style of GeoJSON boundaries
function style() {
  return {
    fillColor: "transparent",
    weight: 3,
    opacity: 1,
    color: "blue",
    dashArray: "1",
    fillOpacity: 0.0,
  };
}

// Function to create colored icons for markers
const createColoredIcon = (color) => {
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

// The main MapView component
const MapView = ({ selectedMunicipalityId, osmId = "all" }) => {
  const [boundaryData, setBoundaryData] = useState(null);
  const [issues, setIssues] = useState([]);
  const [center, setCenter] = useState([42.6977, 23.3219]); // Default center coordinates Sofia Bulgaria
  const mapRef = useRef();
  const [zoom, setZoom] = useState(13);
  const appUser = useSelector((state) => state.user); // Accessing user information from Redux store
  //const token = useSelector((state) => state.user.accessToken); // Accessing user's access token from Redux store
  const navigate = useNavigate(); // React Router hook for navigation

  // Function to get the color based on the type of issue
  const getStatusColor = (type) => {
    if (type === "report") {
      return `blue`;
    } else if (type === "emergency") {
      return `red`;
    } else if (type === "infrastructure_issue") {
      return `yellow`;
    }
  };

  // Function to get the link to edit based on the type of issue
  const getLinkToEdit = (type) => {
    if (type === "report") {
      return `reports`;
    } else if (type === "emergency") {
      return `emergencies`;
    } else if (type === "infrastructure_issue") {
      return `infrastructure_issues`;
    }
  };

  // useEffect to fetch data when selectedMunicipalityId or osmId changes
  useEffect(() => {
    // Fetching municipality boundaries from OpenStreetMap
    if (osmId !== undefined) {
      MunicipalityBoundaries(osmId).then((response) => {
        const geoJson = osmtogeojson(response?.data);
        setBoundaryData(geoJson);
      });
    }

    // Fetching issues by municipality
    if (selectedMunicipalityId !== undefined) {
      Promise.all([
        GetReportsByMunicipality(selectedMunicipalityId),
        GetEmergenciesByMunicipality(selectedMunicipalityId),
        GetInfIssuesByMunicipality(selectedMunicipalityId),
      ])
        .then((results) => {
          const mergedResults = [].concat(...results);
          setIssues(mergedResults);
        })
        .catch((error) => {
          console.error("Error fetching issues: ", error);
        });
    }
  }, [selectedMunicipalityId, osmId]);

  // Rendering the MapContainer with Leaflet map, TileLayer, GeoJSON boundaries, markers, and popups
  return (
    <MapContainer
      ref={mapRef}
      center={center}
      zoom={zoom}
      style={{ width: "100%", height: "100%" }}
      whenCreated={(mapInstance) => {
        mapRef.current = mapInstance;
      }}
    >
      {/* OpenStreetMap Tile Layer */}
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
      />
      {/* Render GeoJSON boundaries if available */}
      {boundaryData && (
        <GeoJSON
          key={JSON.stringify(boundaryData)}
          data={boundaryData}
          style={style()}
        />
      )}
      {/* Render SetBoundary component to fit the map to boundaries */}
      <SetBoundary geoJson={boundaryData} />
      {/* Render markers for each issue */}
      {issues &&
        issues.map((issue) => (
          <Marker
            position={[issue.Latitude, issue.Longitude]}
            key={issue.Id}
            icon={createColoredIcon(getStatusColor(issue.Represent))}
          >
            <Popup minWidth={90}>
              {/* Popup content with issue details */}
              <Typography variant="h6">{issue.Title}</Typography>
              <Typography variant="body1">{issue.Description}</Typography>
              <Typography variant="body2" color="text.secondary">
                Coordinates: {issue.Latitude}, {issue.Longitude}
                <br />
                Address: {issue.Address}
              </Typography>
              <Box display="flex" justifyContent="space-between">
                <Typography variant="body2">
                  Submitted By: {issue.CreatorUsername}
                </Typography>
                {/* Button to edit or view the issue based on user permissions */}
                {appUser.username === issue.CreatorUsername ? (
                  <Button
                    type="text"
                    onClick={() =>
                      navigate(
                        `/${getLinkToEdit(issue.Represent)}/edit/${issue.Id}`
                      )
                    }
                  >
                    Edit Issue
                  </Button>
                ) : (
                  <Button
                    type="text"
                    onClick={() =>
                      navigate(`/${getLinkToEdit(issue.Represent)}/${issue.Id}`)
                    }
                  >
                    View
                  </Button>
                )}
              </Box>
            </Popup>
          </Marker>
        ))}
    </MapContainer>
  );
};

export default MapView;

// SetBoundary component to fit the map to boundaries
const SetBoundary = ({ geoJson }) => {
  const map = useMap();

  // useEffect to fit the map to boundaries when GeoJSON data is available
  useEffect(() => {
    if (geoJson) {
      const geoJsonLayer = L.geoJSON(geoJson);
      map.fitBounds(geoJsonLayer.getBounds());
    }
  }, [geoJson, map]);

  return null;
};
