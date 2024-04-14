import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { urlCircuit } from "../../../endpoints";
import useAPI from "../../../hooks/use-API";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome } from "@fortawesome/free-solid-svg-icons";
import { CCard, CCardBody, CCol, CCardHeader, CRow } from "@coreui/react";

import FormParty2 from "./FormParty2";
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

const FormParty1 = () => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // redux get
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );

  // redux gets
  const [isLoadingParty, setIsLoadingParty] = useState(false);
  const TOTALVotosGLOBAL = useSelector(
    (state) => state.form.reduxVotosTotalSteps
  );
  const [votosPartyTotal, setVotosPartyTotal] = useState(0);
  const [isBumped, triggerBump] = useBumpEffect();

  const delegadoId = useSelector((state) => state.auth.userId);

  const { uploadData, error } = useAPI();

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Scroll to top of the page on startup
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  useEffect(() => {
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

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const formHandlerGeneric = async (
    event,
    isValidArray,
    setIsValidForm,
    setIsDisabled,
    list,
    collectionName,
    setIsSuccess,
    setLoading,
    reduxSelectedCircuit
  ) => {
    event.preventDefault();

    if (!isValidArray.every(Boolean)) {
      setIsValidForm(false);
      return;
    }

    setIsValidForm(true);
    setIsDisabled(true);

    // Actualizar PartyVotesList en reduxSelectedCircuit con los nuevos votos
    const updatedPartyVotesList = reduxSelectedCircuit?.listCircuitParties?.map(
      (partyVote) => {
        const updatedVote = list.find(
          (party) => party.id == partyVote.partyId
        )?.votes;
        return updatedVote !== undefined
          ? { ...partyVote, votes: updatedVote }
          : partyVote;
      }
    );

    setLoading(true);

    let isSuccess = false;
    const updatedCircuitPayload = preparePayload(updatedPartyVotesList);

    try {
      // HTTP Put a Circuits
      await uploadData(
        JSON.stringify(updatedCircuitPayload),
        urlCircuit,
        true,
        reduxSelectedCircuit.id
      );

      if (isSuccess) {
      } else if (error) {
        console.error("Error al actualizar el circuito:", error);
      }

      setIsSuccess(true);
      isSuccess = true;
    } catch (error) {
      console.error("Error al actualizar el circuito:", error);
      setIsSuccess(false);
    }

    setLoading(false);

    // Si el envío fue exitoso, intenta actualizar el circuito
    if (isSuccess) {
      dispatch(liveSettingsActions.setPartyVotesList(updatedPartyVotesList));

      // Actualizar step en Redux
      dispatch(liveSettingsActions.setStepCompletedCircuit(2));

      fetchCircuitList();
      fetchPartyList();
    }
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

  const preparePayload = (updatedPartyVotesList) => {
    // Update reduxSelectedCircuit: Actualizar circuito seleccionado en Redux - parte 2
    const updatedCircuit = {
      ...reduxSelectedCircuit,
      listCircuitParties: updatedPartyVotesList.map((party) => ({
        circuitId: party.circuitId,
        partyId: party.partyId,
        votes: party.votes,
      })),
      listCircuitSlates: reduxSelectedCircuit?.listCircuitSlates,
    };

    // Update reduxSelectedCircuit
    dispatch(liveSettingsActions.setSelectedCircuit(updatedCircuit));

    const updatedCircuitPayload = {
      Number: reduxSelectedCircuit?.number,
      Name: reduxSelectedCircuit?.name,
      Address: reduxSelectedCircuit?.address,
      BlankVotes: reduxSelectedCircuit?.blankVotes,
      NullVotes: reduxSelectedCircuit?.nullVotes,
      ObservedVotes: reduxSelectedCircuit?.observedVotes,
      RecurredVotes: reduxSelectedCircuit?.recurredVotes,
      ListCircuitParties: updatedCircuit?.listCircuitParties,
      ListCircuitSlates: reduxSelectedCircuit?.listCircuitSlates,
      Step1completed: reduxSelectedCircuit?.step1completed,
      Step2completed: true,
      Step3completed: reduxSelectedCircuit?.step3completed,
      LastUpdateDelegadoId: delegadoId,
    };

    return updatedCircuitPayload;
  };

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
              <FormParty2
                formHandlerGeneric={formHandlerGeneric}
                isLoading={isLoadingParty}
                setIsLoading={setIsLoadingParty}
                TOTALVotosParty={votosPartyTotal}
                setVotosPartyTotal={setVotosPartyTotal}
                TOTALVotosGLOBAL={TOTALVotosGLOBAL}
              />
            </CCol>
          </CRow>
        </CCardBody>
      </CCard>
    </>
  );
};

export default FormParty1;
