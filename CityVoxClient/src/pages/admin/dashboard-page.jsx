import React from "react";
import { Link } from "react-router-dom";
import BlockIcon from "@mui/icons-material/Block";
import Button from "@mui/material/Button";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";

const Dashboard = () => {
  const dashboardContainerStyle = {
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    justifyContent: "center",
    position: "fixed",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    background: "#FF4C4C", // Red background
    color: "white",
    padding: "40px",
    borderRadius: "20px",
    maxWidth: "400px",
    boxShadow: "0px 4px 12px rgba(0, 0, 0, 0.2)",
  };

  const headingStyle = {
    fontSize: "24px",
    marginBottom: "20px",
  };

  const iconStyle = {
    fontSize: "64px",
    color: "white",
    marginBottom: "20px",
  };

  const buttonStyle = {
    marginTop: "20px",
  };

  return (
    <div style={dashboardContainerStyle}>
      <BlockIcon style={iconStyle} />
      <h2 style={headingStyle}>Access Denied</h2>
      <p>
        Sorry, you do not have permission to view this page. Please contact
        your administrator for assistance.
      </p>
      {/* Styled Button with Link */}
    <Link to="/home" style={{ textDecoration: "none" }}>
        <Button
          variant="contained"
          color="primary"
          style={buttonStyle}
          startIcon={<ArrowBackIcon />}
        >
          Go Back
        </Button>
      </Link>
    </div>
  );
};

export default Dashboard;
