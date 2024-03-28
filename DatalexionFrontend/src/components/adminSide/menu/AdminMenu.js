import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { CCol, CRow } from "@coreui/react";
import GroupInputDelegado from "./group/GroupInputDelegado";
import GroupInputAnalyst from "./group/GroupInputAnalyst";
import GroupInputAdmin from "./group/GroupInputAdmin";

import { USER_ROLE_ADMIN, USER_ROLE_ANALYST } from "../../../userRoles";

import {
  urlDelegado,
  urlAccount,
} from "../../../endpoints";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { authActions } from "../../../store/auth-slice";

const AdminMenu = () => {
  //#region Const ***********************************

  // redux
  const dispatch = useDispatch();

  //#region RUTA PROTEGIDA
  const navigate = useNavigate();
  const userRole = useSelector((state) => state.auth.userRole);
  useEffect(() => {
    if (userRole != USER_ROLE_ADMIN && userRole != USER_ROLE_ANALYST) {
      dispatch(authActions.logout());
      navigate("/login-general");
    }
  }, [userRole, navigate, dispatch]);
  //#endregion RUTA PROTEGIDA

  //#endregion Const ***********************************

  //#region Functions ***********************************

  const delegadoData = async (
    delegadoName,
    delegadoCI,
    delegadoPhone,
    provinceId,
    municipalityId
  ) => {
    return {
      delegadoId: 1,
      delegadoName,
      delegadoCI,
      delegadoPhone,
      delegadoProvinceId: provinceId,
      municipalityId,
      delegadoRoleNumber: 2,
    };
  };

  const analystData = async (
    userFullname,
    username,
    userPassword,
    userProvinceId
  ) => {
    return {
      userId: 1,
      userFullname,
      username,
      userPassword,
      userProvinceId,
      userRole: 3,
      userMunicipalityId: 1,
    };
  };

  const adminData = async (
    userFullname,
    username,
    userPassword,
    userProvinceId
  ) => {
    return {
      userId: 1,
      userFullname,
      username,
      userPassword,
      userProvinceId,
      userRole: 1,
      userMunicipalityId: 1,
    };
  };

  //#endregion Functions ***********************************

  return (
    <CRow>
      <CCol xs>
        <GroupInputDelegado
          title="Delegados"
          inputFullname="Nombre completo"
          inputCI="Cédula (sin guión, 8 dígitos)"
          inputPhone="Celular (9 dígitos)"
          inputMunicipality="Municipio"
          firebaseUrlName={urlDelegado}
          firebaseUrlClean={urlDelegado}
          firebaseUrlFinal={urlDelegado}
          createDataToUpload={delegadoData}
        />
        <br />
        <GroupInputAnalyst
          title="Analistas"
          inputUsername="Nombre de usuario"
          inputFullname="Nombre completo"
          inputPassword="Password"
          firebaseUrlName={urlAccount}
          firebaseUrlClean={urlAccount}
          firebaseUrlFinal={urlAccount}
          createDataToUpload={analystData}
          labelFk="Departamento"
        />
        <br />
        <GroupInputAdmin
          title="Administradores"
          inputUsername="Nombre de usuario"
          inputFullname="Nombre completo"
          inputPassword="Password"
          firebaseUrlName={urlAccount}
          firebaseUrlClean={urlAccount}
          firebaseUrlFinal={urlAccount}
          createDataToUpload={adminData}
          labelFk="Departamento"
        />
        <br />
        <br />
      </CCol>
    </CRow>
  );
};

export default AdminMenu;
