import CogIcon from "@heroicons/react/24/solid/CogIcon";
import LockClosedIcon from "@heroicons/react/24/solid/LockClosedIcon";
import UserIcon from "@heroicons/react/24/solid/UserIcon";
import UserPlusIcon from "@heroicons/react/24/solid/UserPlusIcon";
import UsersIcon from "@heroicons/react/24/solid/UsersIcon";
import XCircleIcon from "@heroicons/react/24/solid/XCircleIcon";
import GlobeAltIcon from "@heroicons/react/24/solid/GlobeAltIcon"
import SignalIcon from "@heroicons/react/24/solid/SignalIcon";
import FireIcon from "@heroicons/react/24/solid/FireIcon";
import BuildingOffice2Icon from "@heroicons/react/24/solid/BuildingOffice2Icon"
import CubeIcon from "@heroicons/react/24/solid/CubeIcon"
import RectangleStackIcon from "@heroicons/react/24/solid/RectangleStackIcon"
import UserGroupIcon from "@heroicons/react/24/solid/UserGroupIcon"

import { useSelector } from "react-redux";

import { SvgIcon } from "@mui/material";

export const items = () => {
  const user = useSelector((state) => state.user);
  let itemsArray = [
    {
      title: "Map",
      path: "/home",
      icon: (
        <SvgIcon fontSize="small">
          <GlobeAltIcon />
        </SvgIcon>
      ),
    },
    {
      title: "CityVox",
      path: "/posts",
      icon: (
        <SvgIcon fontSize="small">
          <UsersIcon />
        </SvgIcon>
      ),
    },
    {
      title: "Report",
      path: "/report",
      icon: (
        <SvgIcon fontSize="small">
          <SignalIcon />
        </SvgIcon>
      ),
    },
    {
      title: "Emergency",
      path: "/emergency",
      icon: (
        <SvgIcon fontSize="small">
          <FireIcon />
        </SvgIcon>
      ),
    },
    {
      title: "Infrastructure",
      path: "/infrastructure_issue",
      icon: (
        <SvgIcon fontSize="small">
          <BuildingOffice2Icon />
        </SvgIcon>
      ),
    },
    {
      title: 'Account',
      path: '/account',
      icon: (
        <SvgIcon fontSize="small">
          <UserIcon />
        </SvgIcon>
      )
    },
    {
      title: "Settings",
      path: "/settings",
      icon: (
        <SvgIcon fontSize="small">
          <CogIcon />
        </SvgIcon>
      ),
    }
  ];
  if (user === null) {
    itemsArray = [
      ...itemsArray,
      {
        title: "Login",
        path: "/auth/login",
        icon: (
          <SvgIcon fontSize="small">
            <LockClosedIcon />
          </SvgIcon>
        ),
      },
      {
        title: "Register",
        path: "/auth/register",
        icon: (
          <SvgIcon fontSize="small">
            <UserPlusIcon />
          </SvgIcon>
        ),
      },
      // {
      //   title: "Error",
      //   path: "/404",
      //   icon: (
      //     <SvgIcon fontSize="small">
      //       <XCircleIcon />
      //     </SvgIcon>
      //   ),
      // },
    ];
  }

  if (user && user.role === "admin") {
    itemsArray = [
      ...itemsArray,
      {
        title: "Dashboard",
        path: "/admin/dashboard",
        icon: (
          <SvgIcon fontSize="small">
            <CubeIcon />
          </SvgIcon>
        ),
      },
      {
        title: "Requests",
        path: "/admin/requests",
        icon: (
          <SvgIcon fontSize="small">
            <RectangleStackIcon />
          </SvgIcon>
        ),
      },
      {
        title: "Manage Users",
        path: "/admin/manage_users",
        icon: (
          <SvgIcon fontSize="small">
            <UserGroupIcon />
          </SvgIcon>
        ),
      },
    ];
  }

  return itemsArray;
};
