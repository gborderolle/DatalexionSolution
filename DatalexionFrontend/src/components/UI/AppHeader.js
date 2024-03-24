import React, { useState } from "react";
import { NavLink } from "react-router-dom";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { liveSettingsActions } from "../../store/liveSettings-slice";

import {
  CContainer,
  CHeader,
  CHeaderDivider,
  CHeaderNav,
  CHeaderToggler,
  CNavLink,
  CNavItem,
  CImage,
  CBadge,
} from "@coreui/react";
import CIcon from "@coreui/icons-react";
import { cilMenu } from "@coreui/icons";

import { AppBreadcrumb } from "../index";
import { AppHeaderDropdown } from "../header/index";

import logoSmall from "src/assets/images/datalexion-logo.png";
import "./AppHeader.css";

const AppHeader = () => {
  //#region Consts ***********************************

  // redux get
  const dispatch = useDispatch();

  // Redux get
  const circuitNameIsVisible = useSelector(
    (state) => state.ui.circuitNameIsVisible.status
  );
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );

  // Redux get
  const sidebarShow = useSelector((state) => state.oldState.sidebarShow);
  const userRole = useSelector((state) => state.auth.userRole);
  const username = useSelector((state) => state.auth.username);

  // LocalStorage get
  const isMobile = JSON.parse(localStorage.getItem("isMobile"));

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  //#endregion Functions ***********************************

  //#region JSX ***********************************

  const headerStyle = isMobile
    ? {
        "--cui-header-bg": "#697588",
        color: "whitesmoke",
      }
    : {};

  const iconStyle = isMobile
    ? {
        "--ci-primary-color": "whitesmoke",
      }
    : {};

  //#endregion JSX ***********************************

  return (
    <CHeader position="sticky" className="mb-1" style={headerStyle}>
      <CContainer fluid>
        {isMobile ? (
          <CHeaderToggler
            className="ps-1"
            onClick={() => dispatch({ type: "set", sidebarShow: !sidebarShow })}
            style={iconStyle}
          >
            <CImage fluid src={logoSmall} width={70} />
          </CHeaderToggler>
        ) : (
          <CHeaderToggler
            className="ps-1"
            onClick={() => dispatch({ type: "set", sidebarShow: !sidebarShow })}
            style={iconStyle}
          >
            <CIcon icon={cilMenu} size="lg" />
          </CHeaderToggler>
        )}

        {circuitNameIsVisible && reduxSelectedCircuit && (
          <div
            style={{ position: "absolute", width: "-webkit-fill-available" }}
          >
            <CBadge
              color="dark"
              className={`badgeWordWrap`}
              style={{
                position: "absolute",
                top: "50%",
                left: "50%",
                transform: "translate(-50%, -50%)",
              }}
            >
              #{reduxSelectedCircuit.number}: {reduxSelectedCircuit.name}
            </CBadge>
          </div>
        )}

        <CHeaderNav className="d-none d-md-flex me-auto">
          {userRole != "Admin" && userRole != "Analyst" && (
            <CNavItem>
              <CNavLink to="/form" component={NavLink}>
                Formulario
              </CNavLink>
            </CNavItem>
          )}
          {(userRole == "Admin" || userRole == "Analyst") && (
            <CNavItem>
              <CNavLink to="/dashboard" component={NavLink}>
                Dashboard
              </CNavLink>
            </CNavItem>
          )}
        </CHeaderNav>

        <CHeaderNav className="ms-3">
          {!isMobile && <span className="usernameStyle">{username}</span>}
          <AppHeaderDropdown />
        </CHeaderNav>
      </CContainer>
      {!isMobile && <CHeaderDivider />}
      {!isMobile && (
        <CContainer fluid>
          <AppBreadcrumb />
        </CContainer>
      )}
    </CHeader>
  );
};

export default AppHeader;
