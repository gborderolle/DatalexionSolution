import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

let backendURL = process.env.REACT_APP_URL;

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome } from "@fortawesome/free-solid-svg-icons";
import {
  CForm,
  CCard,
  CCardBody,
  CCol,
  CCardHeader,
  CRow,
  CTable,
  CTableHead,
  CTableRow,
  CTableHeaderCell,
  CTableBody,
  CTableDataCell,
  CCardFooter,
  CButton,
} from "@coreui/react";

import useBumpEffect from "../../../utils/useBumpEffect";
import "./FormStart.css";

import { FormStart } from "../../../utils/navigationPaths";
import { getCircuitParty } from "../../../utils/auxiliarFunctions";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { uiActions } from "../../../store/ui-slice";
import { formActions } from "../../../store/form-slice";
import { liveSettingsActions } from "../../../store/liveSettings-slice";
import { SlateGetWing } from "../../../utils/auxiliarFunctions";

const buttonColor = "dark";

const initialFixedCards = [
  {
    id: "nullVotes",
    name: "Anulados",
    photoURL: backendURL + "/uploads/extras/circuitNullVotes.jpg",
    votes: 0,
  },
  {
    id: "blankVotes",
    name: "En blanco",
    photoURL: backendURL + "/uploads/extras/circuitBlankVotes.jpg",
    votes: 0,
  },
  {
    id: "recurredVotes",
    name: "Recurridos",
    photoURL: backendURL + "/uploads/extras/circuitRecurredVotes.jpg",
    votes: 0,
  },
  {
    id: "observedVotes",
    name: "Observados",
    photoURL: backendURL + "/uploads/extras/circuitObservedVotes.jpg",
    votes: 0,
  },
];

const FormSummary = () => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // useSelector
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxSlateList = useSelector(
    (state) => state.generalData.slateList || []
  );
  const reduxPartyList = useSelector(
    (state) => state.generalData.partyList || []
  );
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );
  const reduxWingList = useSelector(
    (state) => state.generalData.wingList || []
  );

  // useStates
  const [filteredSlateList, setFilteredSlateList] = useState([]);
  const [filteredPartyList, setFilteredPartyList] = useState([]);
  const [fixedCards, setFixedCards] = useState(initialFixedCards);
  const [isBumped, triggerBump] = useBumpEffect();
  const [animateTable, setAnimateTable] = useState(false);
  const [imagesUploadedCount, setImagesUploadedCount] = useState(false);

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Scroll to top of the page on startup
  useEffect(() => {
    window.scrollTo(0, 0);
    setAnimateTable(true);

    if (!reduxSelectedCircuit) {
      navigate(FormStart);
    }
  }, []);

  useEffect(() => {
    dispatch(uiActions.showStepSummary());

    return () => {
      dispatch(uiActions.hideStepSummary());
    };
  }, [dispatch]);

  useEffect(() => {
    // redux set ON
    dispatch(uiActions.showCircuitName());

    return () => {
      // redux set OFF
      dispatch(uiActions.hideCircuitName());
    };
  }, [dispatch]);

  useEffect(() => {
    const filteredSlates = getFilteredSlates();
    setFilteredSlateList(filteredSlates);

    const filteredParties = getFilteredParties();
    setFilteredPartyList(filteredParties);
  }, []);

  useEffect(() => {
    if (reduxSelectedCircuit && Object.keys(reduxSelectedCircuit).length > 0) {
      setFixedCards((prevCards) => {
        return prevCards?.map((card) => {
          const circuitParty = getCircuitParty(
            reduxSelectedCircuit,
            reduxClient
          );
          if (circuitParty) {
            setImagesUploadedCount(circuitParty.imagesUploadedCount);

            // Si newVotes tiene una entrada para el cardId actual, actualízalo
            const newVoteCount = circuitParty[card.id];
            return {
              ...card,
              votes: newVoteCount !== undefined ? newVoteCount : card.votes,
            };
          }
        });
      });
    }
  }, [reduxSelectedCircuit]); // La dependencia es newVotes, así que este efecto se ejecutará cada vez que newVotes cambie

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const getFilteredParties = () => {
    if (
      !reduxSelectedCircuit ||
      !reduxSelectedCircuit.listCircuitParties ||
      !reduxSlateList ||
      !reduxClient ||
      !reduxClient.party ||
      !reduxWingList
    )
      return [];

    // Filtrar primero basado en la presencia en listCircuitSlates
    let filteredByCircuit = reduxSelectedCircuit.listCircuitParties
      .map((circuitParty) => {
        const partyDetail = reduxPartyList.find(
          (party) => party.id === circuitParty?.partyId
        );
        if (!partyDetail) return null; // Si no se encuentra detalle, filtrar fuera
        return {
          ...partyDetail,
          votes: circuitParty.totalPartyVotes || 0,
        };
      })
      .filter(Boolean); // Eliminar cualquier elemento nulo resultante de no encontrar detalles

    return filteredByCircuit;
  };

  const getFilteredSlates = () => {
    if (
      !reduxSelectedCircuit ||
      !reduxSelectedCircuit.listCircuitSlates ||
      !reduxSlateList ||
      !reduxClient ||
      !reduxClient.party ||
      !reduxWingList
    )
      return [];

    // Filtrar primero basado en la presencia en listCircuitSlates
    let filteredByCircuit = reduxSelectedCircuit.listCircuitSlates
      .map((circuitSlate) => {
        const slateDetail = reduxSlateList.find(
          (slate) => slate.id === circuitSlate?.slateId
        );
        if (!slateDetail) return null; // Si no se encuentra detalle, filtrar fuera
        return {
          ...slateDetail,
          totalSlateVotes: circuitSlate.totalSlateVotes || 0,
        };
      })
      .filter(Boolean); // Eliminar cualquier elemento nulo resultante de no encontrar detalles

    // Filtrar las listas para incluir solo aquellas que pertenecen al partido del cliente
    let filteredByParty = filteredByCircuit.filter((slate) => {
      // const wing = reduxWingList.find((wing) => wing.id === slate?.wingId);
      const wing = SlateGetWing(slate, reduxWingList);
      return slate && wing && wing?.partyId === reduxClient?.party.id;
    });

    return filteredByParty;
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const bumpHandler = () => {
    triggerBump();
    dispatch(liveSettingsActions.setSelectedCircuit(null));
    dispatch(formActions.emptyAllVotos());

    setTimeout(() => {
      navigate(FormStart);
    }, 200);
  };

  //#endregion Events ***********************************

  //#region JSX props ***********************************

  // ---------- Totales de votos

  const totalSlateVotes = filteredSlateList.reduce(
    (acc, circuitSlate) => acc + Number(circuitSlate.totalSlateVotes),
    0
  );

  const totalPartyVotes = reduxSelectedCircuit?.listCircuitParties?.reduce(
    (acc, party) => acc + Number(party.votes),
    0
  );

  const votosExtrasTotal = fixedCards.reduce(
    (acc, card) => acc + card.votes,
    0
  );

  // ---------- Ordenamiento de listas

  // Ordena filteredPartyList por votos de mayor a menor
  // const sortedPartyList = [
  //   ...(reduxSelectedCircuit?.listCircuitParties || []),
  // ].sort((a, b) => b.votes - a.votes);

  const sortedPartyList = [...(filteredPartyList || [])].sort(
    (a, b) => b.partyVotes - a.partyVotes
  );
  const sortedSlateList = [...(filteredSlateList || [])].sort(
    (a, b) => b.slateVotes - a.slateVotes
  );

  const sortedCardList = [...fixedCards].sort((a, b) => b.votes - a.votes);

  //#endregion JSX props ***********************************

  return (
    <>
      <CForm onSubmit={bumpHandler} style={{ paddingBottom: "5rem" }}>
        <CCard className="mb-4">
          <CCardHeader>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <div>Paso 4. Resumen final</div>
              <button
                onClick={bumpHandler}
                style={{ border: "none", background: "none", float: "right" }}
                className={isBumped ? "bump" : ""}
              >
                <FontAwesomeIcon icon={faHome} color="#697588" />
              </button>
            </div>
          </CCardHeader>
          <CCardBody>
            <CRow>
              <CCol xs={12} sm={12} md={12} lg={12} xl={12}>
                <CTable className={animateTable ? "animated-table" : ""}>
                  <CTableHead>
                    <CTableRow>
                      <CTableHeaderCell>#</CTableHeaderCell>
                      <CTableHeaderCell>Mis listas</CTableHeaderCell>
                      <CTableHeaderCell>Votación</CTableHeaderCell>
                    </CTableRow>
                  </CTableHead>
                  <CTableBody>
                    {sortedSlateList &&
                      sortedSlateList.map((slate, index) => (
                        <CTableRow key={slate.id || index}>
                          <CTableHeaderCell scope="row">
                            {index + 1}
                          </CTableHeaderCell>
                          <CTableDataCell>{slate.name}</CTableDataCell>
                          <CTableDataCell>
                            {slate.totalSlateVotes ? slate.totalSlateVotes : 0}
                          </CTableDataCell>
                        </CTableRow>
                      ))}
                    <CTableRow>
                      <CTableDataCell
                        colSpan="2"
                        style={{ fontWeight: "bold" }}
                      >
                        Total
                      </CTableDataCell>
                      <CTableDataCell style={{ fontWeight: "bold" }}>
                        {totalSlateVotes ? totalSlateVotes : 0}
                      </CTableDataCell>
                    </CTableRow>
                  </CTableBody>
                </CTable>
                <br />
                <CTable className={animateTable ? "animated-table" : ""}>
                  <CTableHead>
                    <CTableRow>
                      <CTableHeaderCell scope="col">#</CTableHeaderCell>
                      <CTableHeaderCell scope="col">Partido</CTableHeaderCell>
                      <CTableHeaderCell scope="col">Votación</CTableHeaderCell>
                    </CTableRow>
                  </CTableHead>
                  <CTableBody>
                    {sortedPartyList &&
                      sortedPartyList.map((party, index) => (
                        <CTableRow key={party.id || index}>
                          <CTableHeaderCell scope="row">
                            {index + 1}
                          </CTableHeaderCell>
                          <CTableDataCell>
                            {party.name ? party.shortName : party.name}
                          </CTableDataCell>
                          <CTableDataCell>
                            {party.votes ? party.votes : 0}
                          </CTableDataCell>
                        </CTableRow>
                      ))}
                    <CTableRow>
                      <CTableDataCell
                        colSpan="2"
                        style={{ fontWeight: "bold" }}
                      >
                        Total
                      </CTableDataCell>
                      <CTableDataCell style={{ fontWeight: "bold" }}>
                        {totalPartyVotes ? totalPartyVotes : 0}
                      </CTableDataCell>
                    </CTableRow>
                  </CTableBody>
                </CTable>
                <br />
                <CTable className={animateTable ? "animated-table" : ""}>
                  <CTableHead>
                    <CTableRow>
                      <CTableHeaderCell>#</CTableHeaderCell>
                      <CTableHeaderCell>Extras</CTableHeaderCell>
                      <CTableHeaderCell>Votación</CTableHeaderCell>
                    </CTableRow>
                  </CTableHead>
                  <CTableBody>
                    {sortedCardList &&
                      sortedCardList.map((card, index) => (
                        <CTableRow key={card.id || index}>
                          <CTableHeaderCell scope="row">
                            {index + 1}
                          </CTableHeaderCell>
                          <CTableDataCell>{card.name}</CTableDataCell>
                          <CTableDataCell>
                            {card.votes ? card.votes : 0}
                          </CTableDataCell>
                        </CTableRow>
                      ))}
                    <CTableRow>
                      <CTableDataCell
                        colSpan="2"
                        style={{ fontWeight: "bold" }}
                      >
                        Total
                      </CTableDataCell>
                      <CTableDataCell style={{ fontWeight: "bold" }}>
                        {votosExtrasTotal ? votosExtrasTotal : 0}
                      </CTableDataCell>
                    </CTableRow>
                    <CTableRow>
                      <CTableDataCell colSpan="2">
                        Actas cargadas
                      </CTableDataCell>
                      <CTableDataCell>
                        {imagesUploadedCount ? imagesUploadedCount : 0}
                      </CTableDataCell>
                    </CTableRow>
                  </CTableBody>
                </CTable>
              </CCol>
            </CRow>
            <CCardFooter
              className="text-medium-emphasis"
              style={{ textAlign: "center" }}
            >
              <div style={{ textAlign: "center" }}>
                <CButton type="submit" color={buttonColor}>
                  Finalizar
                </CButton>
              </div>
            </CCardFooter>
          </CCardBody>
        </CCard>
      </CForm>
    </>
  );
};

export default FormSummary;
