import React, {
  useEffect,
  useState,
  useRef,
  useMemo,
  useCallback,
} from "react";
import {
  MapContainer,
  TileLayer,
  GeoJSON,
  Marker,
  Popup,
  useMap,
  useMapEvents,
} from "react-leaflet";
import opencage from "opencage-api-client";
import {
  MunicipalityBoundaries,
  GetReportsByMunicipality,
  GetEmergenciesByMunicipality,
  GetInfIssuesByMunicipality,
} from "../utils/api";
import osmtogeojson from "osmtogeojson";
import L, { marker } from "leaflet";
import * as turf from "@turf/turf";
import { useSelector } from "react-redux/es/hooks/useSelector";
import { Button, Typography, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";



function style() {
  return {
    fillColor: "transparent", // Or set to the same color as your map's background
    weight: 3, // This is the border thickness
    opacity: 1,
    color: "blue", // The border color
    dashArray: "1",
    fillOpacity: 0.0, // No fill
  };
}

const createColoredIcon = (color) => {
  return L.icon({
    iconUrl: `https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-${color}.png`,
    shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
  });
};

const MapView = ({ selectedMunicipalityId, osmId, issueToPresent = "all" }) => {
  const [boundaryData, setBoundaryData] = useState(null);
  const [issues, setIssues] = useState([]);
  const [center, setCenter] = useState([42.6977, 23.3219]); // Default center coordinates Sofia Bulgaria
  const mapRef = useRef();
  const [zoom, setZoom] = useState(13);
  const appUser = useSelector((state) => state.user);
  const navigate = useNavigate();

  const getStatusColor = (type) => {
    if (type === "report") {
      return `blue`; //red
    } else if (type === "emergency") {
      return `red`;
    } else if (type === "infrastructure_issue") {
      return `yellow`;
    }
  };

  const getLinkToEdit = (type) =>{
    if (type === "report") {
      return `reports`; //red
    } else if (type === "emergency") {
      return `emergencies`;
    } else if (type === "infrastructure_issue") {
      return `infrastructure_issues`;
    }
  }

  useEffect(() => {

    if (osmId !== undefined) {
      MunicipalityBoundaries(osmId).then((response) => {
        const geoJson = osmtogeojson(response?.data);
        setBoundaryData(geoJson);
      });
    }

    if (selectedMunicipalityId !== undefined) {
      Promise.all([
        GetReportsByMunicipality(selectedMunicipalityId),
        GetEmergenciesByMunicipality(selectedMunicipalityId),
        GetInfIssuesByMunicipality(selectedMunicipalityId)
      ]).then(results => {
        // Assuming each API returns an array of issues
        const mergedResults = [].concat(...results);
        setIssues(mergedResults);
      }).catch(error => {
        console.error("Error fetching issues: ", error);
      });
    }

  }, [selectedMunicipalityId, osmId]);

  return (
    <MapContainer
      ref={mapRef}
      center={center}
      zoom={zoom}
      style={{ width: "100%", height:"100%" }}
      whenCreated={(mapInstance) => {
        mapRef.current = mapInstance;
      }}
    >
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
      />
      {boundaryData && (
        <GeoJSON
          key={JSON.stringify(boundaryData)}
          data={boundaryData}
          style={style()}
        />
      )}
      <SetBoundary geoJson={boundaryData} />
      {issues &&
        issues.map((issue) => (
          <Marker
            position={[issue.Latitude, issue.Longitude]}
            key={issue.Id}
            icon={createColoredIcon(getStatusColor(issue.Represent))} // Pass the correct color based on the right representation
          >
            <Popup minWidth={90}>
              <Typography variant="h6">
                {issue.Title}
              </Typography>
              <Typography variant="body1">
                {issue.Description}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Coordinates: {issue.Latitude}, {issue.Longitude}
                <br />
                Address: {issue.Address}
              </Typography>
              <Box display="flex" justifyContent="space-between">
              <Typography variant="body2">
                Submitted By: {issue.CreatorUsername}
              </Typography>
              {appUser.username === issue.CreatorUsername ? (
                <Button type="text" onClick={() => navigate(`/${getLinkToEdit(issue.Represent)}/edit/${issue.Id}`)}>
                  Edit Issue
                </Button>
              ):(
                <Button  type="text" onClick={() => navigate(`/${getLinkToEdit(issue.Represent)}/${issue.Id}`)}>
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

const SetBoundary = ({ geoJson }) => {
  const map = useMap();

  useEffect(() => {
    if (geoJson) {
      const geoJsonLayer = L.geoJSON(geoJson);
      map.fitBounds(geoJsonLayer.getBounds());
    }
  }, [geoJson, map]);

  return null;
};