import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { batch, useDispatch, useSelector } from "react-redux";

import { HubConnectionBuilder } from "@microsoft/signalr";

import {
  CCard,
  CCardBody,
  CCol,
  CCardHeader,
  CRow,
  CProgress,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
  CFormCheck,
} from "@coreui/react";
import { motion } from "framer-motion";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRefresh } from "@fortawesome/free-solid-svg-icons";
import { CChartPie } from "@coreui/react-chartjs";

import useBumpEffect from "../../../utils/useBumpEffect";
import DashboardFilter from "./DashboardFilter";

// redux imports
import { authActions } from "../../../store/auth-slice";
import {
  fetchPartyList,
  fetchProvinceList,
  fetchMunicipalityList,
  fetchCircuitList,
  fetchSlateList,
  fetchCandidateList,
} from "../../../store/generalData-actions";

import "./Dashboard.css";
import classesMobile from "./DashboardMobile.module.css";

import styled from "styled-components";
const StyledProgress = styled(CProgress)`
  .c-progress-bar {
    background-color: ${(props) => props.bgColor} !important;
  }
`;

const Dashboard = () => {
  //#region Consts ***********************************

  const [totalSlateVotes, setTotalSlateVotes] = useState(0);
  const [totalPartyVotes, setTotalPartyVotes] = useState(0);

  const [selectedProvince, setSelectedProvince] = useState(null);
  const [selectedMunicipality, setSelectedMunicipality] = useState(null);
  const [selectedCircuit, setSelectedCircuit] = useState(null);

  const [isExpanded, setIsExpanded] = useState(false);

  const [partyChartData, setPartyChartData] = useState({
    datasets: [],
    labels: [],
  });
  const [slateChartData, setSlateChartData] = useState({
    datasets: [],
    labels: [],
  });

  const [isRelativePercentage, setIsRelativePercentage] = useState(true);
  const [isBumped, triggerBump] = useBumpEffect();
  const [filteredCircuitParties, setFilteredCircuitParties] = useState([]);
  const [filteredCircuitSlates, setFilteredCircuitSlates] = useState([]);

  const isMobile = JSON.parse(localStorage.getItem("isMobile"));

  // Estado para la conexión de SignalR
  const [hubConnection, setHubConnection] = useState(null);

  // redux
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
  const reduxSlateList = useSelector((state) => state.generalData.slateList);
  const reduxMunicipalityList = useSelector(
    (state) => state.generalData.municipalityList
  );
  const reduxCircuitList = useSelector(
    (state) => state.generalData.circuitList
  );

  const [sortedSlateList, setSortedSlateList] = useState([]);
  const [slateColors, setSlateColors] = useState({});

  const dropdownVariants = {
    open: {
      opacity: 1,
      height: "auto",
      transition: { duration: 0.5, type: "spring", stiffness: 120 },
    },
    closed: { opacity: 0, height: 0, transition: { duration: 0.5 } },
  };

  const calculateTotalSlateVotes = () => {
    let totalVotes = 0;

    if (selectedCircuit && Array.isArray(selectedCircuit?.listCircuitSlates)) {
      totalVotes = selectedCircuit?.listCircuitSlates.reduce(
        (sum, slate) => sum + slate.votes,
        0
      );
    } else if (selectedMunicipality) {
      // Caso en que hay un municipio seleccionado
      const circuitsInMunicipality = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
      circuitsInMunicipality.forEach((circuit) => {
        if (Array.isArray(circuit.listCircuitSlates)) {
          totalVotes += circuit.listCircuitSlates.reduce(
            (sum, slate) => sum + slate.votes,
            0
          );
        }
      });
    } else if (selectedProvince) {
      // Caso en que hay una provincia seleccionada
      const municipalityIdsInProvince = reduxMunicipalityList
        ?.filter((municipality) => municipality.id === selectedProvince.id)
        .map((municipality) => municipality.id);

      const circuitsInProvince = reduxCircuitList?.filter((circuit) =>
        municipalityIdsInProvince.includes(circuit.municipalityId)
      );

      circuitsInProvince.forEach((circuit) => {
        if (Array.isArray(circuit.listCircuitSlates)) {
          totalVotes += circuit.listCircuitSlates.reduce(
            (sum, slate) => sum + slate.votes,
            0
          );
        }
      });
    } else {
      // Caso en que no hay nada seleccionado
      reduxCircuitList.forEach((circuit) => {
        if (Array.isArray(circuit.listCircuitSlates)) {
          totalVotes += circuit.listCircuitSlates.reduce(
            (sum, slate) => sum + slate.votes,
            0
          );
        }
      });
    }

    return totalVotes;
  };

  const calculateTotalPartyVotes = () => {
    let totalVotes = 0;
    filteredCircuitParties.forEach((party) => {
      totalVotes += party.votes;
    });
    return totalVotes;
  };

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  function sortSlateList() {
    if (selectedCircuit && selectedCircuit?.listCircuitSlates) {
      const sortedList = [...selectedCircuit?.listCircuitSlates]?.sort(
        (a, b) => b.votes - a.votes
      );
      setSortedSlateList(sortedList);
    }
  }

  useEffect(() => {
    const totalVotes = calculateTotalSlateVotes();
    setTotalSlateVotes(totalVotes);

    calculatePartyChartData();
    sortSlateList();
    setSlateChartData(calculateSlateChartDataAndHandleSelected());
  }, [
    selectedProvince,
    selectedMunicipality,
    selectedCircuit,
    reduxCircuitList,
    reduxMunicipalityList,
    totalSlateVotes,
  ]);

  // Genera un color aleatorio para cada lista cuando la lista de slates cambie
  useEffect(() => {
    const newSlateColors = reduxSlateList?.reduce((colors, slate) => {
      colors[slate.id] = slate.color || getRandomColor();
      return colors;
    }, {});

    setSlateColors(newSlateColors);
  }, [reduxSlateList]);

  useEffect(() => {
    // Asegúrate de que reduxSelectedCircuit y slateVotesList existen
    if (selectedCircuit && selectedCircuit?.listCircuitSlates) {
      selectedCircuit?.listCircuitSlates.forEach((slate, index) => {
        const slateColor = slateColors[slate.id] || getRandomColor();
        const element = document.getElementById(`idSlate-${index}`);
        if (element) {
          element.style.borderLeft = `4px solid ${slateColor} !important`;
        }
      });
    }
  }, [selectedCircuit, slateColors]);

  // Usar useEffect o similar para aplicar este filtrado cuando sea necesario
  useEffect(() => {
    const slatesFiltered = filterSlatesByClientParty();
    // Hacer algo con las slates filtradas, como actualizar el estado
    setFilteredCircuitSlates(slatesFiltered);
  }, [reduxSlateList, reduxClient, reduxWingList]); // Asegúrate de incluir reduxWingList en las dependencias

  useEffect(() => {
    const createHubConnection = async () => {
      const connection = new HubConnectionBuilder()
        // .withUrl("https://localhost:8015/notifyHub") // Asegúrate de que la URL coincida con la configuración de tu backend
        .withUrl("http://localhost:8015/notifyHub") // Asegúrate de que la URL coincida con la configuración de tu backend
        .withAutomaticReconnect()
        .build();

      connection.on("ReceiveUpdate", (message) => {
        console.log(message); // Aquí puedes manejar el mensaje, por ejemplo, mostrando una notificación o refrescando datos
        alert(message); // Solo para demostración, considera usar una solución menos intrusiva
        // Aquí podrías, por ejemplo, disparar una acción de Redux para actualizar tus datos o llamar directamente a una función que haga un nuevo fetch
      });

      try {
        await connection.start();
        console.log("Conexión a SignalR establecida.");
      } catch (error) {
        console.error("SignalR Connection Error: ", error);
        // setTimeout(createHubConnection, 5000); // Reintentar la conexión
      }
      setHubConnection(connection);
    };

    createHubConnection();

    return () => {
      hubConnection?.stop();
    };
  }, []);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const filterSlatesByClientParty = () => {
    // Verifica si reduxClient.partyId, selectedCircuit y reduxWingList están disponibles
    if (
      !reduxClient ||
      !reduxClient.party ||
      !selectedCircuit ||
      !reduxWingList
    )
      return [];

    // Filtrar las CircuitSlates dentro del selectedCircuit donde el PartyId del Wing asociado coincide con reduxClient.partyId
    const filteredCircuitSlates = selectedCircuit?.listCircuitSlates?.filter(
      (circuitSlate) => {
        // Encuentra el Slate correspondiente al SlateId de la CircuitSlate
        const slate = reduxSlateList?.find(
          (s) => s.id === circuitSlate.slateId
        );
        // Encuentra el Wing correspondiente al WingId del Slate encontrado
        const wing = reduxWingList?.find((w) => w.id === slate?.wingId);
        // Verifica si el PartyId del Wing encontrado coincide con el partyId del cliente
        return wing && wing.partyId === reduxClient.party.id;
      }
    );

    return filteredCircuitSlates;
  };

  // Utilidad para el cálculo del porcentaje.
  const calculatePercentage = (partialValue, totalValue) => {
    if (totalValue === 0) {
      return 0; // O cualquier valor que consideres apropiado para divisiones por cero
    }
    return Math.round((partialValue / totalValue) * 100);
  };

  const sumPartyVotes = (circuits) => {
    const totalVotesByParty = {};

    circuits.forEach((circuit) => {
      if (circuit.listCircuitParties) {
        circuit.listCircuitParties.forEach((circuitParties) => {
          if (totalVotesByParty[circuitParties.partyId]) {
            totalVotesByParty[circuitParties.partyId].votes +=
              circuitParties.votes;
          } else {
            totalVotesByParty[circuitParties.partyId] = {
              ...circuitParties,
              votes: circuitParties.votes,
            };
          }
        });
      }
    });
    return Object.values(totalVotesByParty);
  };

  const sumSlateVotes = (circuits) => {
    const totalVotesBySlate = {};

    circuits.forEach((circuit) => {
      if (circuit.listCircuitSlates) {
        circuit.listCircuitSlates.forEach((circuitSlate) => {
          if (totalVotesBySlate[circuitSlate.slateId]) {
            totalVotesBySlate[circuitSlate.slateId].votes += circuitSlate.votes;
          } else {
            totalVotesBySlate[circuitSlate.slateId] = {
              ...circuitSlate,
              votes: circuitSlate.votes,
            };
          }
        });
      }
    });
    return Object.values(totalVotesBySlate);
  };

  function getRandomColor() {
    const letters = "0123456789ABCDEF";
    let color = "#";
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }

  function hexToRGBA(hex, alpha = 1) {
    let r = parseInt(hex.slice(1, 3), 16);
    let g = parseInt(hex.slice(3, 5), 16);
    let b = parseInt(hex.slice(5, 7), 16);

    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
  }

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

  const getSlateById = (slateId) => {
    return reduxSlateList?.find((p) => p.id === slateId);
  };

  const updatePartyChartData = () => {
    // Preprocesamiento para reducir llamadas a funciones repetitivas
    const chartData = filteredCircuitParties?.reduce(
      (acc, circuitParty) => {
        const party = getPartyById(circuitParty.partyId);
        const partyColor = party?.color || getRandomColor();
        const hoverColor = hexToRGBA(partyColor, 0.9);

        acc.labels.push(`${party?.name} (${circuitParty.votes})`);
        acc.data.push(circuitParty.votes);
        acc.backgroundColor.push(partyColor);
        acc.hoverBackgroundColor.push(hoverColor);

        return acc;
      },
      { labels: [], data: [], backgroundColor: [], hoverBackgroundColor: [] }
    );

    const newChartData = {
      labels: chartData.labels,
      datasets: [
        {
          data: chartData.data,
          backgroundColor: chartData.backgroundColor,
          hoverBackgroundColor: chartData.hoverBackgroundColor,
        },
      ],
    };

    setPartyChartData(newChartData);
  };

  const calculatePartyChartData = () => {
    let filteredPartyVotes1 = [];

    if (selectedCircuit) {
      filteredPartyVotes1 = reduxCircuitList?.find(
        (circuit) => circuit.id === selectedCircuit.id
      )?.listCircuitParties;
    } else if (selectedMunicipality) {
      const circuitsInMunicipality = reduxCircuitList?.filter(
        (circuit) => circuit?.municipalityId === selectedMunicipality.id
      );
      filteredPartyVotes1 = sumPartyVotes(circuitsInMunicipality);
    } else if (selectedProvince) {
      const municipalitiesInProvince = reduxMunicipalityList?.filter(
        (municipality) => municipality.id === selectedProvince.id
      );
      const circuitsInProvince = reduxCircuitList?.filter((circuit) =>
        municipalitiesInProvince?.some(
          (municipality) => municipality.id === circuit?.municipalityId
        )
      );
      filteredPartyVotes1 = sumPartyVotes(circuitsInProvince);
    } else {
      filteredPartyVotes1 = sumPartyVotes(reduxCircuitList);
    }

    // Ordenar filteredCircuitParties por cantidad de votos de mayor a menor.
    const sortedFilteredCircuitParties = [...filteredPartyVotes1]?.sort(
      (a, b) => b.votes - a.votes
    );

    // Actualizar el estado de los datos del gráfico
    if (sortedFilteredCircuitParties) {
      setFilteredCircuitParties(sortedFilteredCircuitParties);
      updatePartyChartData();
      setTotalPartyVotes(calculateTotalPartyVotes());
    }
  };

  const calculateSlateChartDataAndHandleSelected = () => {
    let filteredCircuitSlates1 = [];

    // Aplica el filtrado según la selección actual (circuito, municipio, provincia)
    if (selectedCircuit) {
      filteredCircuitSlates1 = reduxCircuitList
        ?.find((circuit) => circuit.id === selectedCircuit.id)
        ?.listCircuitSlates?.filter(slateBelongsToClientParty);
    } else if (selectedMunicipality) {
      const circuitsInMunicipality = reduxCircuitList?.filter(
        (circuit) => circuit?.municipalityId === selectedMunicipality.id
      );
      filteredCircuitSlates1 = sumSlateVotes(circuitsInMunicipality)?.filter(
        slateBelongsToClientParty
      );
    } else if (selectedProvince) {
      const municipalitiesInProvince = reduxMunicipalityList?.filter(
        (municipality) => municipality?.provinceId === selectedProvince.id
      );
      const circuitsInProvince = reduxCircuitList?.filter((circuit) =>
        municipalitiesInProvince?.some(
          (municipality) => municipality?.id === circuit?.municipalityId
        )
      );
      filteredCircuitSlates1 = sumSlateVotes(circuitsInProvince)?.filter(
        slateBelongsToClientParty
      );
    } else {
      filteredCircuitSlates1 = sumSlateVotes(reduxCircuitList)?.filter(
        slateBelongsToClientParty
      );
    }

    // Función para verificar si un Slate pertenece al partido del cliente
    function slateBelongsToClientParty(circuitSlate) {
      const slate = reduxSlateList?.find((s) => s.id === circuitSlate?.slateId);
      return reduxWingList?.some(
        (w) => w.id === slate?.wingId && w?.partyId === reduxClient?.party.id
      );
    }

    // Prepara los datos para el gráfico (este código permanece igual)
    const slateChartData = {
      labels: [],
      datasets: [
        {
          data: [],
          backgroundColor: [],
          hoverBackgroundColor: [],
        },
      ],
    };

    // Ordenar filteredCircuitParties por cantidad de votos de mayor a menor.
    const sortedFilteredCircuitSlates = [...filteredCircuitSlates1]?.sort(
      (a, b) => b.votes - a.votes
    );

    sortedFilteredCircuitSlates.forEach((circuitSlate) => {
      const slate = getSlateById(circuitSlate.slateId);
      if (slate) {
        slateChartData.labels.push(`${slate.name} (${circuitSlate.votes})`);
        slateChartData.datasets[0].data.push(circuitSlate.votes);
        slateChartData.datasets[0].backgroundColor.push(
          slate.color || getRandomColor()
        );
        slateChartData.datasets[0].hoverBackgroundColor.push(
          hexToRGBA(slate.color || getRandomColor(), 0.9)
        );
      }
    });

    // Actualizar el estado de los datos del gráfico
    // if (sortedFilteredCircuitSlates.length > 0) {
    setFilteredCircuitSlates(sortedFilteredCircuitSlates);
    // }
    return slateChartData;
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const handleRelativePercentageChange = (e) => {
    setIsRelativePercentage(e.target.checked);
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
    calculatePartyChartData();
  };

  //#endregion Events ***********************************

  //#region JSX ***********************************

  const addStyle = (styleString) => {
    const style = document.createElement("style");
    style.textContent = styleString;
    document.head.append(style);
  };

  const getDynamicClassName = (color) => {
    // Verifica si el color existe y es un string antes de llamar a replace.
    if (typeof color === "string") {
      const className = `progress-bar-${color.replace("#", "")}`;
      addStyle(`.${className} { background-color: ${color}; }`);
      return className;
    } else {
      console.log("Color no proporcionado o no es un string", color);
      // Devuelve una clase predeterminada o maneja el error de alguna manera.
      return "progress-bar-default";
    }
  };

  const setDynamicBorderStyle = (color, index, element) => {
    if (typeof color === "string") {
      const colorCode = color.replace("#", "");
      addStyle(
        `#${element}-${index} { border-color: #${colorCode} !important; }`
      );
    } else {
      console.log(
        `Color no proporcionado o no es un string para el elemento ${element}-${index}`,
        color
      );
      // Maneja el caso en que color no es un string, por ejemplo, asignando un color predeterminado o ignorando el estilo.
    }
  };

  const totalVotes = partyChartData.datasets.reduce(
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

  let totalVotesOfSelectedParty = 0;

  if (isRelativePercentage) {
    // Asumiendo que filteredCircuitParties ya contiene la información de los partidos y sus votos
    // para el circuito seleccionado.
    const partyData = filteredCircuitParties.find(
      (party) => party.partyId === reduxClient.party?.id
    );
    totalVotesOfSelectedParty = partyData ? partyData.votes : 0;
  }

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
            <CRow className="justify-content-center">
              {/*  */}
              <DashboardFilter
                selectedCircuit={selectedCircuit}
                setSelectedCircuit={setSelectedCircuit}
                selectedMunicipality={selectedMunicipality}
                setSelectedMunicipality={setSelectedMunicipality}
                selectedProvince={selectedProvince}
                setSelectedProvince={setSelectedProvince}
                partyChartData={partyChartData}
                setPartyChartData={setPartyChartData}
                slateChartData={slateChartData}
                setSlateChartData={setSlateChartData}
                isExpanded={isExpanded}
                setIsExpanded={setIsExpanded}
              />
              {/*  */}
              {isMobile && "\u00A0"}
              {/* <CCol xs={12} sm={6} md={6} lg={4} xl={4}> */}
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
                        {filteredCircuitParties ? (
                          <>
                            <CRow>
                              {filteredCircuitParties &&
                                filteredCircuitParties.map(
                                  (circuitParty, index) => {
                                    const votePercentage = calculatePercentage(
                                      circuitParty.votes,
                                      totalPartyVotes
                                    );
                                    const partyColor = getPartyById(
                                      circuitParty.partyId
                                    ).color;
                                    setDynamicBorderStyle(
                                      partyColor,
                                      index,
                                      "idParty"
                                    );

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
                                            {
                                              getPartyById(circuitParty.partyId)
                                                .name
                                            }
                                          </div>
                                          <div
                                            className={`text-medium-emphasis small ${classesMobile.value}`}
                                          >
                                            {`Votos: ${circuitParty.votes} (${votePercentage}%)`}
                                          </div>
                                        </div>
                                      </CCol>
                                    );
                                  }
                                )}
                            </CRow>

                            <hr className="mt-0" />

                            {filteredCircuitParties &&
                              filteredCircuitParties.map(
                                (circuitParty, index) => {
                                  const votePercentage = calculatePercentage(
                                    circuitParty.votes,
                                    totalPartyVotes
                                  );
                                  const partyColor = getPartyById(
                                    circuitParty.partyId
                                  ).color;
                                  const progressBarClass =
                                    getDynamicClassName(partyColor);

                                  // Usar tanto partyId como slateId para formar una key compuesta
                                  const key = `${circuitParty.circuitId}-${circuitParty.partyId}-${index}`;

                                  return (
                                    <div
                                      className="progress-group mb-4"
                                      key={key}
                                    >
                                      <div className="progress-group-header">
                                        <span>
                                          Partido:{" "}
                                          {
                                            getPartyById(circuitParty.partyId)
                                              .name
                                          }
                                        </span>
                                        <span className="ms-auto">{`Votos: ${circuitParty.votes} (${votePercentage}%)`}</span>
                                      </div>
                                      <div className="progress-group-bars">
                                        <StyledProgress
                                          value={votePercentage}
                                          progressBarClassName={
                                            progressBarClass
                                          }
                                          variant="striped"
                                          animated
                                        />
                                      </div>
                                    </div>
                                  );
                                }
                              )}

                            <CAccordion activeItemKey={1}>
                              <CAccordionItem itemKey={1}>
                                <CAccordionHeader className="custom-accordion-header">
                                  Distribución
                                </CAccordionHeader>
                                <CAccordionBody>
                                  {partyChartData && totalVotes > 0 ? (
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
                          </>
                        ) : (
                          <span style={{ color: "blue", fontStyle: "italic" }}>
                            No hay votos aún.
                          </span>
                        )}
                      </CAccordionBody>
                    </CAccordionItem>
                  </motion.div>
                </CAccordion>
              </CCol>
              {/*  */}
              {isMobile && "\u00A0"}
              {/* <CCol xs={12} sm={6} md={6} lg={4} xl={4}> */}
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
                            label="Relativo %"
                            checked={isRelativePercentage}
                            onChange={handleRelativePercentageChange}
                            style={{
                              display: "inline-block",
                              marginLeft: "10px",
                            }}
                          />
                        </div>
                        <br />
                      </CAccordionHeader>
                      <CAccordionBody>
                        {filteredCircuitSlates ? (
                          <>
                            <CRow>
                              {filteredCircuitSlates &&
                                filteredCircuitSlates.map(
                                  (circuitSlate, index) => {
                                    const votePercentage = isRelativePercentage
                                      ? calculatePercentage(
                                          circuitSlate.votes,
                                          totalVotesOfSelectedParty
                                        )
                                      : calculatePercentage(
                                          circuitSlate.votes,
                                          totalSlateVotes
                                        );

                                    const slateColor =
                                      slateColors[circuitSlate.slateId] ||
                                      getRandomColor();
                                    setDynamicBorderStyle(
                                      slateColor,
                                      index,
                                      "idSlate"
                                    );

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
                                            {
                                              getSlateById(circuitSlate.slateId)
                                                .name
                                            }
                                          </div>
                                          <div
                                            className={`text-medium-emphasis small ${classesMobile.value}`}
                                          >
                                            {`Votos: ${circuitSlate.votes} (${votePercentage}%)`}
                                          </div>
                                        </div>
                                      </CCol>
                                    );
                                  }
                                )}
                            </CRow>

                            <hr className="mt-0" />

                            {filteredCircuitSlates &&
                              filteredCircuitSlates.map(
                                (circuitSlate, index) => {
                                  const votePercentage = isRelativePercentage
                                    ? calculatePercentage(
                                        circuitSlate.votes,
                                        totalVotesOfSelectedParty
                                      )
                                    : calculatePercentage(
                                        circuitSlate.votes,
                                        totalSlateVotes
                                      );

                                  const slateColor =
                                    slateColors[circuitSlate.slateId] ||
                                    getRandomColor();
                                  const progressBarClass =
                                    getDynamicClassName(slateColor);

                                  // Usar tanto partyId como slateId para formar una key compuesta
                                  const key = `${circuitSlate.circuitId}-${circuitSlate.slateId}-${index}`;

                                  return (
                                    <div
                                      className="progress-group mb-4"
                                      key={key}
                                    >
                                      <div className="progress-group-header">
                                        <span>
                                          Lista:{" "}
                                          {
                                            getSlateById(circuitSlate.slateId)
                                              .name
                                          }
                                        </span>
                                        <span className="ms-auto">{`Votos: ${circuitSlate.votes} (${votePercentage}%)`}</span>
                                      </div>
                                      <div className="progress-group-bars">
                                        <StyledProgress
                                          value={votePercentage}
                                          progressBarClassName={
                                            progressBarClass
                                          }
                                          variant="striped"
                                          animated
                                        />
                                      </div>
                                    </div>
                                  );
                                }
                              )}
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
                              slateChartData.datasets[0].data.length > 0 &&
                              totalVotes > 0 ? (
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
              {/*  */}
            </CRow>
          </CCardBody>
        </CCard>
      </motion.div>
    </>
  );
};

export default Dashboard;
