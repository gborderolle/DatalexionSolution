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

import { LoginGeneral } from "../../../utils/navigationPaths";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { authActions } from "../../../store/auth-slice";

import ProvinceTable from "./tables/ProvinceTable";
import MunicipalityTable from "./tables/MunicipalityTable";
import CandidateTable from "./tables/CandidateTable";
import PartyTable from "./tables/PartyTable";
import WingTable from "./tables/WingTable";
import SlateTable from "./tables/SlateTable";
import CircuitTable from "./tables/CircuitTable";
import ClientTable from "./tables/ClientTable";

const MenuData = () => {
  //#region Const ***********************************

  // redux
  const dispatch = useDispatch();

  //#region RUTA PROTEGIDA
  const navigate = useNavigate();
  const userRole = useSelector((state) => state.auth.userRole);
  useEffect(() => {
    if (userRole != USER_ROLE_ADMIN && userRole != USER_ROLE_ANALYST) {
      dispatch(authActions.logout());
      navigate(LoginGeneral);
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

  const partyData = async (name, shortName, color) => {
    return {
      name,
      shortName,
      color,
    };
  };

  const wingData = async (name, partyId) => {
    return {
      name,
      partyId,
    };
  };

  const slateData = async (name, partyId) => {
    return {
      name,
      partyId,
    };
  };

  const provinceData = async (name) => {
    return {
      name,
    };
  };

  const municipalityData = async (name, provinceId) => {
    return {
      name,
      provinceId,
    };
  };

  const candidateData = async (name, slateId) => {
    return {
      name,
      slateId,
    };
  };

  //#endregion Functions ***********************************

  return (
    <CRow>
      <CCol xs>
        <CAccordion>
          <CAccordionItem itemKey={1}>
            <CAccordionHeader className="custom-accordion-header">
              Menú mi partido (cliente)
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <ClientTable
                    title="Menú mi partido (cliente)"
                    name="Nombre"
                    party="Partido"
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
              Menú partidos políticos
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <PartyTable
                    title="Menú partidos políticos"
                    name="Nombre"
                    shortName="Nombre corto"
                    color="Color"
                    createDataToUpload={partyData}
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
              Menú departamentos
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <ProvinceTable
                    title="Menú departamentos"
                    name="Nombre"
                    createDataToUpload={provinceData}
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
              Menú municipios
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <MunicipalityTable
                    title="Menú municipios"
                    name="Nombre"
                    province="Departamento"
                    createDataToUpload={municipalityData}
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
              Menú circuitos
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <CircuitTable
                    title="Menú circuitos"
                    name="Nombre"
                    number="Número"
                    address="Dirección"
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
              Menú agrupaciones
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <WingTable
                    title="Menú agrupaciones"
                    name="Nombre"
                    party="Partido"
                    createDataToUpload={wingData}
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
              Menú listas
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <SlateTable
                    title="Menú listas"
                    name="Nombre"
                    wing="Agrupación"
                    province="Departamento"
                    candidate="Candidato"
                    createDataToUpload={slateData}
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
              Menú candidatos
            </CAccordionHeader>
            <CAccordionBody>
              <CCard>
                <CCardHeader>Tabla de datos</CCardHeader>
                <CCardBody>
                  <CandidateTable
                    title="Menú candidatos"
                    name="Nombre"
                    createDataToUpload={candidateData}
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

export default MenuData;
