import { Box, Typography, useTheme } from "@mui/material";
import ProgressCircle from "./progress-circle";

const StatBox = ({ title, subtitle, icon, progress, increase }) => {
    const { palette } = useTheme();

  return (
    <Box width="100%" m="0 30px">
      <Box display="flex" justifyContent="space-between">
        <Box>
          {icon}
          <Typography
            variant="h4"
            fontWeight="bold"
            sx={{ color: palette.dashboard.cardTextPrimary }}
          >
            {title}
          </Typography>
        </Box>
        <Box>
          <ProgressCircle progress={progress} />
        </Box>
      </Box>
      <Box display="flex" justifyContent="space-between" mt="2px">
        <Typography variant="h5" sx={{ color: palette.dashboard.cardTextNumbers }}>
          {subtitle}
        </Typography>
        <Typography
          variant="h5"
          fontStyle="italic"
          sx={{ color: palette.dashboard.cardTextNumbers }}
        >
          {increase}
        </Typography>
      </Box>
    </Box>
  );
};

export default StatBox;