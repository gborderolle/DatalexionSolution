import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { urlCircuit } from "../../../endpoints";
import useAPI from "../../../hooks/use-API";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome } from "@fortawesome/free-solid-svg-icons";
import { CCard, CCardBody, CCol, CCardHeader, CRow } from "@coreui/react";

import FormSlate2 from "./FormSlate2";
import useBumpEffect from "../../../utils/useBumpEffect";
import "./FormStart.css";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { formActions } from "../../../store/form-slice";
import { liveSettingsActions } from "../../../store/liveSettings-slice";
import { uiActions } from "../../../store/ui-slice";
import {
  fetchPartyList,
  fetchCircuitList,
  fetchSlateList,
} from "../../../store/generalData-actions";

const FormSlate1 = () => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // redux get
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );
  const client = useSelector((state) => state.generalData.client);

  // redux gets
  const [isLoadingSlate, setIsLoadingSlate] = useState(false);
  const TOTALVotosGLOBAL = useSelector(
    (state) => state.form.reduxVotosTotalSteps
  );
  const [votosSlateTotal, setVotosSlateTotal] = useState(0);
  const [isBumped, triggerBump] = useBumpEffect();

  const [updatedPartyVotesList, setUpdatedPartyVotesList] = useState([]);
  const delegadoId = useSelector((state) => state.auth.userId);

  const { uploadData, error } = useAPI();

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Scroll to top of the page on startup
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  useEffect(() => {
    dispatch(formActions.setReduxVotosTotalSteps(votosSlateTotal));
  }, [votosSlateTotal, dispatch]);

  useEffect(() => {
    // redux set ON
    dispatch(uiActions.showCircuitName());

    return () => {
      // redux set OFF
      dispatch(uiActions.hideCircuitName());
    };
  }, [dispatch]);

  // Calcula la suma total de todos los votos (slate votes) y actualiza updatedPartyVotesList
  useEffect(() => {
    const totalVotes = reduxSelectedCircuit?.listCircuitSlates.reduce(
      (acc, slate) => acc + slate.votes,
      0
    );
    setVotosSlateTotal(totalVotes);

    setUpdatedPartyVotesList([
      {
        PartyId: client.party?.id,
        Votes: totalVotes,
      },
    ]);

    // Despacha la acción con la lista actualizada
    dispatch(liveSettingsActions.setPartyVotesList(updatedPartyVotesList));
  }, [reduxSelectedCircuit?.listCircuitSlates, dispatch]);

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

    // Actualizar SlateVotesList en reduxSelectedCircuit con los nuevos votos
    const updatedSlateVotesList = reduxSelectedCircuit?.listCircuitSlates?.map(
      (slateVote) => {
        const updatedVote = list.find(
          (slate) => slate.id === slateVote.slateId
        )?.votes;
        return updatedVote !== undefined
          ? { ...slateVote, votes: updatedVote }
          : slateVote;
      }
    );

    setLoading(true);

    let isSuccess = false;
    const updatedCircuitPayload = preparePayload(updatedSlateVotesList);

    try {
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
      dispatch(liveSettingsActions.setSlateVotesList(updatedSlateVotesList));
      dispatch(liveSettingsActions.setPartyVotesList(updatedPartyVotesList));

      // Actualizar step en Redux
      dispatch(liveSettingsActions.setStepCompletedCircuit(1));

      fetchCircuitList();
      fetchSlateList();
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

  const preparePayload = (updatedSlateVotesList) => {
    // Update reduxSelectedCircuit: Actualizar circuito seleccionado en Redux - parte 1
    const updatedListCircuitParties =
      reduxSelectedCircuit.listCircuitParties.map((party) => {
        if (party.partyId === client.party?.id) {
          return { ...party, votes: votosSlateTotal }; // Actualiza los votos
        }
        return party; // Retorna los partidos sin cambios si no es el partido del cliente
      });

    // Update reduxSelectedCircuit: Actualizar circuito seleccionado en Redux - parte 2
    const updatedCircuit = {
      ...reduxSelectedCircuit,
      listCircuitParties: updatedListCircuitParties,
      listCircuitSlates: updatedSlateVotesList.map((slate) => ({
        circuitId: slate.circuitId,
        slateId: slate.slateId,
        votes: slate.votes,
      })),
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
      ListCircuitSlates: updatedCircuit?.listCircuitSlates,
      ListCircuitParties: updatedCircuit?.listCircuitParties,
      Step1completed: true,
      Step2completed: reduxSelectedCircuit?.step2completed,
      Step3completed: reduxSelectedCircuit?.step3completed,
      LastUpdateDelegadoId: delegadoId,
    };

    return updatedCircuitPayload;
  };

  return (
    <CCard className="mb-4">
      <CCardHeader>
        Paso 1. Listas de mi partido
        <button
          onClick={bumpHandler}
          style={{ border: "none", background: "none", float: "right" }}
          className={isBumped ? "bump" : ""}
        >
          <FontAwesomeIcon icon={faHome} color="#697588" />{" "}
        </button>
      </CCardHeader>
      <CCardBody>
        <CRow>
          <CCol xs={12}>
            <FormSlate2
              formHandlerGeneric={formHandlerGeneric}
              isLoading={isLoadingSlate}
              setIsLoading={setIsLoadingSlate}
              myPartyImageURL={client?.party?.photoShort?.url}
              setVotosSlateTotal={setVotosSlateTotal}
              TOTALVotosGLOBAL={TOTALVotosGLOBAL}
            />
          </CCol>
        </CRow>
      </CCardBody>
    </CCard>
  );
};

export default FormSlate1;
