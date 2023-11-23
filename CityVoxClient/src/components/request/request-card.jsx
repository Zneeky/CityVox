import PropTypes from "prop-types";
import { Link } from "react-router-dom";
import ArrowDownOnSquareIcon from "@heroicons/react/24/solid/ArrowDownOnSquareIcon";
import ClockIcon from "@heroicons/react/24/solid/ClockIcon";
import {
  Avatar,
  Box,
  Card,
  CardContent,
  Divider,
  Stack,
  SvgIcon,
  Typography,
  Button,
} from "@mui/material";
import { indigo, success, warning, info } from "../../theme/colors";

import { alpha } from "@mui/material";

export const RequestCard = ({ request, type }) => {

  const getStatusColor = () => {
    if (request.Status === "Reported") {
      return `${warning.main}`; //red
    } else {
      return `${info.main}`;
    }
  };

  return (
    <Card
      sx={{
        display: "flex",
        flexDirection: "column",
        height: "100%",
      }}
    >
      <CardContent>
        <Box
          sx={{
            pb: 3,
          }}
        ><Box sx={{ pb: "5px", display: "flex", alignItems: "center" }}>
            <Typography
              sx={{
                m: "0 auto",
                p: "2px 10px",
                borderRadius: "20px",
              }}
            >
              {request.Municipality}
            </Typography>
          </Box>
          <Box sx={{ pb: "5px", display: "flex", alignItems: "center" }}>
            <Typography
              sx={{
                m: "0 auto",
                backgroundColor: alpha(getStatusColor(), 0.3),
                color: getStatusColor(),
                p: "2px 10px",
                borderRadius: "20px",
              }}
            >
              {request.Status}
            </Typography>
          </Box>
          <Box
            component="img"
            src={request.ImageUrl}
            sx={{
              display: "flex",
              justifyContent: "center",
              pb: 3,
              width: "300px",
              height: "200px",
              m: "auto",
            }}
          ></Box>
        </Box>
        <Typography align="center" gutterBottom variant="h5">
          {request.Title}
        </Typography>
        <Typography align="center" variant="body1">
          {request.Description}
        </Typography>
      </CardContent>
      <Box sx={{ flexGrow: 1 }} />
      <Divider />
      <Stack
        alignItems="center"
        direction="row"
        justifyContent="space-between"
        spacing={2}
        sx={{ p: 2 }}
      >
        <Stack alignItems="center" direction="row" spacing={1}>
          <SvgIcon color="action" fontSize="small">
            <ArrowDownOnSquareIcon />
          </SvgIcon>
          <Typography color="text.secondary" display="inline" variant="body2">
            {request.CreatorUsername}
          </Typography>
        </Stack>
        <Stack alignItems="center" direction="row" spacing={1}>
          <SvgIcon color="action" fontSize="small">
            <ClockIcon />
          </SvgIcon>
          <Typography color="text.secondary" display="inline" variant="body2">
            {request.ReportTime}
          </Typography>
        </Stack>
        <Stack alignItems="center" direction="row" spacing={1}>
          <Typography color="text.secondary" display="inline" variant="body2">
            <Link
              to={`/${type}/edit/${request.Id}`}
              style={{ textDecoration: "none", color: `${indigo.dark}` }}
            >
              Edit Status
            </Link>
          </Typography>
        </Stack>
      </Stack>
    </Card>
  );
};

RequestCard.propTypes = {
  request: PropTypes.object.isRequired,
};
