import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { batch, useDispatch, useSelector } from "react-redux";
import {
  CCol,
  CRow,
  CProgress,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
  CFormCheck,
} from "@coreui/react";
import { motion } from "framer-motion";

import { USER_ROLE_ADMIN, USER_ROLE_ANALYST } from "../../../userRoles";

import { CChartPie } from "@coreui/react-chartjs";

import { LoginGeneral } from "../../../utils/navigationPaths";

// redux imports
import { authActions } from "../../../store/auth-slice";

import "./Dashboard.css";
import classesMobile from "./DashboardMobile.module.css";

import {
  calculatePercentage,
  getDynamicClassName,
  setDynamicBorderStyle,
} from "src/utils/auxiliarFunctions";

import styled from "styled-components";
const StyledProgress = styled(CProgress)`
  .c-progress-bar {
    background-color: ${(props) => props.bgColor} !important;
  }
`;

const PoliticalColParties = ({
  isExpanded,
  filteredCircuitParties,
  partyChartData,
  totalPartyVotes,
}) => {
  //#region Consts ***********************************

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

  // redux gets
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxPartyList = useSelector((state) => state.generalData.partyList);

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const getPartyById = (partyId) => {
    // Intenta encontrar el partido por ID
    const party = reduxPartyList?.find((p) => p.id === partyId);

    // Devuelve el partido encontrado o, si no se encuentra, devuelve un objeto predeterminado
    return (
      party || {
        // Usa el nombre del partido del cliente si está disponible; de lo contrario, usa "N/A"
        name:
          reduxClient && reduxClient.party && reduxClient.party.name
            ? reduxClient.party.name
            : "N/A",
        // Usa el color del partido del cliente si está disponible; de lo contrario, usa "N/A"
        color:
          reduxClient && reduxClient.party && reduxClient.party.color
            ? reduxClient.party.color
            : "N/A",
      }
    );
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  //#endregion Events ***********************************

  //#region JSX ***********************************

  const totalVotes = partyChartData?.datasets.reduce(
    (total, dataset) =>
      total + dataset.data.reduce((sum, value) => sum + value, 0),
    0
  );

  const filterVariants = {
    hidden: {
      y: -20,
      opacity: 0,
    },
    visible: {
      y: 0,
      opacity: 1,
      transition: {
        duration: 0.3,
        ease: "easeInOut",
      },
    },
  };

  const sortedParties = [...filteredCircuitParties].sort(
    (a, b) => b.totalPartyVotes - a.totalPartyVotes
  );

  //#endregion JSX ***********************************

  return (
    <CCol
      xs={12}
      sm={!isExpanded ? 6 : 7}
      md={!isExpanded ? 6 : 7}
      lg={!isExpanded ? 4 : 5}
      xl={!isExpanded ? 4 : 5}
    >
      <CAccordion activeItemKey={1}>
        <motion.div
          initial="hidden"
          animate="visible"
          variants={filterVariants}
        >
          <CAccordionItem itemKey={1}>
            <CAccordionHeader className="custom-accordion-header">
              Partidos políticos
            </CAccordionHeader>
            <CAccordionBody>
              {sortedParties ? (
                <>
                  <CRow>
                    {sortedParties &&
                      sortedParties.map((circuitParty, index) => {
                        const votePercentage = calculatePercentage(
                          circuitParty.totalPartyVotes,
                          totalPartyVotes
                        );
                        const party = getPartyById(circuitParty.partyId);
                        if (party) {
                          setDynamicBorderStyle(party.color, index, "idParty");

                          // Usar tanto partyId como slateId para formar una key compuesta
                          const key = `${circuitParty.circuitId}-${circuitParty.partyId}-${index}`;

                          return (
                            <CCol xs={6} key={key}>
                              <div
                                id={`idParty-` + index}
                                className={`border-start border-start-5 py-1 px-3 mb-3`}
                              >
                                <div
                                  className={`fs-5 fw-semibold ${classesMobile.label}`}
                                >
                                  {party.name}
                                </div>
                                <div
                                  className={`text-medium-emphasis small ${classesMobile.value}`}
                                >
                                  {`Votos: ${circuitParty.totalPartyVotes} (${votePercentage}%)`}
                                </div>
                              </div>
                            </CCol>
                          );
                        }
                      })}
                  </CRow>

                  <hr className="mt-0" />

                  {sortedParties &&
                    sortedParties.map((circuitParty, index) => {
                      const votePercentage = calculatePercentage(
                        circuitParty.totalPartyVotes,
                        totalPartyVotes
                      );
                      const party = getPartyById(circuitParty.partyId);
                      if (party) {
                        const progressBarClass = getDynamicClassName(
                          party.color
                        );

                        // Usar tanto partyId como slateId para formar una key compuesta
                        const key = `${circuitParty.circuitId}-${circuitParty.partyId}-${index}`;

                        return (
                          <div className="progress-group mb-4" key={key}>
                            <div className="progress-group-header">
                              <span>{party.name}</span>
                              <span className="ms-auto">{`Votos: ${circuitParty.totalPartyVotes} (${votePercentage}%)`}</span>
                            </div>
                            <div className="progress-group-bars">
                              <StyledProgress
                                value={votePercentage}
                                progressBarClassName={progressBarClass}
                                variant="striped"
                                animated
                              />
                            </div>
                          </div>
                        );
                      }
                    })}
                </>
              ) : (
                <span style={{ color: "blue", fontStyle: "italic" }}>
                  No hay votos aún.
                </span>
              )}
              <CAccordion activeItemKey={1}>
                <CAccordionItem itemKey={1}>
                  <CAccordionHeader className="custom-accordion-header">
                    Distribución
                  </CAccordionHeader>
                  <CAccordionBody>
                    {partyChartData &&
                    partyChartData.datasets[0] &&
                    partyChartData.datasets[0].data.length > 0 ? (
                      <CChartPie data={partyChartData} />
                    ) : (
                      <span
                        style={{
                          color: "blue",
                          fontStyle: "italic",
                        }}
                      >
                        No hay votos aún.
                      </span>
                    )}
                  </CAccordionBody>
                </CAccordionItem>
              </CAccordion>
            </CAccordionBody>
          </CAccordionItem>
        </motion.div>
      </CAccordion>
    </CCol>
  );
};

export default PoliticalColParties;
