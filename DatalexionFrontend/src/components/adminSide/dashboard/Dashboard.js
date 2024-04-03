import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { batch, useDispatch, useSelector } from "react-redux";
import { motion } from "framer-motion";

import { USER_ROLE_ADMIN, USER_ROLE_ANALYST } from "../../../userRoles";

import { HubConnectionBuilder } from "@microsoft/signalr";

import { CCard, CCardBody, CCardHeader, CRow } from "@coreui/react";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRefresh } from "@fortawesome/free-solid-svg-icons";

import useBumpEffect from "../../../utils/useBumpEffect";
import DashboardFilter from "./DashboardFilter";
import PoliticalColSlates from "./PoliticalColSlates";
import PoliticalColParties from "./PoliticalColParties";

// redux imports
import { authActions } from "../../../store/auth-slice";
import {
  fetchClientByUser,
  fetchPartyList,
  fetchProvinceList,
  fetchMunicipalityList,
  fetchCircuitList,
  fetchSlateList,
  fetchCandidateList,
} from "../../../store/generalData-actions";

import { getRandomColor, hexToRGBA } from "src/utils/auxiliarFunctions";

import "./Dashboard.css";

const Dashboard = () => {
  //#region Consts ***********************************

  // Votos totales de la selección (departamento, municipio o circuito; incluye todos los partidos)
  const [totalSelectedVotes, setTotalSelectedVotes] = useState(0);
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

    if (selectedCircuit && Array.isArray(selectedCircuit?.listCircuitParties)) {
      totalVotes = selectedCircuit?.listCircuitParties.reduce(
        (sum, party) => sum + party.votes,
        0
      );
    } else if (selectedMunicipality) {
      // Caso en que hay un municipio seleccionado
      const circuitsInMunicipality = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
      circuitsInMunicipality.forEach((circuit) => {
        if (Array.isArray(circuit.listCircuitParties)) {
          totalVotes += circuit.listCircuitParties.reduce(
            (sum, party) => sum + party.votes,
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
        if (Array.isArray(circuit.listCircuitParties)) {
          totalVotes += circuit.listCircuitParties.reduce(
            (sum, party) => sum + party.votes,
            0
          );
        }
      });
    } else {
      // Caso en que no hay nada seleccionado
      reduxCircuitList.forEach((circuit) => {
        if (Array.isArray(circuit.listCircuitParties)) {
          totalVotes += circuit.listCircuitParties.reduce(
            (sum, party) => sum + party.votes,
            0
          );
        }
      });
    }
    return totalVotes;
  };

  const calculateTotalPartyVotes = (sortedFilteredCircuitParties) => {
    let totalVotes = 0;
    // filteredCircuitParties.forEach((circuitParty) => {
    sortedFilteredCircuitParties.forEach((circuitParty) => {
      totalVotes += circuitParty.votes;
    });
    return totalVotes;
  };

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    const totalVotes = calculateTotalSlateVotes();
    setTotalSelectedVotes(totalVotes);

    calculatePartyChartData();
    setSlateChartData(calculateSlateChartData());
  }, [
    selectedProvince,
    selectedMunicipality,
    selectedCircuit,
    totalSelectedVotes,
    reduxCircuitList,
    reduxMunicipalityList,
    reduxPartyList,
    reduxSlateList,
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

  // ** SignalR ** //
  // useEffect(() => {
  //   const createHubConnection = async () => {
  //     const connection = new HubConnectionBuilder()
  //       // .withUrl("https://localhost:8015/notifyHub") // Asegúrate de que la URL coincida con la configuración de tu backend
  //       .withUrl("http://localhost:8015/notifyHub") // Asegúrate de que la URL coincida con la configuración de tu backend
  //       .withAutomaticReconnect()
  //       .build();

  //     connection.on("ReceiveUpdate", (message) => {
  //       console.log(message); // Aquí puedes manejar el mensaje, por ejemplo, mostrando una notificación o refrescando datos
  //       alert(message); // Solo para demostración, considera usar una solución menos intrusiva
  //       // Aquí podrías, por ejemplo, disparar una acción de Redux para actualizar tus datos o llamar directamente a una función que haga un nuevo fetch
  //     });

  //     try {
  //       await connection.start();
  //       console.log("Conexión a SignalR establecida.");
  //     } catch (error) {
  //       console.error("SignalR Connection Error: ", error);
  //       // setTimeout(createHubConnection, 5000); // Reintentar la conexión
  //     }
  //     setHubConnection(connection);
  //   };
  //   createHubConnection();
  //   return () => {
  //     hubConnection?.stop();
  //   };
  // }, []);
  // ** SignalR ** //

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

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

  const updatePartyChartData = (sortedFilteredCircuitParties) => {
    const chartData = sortedFilteredCircuitParties?.reduce(
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
        (municipality) => municipality.provinceId === selectedProvince.id
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
      updatePartyChartData(sortedFilteredCircuitParties);
      const totalVotes = calculateTotalPartyVotes(sortedFilteredCircuitParties);
      setTotalPartyVotes(totalVotes);
    }
  };

  const calculateSlateChartData = () => {
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
      if (reduxSlateList && reduxSlateList.length > 0) {
        const slate = reduxSlateList?.find(
          (s) => s.id === circuitSlate?.slateId
        );
        return reduxWingList?.some(
          (w) => w.id === slate?.wingId && w?.partyId === reduxClient?.party?.id
        );
      }
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

  const bumpHandler = () => {
    triggerBump();

    const fetchGeneralData = async () => {
      batch(() => {
        dispatch(fetchClientByUser());
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
              />
              {/*  */}
              {isMobile && "\u00A0"}
              <PoliticalColSlates
                isExpanded={isExpanded}
                filteredCircuitParties={filteredCircuitParties}
                filteredCircuitSlates={filteredCircuitSlates}
                slateChartData={slateChartData}
                slateColors={slateColors}
                totalSelectedVotes={totalSelectedVotes}
              />
              {/*  */}
              {isMobile && "\u00A0"}
              <PoliticalColParties
                isExpanded={isExpanded}
                filteredCircuitParties={filteredCircuitParties}
                partyChartData={partyChartData}
                totalPartyVotes={totalPartyVotes}
              />
              {/*  */}
            </CRow>
          </CCardBody>
        </CCard>
      </motion.div>
    </>
  );
};

export default Dashboard;
