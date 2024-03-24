import React from "react";
import CIcon from "@coreui/icons-react";
import {
  cilFeaturedPlaylist,
  cilSpeedometer,
  cilMap,
  cilPeople,
  cilTags,
  cilCog,
  cilBellExclamation,
} from "@coreui/icons";
import { CNavGroup, CNavItem } from "@coreui/react";

const _nav = [
  {
    roles: ["Delegado"],
    component: CNavItem,
    name: "Formulario",
    to: "/formStart",
    icon: <CIcon icon={cilFeaturedPlaylist} customClassName="nav-icon" />,
  },
  {
    roles: ["Admin","Analyst"],
    component: CNavItem,
    name: "Dashboard",
    to: "/dashboard",
    icon: <CIcon icon={cilSpeedometer} customClassName="nav-icon" />,
  },
  {
    roles: ["Admin","Analyst"],
    component: CNavItem,
    name: "Geográfico",
    to: "/maps",
    icon: <CIcon icon={cilMap} customClassName="nav-icon" />,
  },
  {
    roles: ["Admin","Analyst"],
    component: CNavItem,
    name: "Delegados",
    to: "/delegates",
    icon: <CIcon icon={cilPeople} customClassName="nav-icon" />,
  },
  {
    roles: ["Admin"],
    component: CNavItem,
    name: "Menú datos",
    to: "/menu-data",
    icon: <CIcon icon={cilTags} customClassName="nav-icon" />,
  },
  {
    roles: ["Admin"],
    component: CNavItem,
    name: "Menú admin",
    to: "/menu-admin",
    icon: <CIcon icon={cilCog} customClassName="nav-icon" />,
  },
  {
    roles: ["Admin"],
    component: CNavItem,
    name: "Logs",
    to: "/logs",
    icon: <CIcon icon={cilBellExclamation} customClassName="nav-icon" />,
  },
];

export default _nav;
