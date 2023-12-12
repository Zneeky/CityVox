import React from "react";
import { Paper, Typography } from "@mui/material";

function formatDate(dateString) {
  const now = new Date();
  const date = new Date(dateString);

  const timeDifference = now - date;
  const seconds = Math.floor(timeDifference / 1000);
  const minutes = Math.floor(seconds / 60);
  const hours = Math.floor(minutes / 60);
  const days = Math.floor(hours / 24);

  if (days > 0) {
    return `${days} day${days > 1 ? "s" : ""} ago`;
  }
  if (hours > 0) {
    return `${hours} day${hours > 1 ? "s" : ""} ago`;
  }
  if (minutes > 0) {
    return `${minutes} day${minutes > 1 ? "s" : ""} ago`;
  }
  if (seconds > 0) {
    return `${seconds} day${seconds > 1 ? "s" : ""} ago`;
  }
}

const NotificationForm = ({ title, dateTime, isRead, handleClick }) => {
  const handleClickNotification = () => {
    handleClick();
  };
  return (
    <Paper
      elevation={3}
      sx={{
        padding: 2,
        backgroundColor: isRead ? "white" : "#E3F2FD",
        borderLeft: isRead ? "4px slid transperant" : "4px solid #1976D2",
        width: "100%",
      }}
      onClick={handleClickNotification}
    >
      <Typography variant="h6" color={isRead ? "textPrimary" : "primary"}>
        {title}
      </Typography>
      <Typography variant="body2" color={"#616161"} textAlign={"right"}>
        {formatDate(dateTime)}
      </Typography>
    </Paper>
  );
};

export default NotificationForm;
