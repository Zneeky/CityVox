import { useLocation } from "react-router-dom";
import PropTypes from "prop-types";
import ArrowTopRightOnSquareIcon from "@heroicons/react/24/solid/ArrowTopRightOnSquareIcon";
import {
  Box,
  Button,
  Divider,
  Drawer,
  Stack,
  SvgIcon,
  Typography,
  useMediaQuery,
  Link as LinkMui
} from "@mui/material";
import { Logo } from "../styling/Logo";
import { Scrollbar } from "../styling/scroll-bar";
import { items } from "./configuration";
import { SideNavItem } from "./side-nav-item";
import ThreeScene from "../planet/planet-component";

export const SideNav = (props) => {
  const { open, onClose } = props;
  const pathname = useLocation();
  const lgUp = useMediaQuery((theme) => theme.breakpoints.up("lg"));
  const itemsArray = items();
  


  const content = (
    <Scrollbar
      sx={{
        height: "100%",
        "& .simplebar-content": {
          height: "100%",
        },
        "& .simplebar-placeholder":{
          display: "none"
        },
        "& .simplebar-scrollbar:before": {
          background: "neutral.400",
        },
      }}
    >
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          height: "100%",
        }}
      >
        <Box sx={{ p: 3 }}>
          <LinkMui
            to="/"
            style={{
              display: "inline-flex",
              height: 32,
              width: 32,
            }}
          >
            <Logo />
          </LinkMui>
          <Box
            sx={{
              alignItems: "center",
              backgroundColor: "rgba(255, 255, 255, 0.04)",
              borderRadius: 1,
              cursor: "pointer",
              display: "flex",
              justifyContent: "space-between",
              mt: 2,
              p: "12px",
            }}
          >
            <div>
              <Typography color="inherit" variant="subtitle1">
                CityVox
              </Typography>
              <Typography color="neutral.400" variant="body2">
                AATechnology™
              </Typography>
            </div>
            {/* <SvgIcon fontSize="small" sx={{ color: "neutral.500" }}>
              <ChevronUpDownIcon />
            </SvgIcon> */}
          </Box>
        </Box>
        <Divider sx={{ borderColor: "neutral.700" }} />
        <Box
          component="nav"
          sx={{
            flexGrow: 1,
            px: 2,
            py: 3,
          }}
        >
          <Stack
            component="ul"
            spacing={0.5}
            sx={{
              listStyle: "none",
              p: 0,
              m: 0,
            }}
          >
            {itemsArray.map((item) => {
              const active = item.path ? pathname.pathname === item.path : false;

              return (
                <SideNavItem
                  active={active}
                  disabled={item.disabled}
                  external={item.external}
                  icon={item.icon}
                  key={item.title}
                  path={item.path}
                  title={item.title}
                />
              );
            })}
          </Stack>
        </Box>
        <Divider sx={{ borderColor: "neutral.700" }} />
        <Box
          sx={{
            px: 2,
            py: 3,
          }}
        >
          <Typography color="neutral.100" variant="subtitle2">
            Better world together, step by step!
          </Typography>
          {/* <Typography color="neutral.400" variant="body2">
            Open for hiring.
          </Typography> */}
          {/* <Typography color="neutral.500" variant="body2">
            Check out my GitHub!
          </Typography> */}
          <Box
            sx={{
              display: "flex",
              mt: 2,
              mx: "auto",
              width: "160px",
              "& img": {
                width: "100%",
                height: "170px"
              },
            }}
          >
            <ThreeScene/>
           
          </Box>
          <Button
            component="a"
            endIcon={
              <SvgIcon fontSize="small">
                <ArrowTopRightOnSquareIcon />
              </SvgIcon>
            }
            fullWidth
            href="https://github.com/Zneeky"
            sx={{ mt: 2 }}
            target="_blank"
            variant="contained"
          >
            Zneeky
          </Button>
        </Box>
      </Box>
    </Scrollbar>
  );

  if (lgUp) {
    return (
      <Drawer
        anchor="left"
        open
        PaperProps={{
          sx: {
            backgroundColor: "neutral.800",
            color: "common.white",
            width: 280,
            overflowY: 'hidden',
          },
        }}
        variant="permanent"
      >
        {content}
      </Drawer>
    );
  }

  return (
    <Drawer
      anchor="left"
      onClose={onClose}
      open={open}
      PaperProps={{
        sx: {
          backgroundColor: "neutral.800",
          color: "common.white",
          width: 280,
          overflowY: 'hidden',
        },
      }}
      sx={{ zIndex: (theme) => theme.zIndex.appBar + 100 }}
      variant="temporary"
    >
      {content}
    </Drawer>
  );
};

SideNav.propTypes = {
  onClose: PropTypes.func,
  open: PropTypes.bool,
};
