import PropTypes from "prop-types";
import BellIcon from "@heroicons/react/24/solid/BellIcon";
import UsersIcon from "@heroicons/react/24/solid/UsersIcon";
import Bars3Icon from "@heroicons/react/24/solid/Bars3Icon";
import MagnifyingGlassIcon from "@heroicons/react/24/solid/MagnifyingGlassIcon";
import SunIcon from "@heroicons/react/24/solid/SunIcon";
import MoonIcon from "@heroicons/react/24/solid/MoonIcon";

import {
  Avatar,
  Badge,
  Box,
  IconButton,
  Stack,
  SvgIcon,
  Tooltip,
  useMediaQuery,
  useTheme,
  Menu,
  MenuItem,
} from "@mui/material";
import NotificationForm from "../notification/notification-form";
import { alpha } from "@mui/material/styles";
import { usePopover } from "../../hooks/use-popover";
import { AccountPopover } from "./account-popover";
import { useDispatch, useSelector } from "react-redux";
import { setMode, setLogout } from "../../redux/index";
import { useCallback, useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { GetAllNotifications } from "../../utils/api";

const SIDE_NAV_WIDTH = 280;
const TOP_NAV_HEIGHT = 64;

// Additional logic, if needed, can be placed here

export const TopNav = (props) => {
  const { onNavOpen } = props;
  const user = useSelector((state) => state.user);
  const lgUp = useMediaQuery((theme) => theme.breakpoints.up("lg"));
  const accountPopover = usePopover();
  const dispatch = useDispatch();
  const theme = useTheme();
  const dark = theme.palette.static.dark;
  const navigate = useNavigate();

  const [userNotifications, setNotifications] = useState([]);
  const [notificationMenu, setNotificationMenu] = useState(null);

  useEffect(() => {
    const fetchNotifications = async () => {
      try {
        const notifications = await GetAllNotifications(user.id);
        setNotifications(notifications);
      } catch (error) {
        console.error("Error fetching notifications:", error);
      }
    };

    fetchNotifications();
  }, [user.id]);

  const onClickNotification = (notification) => {
    //notification.IsRead = true;
    onNotificatonsDropdownClose();
    const issueType = notification.Content.split(",")[1].trim();
    const reportId = notification.Content.split(",")[2].trim();
    navigate(`/${issueType}s/edit/${reportId}`);
  };

  const onBellClick = (event) => {
    setNotificationMenu(event.currentTarget);
  };

  const onNotificatonsDropdownClose = () => {
    setNotificationMenu(null);
  };

  return (
    <>
      <Box
        component="header"
        sx={{
          backdropFilter: "blur(6px)",
          backgroundColor: (theme) =>
            alpha(theme.palette.background.default, 0.8),
          position: "sticky",
          left: {
            lg: `${SIDE_NAV_WIDTH}px`,
          },
          top: 0,
          width: {
            lg: `calc(100% - ${SIDE_NAV_WIDTH}px)`,
          },
          zIndex: (theme) => theme.zIndex.appBar,
        }}
      >
        <Stack
          alignItems="center"
          direction="row"
          justifyContent="space-between"
          spacing={2}
          sx={{
            minHeight: TOP_NAV_HEIGHT,
            px: 2,
          }}
        >
          <Stack alignItems="center" direction="row" spacing={2}>
            {!lgUp && (
              <IconButton onClick={onNavOpen}>
                <SvgIcon fontSize="small">
                  <Bars3Icon />
                </SvgIcon>
              </IconButton>
            )}
          </Stack>
          <Stack alignItems="center" direction="row" spacing={2}>
            <Tooltip title="mode">
              <IconButton
                onClick={() => dispatch(setMode())}
                sx={{ fontSize: "20px" }}
              >
                <SvgIcon
                  sx={{
                    fontSize: "20px",
                    color: theme.palette.mode === "dark" ? "white" : `success`,
                  }}
                >
                  {theme.palette.mode === "dark" ? <SunIcon /> : <MoonIcon />}
                </SvgIcon>
              </IconButton>
            </Tooltip>
            <Tooltip title="Notifications">
              <IconButton onClick={onBellClick}>
                <Badge
                /*badgeContent={
                    /*GetAllUserUnreadNotificationsCount > 0
                      ? GetAllUserUnreadNotificationsCount
                      : null
                  }
                  color={"error"}*/
                >
                  <SvgIcon fontSize="medium">
                    <BellIcon />
                  </SvgIcon>
                </Badge>
              </IconButton>
            </Tooltip>
            {/*notifications dropdown */}
            <Menu
              anchorEl={notificationMenu}
              open={Boolean(notificationMenu)}
              onClose={onNotificatonsDropdownClose}
              PaperProps={{
                style: {
                  maxHeight: "65%",
                },
              }}
            >
              {userNotifications.length > 0 &&
                userNotifications.$values.map((notification) => (
                  <MenuItem key={notification.Id}>
                    <NotificationForm
                      title={notification.Content.split(",")[0]}
                      dateTime={notification.TimeSent}
                      isRead={notification.IsRead}
                      handleClick={() => onClickNotification(notification)}
                    />
                  </MenuItem>
                ))}
            </Menu>
            <Avatar
              onClick={accountPopover.handleOpen}
              ref={accountPopover.anchorRef}
              sx={{
                cursor: "pointer",
                height: 40,
                width: 40,
              }}
              src={`${user.pfp}`}
            />
          </Stack>
        </Stack>
      </Box>
      <AccountPopover
        anchorEl={accountPopover.anchorRef.current}
        open={accountPopover.open}
        onClose={accountPopover.handleClose}
      />
    </>
  );
};

TopNav.propTypes = {
  onNavOpen: PropTypes.func,
};
