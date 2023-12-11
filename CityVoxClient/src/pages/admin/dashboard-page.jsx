import { Box, Button, IconButton, Typography, useTheme, Paper } from "@mui/material";
import React from "react";
import { LayoutRoot, LayoutContainer, SIDE_NAV_WIDTH } from '../home-page';
import DownloadOutlinedIcon from "@mui/icons-material/DownloadOutlined";
import PeopleIcon from '@mui/icons-material/People';
import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
import MmsIcon from '@mui/icons-material/Mms';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { SideNav } from "../../components/navigation/side-nav";
import { TopNav } from "../../components/navigation/top-nav";
import LineChart from "../../components/dashboardcomponents/line-chart";
import BarChart from "../../components/dashboardcomponents/bar-chart";
import StatBox from "../../components/dashboardcomponents/stat-box";
import MapView from "../../components/map-view";

const Dashboard = () => {
  const { palette } = useTheme();
  const [openNav, setOpenNav] = React.useState(false);
  return (
    <>

    <TopNav onNavOpen={() => setOpenNav(true)} />
       <SideNav onClose={() => setOpenNav(false)} open={openNav} />
       <LayoutRoot>
      <LayoutContainer>

    <Box m="20px">
      {/* HEADER */}
      <Box display="flex" justifyContent="space-between" alignItems="center">


        <Box>
          <Typography variant="h2">Dashboard</Typography>
      
        </Box>
      </Box>

      {/* GRID & CHARTS */}
      <Box
        display="grid"
        gridTemplateColumns="repeat(12, 1fr)"
        gridAutoRows="140px"
        gap="20px"
      >
        {/* ROW 1 */}
        <Box
          gridColumn="span 3"
          backgroundColor={palette.dashboard.cardBackground}
          display="flex"
          alignItems="center"
          justifyContent="center"
        >
          <StatBox
            title="12,361"
            subtitle="Total Members"
            progress="0.75"
            increase="+14%"
            icon={
              <PeopleIcon
                sx={{ color: palette.static.medium, fontSize: "26px" }}
              />
            }
          />
        </Box>
        <Box
          gridColumn="span 3"
          backgroundColor={palette.dashboard.cardBackground}
          display="flex"
          alignItems="center"
          justifyContent="center"
        >
          <StatBox
            title="431,225"
            subtitle="Total Requests"
            progress="0.50"
            increase="+50%"
            icon={
              <FormatListBulletedIcon
                sx={{ color:  palette.static.medium, fontSize: "26px" }}
              />
            }
          />
        </Box>
        <Box
          gridColumn="span 3"
          backgroundColor={palette.dashboard.cardBackground}
          display="flex"
          alignItems="center"
          justifyContent="center"
        >
          <StatBox
            title="32,441"
            subtitle="Total Posts"
            progress="0.30"
            increase="+5%"
            icon={
              <MmsIcon
                sx={{ color:  palette.static.medium, fontSize: "26px" }}
              />
            }
          />
        </Box>
        <Box
          gridColumn="span 3"
          backgroundColor={palette.dashboard.cardBackground}
          display="flex"
          alignItems="center"
          justifyContent="center"
        >
          <StatBox
            title="1,325,134"
            subtitle="Others"
            progress="0.80"
            increase="+43%"
            icon={
              <VisibilityIcon
                sx={{ color:  palette.static.medium, fontSize: "26px" }}
              />
            }
          />
        </Box>

        {/* ROW 2 */}
        <Box
          gridColumn="span 8"
          gridRow="span 2"
          backgroundColor={palette.dashboard.cardBackground}
        >
          <Box
            mt="25px"
            p="0 30px"
            display="flex "
            justifyContent="space-between"
            alignItems="center"
          >
            <Box>
              <Typography
                variant="h5"
                fontWeight="600"
                color={palette.dashboard.cardTextPrimary}
              >
                Total Reports
              </Typography>
              <Typography
                variant="h3"
                fontWeight="bold"
                color={palette.dashboard.cardTextNumbers}
              >
                Monthly scale
              </Typography>
            </Box>
          </Box>
          <Box height="250px" m="-20px 0 0 0">
            <LineChart isDashboard={true} />
          </Box>
        </Box>
      

        {/* ROW 3 */}
        <Box
          gridColumn="span 4"
          gridRow="span 4"
          backgroundColor={palette.dashboard.cardBackground}
          p="30px"
        >
          <Typography variant="h5" fontWeight="600">
            Recently Added
          </Typography>
          <Box
            display="flex"
            flexDirection="column"
            alignItems="center"
            mt="25px"
          >
          
          </Box>
        </Box>
        <Box
          gridColumn="span 4"
          gridRow="span 2"
          backgroundColor={palette.dashboard.cardBackground}
        >
          <Typography
            variant="h5"
            fontWeight="600"
            sx={{ padding: "30px 30px 0 30px" }}
          >
            Idk bro
          </Typography>
          <Box height="250px" mt="20px">
            <img src="https://fpmi.bg/cms/wp-content/uploads/2018/02/al_petkov.jpg" alt="Sashko savage" style={{ width: '100%', height: '90%', objectFit: 'cover' }}/>
            
          </Box>
        </Box>
        <Box
          gridColumn="span 4"
          gridRow="span 2"
          backgroundColor={palette.dashboard.cardBackground}
          padding="30px"
        >
          <Typography
            variant="h5"
            fontWeight="600"
            sx={{ marginBottom: "15px" }}
          >
            Geographic Map
          </Typography>

          <Box height="200px">
          <Paper elevation={3} sx={{ p: 1, height: '200px', cursor:'pointer' }}>
        <MapView />
      </Paper>
          </Box>
        </Box>
      </Box>
    </Box>
    </LayoutContainer>
    </LayoutRoot>
    </>
  );
};

export default Dashboard;