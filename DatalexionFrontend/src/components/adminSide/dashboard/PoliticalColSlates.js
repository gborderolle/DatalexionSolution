import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
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
  getRandomColor,
  getDynamicClassName,
  setDynamicBorderStyle,
} from "src/utils/auxiliarFunctions";

import styled from "styled-components";
const StyledProgress = styled(CProgress)`
  .c-progress-bar {
    background-color: ${(props) => props.bgColor} !important;
  }
`;

const PoliticalColSlates = ({
  isExpanded,
  filteredCircuitParties,
  filteredCircuitSlates,
  slateChartData,
  slateColors,
  totalSelectedVotes,
}) => {
  //#region Consts ***********************************

  const [isRelativePercentage, setIsRelativePercentage] = useState(true);

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
  const reduxSlateList = useSelector((state) => state.generalData.slateList);
  const reduxWingList = useSelector((state) => state.generalData.wingList);

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    if (isRelativePercentage && filteredCircuitParties) {
      const partyData = filteredCircuitParties.find(
        (party) => party.partyId === reduxClient.party?.id
      );
      totalVotesOfSelectedParty = partyData ? partyData.votes : 0;
    }
  }, [isRelativePercentage, filteredCircuitParties, reduxClient.party?.id]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const getSlateById = (slateId) => {
    return reduxSlateList?.find((p) => p.id === slateId);
  };

  const getWingBySlateId = (slateId) => {
    // Primero, encuentra la Slate usando el slateId proporcionado
    const slate = reduxSlateList?.find((slate) => slate.id === slateId);

    // Si la Slate existe y tiene un wingId asociado, busca el Wing correspondiente
    if (slate && slate.wingId) {
      // Encuentra el Wing en reduxWingList usando el wingId de la Slate
      const wing = reduxWingList?.find((wing) => wing.id === slate.wingId);

      // Retorna el Wing encontrado
      return wing;
    }
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const handleRelativePercentageChange = (e) => {
    setIsRelativePercentage(e.target.checked);
  };

  //#endregion Events ***********************************

  //#region JSX ***********************************

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

  let totalVotesOfSelectedParty = 0;

  if (isRelativePercentage) {
    // Asumiendo que filteredCircuitParties ya contiene la información de los partidos y sus votos
    // para el circuito seleccionado.
    const partyData = filteredCircuitParties.find(
      (party) => party.partyId === reduxClient.party?.id
    );
    totalVotesOfSelectedParty = partyData ? partyData.votes : 0;
  }

  const sortedSlates = [...filteredCircuitSlates].sort(
    (a, b) => b.votes - a.votes
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
              Listas del partido
              <div className="float-end">
                <CFormCheck
                  type="checkbox"
                  id="relativePercentage"
                  label={<span style={{ marginLeft: "5px" }}>Relativo</span>}
                  checked={isRelativePercentage}
                  onChange={handleRelativePercentageChange}
                  onClick={(e) => e.stopPropagation()} // Detiene la propagación aquí
                  style={{
                    display: "inline-block",
                    marginLeft: "10px",
                  }}
                />
              </div>
              <br />
            </CAccordionHeader>
            <CAccordionBody>
              {sortedSlates ? (
                <>
                  <CRow>
                    {sortedSlates &&
                      sortedSlates.map((circuitSlate, index) => {
                        const votePercentage = isRelativePercentage
                          ? calculatePercentage(
                              circuitSlate.votes,
                              totalVotesOfSelectedParty
                            )
                          : calculatePercentage(
                              circuitSlate.votes,
                              totalSelectedVotes
                            );

                        const slateColor =
                          slateColors[circuitSlate.slateId] || getRandomColor();
                        setDynamicBorderStyle(slateColor, index, "idSlate");

                        // Usar tanto partyId como slateId para formar una key compuesta
                        const key = `${circuitSlate.circuitId}-${circuitSlate.slateId}-${index}`;

                        return (
                          <CCol xs={6} key={key}>
                            <div
                              id={`idSlate-` + index}
                              className={`border-start border-start-5 py-1 px-3 mb-3`}
                            >
                              <div
                                className={`fs-5 fw-semibold ${classesMobile.label}`}
                              >
                                {getSlateById(circuitSlate.slateId).name}
                              </div>
                              <div
                                className={`text-medium-emphasis small ${classesMobile.value}`}
                              >
                                {`Votos: ${circuitSlate.votes} (${votePercentage}%)`}
                              </div>
                            </div>
                          </CCol>
                        );
                      })}
                  </CRow>

                  <hr className="mt-0" />

                  {sortedSlates &&
                    sortedSlates.map((circuitSlate, index) => {
                      const votePercentage = isRelativePercentage
                        ? calculatePercentage(
                            circuitSlate.votes,
                            totalVotesOfSelectedParty
                          )
                        : calculatePercentage(
                            circuitSlate.votes,
                            totalSelectedVotes
                          );

                      const slateColor =
                        slateColors[circuitSlate.slateId] || getRandomColor();
                      const progressBarClass = getDynamicClassName(slateColor);

                      // Usar tanto partyId como slateId para formar una key compuesta
                      const key = `${circuitSlate.circuitId}-${circuitSlate.slateId}-${index}`;
                      const wing = getWingBySlateId(circuitSlate.slateId);
                      const slate = getSlateById(circuitSlate.slateId);

                      return (
                        <div className="progress-group mb-4" key={key}>
                          <div className="progress-group-header">
                            <span>{`${wing?.name}: ${slate?.name}`}</span>
                            <span className="ms-auto">{`Votos: ${circuitSlate.votes} (${votePercentage}%)`}</span>
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
                    {slateChartData &&
                    slateChartData.datasets[0] &&
                    slateChartData.datasets[0].data.length > 0 ? (
                      <CChartPie data={slateChartData} />
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

export default PoliticalColSlates;
