import React from "react";

import { CRow, CFooter } from "@coreui/react";
import {
  cilFeaturedPlaylist,
  cilMap,
  cilChartPie,
  cilTags,
  cilCog,
  cilPeople,
} from "@coreui/icons";

import CustomButton from "./CustomButton";

import classes from "./AppFooterMobileAdmin.module.css";

const AppFooterMobileAdmin = (props) => {
  const buttonsConfig = [
    {
      icon: cilChartPie,
      color: "info",
      requiredRole: ["Admin", "Analyst"],
      path: "/dashboard",
    },
    {
      icon: cilMap,
      color: "info",
      requiredRole: ["Admin", "Analyst"],
      path: "/maps",
    },
    {
      icon: cilPeople,
      color: "info",
      requiredRole: ["Admin", "Analyst"],
      path: "/delegates",
    },
    {
      icon: cilTags,
      color: "info",
      requiredRole: ["Admin", "Analyst"],
      path: "/datos",
    },
    {
      icon: cilCog,
      color: "info",
      requiredRole: "Admin",
      path: "/admin",
    },
    {
      icon: cilFeaturedPlaylist,
      color: "info",
      requiredRole: "Delegado",
      path: "/form",
    },
  ];

  return (
    <CFooter className={`${classes.footer} ${classes.fixedFooter}`}>
      <CRow className={classes.buttonContainer}>
        {buttonsConfig.map((button, index) => (
          <CustomButton
            key={index}
            icon={button.icon}
            color="dark"
            path={button.path}
            userRole={props.userRole}
            requiredRole={button.requiredRole}
          />
        ))}
      </CRow>
    </CFooter>
  );
};

export default React.memo(AppFooterMobileAdmin);
