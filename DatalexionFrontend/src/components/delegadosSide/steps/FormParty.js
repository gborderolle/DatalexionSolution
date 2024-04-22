import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";

import { urlCircuitUpdateStep2 } from "../../../endpoints";
import useAPI from "../../../hooks/use-API";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome } from "@fortawesome/free-solid-svg-icons";
import {
  CCard,
  CCardBody,
  CCol,
  CCardHeader,
  CRow,
  CForm,
  CCardFooter,
  CButton,
} from "@coreui/react";

import { LoadingSpinner } from "../../../utils/LoadingSpinner";
import WidgetCard from "../widgets/WidgetCard";

import { FormStart, FormExtras } from "../../../utils/navigationPaths";
import useBumpEffect from "../../../utils/useBumpEffect";
import "./FormStart.css";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { uiActions } from "../../../store/ui-slice";
import { formActions } from "../../../store/form-slice";
import { liveSettingsActions } from "../../../store/liveSettings-slice";
import {
  fetchPartyList,
  fetchCircuitList,
} from "../../../store/generalData-actions";

const buttonColor = "dark";

const FormParty = () => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // useSelector
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );
  const TOTALVotosGLOBAL = useSelector(
    (state) => state.form.reduxVotosTotalSteps
  );
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxPartyList = useSelector((state) => state.generalData.partyList);
  const delegadoId = useSelector((state) => state.auth.userId);

  // useStates
  const [isLoadingParty, setIsLoadingParty] = useState(false);
  const [votosPartyTotal, setVotosPartyTotal] = useState(0);
  const [isDisabledParty, setIsDisabledParty] = useState(false);
  const [isValidArrayParty, setIsValidArrayParty] = useState([true]);
  const [isValidFormParty, setIsValidFormParty] = useState(true);
  const [isSuccessParty, setIsSuccessParty] = useState(false);
  const [filteredPartyList, setFilteredPartyList] = useState([]);
  const [isBumped, triggerBump] = useBumpEffect();

  const { uploadData, error } = useAPI();

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Scroll to top of the page on startup
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  useEffect(() => {
    // Actualiza el conteo parcial de votos (total de partidos de la página actual)
    dispatch(formActions.setReduxVotosTotalSteps(votosPartyTotal));
  }, [votosPartyTotal, dispatch]);

  useEffect(() => {
    // redux set ON
    dispatch(uiActions.showCircuitName());

    return () => {
      // redux set OFF
      dispatch(uiActions.hideCircuitName());
    };
  }, [dispatch]);

  useEffect(() => {
    let list = getFilteredParties();
    setFilteredPartyList(list);

    // Filtrar los votos del partido del cliente
    const totalVotosParty = reduxSelectedCircuit?.listCircuitParties
      // .filter((circuitParty) => circuitParty.partyId !== reduxClient?.party.id) // Excluir el partido del cliente
      .reduce((total, circuitParty) => {
        return total + Number(circuitParty.totalPartyVotes || 0); // Asegurar que party.votes sea un número
      }, 0);

    setVotosPartyTotal(totalVotosParty);

    // SET REDUX ACA
    dispatch(formActions.setReduxVotosStep2(totalVotosParty));
  }, [reduxSelectedCircuit, reduxClient, setVotosPartyTotal, dispatch]);

  // Actualizo la cantidad de votos total para todas las listas (footer label)
  useEffect(() => {
    const totalVotosParty = filteredPartyList
      // .filter((party) => party.id !== reduxClient?.party.id) // Excluir el partido del cliente
      .reduce((total, party) => total + Number(party.votes), 0);

    setVotosPartyTotal(totalVotosParty);

    // SET REDUX ACA
    dispatch(formActions.setReduxVotosStep2(totalVotosParty));
  }, [filteredPartyList]);

  useEffect(() => {
    const initialArray = Array(reduxPartyList.length).fill(true);
    setIsValidArrayParty(initialArray);
  }, [reduxPartyList]);

  useEffect(() => {
    if (isSuccessParty) {
      dispatch(
        uiActions.setStepsSubmitted({ step: "step2", isSubmitted: true })
      );

      setTimeout(() => {
        navigate(FormExtras);
      }, 100);
    }
  }, [isSuccessParty, dispatch]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const getFilteredParties = () => {
    if (
      !reduxPartyList ||
      !reduxSelectedCircuit ||
      !reduxSelectedCircuit.listCircuitParties
    ) {
      return [];
    }

    // Crea una copia de la lista de partidos antes de ordenarla
    const sortedParties = [...reduxPartyList].sort((a, b) => a.id - b.id);

    // Continúa con la filtración y mapeo como antes
    const filteredAndMappedParties = sortedParties
      .filter((party) =>
        reduxSelectedCircuit.listCircuitParties.some(
          (circuitParty) => circuitParty.partyId === party.id
        )
      )
      .map((party) => {
        const circuitParty = reduxSelectedCircuit.listCircuitParties.find(
          (circuitParty) => circuitParty.partyId === party.id
        );
        return {
          ...party,
          votes: circuitParty?.totalPartyVotes || 0,
        };
      });

    // Priorizar el partido del cliente
    const clientPartyIndex = filteredAndMappedParties.findIndex(
      (party) => party.id === reduxClient?.party.id
    );
    if (clientPartyIndex > -1) {
      const [clientParty] = filteredAndMappedParties.splice(
        clientPartyIndex,
        1
      );
      filteredAndMappedParties.unshift(clientParty);
    }

    return filteredAndMappedParties;
  };

  const partyList1 = filteredPartyList?.map((party, index) => (
    <motion.div
      key={party.id}
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.8, delay: index * 0.1 }}
    >
      <WidgetCard
        key={party.id}
        id={String(party.id)}
        title={party.name}
        defaultValue={party.votes || 0}
        onValidityChange={(isValid) => validityHandlerParty(index, isValid)}
        onUpdateVotes={(newVotes) =>
          updateVotesHandlerParty(party.id, +newVotes)
        }
        disabled={isLoadingParty || party.id === reduxClient?.party.id}
        otherVotes={Number(TOTALVotosGLOBAL) || 0}
        name={party.name}
        photoURL={party.photoLongURL}
        maxValue={500}
        currentGlobalVotes={votosPartyTotal}
      />
    </motion.div>
  ));

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

  const formSubmitHandlerParty = async (event) => {
    event.preventDefault();

    if (!isValidArrayParty.every(Boolean)) {
      setIsValidFormParty(false);
      return;
    }

    setIsValidFormParty(true);
    setIsDisabledParty(true);

    // Actualizar PartyVotesList en reduxSelectedCircuit con los nuevos votos
    const updatedPartyVotesList = reduxSelectedCircuit?.listCircuitParties?.map(
      (circuitParty) => {
        const updatedVote = filteredPartyList.find(
          (party) => party.id == circuitParty.partyId
        )?.votes;
        return updatedVote !== undefined
          ? { ...circuitParty, totalPartyVotes: updatedVote }
          : circuitParty;
      }
    );

    setIsLoadingParty(true);

    let isSuccess = false;
    const circuitStep2DTO = preparePayload(updatedPartyVotesList);

    try {
      // HTTP Put a Circuits
      await uploadData(
        JSON.stringify(circuitStep2DTO),
        // urlCircuit,
        urlCircuitUpdateStep2,
        true,
        reduxSelectedCircuit.id
      );

      if (isSuccess) {
      } else if (error) {
        console.error("Error al actualizar el circuito:", error);
      }

      setIsSuccessParty(true);
      isSuccess = true;
    } catch (error) {
      console.error("Error al actualizar el circuito:", error);
      setIsSuccessParty(false);
    }

    setIsLoadingParty(false);

    // Si el envío fue exitoso, intenta actualizar el circuito
    if (isSuccess) {
      // Actualizar step en Redux
      dispatch(liveSettingsActions.setStepCompletedCircuit(2));

      fetchCircuitList();
      fetchPartyList();
    }

    // SET REDUX ACA
    dispatch(formActions.setReduxVotosStep2(votosPartyTotal)); //ToDo revisar

    // update lista
    setFilteredPartyList(getFilteredParties(reduxPartyList));

    // Redux fetch DB
    dispatch(fetchPartyList()); // refresh DB data
  };

  const validityHandlerParty = (index, isValid) => {
    setIsValidArrayParty((prevIsValidArray) => {
      const updatedIsValidArray = [...prevIsValidArray];
      updatedIsValidArray[index] = isValid;
      return updatedIsValidArray;
    });
  };

  const updateVotesHandlerParty = (partyId, newVotes) => {
    const updatedPartyList = filteredPartyList?.map((party) =>
      party.id === partyId ? { ...party, votes: newVotes } : party
    );
    setFilteredPartyList(updatedPartyList);
  };

  //#endregion Events ***********************************

  //#region JSX props ***********************************

  const preparePayload = (updatedListCircuitParties) => {
    // Update reduxSelectedCircuit: Actualizar circuito seleccionado en Redux - parte 2
    const updatedCircuit = {
      ...reduxSelectedCircuit,
      listCircuitParties: updatedListCircuitParties.map((circuitParty) => ({
        ...circuitParty,
        circuitId: circuitParty.circuitId,
        partyId: circuitParty.partyId,
        totalPartyVotes: circuitParty.totalPartyVotes,
        step2completed: true,
      })),
      listCircuitSlates: reduxSelectedCircuit?.listCircuitSlates,
    };

    // Update reduxSelectedCircuit
    dispatch(liveSettingsActions.setSelectedCircuit(updatedCircuit));

    const circuitStep2DTO = {
      Id: reduxSelectedCircuit?.id,
      Number: reduxSelectedCircuit?.number,
      Name: reduxSelectedCircuit?.name,
      ListCircuitParties: updatedCircuit?.listCircuitParties,
      LastUpdateDelegadoId: delegadoId,
      ClientId: reduxClient.id,
    };

    return circuitStep2DTO;
  };

  const labelSelectCircuit = (
    <span style={{ color: "blue", fontStyle: "italic", width: "auto" }}>
      Seleccione un circuito.
    </span>
  );

  //#endregion JSX props ***********************************

  return (
    <>
      <CCard className="mb-4">
        <CCardHeader>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            <div>Paso 2. Todos los partidos</div>
            <button
              onClick={bumpHandler}
              style={{ border: "none", background: "none", float: "right" }}
              className={isBumped ? "bump" : ""}
            >
              <FontAwesomeIcon icon={faHome} color="#697588" />{" "}
            </button>
          </div>
        </CCardHeader>
        <CCardBody>
          <CRow>
            <CCol xs={12} sm={12} md={12} lg={12} xl={12}>
              <CForm
                onSubmit={formSubmitHandlerParty}
                style={{ paddingBottom: "4rem" }}
              >
                <CRow className="justify-content-center">
                  {isLoadingParty ? (
                    <LoadingSpinner />
                  ) : reduxSelectedCircuit && reduxSelectedCircuit.id > 0 ? (
                    partyList1
                  ) : (
                    <>
                      {labelSelectCircuit}
                      <br />
                      <br />
                    </>
                  )}
                </CRow>
                <CCardFooter
                  className="text-medium-emphasis"
                  style={{ textAlign: "center" }}
                >
                  <div style={{ textAlign: "center" }}>
                    <CButton type="submit" color={buttonColor}>
                    Guardar
                    </CButton>
                  </div>
                </CCardFooter>
              </CForm>
            </CCol>
          </CRow>
        </CCardBody>
      </CCard>
    </>
  );
};

export default FormParty;
