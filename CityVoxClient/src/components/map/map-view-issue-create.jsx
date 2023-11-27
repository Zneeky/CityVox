// Import necessary React components and libraries
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
} from "../../utils/api";
import osmtogeojson from "osmtogeojson";
import L, { marker } from "leaflet";
import * as turf from "@turf/turf";
import { useSelector } from "react-redux/es/hooks/useSelector";
import { Typography } from "@mui/material";

// Function to determine the style of GeoJSON features based on the type
function style(type) {
  // Style for report features
  if (type === "report") {
    return {
      fillColor: "transparent", // Or set to the same color as your map's background
      weight: 3, // This is the border thickness
      opacity: 1,
      color: "blue", // The border color
      dashArray: "1",
      fillOpacity: 0.0, // No fill
    };
  } else if (type === "emergency") {
    // Style for emergency features
    return {
      fillColor: "red", // Or set to the same color as your map's background
      weight: 3, // This is the border thickness
      opacity: 1,
      color: "red", // The border color
      dashArray: "1",
      fillOpacity: 0.1, // No fill
    };
  } else {
    // Default style for other features
    return {
      fillColor: "transparent", // Or set to the same color as your map's background
      weight: 3, // This is the border thickness
      opacity: 1,
      color: "yellow", // The border color
      dashArray: "1",
      fillOpacity: 0.0, // No fill
    };
  }
}

// Function to create a colored icon for markers
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

// Main MapViewIssueCreate component
const MapViewIssueCreate = ({
  selectedMunicipalityId,
  osmId,
  formData,
  setFormData,
  type,
}) => {
  // ...other states and useEffects
  const [boundaryData, setBoundaryData] = useState(null);
  const [reports, setReports] = useState([]);
  const [center, setCenter] = useState([42.6977, 23.3219]); // Default center coordinates Sofia Bulgaria
  const mapRef = useRef();
  const [zoom, setZoom] = useState(13);
  const [popup, setPopup] = useState({ show: false, position: null });
  const token = useSelector((state) => state.user.accessToken);
  const newMarkerRef = useRef(null);

  // Function to get the status color based on the type
  const getStatusColor = () => {
    if (type === "report") {
      return `blue`; //red
    } else if (type === "emergency") {
      return `red`;
    } else if (type === "infIssue") {
      return `yellow`;
    }
  };

  useEffect(() => {
    // Fetch boundary data from the API
    if (osmId !== undefined) {
      MunicipalityBoundaries(osmId).then((response) => {
        const geoJson = osmtogeojson(response?.data);
        setBoundaryData(geoJson);
      });
    }

    // Fetch reports from backend API
    if (selectedMunicipalityId !== undefined) {
      if (type === "report") {
        GetReportsByMunicipality(selectedMunicipalityId).then((response) => {
          setReports(response);
        });
      } else if (type === "emergency") {
        GetEmergenciesByMunicipality(selectedMunicipalityId).then(
          (response) => {
            setReports(response);
          }
        );
      } else if (type === "infIssue") {
        GetInfIssuesByMunicipality(selectedMunicipalityId).then((response) => {
          setReports(response);
        });
      }
    }

    if (newMarkerRef.current) {
      // Remove the marker from the map
      newMarkerRef.current.remove();

      // Reset formData values associated with the marker
      setFormData((prevData) => ({
        ...prevData,
        latitude: null,
        longitude: null,
        address: null,
      }));

      // Hide the popup
      setPopup({ show: false, position: null, address: null });
    }
  }, [selectedMunicipalityId, osmId]);

  return (
    <MapContainer
      ref={mapRef}
      center={center}
      zoom={zoom}
      style={{ width: "100%" }}
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
          style={style(type)}
        />
      )}
      <SetBoundary geoJson={boundaryData} />
      {boundaryData && (
        <MapEvents
          setPopup={setPopup}
          setFormData={setFormData}
          boundaryData={boundaryData}
        />
      )}
      {boundaryData && popup.position && (
        <Marker
          ref={newMarkerRef}
          position={popup.position}
          icon={createColoredIcon("black")}
        >
          <Popup minWidth={90}>
            <span>
              Coordinates: {popup.position.lat}, {popup.position.lng}
              <br />
              Address: {popup.address}
            </span>
          </Popup>
        </Marker>
      )}
      {reports &&
        reports.map((report) => (
          <Marker
            position={[report.Latitude, report.Longitude]}
            key={report.Id}
            icon={createColoredIcon(getStatusColor(type))} // Pass the correct color based on the type
          >
            <Popup minWidth={90}>
              <Typography variant="h6">{report.Title}</Typography>
              <Typography variant="body1">{report.Description}</Typography>
              <Typography variant="body2" color="text.secondary">
                Coordinates: {report.Latitude}, {report.Longitude}
                <br />
                Address: {report.Address}
              </Typography>
              <Typography variant="body2">
                Submitted By: {report.CreatorUsername}
              </Typography>
            </Popup>
          </Marker>
        ))}
    </MapContainer>
  );
};
// Export the MapViewIssueCreate component as the default export
export default MapViewIssueCreate;

// Component for handling map events
const MapEvents = ({ setPopup, setFormData, boundaryData }) => {
  const fetchAddress = async (lat, lng) => {
    const response = await opencage.geocode({
      q: `${lat}+${lng}`,
      key: "3447ad1e12f74ca2939f99e51b472648",
    });
    return response.results[0].formatted; // This will give you the address as a string
  };

  useMapEvents({
    click(e) {
      const point = turf.point([e.latlng.lng, e.latlng.lat]);

      if (boundaryData) {
        const polygon = turf.polygon(
          boundaryData.features[0].geometry.coordinates
        );
        const isInside = turf.booleanPointInPolygon(point, polygon);

        if (!isInside) {
          // Do not show the popup if the point is not inside the boundary
          return;
        }
      }

      fetchAddress(e.latlng.lat, e.latlng.lng).then((address) => {
        setFormData((prevData) => ({
          ...prevData,
          latitude: e.latlng.lat,
          longitude: e.latlng.lng,
          address: address,
        }));
        setPopup({ show: true, position: e.latlng, address: address });
      });
    },
  });

  return null;
};

// Component for a draggable marker on the map
const DraggableMarker = ({ setFormData, positionIntial }) => {
  const [draggable, setDraggable] = useState(true);
  const [position, setPosition] = useState(positionIntial);
  const markerRef = useRef(null);
  const eventHandlers = useMemo(
    () => ({
      dragend() {
        const marker = markerRef.current;
        if (marker != null) {
          setPosition(marker.getLatLng());
          setFormData((prevData) => ({
            ...prevData,
            lat: marker.getLatLng().lat,
            lng: marker.getLatLng().lng,
            // TODO: get address from lat and lng
          }));
        }
      },
    }),
    [setFormData]
  );
  const toggleDraggable = useCallback(() => {
    setDraggable((d) => !d);
  }, []);

  return (
    <Marker
      draggable={draggable}
      eventHandlers={eventHandlers}
      position={position}
      ref={markerRef}
    >
      <Popup minWidth={90}>
        <span onClick={toggleDraggable}>
          {draggable
            ? "Marker is draggable"
            : "Click here to make marker draggable"}
        </span>
      </Popup>
    </Marker>
  );
};

// Component to set the boundary of the map based on GeoJSON data
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
