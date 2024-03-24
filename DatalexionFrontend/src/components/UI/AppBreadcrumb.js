import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import routes from "../../routes";
import { CBreadcrumb, CBreadcrumbItem } from "@coreui/react";

// Redux imports
import { useSelector } from "react-redux";

import Clock from "../clock/Clock"; // Importa el componente Clock

import classes from "./AppBreadcrumb.module.css";

const AppBreadcrumb = (props) => {
  //#region Consts ***********************************

  const navigate = useNavigate();

  // Redux get
  const circuitDDLIsVisible = useSelector(
    (state) => state.ui.circuitDDLIsVisible.status
  );
  const provinceDDLIsVisible = useSelector(
    (state) => state.ui.provinceDDLIsVisible.status
  );

  const isMobile = useSelector((state) => state.auth.isMobile);

  const currentLocation = useLocation().pathname;

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  // Función para manejar el clic en Home
  const handleHomeClick = () => {
    navigate("/dashboard"); // Asegúrate de que la ruta '/dashboard' esté definida en tus rutas
  };

  const getRouteName = (pathname, routes) => {
    const currentRoute = routes.find((route) => route.path === pathname);
    return currentRoute ? currentRoute.name : false;
  };

  const getBreadcrumbs = (location) => {
    const breadcrumbs = [];
    location.split("/").reduce((prev, curr, index, array) => {
      const currentPathname = `${prev}/${curr}`;
      const routeName = getRouteName(currentPathname, routes);
      routeName &&
        breadcrumbs.push({
          pathname: currentPathname,
          name: routeName,
          active: index + 1 === array.length ? true : false,
        });
      return currentPathname;
    });
    return breadcrumbs;
  };

  const breadcrumbs = getBreadcrumbs(currentLocation);

  //#endregion Functions ***********************************

  return (
    <div className="d-flex justify-content-between align-items-center w-100">
      {!isMobile && (
        <CBreadcrumb className={`m-0 ms-2 ${classes.CBreadcrumb}`}>
          <CBreadcrumbItem onClick={handleHomeClick}>Home</CBreadcrumbItem>
          {breadcrumbs.map((breadcrumb, index) => {
            return (
              <CBreadcrumbItem
                {...(breadcrumb.active
                  ? { active: true }
                  : { href: breadcrumb.pathname })}
                key={index}
              >
                {breadcrumb.name}
              </CBreadcrumbItem>
            );
          })}
        </CBreadcrumb>
      )}
      {circuitDDLIsVisible && (
        <div style={{ margin: "auto" }}>{props.children}</div>
      )}
      {provinceDDLIsVisible && (
        <div style={{ margin: "auto" }}>{props.children}</div>
      )}

      {!isMobile && <Clock />}
    </div>
  );
};

export default React.memo(AppBreadcrumb);
