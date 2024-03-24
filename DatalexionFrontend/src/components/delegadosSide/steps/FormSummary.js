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

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { uiActions } from "../../../store/ui-slice";
import { formActions } from "../../../store/form-slice";
import { liveSettingsActions } from "../../../store/liveSettings-slice";
import { SlateGetWing } from "../../../utils/auxiliarFunctions";

const buttonColor = "dark";

if (!backendURL) {
  backendURL = "http://localhost:8015";
}

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

  const [filteredSlateList, setFilteredSlateList] = useState([]);
  const [fixedCards, setFixedCards] = useState(initialFixedCards);

  // redux gets
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxSlateList = useSelector(
    (state) => state.generalData.slateList || []
  );
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );
  const reduxWingList = useSelector(
    (state) => state.generalData.wingList || []
  );
  const circuitImagesUploadedCount = reduxSelectedCircuit
    ? reduxSelectedCircuit.imagesUploadedCount
    : 0;

  const [isBumped, triggerBump] = useBumpEffect();
  const [animateTable, setAnimateTable] = useState(false);

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Scroll to top of the page on startup
  useEffect(() => {
    window.scrollTo(0, 0);
    setAnimateTable(true);

    if (!reduxSelectedCircuit) {
      navigate("/formStart");
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
  }, []);

  useEffect(() => {
    // Asegúrate de que newVotes no sea nulo o undefined y tenga la forma esperada
    if (reduxSelectedCircuit && Object.keys(reduxSelectedCircuit).length > 0) {
      setFixedCards((prevCards) => {
        return prevCards?.map((card) => {
          // Si newVotes tiene una entrada para el cardId actual, actualízalo
          const newVoteCount = reduxSelectedCircuit[card.id];
          return {
            ...card,
            votes: newVoteCount !== undefined ? newVoteCount : card.votes,
          };
        });
      });
    }
  }, [reduxSelectedCircuit]); // La dependencia es newVotes, así que este efecto se ejecutará cada vez que newVotes cambie

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

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
          votes: circuitSlate.votes || 0,
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
      navigate("/formStart");
    }, 200);
  };

  //#endregion Events ***********************************

  //#region JSX props ***********************************

  // ---------- Totales de votos

  const totalSlateVotes = filteredSlateList.reduce(
    (acc, slate) => acc + Number(slate.votes),
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
  const sortedPartyList = [
    ...(reduxSelectedCircuit?.listCircuitParties || []),
  ].sort((a, b) => b.votes - a.votes);

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
                            {slate.votes ? slate.votes : 0}
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
                            {party.name ? party.name : party.shortName}
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
                        {circuitImagesUploadedCount
                          ? circuitImagesUploadedCount
                          : 0}
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
