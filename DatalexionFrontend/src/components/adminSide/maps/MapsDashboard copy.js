import React from "react";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import {
  CCard,
  CCardBody,
  CCol,
  CCardHeader,
  CRow,
  CListGroup,
  CListGroupItem,
  CInputGroup,
  CFormInput,
  CPagination,
  CPaginationItem,
  CProgress,
  CFormCheck,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
} from "@coreui/react";
import { motion } from "framer-motion";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRefresh } from "@fortawesome/free-solid-svg-icons";

import useBumpEffect from "../../../utils/useBumpEffect";

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

  const [searchProvince, setSearchProvince] = useState("");
  const [searchMunicipality, setSearchMunicipality] = useState("");
  const [searchCircuit, setSearchCircuit] = useState("");

  // pagination logic
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const [pageCount, setPageCount] = useState(0);

  const [isBumped, triggerBump] = useBumpEffect();

  const [filteredProvinceList, setFilteredProvinceList] = useState([]);
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
    const USER_ROLE_ADMIN = "Admin";
    const USER_ROLE_ANALYST = "Analyst";
    if (userRole != USER_ROLE_ADMIN && userRole != USER_ROLE_ANALYST) {
      dispatch(authActions.logout());
      navigate("/login-general");
    }
  }, [userRole, navigate, dispatch]);
  //#endregion RUTA PROTEGIDA

  // redux gets
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxPartyList = useSelector((state) => state.generalData.partyList);
  const reduxWingList = useSelector((state) => state.generalData.wingList);
  const reduxProvinceList = useSelector(
    (state) => state.generalData.provinceList
  );
  const reduxSlateList = useSelector((state) => state.generalData.slateList);

  const [municipalityList, setMunicipalityList] = useState([]);
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

  // Filtrar municipios y circuitos basados en las selecciones
  const filteredMunicipalityList = selectedProvince
    ? reduxMunicipalityList.filter(
        (municipality) =>
          municipality.provinceId === selectedProvince.id &&
          municipality.name
            .toLowerCase()
            .includes(searchMunicipality.toLowerCase())
      )
    : [];

  const filteredCircuitList = selectedMunicipality
    ? reduxCircuitList.filter(
        (circuit) =>
          (circuit.municipalityId === selectedMunicipality.id &&
            circuit.name.toLowerCase().includes(searchCircuit.toLowerCase())) ||
          (circuit.municipalityId === selectedMunicipality.id &&
            circuit.number.toString().includes(searchCircuit))
      )
    : [];

  const getFilteredCircuitList = () => {
    switch (filterType) {
      case "completados":
        return circuitList.filter(
          (circuit) =>
            circuit.step1completed &&
            circuit.step2completed &&
            circuit.step3completed
        );
      case "sinCompletar":
        return circuitList.filter(
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

  // pagination logic
  useEffect(() => {
    setPageCount(Math.ceil(currentFilteredCircuitList.length / itemsPerPage));
  }, [currentFilteredCircuitList]);

  // redux gets
  useEffect(() => {
    setMunicipalityList(reduxMunicipalityList);
    setCircuitList(reduxCircuitList);
  }, [reduxMunicipalityList, reduxCircuitList]);

  useEffect(() => {
    // Cuando cambia la provincia seleccionada, se limpian los estados de municipio y circuito seleccionados.
    setSelectedMunicipality(null);
    setSelectedCircuit(null);
  }, [selectedProvince]); // La dependencia es selectedProvince para que este efecto se ejecute solo cuando cambie.

  // Actualizar la lista de municipios filtrados cuando cambie selectedProvince o searchMunicipality
  useEffect(() => {
    const filtered = reduxMunicipalityList.filter(
      (municipality) =>
        municipality.provinceId === selectedProvince?.id &&
        municipality?.name
          ?.toLowerCase()
          .includes(searchMunicipality.toLowerCase())
    );
    setMunicipalityList(filtered);
  }, [selectedProvince, searchMunicipality, reduxMunicipalityList]);

  // Actualizar la lista de circuitos filtrados cuando cambie selectedMunicipality o searchCircuit
  useEffect(() => {
    const filtered = reduxCircuitList.filter(
      (circuit) =>
        circuit?.municipalityId === selectedMunicipality?.id &&
        circuit?.name?.toLowerCase().includes(searchCircuit.toLowerCase())
    );
    setCircuitList(filtered);
  }, [selectedMunicipality, searchCircuit, reduxCircuitList]);

  useEffect(() => {
    const filtered = reduxProvinceList.filter((province) =>
      province?.name?.toLowerCase().includes(searchProvince.toLowerCase())
    );
    setFilteredProvinceList(filtered);
  }, [searchProvince]);

  // Efecto para actualizar municipios cuando cambia la provincia seleccionada
  useEffect(() => {
    const filteredMunicipalities = selectedProvince
      ? reduxMunicipalityList.filter(
          (municipality) => municipality.provinceId === selectedProvince.id
        )
      : [];
    setMunicipalityList(filteredMunicipalities);
    // No resetear municipio o circuito aquí
  }, [selectedProvince, reduxMunicipalityList]);

  // Actualiza la lista de circuitos basada en la provincia o municipio seleccionado
  useEffect(() => {
    let filteredCircuits;
    if (selectedMunicipality) {
      filteredCircuits = reduxCircuitList.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
    } else if (selectedProvince) {
      const municipalitiesInProvince = reduxMunicipalityList.filter(
        (municipality) => municipality.provinceId === selectedProvince.id
      );
      filteredCircuits = reduxCircuitList.filter((circuit) =>
        municipalitiesInProvince.some(
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
      filtered = reduxCircuitList.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
    } else if (selectedProvince) {
      // Primero, obtiene todos los municipios que pertenecen a la provincia seleccionada
      const municipalitiesInProvince = reduxMunicipalityList.filter(
        (municipality) => municipality.provinceId === selectedProvince.id
      );

      // Luego, filtra los circuitos basados en los municipios de la provincia seleccionada
      filtered = reduxCircuitList.filter((circuit) =>
        municipalitiesInProvince.some(
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
    const totalCompleted = reduxCircuitList.filter(
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

  // Determinar el rango de páginas a mostrar alrededor de la página actual
  const pagesToShow = 3; // Ajusta este número según sea necesario
  let startPage = Math.max(currentPage - Math.floor(pagesToShow / 2), 1);
  let endPage = Math.min(startPage + pagesToShow - 1, pageCount);

  if (endPage - startPage + 1 < pagesToShow) {
    startPage = Math.max(endPage - pagesToShow + 1, 1);
  }

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentCircuits = currentFilteredCircuitList.slice(
    indexOfFirstItem,
    indexOfLastItem
  );

  // Función modificada para renderizar los circuitos de la página actual
  const renderCircuitList = () => {
    // Si hay circuitos disponibles para mostrar
    if (currentFilteredCircuitList && currentFilteredCircuitList.length > 0) {
      return currentFilteredCircuitList.map((circuit) => {
        const totalVotes = getTotalVotesByCircuit(circuit.id);
        const isCompleted =
          circuit.step1completed &&
          circuit.step2completed &&
          circuit.step3completed;
        const listItemStyle = isCompleted ? { color: "green" } : {};

        return (
          <CListGroupItem
            component="button"
            key={circuit.id}
            onClick={() => handleCircuitClick(circuit)}
            className={
              selectedCircuit && selectedCircuit.id === circuit.id
                ? "selected-item"
                : ""
            }
          >
            <span
              style={listItemStyle}
            >{`#${circuit.number}: ${circuit.name} (${totalVotes} votos)`}</span>
          </CListGroupItem>
        );
      });
    } else {
      // Mostrar un mensaje cuando no haya circuitos disponibles
      return (
        <CListGroupItem>
          No hay circuitos disponibles para la selección actual.
        </CListGroupItem>
      );
    }
  };

  // const renderCircuitList = () => {
  //   if (currentCircuits && currentCircuits.length > 0) {
  //     return currentCircuits?.map((circuit) => {
  //       const totalVotes = getTotalVotesByCircuit(circuit.id);
  //       const isCompleted =
  //         circuit.step1completed &&
  //         circuit.step2completed &&
  //         circuit.step3completed;
  //       const listItemStyle = isCompleted ? { color: "green" } : {};

  //       return (
  //         <CListGroupItem
  //           component="button"
  //           key={circuit.id}
  //           onClick={() => handleCircuitClick(circuit)}
  //           className={
  //             selectedCircuit && selectedCircuit.id === circuit.id
  //               ? "selected-item"
  //               : ""
  //           }
  //         >
  //           <span
  //             style={listItemStyle}
  //           >{`#${circuit.number}: ${circuit.name} (${totalVotes} votos)`}</span>
  //         </CListGroupItem>
  //       );
  //     });
  //   } else {
  //     // setSelectedCircuit(null);
  //   }
  // };

  const getTotalVotesByProvince = (id) => {
    const municipalitiesInProvince = reduxMunicipalityList.filter(
      (municipality) => municipality.id === id
    );

    let totalVotes = 0;
    municipalitiesInProvince.forEach((municipality) => {
      const circuitsInMunicipality = reduxCircuitList.filter(
        (circuit) => circuit.municipalityId === municipality.id
      );

      circuitsInMunicipality.forEach((circuit) => {
        if (circuit.listCircuitParties) {
          totalVotes += circuit.listCircuitParties.reduce(
            (sum, partyVote) => sum + partyVote.votes,
            0
          );
        }
      });
    });

    return totalVotes;
  };

  const getTotalVotesAllProvinces = () => {
    let totalVotes = 0;
    reduxProvinceList.forEach((province) => {
      totalVotes += getTotalVotesByProvince(province.id);
    });
    return totalVotes;
  };

  const getTotalVotesAllMunicipalities = () => {
    let totalVotes = 0;

    if (selectedProvince) {
      const municipalityInProvince = municipalityList.filter(
        (municipality) => municipality.id === selectedProvince.id
      );

      municipalityInProvince.forEach((municipality) => {
        const circuitsInMunicipality = reduxCircuitList.filter(
          (circuit) => circuit.municipalityId === municipality.id
        );

        circuitsInMunicipality.forEach((circuit) => {
          if (circuit.listCircuitParties) {
            totalVotes += circuit.listCircuitParties.reduce(
              (sum, partyVote) => sum + partyVote.votes,
              0
            );
          }
        });
      });
    }
    return totalVotes;
  };

  const getTotalVotesAllCircuits = () => {
    let totalVotes = 0;

    if (selectedMunicipality) {
      const circuitsInMunicipality = reduxCircuitList.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );

      circuitsInMunicipality.forEach((circuit) => {
        if (circuit.listCircuitParties) {
          totalVotes += circuit.listCircuitParties.reduce(
            (sum, partyVote) => sum + partyVote.votes,
            0
          );
        }
      });
    }
    return totalVotes;
  };

  const getTotalVotesByMunicipality = (municipalityId) => {
    const circuitsInMunicipality = reduxCircuitList.filter(
      (circuit) => circuit.municipalityId === municipalityId
    );

    let totalVotes = 0;
    circuitsInMunicipality.forEach((circuit) => {
      if (circuit.listCircuitParties) {
        totalVotes += circuit.listCircuitParties.reduce(
          (sum, partyVote) => sum + partyVote.votes,
          0
        );
      }
    });

    return totalVotes;
  };

  const getTotalVotesByCircuit = (circuitId) => {
    const circuit = reduxCircuitList.find((c) => c.id === circuitId);

    if (!circuit || !circuit.listCircuitParties) {
      return 0;
    }

    return circuit.listCircuitParties.reduce(
      (sum, partyVote) => sum + partyVote.votes,
      0
    );
  };

  const getProgressByProvince = (provinceId) => {
    const circuitsInProvince = reduxCircuitList.filter(
      (circuit) => circuit.provinceId === provinceId
    );
    const totalCircuits = circuitsInProvince.length;
    const completedCircuits = circuitsInProvince.filter(
      (circuit) =>
        circuit.step1completed &&
        circuit.step2completed &&
        circuit.step3completed
    ).length;

    return totalCircuits > 0 ? (completedCircuits / totalCircuits) * 100 : 0;
  };

  const getProvinceById = (provinceId) => {
    return reduxProvinceList.find((p) => p.id === provinceId);
  };

  const getMunicipalityById = (municipalityId) => {
    return reduxMunicipalityList.find((p) => p.id === municipalityId);
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const handleFilterChange = (event) => {
    setFilterType(event.target.value);
  };

  // pagination logic
  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const handleProvinceClick = (province) => {
    setSelectedProvince(province);
    setSelectedMunicipality(null); // Resetea la selección de municipio
  };

  // Manejador para cuando se selecciona un municipio
  const handleMunicipalityClick = (municipality) => {
    setSelectedMunicipality(municipality);
    setSelectedCircuit(null); // Resetea la selección de municipio
  };

  const handleCircuitClick = (circuit) => {
    setSelectedCircuit(circuit);
  };

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

  // Actualizar las funciones de renderizado para incorporar el manejo de clics
  const renderProvinceList = () => {
    if (filteredProvinceList && filteredProvinceList.length > 0) {
      return filteredProvinceList?.map((province) => {
        const totalVotes = getTotalVotesByProvince(province.id);
        return (
          <CListGroupItem
            component="button"
            key={province.id}
            onClick={() => handleProvinceClick(province)}
            className={
              selectedProvince && selectedProvince.id === province.id
                ? "selected-item"
                : ""
            }
          >
            {`${province.name} (${totalVotes} votos)`}
          </CListGroupItem>
        );
      });
    }
  };

  const renderMunicipalityList = () => {
    if (filteredMunicipalityList && filteredMunicipalityList.length > 0) {
      return filteredMunicipalityList?.map((municipality) => {
        const totalVotes = getTotalVotesByMunicipality(municipality.id);
        return (
          <CListGroupItem
            component="button"
            key={municipality.id}
            onClick={() => handleMunicipalityClick(municipality)}
            className={
              selectedMunicipality &&
              selectedMunicipality.id === municipality.id
                ? "selected-item"
                : ""
            }
          >
            {`${municipality.name} (${totalVotes} votos)`}
          </CListGroupItem>
        );
      });
    }
  };

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
              <CCol xs={12} sm={3} md={3} lg={3} xl={3}>
                <CAccordion activeItemKey={activeKey}>
                  <motion.div
                    initial="hidden"
                    animate="visible"
                    variants={filterVariants}
                  >
                    <CAccordionItem itemKey={1}>
                      <CAccordionHeader className="custom-accordion-header">
                        Filtros
                      </CAccordionHeader>
                      <CAccordionBody>
                        <CListGroup>
                          <CInputGroup>
                            <CFormInput
                              placeholder="Filtrar departamento..."
                              onChange={(e) =>
                                setSearchProvince(e.target.value)
                              }
                              size="sm"
                            />
                          </CInputGroup>
                          <CListGroupItem active>
                            Departamentos ({getTotalVotesAllProvinces()} votos)
                          </CListGroupItem>
                          {renderProvinceList()}{" "}
                          {/* Renderiza la lista de departamentos */}
                        </CListGroup>
                        <br />
                        <CListGroup>
                          <CInputGroup>
                            <CFormInput
                              placeholder="Filtrar municipio..."
                              onChange={(e) =>
                                setSearchMunicipality(e.target.value)
                              }
                              size="sm"
                            />
                          </CInputGroup>
                          <CListGroupItem active>
                            Municipios ({getTotalVotesAllMunicipalities()}{" "}
                            votos)
                          </CListGroupItem>
                          {renderMunicipalityList()}{" "}
                          {/* Renderiza la lista de municipios */}
                        </CListGroup>
                        <br />
                        <CListGroup>
                          <CInputGroup>
                            <CFormInput
                              placeholder="Filtrar circuito..."
                              onChange={(e) => setSearchCircuit(e.target.value)}
                              size="sm"
                            />
                          </CInputGroup>
                          <CListGroupItem active>
                            Circuitos ({getTotalVotesAllCircuits()} votos)
                          </CListGroupItem>
                          {renderCircuitList()}{" "}
                          {/* Renderiza la lista de circuitos */}
                          <br />
                          <CPagination
                            align="center"
                            aria-label="Page navigation example"
                          >
                            {startPage > 1 && (
                              <CPaginationItem
                                onClick={() => handlePageChange(1)}
                              >
                                1
                              </CPaginationItem>
                            )}
                            {startPage > 2 && (
                              <CPaginationItem>...</CPaginationItem>
                            )}
                            {[...Array(endPage - startPage + 1)].map(
                              (_, index) => (
                                <CPaginationItem
                                  key={startPage + index}
                                  active={startPage + index === currentPage}
                                  onClick={() =>
                                    handlePageChange(startPage + index)
                                  }
                                >
                                  {startPage + index}
                                </CPaginationItem>
                              )
                            )}
                            {endPage < pageCount - 1 && (
                              <CPaginationItem>...</CPaginationItem>
                            )}
                            {endPage < pageCount && (
                              <CPaginationItem
                                onClick={() => handlePageChange(pageCount)}
                              >
                                {pageCount}
                              </CPaginationItem>
                            )}
                          </CPagination>
                        </CListGroup>
                        <br />

                        <hr className="mt-0" />

                        <div key={filterType}>
                          <CFormCheck
                            type="radio"
                            name="radFilterCircuits"
                            value="todos"
                            id="flexRadioDefault1"
                            label="Todos los circuitos"
                            onChange={handleFilterChange}
                            checked={filterType === "todos"}
                          />
                          <CFormCheck
                            type="radio"
                            name="radFilterCircuits"
                            value="completados"
                            id="flexRadioDefault2"
                            label="Sólo cerrados"
                            onChange={handleFilterChange}
                            checked={filterType === "completados"}
                          />
                          <CFormCheck
                            type="radio"
                            name="radFilterCircuits"
                            value="sinCompletar"
                            id="flexRadioDefault3"
                            label="Sólo abiertos"
                            onChange={handleFilterChange}
                            checked={filterType === "sinCompletar"}
                          />
                        </div>
                      </CAccordionBody>
                    </CAccordionItem>
                  </motion.div>
                </CAccordion>
              </CCol>
              {/*  */}
              {isMobile && "\u00A0"}
              <CCol xs={12} sm={9} md={9} lg={9} xl={9}>
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
                            <span style={{ fontWeight: "bold" }}>
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
                            <span style={{ fontWeight: "bold" }}>
                              Progreso actual por departamento
                            </span>
                          </div>

                          <div className="progress-group-bars">
                            {reduxProvinceList &&
                              reduxProvinceList.map((province) => {
                                // Filtrar wings por provincia
                                const wingsInProvince = reduxWingList.filter(
                                  (wing) => wing.provinceId === province.id
                                );

                                // Obtener todos los circuitos en los wings de la provincia actual
                                const circuitsInProvince =
                                  reduxCircuitList.filter((circuit) =>
                                    wingsInProvince.some(
                                      (wing) => wing.id === circuit.wingId
                                    )
                                  );

                                // Calcular circuitos completados y total de circuitos
                                const totalCircuitsInProvince =
                                  circuitsInProvince.length;
                                const completedCircuitsInProvince =
                                  circuitsInProvince.filter(
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
