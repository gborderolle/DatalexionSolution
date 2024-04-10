import React, { useEffect } from "react";
import { Navigate, useNavigate } from "react-router-dom";

import { USER_ROLE_ADMIN, USER_ROLE_ANALYST } from "../userRoles";

// redux imports
import { useSelector, useDispatch } from "react-redux";

export default function RedirectHome() {
  // redux
  const dispatch = useDispatch();

  //#region REDIRIGIR RUTA
  const navigate = useNavigate();
  const userRole = useSelector((state) => state.auth.userRole);
  useEffect(() => {
    if (userRole === USER_ROLE_ADMIN || userRole === USER_ROLE_ANALYST) {
      // dispatch(authActions.logout());
      navigate("/dashboard");
    } else {
      navigate("/formStart");
    }
  }, [userRole, navigate, dispatch]);
  //#endregion REDIRIGIR RUTA

  return <Navigate to="/" />;
}
