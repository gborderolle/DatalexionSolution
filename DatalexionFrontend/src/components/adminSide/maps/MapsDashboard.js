import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import {
  CCard,
  CCardBody,
  CCol,
  CCardHeader,
  CRow,
  CPagination,
  CPaginationItem,
  CProgress,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
} from "@coreui/react";
import { motion } from "framer-motion";

import { USER_ROLE_ADMIN, USER_ROLE_ANALYST } from "../../../userRoles";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRefresh } from "@fortawesome/free-solid-svg-icons";

import useBumpEffect from "../../../utils/useBumpEffect";
import MapsDashboardFilter from "./MapsDashboardFilter";

// redux imports
import { batch, useDispatch, useSelector } from "react-redux";
import { authActions } from "../../../store/auth-slice";
import {
  fetchPartyList,
  fetchProvinceList,
  fetchMunicipalityList,
  fetchCircuitList,
  fetchSlateList,
  fetchCandidateList,
} from "../../../store/generalData-actions";

import CircuitMap from "./CircuitMap";

import "./MapsDashboard.css";

const MapsDashboard = () => {
  //#region Consts ***********************************

  const [selectedProvince, setSelectedProvince] = useState(null);
  const [selectedMunicipality, setSelectedMunicipality] = useState(null);
  const [selectedCircuit, setSelectedCircuit] = useState(null);

  const [isExpanded, setIsExpanded] = useState(false);

  // Pagination ***********************************

  const [currentPageProgressbar, setCurrentPageProgressbar] = useState(1);
  const itemsPerPageProgressbar = 5;
  const [pageCountProgressbar, setPageCountProgressbar] = useState(0);

  // Pagination ***********************************

  const [isBumped, triggerBump] = useBumpEffect();

  const [completedCircuits, setCompletedCircuits] = useState(0);
  const [filterType, setFilterType] = useState("todos");

  const [activeKey, setActiveKey] = useState(null);
  const isMobile = JSON.parse(localStorage.getItem("isMobile"));

  // redux init
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

  // redux gets
  const reduxWingList = useSelector((state) => state.generalData.wingList);
  const reduxProvinceList = useSelector(
    (state) => state.generalData.provinceList
  );

  const reduxMunicipalityList = useSelector(
    (state) => state.generalData.municipalityList
  );

  const [circuitList, setCircuitList] = useState([]);
  const reduxCircuitList = useSelector(
    (state) => state.generalData.circuitList
  );

  const dropdownVariants = {
    open: {
      opacity: 1,
      height: "auto",
      transition: { duration: 0.5, type: "spring", stiffness: 120 },
    },
    closed: { opacity: 0, height: 0, transition: { duration: 0.5 } },
  };

  const getFilteredCircuitList = () => {
    switch (filterType) {
      case "completados":
        return circuitList?.filter(
          (circuit) =>
            circuit.step1completed &&
            circuit.step2completed &&
            circuit.step3completed
        );
      case "sinCompletar":
        return circuitList?.filter(
          (circuit) =>
            !(
              circuit.step1completed &&
              circuit.step2completed &&
              circuit.step3completed
            )
        );
      default:
        return circuitList;
    }
  };

  const currentFilteredCircuitList = getFilteredCircuitList();

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    setActiveKey(isMobile ? null : 1); // Ajusta el '1' al key del acordeón que quieres abierto por defecto en modo no móvil
  }, [isMobile]);

  // Pagination Progressbar ***********************************

  useEffect(() => {
    setPageCountProgressbar(
      Math.ceil(reduxProvinceList.length / itemsPerPageProgressbar)
    );
  }, [reduxProvinceList]);

  // Pagination ***********************************

  // redux gets
  useEffect(() => {
    setCircuitList(reduxCircuitList);
  }, [reduxMunicipalityList, reduxCircuitList]);

  useEffect(() => {
    // Cuando cambia la provincia seleccionada, se limpian los estados de municipio y circuito seleccionados.
    setSelectedMunicipality(null);
    setSelectedCircuit(null);
  }, [selectedProvince]); // La dependencia es selectedProvince para que este efecto se ejecute solo cuando cambie.

  // Efecto para actualizar municipios cuando cambia la provincia seleccionada
  useEffect(() => {
    const filteredMunicipalities = selectedProvince
      ? reduxMunicipalityList?.filter(
          (municipality) => municipality.provinceId === selectedProvince.id
        )
      : [];
    // No resetear municipio o circuito aquí
  }, [selectedProvince, reduxMunicipalityList]);

  // Actualiza la lista de circuitos basada en la provincia o municipio seleccionado
  useEffect(() => {
    let filteredCircuits;
    if (selectedMunicipality) {
      filteredCircuits = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
    } else if (selectedProvince) {
      const municipalitiesInProvince = reduxMunicipalityList?.filter(
        (municipality) => municipality.provinceId === selectedProvince.id
      );
      filteredCircuits = reduxCircuitList?.filter((circuit) =>
        municipalitiesInProvince?.some(
          (municipality) => municipality.id === circuit.municipalityId
        )
      );
    } else {
      filteredCircuits = [...reduxCircuitList]; // Mantiene todos los circuitos si no hay selección
    }
    setCircuitList(filteredCircuits);
  }, [
    selectedProvince,
    selectedMunicipality,
    reduxCircuitList,
    reduxMunicipalityList,
  ]);

  useEffect(() => {
    let filtered = [];

    if (selectedCircuit) {
      // Si hay un circuito seleccionado, crea una lista que contenga solo ese circuito
      filtered = [selectedCircuit];
    } else if (selectedMunicipality) {
      // Filtra los circuitos basados en el municipio seleccionado
      filtered = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
    } else if (selectedProvince) {
      // Primero, obtiene todos los municipios que pertenecen a la provincia seleccionada
      const municipalitiesInProvince = reduxMunicipalityList?.filter(
        (municipality) => municipality.provinceId === selectedProvince.id
      );

      // Luego, filtra los circuitos basados en los municipios de la provincia seleccionada
      filtered = reduxCircuitList?.filter((circuit) =>
        municipalitiesInProvince?.some(
          (municipality) => municipality.id === circuit.municipalityId
        )
      );
    } else {
      // Muestra todos los circuitos si no hay provincia ni municipio seleccionado
      filtered = [...reduxCircuitList];
    }

    setCircuitList(filtered);
  }, [
    selectedProvince,
    selectedMunicipality,
    selectedCircuit,
    reduxCircuitList,
    reduxMunicipalityList,
  ]);

  // Calcula el total de circuitos y los completados
  useEffect(() => {
    const totalCompleted = reduxCircuitList?.filter(
      (circuit) =>
        circuit.step1completed &&
        circuit.step2completed &&
        circuit.step3completed
    ).length;
    setCompletedCircuits(totalCompleted);
  }, [reduxCircuitList]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  // Utilidad para el cálculo del porcentaje.
  const calculatePercentage = (partialValue, totalValue) => {
    if (totalValue === 0) {
      return 0; // O cualquier valor que consideres apropiado para divisiones por cero
    }
    return Math.round((partialValue / totalValue) * 100);
  };

  const municipalityMap = new Map();
  reduxMunicipalityList.forEach((municipality) => {
    if (!municipalityMap.has(municipality.provinceId)) {
      municipalityMap.set(municipality.provinceId, []);
    }
    municipalityMap.get(municipality.provinceId).push(municipality);
  });

  const circuitMap = new Map();
  reduxCircuitList.forEach((circuit) => {
    if (!circuitMap.has(circuit.municipalityId)) {
      circuitMap.set(circuit.municipalityId, []);
    }
    circuitMap.get(circuit.municipalityId).push(circuit);
  });

  const getCircuitsInProvince = (province) => {
    const municipalitiesInProvince = municipalityMap.get(province.id) || [];
    return municipalitiesInProvince.reduce((circuits, municipality) => {
      const circuitsInMunicipality = circuitMap.get(municipality.id) || [];
      return circuits.concat(circuitsInMunicipality);
    }, []);
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  //#region Pagination Progressbar ***********************************

  const handlePageChangeProgressbar = (pageNumber) => {
    setCurrentPageProgressbar(pageNumber);
  };

  //#endregion Pagination ***********************************

  const bumpHandler = () => {
    triggerBump();

    const fetchGeneralData = async () => {
      batch(() => {
        dispatch(fetchPartyList());
        dispatch(fetchProvinceList());
        dispatch(fetchMunicipalityList());
        dispatch(fetchCircuitList());
        dispatch(fetchSlateList());
        dispatch(fetchCandidateList());
      });
    };
    fetchGeneralData();
  };

  //#endregion Events ***********************************

  //#region JSX ***********************************

  // Calcula el porcentaje de progreso
  const totalCircuits = reduxCircuitList.length;
  const progressPercentage = calculatePercentage(
    completedCircuits,
    totalCircuits
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

  // Pagination Progressbar ***********************************
  // Determinar el rango de páginas a mostrar alrededor de la página actual
  let pagesToShowProgressbar = 3; // Ajusta este número según sea necesario
  let startPageProgressbar = Math.max(
    currentPageProgressbar - Math.floor(pagesToShowProgressbar / 2),
    1
  );
  let endPageProgressbar = Math.min(
    startPageProgressbar + pagesToShowProgressbar - 1,
    pageCountProgressbar
  );

  if (endPageProgressbar - startPageProgressbar + 1 < pagesToShowProgressbar) {
    startPageProgressbar = Math.max(
      endPageProgressbar - pagesToShowProgressbar + 1,
      1
    );
  }

  let indexOfLastItemProgressbar =
    currentPageProgressbar * itemsPerPageProgressbar;
  let indexOfFirstItemProgressbar =
    indexOfLastItemProgressbar - itemsPerPageProgressbar;
  let currentProgressbar = reduxProvinceList?.slice(
    indexOfFirstItemProgressbar,
    indexOfLastItemProgressbar
  );

  // Pagination end ***********************************

  //#endregion JSX ***********************************

  return (
    <>
      <motion.div initial="closed" animate="open" variants={dropdownVariants}>
        <CCard className="mb-4" style={{ paddingBottom: "4rem" }}>
          <CCardHeader>
            Panel general
            <button
              onClick={bumpHandler}
              style={{ border: "none", background: "none", float: "right" }}
              className={isBumped ? "bump" : ""}
            >
              <FontAwesomeIcon icon={faRefresh} color="#697588" />{" "}
            </button>
          </CCardHeader>
          <CCardBody>
            <CRow>
              {/*  */}
              <MapsDashboardFilter
                selectedCircuit={selectedCircuit}
                setSelectedCircuit={setSelectedCircuit}
                selectedMunicipality={selectedMunicipality}
                setSelectedMunicipality={setSelectedMunicipality}
                selectedProvince={selectedProvince}
                setSelectedProvince={setSelectedProvince}
                isExpanded={isExpanded}
                setIsExpanded={setIsExpanded}
              />
              {/*  */}
              {isMobile && "\u00A0"}
              {/* <CCol xs={12} sm={9} md={9} lg={9} xl={9}> */}
              <CCol
                xs={12}
                sm={!isExpanded ? 9 : 10}
                md={!isExpanded ? 9 : 10}
                lg={!isExpanded ? 9 : 10}
                xl={!isExpanded ? 9 : 10}
              >
                <CAccordion activeItemKey={1}>
                  <motion.div
                    initial="hidden"
                    animate="visible"
                    variants={filterVariants}
                  >
                    <CAccordionItem itemKey={1}>
                      <CAccordionHeader className="custom-accordion-header">
                        Distribución geográfica
                      </CAccordionHeader>
                      <CAccordionBody>
                        <CircuitMap circuitList={currentFilteredCircuitList} />
                      </CAccordionBody>
                    </CAccordionItem>
                  </motion.div>
                </CAccordion>
                {"\u00A0"}
                <CAccordion activeItemKey={1}>
                  <motion.div
                    initial="hidden"
                    animate="visible"
                    variants={filterVariants}
                  >
                    <CAccordionItem itemKey={1}>
                      <CAccordionHeader className="custom-accordion-header">
                        Estado de carga
                      </CAccordionHeader>
                      <CAccordionBody>
                        <div className="progress-group mb-4">
                          <div className="progress-group-header">
                            <span
                              style={{ fontWeight: "bold", color: "#4f5d73" }}
                            >
                              Progreso actual del país
                            </span>
                            <span className="ms-auto">{`Circuitos cerrados: ${completedCircuits}/${totalCircuits} (${progressPercentage}%)`}</span>
                          </div>
                          <div className="progress-group-bars">
                            <CProgress
                              value={progressPercentage}
                              variant="striped"
                              animated
                              color="success"
                            />
                          </div>
                        </div>
                        <div className="progress-group mb-4">
                          <div className="progress-group-header">
                            <span
                              style={{ fontWeight: "bold", color: "#4f5d73" }}
                            >
                              Progreso actual por departamento
                            </span>
                          </div>

                          <div className="progress-group-bars">
                            {currentProgressbar &&
                              currentProgressbar.map((province) => {
                                // Filtrar wings por provincia
                                const wingsInProvince = reduxWingList?.filter(
                                  (wing) => wing.provinceId === province.id
                                );

                                const circuitsInProvince =
                                  getCircuitsInProvince(province);

                                // Calcular circuitos completados y total de circuitos
                                const totalCircuitsInProvince =
                                  circuitsInProvince.length;
                                const completedCircuitsInProvince =
                                  circuitsInProvince?.filter(
                                    (circuit) =>
                                      circuit.step1completed &&
                                      circuit.step2completed &&
                                      circuit.step3completed
                                  ).length;

                                // Calcular el progreso de la provincia
                                const provinceProgress =
                                  totalCircuitsInProvince > 0
                                    ? (completedCircuitsInProvince /
                                        totalCircuitsInProvince) *
                                      100
                                    : 0;

                                return (
                                  <div key={province.id}>
                                    <div className="progress-group-header">
                                      <span>{province.name}</span>
                                      <span className="ms-auto">
                                        {`Circuitos cerrados: ${completedCircuitsInProvince}/${totalCircuitsInProvince} (${Math.round(
                                          provinceProgress
                                        )}%)`}
                                      </span>
                                    </div>
                                    <CProgress
                                      value={provinceProgress}
                                      variant="striped"
                                      animated
                                      color="info"
                                    />
                                    {"\u00A0"}
                                  </div>
                                );
                              })}

                            <br />
                            <CPagination
                              align="center"
                              aria-label="Page navigation"
                            >
                              {startPageProgressbar > 1 && (
                                <CPaginationItem
                                  onClick={() => handlePageChangeProgressbar(1)}
                                >
                                  1
                                </CPaginationItem>
                              )}
                              {startPageProgressbar > 2 && (
                                <CPaginationItem>...</CPaginationItem>
                              )}
                              {[
                                ...Array(
                                  endPageProgressbar - startPageProgressbar + 1
                                ),
                              ].map((_, index) => (
                                <CPaginationItem
                                  key={startPageProgressbar + index}
                                  active={
                                    startPageProgressbar + index ===
                                    currentPageProgressbar
                                  }
                                  onClick={() =>
                                    handlePageChangeProgressbar(
                                      startPageProgressbar + index
                                    )
                                  }
                                >
                                  {startPageProgressbar + index}
                                </CPaginationItem>
                              ))}
                              {endPageProgressbar <
                                pageCountProgressbar - 1 && (
                                <CPaginationItem>...</CPaginationItem>
                              )}
                              {endPageProgressbar < pageCountProgressbar && (
                                <CPaginationItem
                                  onClick={() =>
                                    handlePageChangeProgressbar(
                                      pageCountProgressbar
                                    )
                                  }
                                >
                                  {pageCountProgressbar}
                                </CPaginationItem>
                              )}
                            </CPagination>
                          </div>
                        </div>
                      </CAccordionBody>
                    </CAccordionItem>
                  </motion.div>
                </CAccordion>
              </CCol>
            </CRow>
          </CCardBody>
        </CCard>
      </motion.div>
    </>
  );
};

export default MapsDashboard;
