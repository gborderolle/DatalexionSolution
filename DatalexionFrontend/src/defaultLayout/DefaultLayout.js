import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import {
  AppContent,
  AppSidebar,
  AppFooter,
  AppFooterMobileAdmin,
  AppFooterMobileDelegados,
  AppHeader,
} from "../components/index";

// Redux import
import { authActions } from "../store/auth-slice";

import classes from "./DefaultLayout.module.css";

import { LoginGeneral } from "../utils/navigationPaths";

// Redux imports
import { batch, useDispatch, useSelector } from "react-redux";
import {
  fetchClientByUser,
  fetchPartyList,
  fetchWingList,
  fetchSlateList,
  fetchProvinceList,
  fetchMunicipalityList,
  fetchCircuitList,
  fetchCandidateListByClient,
  fetchDelegadoListByClient,
  fetchUserListByClient,
} from "../store/generalData-actions";

const DefaultLayout = () => {
  //#region Consts

  const navigate = useNavigate();

  // Redux get
  const userRole = useSelector((state) => state.auth.userRole);

  // LocalStorage get
  const isMobile = JSON.parse(localStorage.getItem("isMobile"));

  // Redux fetch DB
  const dispatch = useDispatch();

  //#endregion Consts

  //#region Hooks

  // Hook para revisar expiración del token
  useEffect(() => {
    const checkTokenExpiration = () => {
      const loggedInData = JSON.parse(localStorage.getItem("loggedInData"));
      if (!loggedInData || new Date().getTime() >= loggedInData.expiry) {
        dispatch(authActions.logout());
        navigate(LoginGeneral);
      }
    };

    const intervalId = setInterval(() => {
      checkTokenExpiration();
      // }, 300000); // 300000 ms son 5 minutos
    }, 3600000); // 3600000 ms son 1 hora

    // Limpieza al desmontar el componente
    return () => clearInterval(intervalId);
  }, []);

  // Redux fetch DB (carga inicial)
  useEffect(() => {
    if (isMobile) {
      // Aplicar estilos al montar el componente
      document.documentElement.style.fontSize = "small";
      document.body.style.fontSize = "small";
    }

    const fetchGeneralData = async () => {
      batch(() => {
        dispatch(fetchClientByUser());
        dispatch(fetchPartyList());
        dispatch(fetchWingList());
        dispatch(fetchSlateList());
        dispatch(fetchProvinceList());
        dispatch(fetchMunicipalityList());
        dispatch(fetchCircuitList());
        dispatch(fetchCandidateListByClient());
        dispatch(fetchDelegadoListByClient());
        dispatch(fetchUserListByClient());
      });
    };
    fetchGeneralData();

    // Limpiar estilos al desmontar el componente
    return () => {
      document.documentElement.style.fontSize = "";
      document.body.style.fontSize = "";
    };
  }, [dispatch]);

  //#endregion Hooks

  return (
    <div>
      {!isMobile && <AppSidebar />}
      <div className="wrapper d-flex flex-column min-vh-100 bg-light">
        <AppHeader />
        &nbsp;
        <div className="body flex-grow-1 px-3">
          <AppContent />
        </div>
        {(userRole == "Admin" || userRole == "Analyst") && !isMobile && (
          <AppFooter className={classes.AppFooter} />
        )}
        {(userRole == "Admin" || userRole == "Analyst") && isMobile && (
          <AppFooterMobileAdmin
            userRole={userRole}
            className={classes.AppFooter}
          />
        )}
        {userRole == "Delegado" && (
          <AppFooterMobileDelegados
            userRole={userRole}
            className={classes.AppFooter}
          />
        )}
      </div>
    </div>
  );
};

export default DefaultLayout;
