import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import {
  CCol,
  CRow,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
  CCard,
  CCardHeader,
  CCardBody,
} from "@coreui/react";

import { USER_ROLE_ADMIN, USER_ROLE_ANALYST } from "../../../userRoles";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { authActions } from "../../../store/auth-slice";
import UserRoleTable from "./UserRoleTable";
import UserTable from "./UserTable";
import DelegadoTable from "./DelegadoTable";

const MenuAdmin = () => {
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

  const userData = async (name, username, roleId, email) => {
    return {
      name,
      username,
      roleId,
      email,
    };
  };

  const userRoleData = async (roleName) => {
    return {
      roleName,
    };
  };

  const delegadoData = async (name, CI, mobile) => {
    return {
      name,
      CI,
      mobile,
      email,
    };
  };

  //#endregion Functions ***********************************

  return (
    <CRow>
      <CCol xs>
        <CAccordion>
          <CAccordionItem itemKey={1}>
            <CAccordionHeader className="custom-accordion-header">
              Menú usuarios
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <UserTable
                    title="Menú usuarios"
                    name="Nombre"
                    username="Usuario"
                    password="Password"
                    email="Email"
                    createDataToUpload={userData}
                  />
                </CCardBody>
              </CCard>
            </CAccordionBody>
          </CAccordionItem>
        </CAccordion>
        <br />
        <CAccordion>
          <CAccordionItem itemKey={1}>
            <CAccordionHeader className="custom-accordion-header">
              Menú roles de usuario
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <UserRoleTable
                    title="Menú roles de usuario"
                    inputName="Nombre"
                    createDataToUpload={userRoleData}
                  />
                </CCardBody>
              </CCard>
            </CAccordionBody>
          </CAccordionItem>
        </CAccordion>
        <br />
        <CAccordion>
          <CAccordionItem itemKey={1}>
            <CAccordionHeader className="custom-accordion-header">
              Menú delegados
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <DelegadoTable
                    title="Menú delegados"
                    name="Nombre completo"
                    CI="Cédula (sin guión, 8 dígitos)"
                    phone="Celular (9 dígitos)"
                    province="Departamento"
                    municipality="Municipios"
                    email="Email"
                    createDataToUpload={delegadoData}
                  />
                </CCardBody>
              </CCard>
            </CAccordionBody>
          </CAccordionItem>
        </CAccordion>
        <br />
      </CCol>
    </CRow>
  );
};

export default MenuAdmin;
