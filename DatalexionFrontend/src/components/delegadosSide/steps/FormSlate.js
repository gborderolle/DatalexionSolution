import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";

import { urlCircuitUpdateStep1 } from "../../../endpoints";
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
import ProfileCard from "../profileCard/ProfileCard";

import { FormParty, FormStart } from "../../../utils/navigationPaths";
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
  fetchVotosTotal,
} from "../../../store/generalData-actions";
import { SlateGetWing, SlateGetCandidate } from "src/utils/auxiliarFunctions";

const buttonColor = "dark";

const FormSlate = () => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const { uploadData, error } = useAPI();

  // useSelector
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxSlateList = useSelector(
    (state) => state.generalData.slateList || []
  );
  const reduxCandidateList = useSelector(
    (state) => state.generalData.candidateList || []
  );
  const reduxWingList = useSelector(
    (state) => state.generalData.wingList || []
  );
  const TOTALVotosGLOBAL = useSelector(
    (state) => state.form.reduxVotosTotalSteps
  );
  const delegadoId = useSelector((state) => state.auth.userId);

  // useStates
  const [isLoadingSlate, setIsLoadingSlate] = useState(false);
  const [votosSlateTotal, setVotosSlateTotal] = useState(0);
  const [isBumped, triggerBump] = useBumpEffect();
  const [updatedPartyVotesList, setUpdatedPartyVotesList] = useState([]);
  const [isDisabledSlate, setIsDisabledSlate] = useState(false);
  const [isValidArraySlate, setIsValidArraySlate] = useState([true]);
  const [isValidFormSlate, setIsValidFormSlate] = useState(true);
  const [isSuccessSlate, setIsSuccessSlate] = useState(false);
  const [filteredSlateList, setFilteredSlateList] = useState([]);
  const [isFormDirty, setIsFormDirty] = useState(false);

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Scroll to top of the page on startup
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  useEffect(() => {
    // Actualiza el conteo parcial de votos (total de slates de la página actual)
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
    if (reduxSelectedCircuit && reduxSelectedCircuit.listCircuitSlates) {
      const totalVotes = reduxSelectedCircuit.listCircuitSlates.reduce(
        (acc, slate) => acc + (slate.totalSlateVotes || 0), // Ensure votes is a number
        0
      );
      setVotosSlateTotal(totalVotes);
    }
  }, [reduxSelectedCircuit, dispatch]);

  useEffect(() => {
    const fetchData = async () => {
      if (reduxSelectedCircuit) {
        await dispatch(fetchVotosTotal(reduxSelectedCircuit));
      }
    };

    fetchData();

    let circuitSlates = getFilteredSlates();
    setFilteredSlateList(circuitSlates);

    const totalVotosSlate = circuitSlates.reduce(
      (total, circuitSlate) => total + Number(circuitSlate.votes),
      0
    );

    setVotosSlateTotal(totalVotosSlate);

    // SET REDUX ACA
    dispatch(formActions.setReduxVotosStep1(totalVotosSlate));
  }, [reduxSelectedCircuit, setVotosSlateTotal]);

  // Actualizo la cantidad de votos total para todas las listas (footer label)
  useEffect(() => {
    const totalVotosSlate = filteredSlateList.reduce(
      (total, circuitSlate) => total + Number(circuitSlate.votes),
      0
    );

    setVotosSlateTotal(totalVotosSlate);

    // SET REDUX ACA
    dispatch(formActions.setReduxVotosStep1(totalVotosSlate));
  }, [filteredSlateList]);

  useEffect(() => {
    if (isSuccessSlate) {
      dispatch(
        uiActions.setStepsSubmitted({ step: "step1", isSubmitted: true })
      );

      setTimeout(() => {
        navigate(FormParty);
      }, 100);
    }
  }, [isSuccessSlate, dispatch]);

  useEffect(() => {
    const confirmNavigation = (e) => {
      if (isFormDirty) {
        const confirmMessage =
          "Tienes cambios sin guardar. ¿Estás seguro de que quieres salir?";
        e.returnValue = confirmMessage; // Para navegadores estándar
        return confirmMessage; // Para algunos navegadores más antiguos
      }
    };

    window.addEventListener("beforeunload", confirmNavigation);
    return () => {
      window.removeEventListener("beforeunload", confirmNavigation);
    };
  }, [isFormDirty]);

  const handleNavigateAway = () => {
    if (
      isFormDirty &&
      !window.confirm(
        "Tienes cambios sin guardar. ¿Estás seguro de que quieres salir?"
      )
    ) {
      return; // Detener la navegación si hay cambios sin guardar y el usuario no confirma
    }
    navigate(FormParty); // Reemplaza esto con tu ruta objetivo
  };

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const getFilteredSlates = () => {
    if (
      !reduxSlateList ||
      !reduxSelectedCircuit ||
      !reduxSelectedCircuit.listCircuitSlates ||
      !reduxClient ||
      !reduxClient.party ||
      !reduxWingList
    ) {
      return [];
    }

    // Filtra las listas de circuito primero, para obtener solo aquellas que pertenecen al partido del cliente
    const filteredCircuitSlates = reduxSelectedCircuit.listCircuitSlates.filter(
      (circuitSlate) => {
        const slate = reduxSlateList.find(
          (slate) => slate.id === circuitSlate?.slateId
        );
        const wing = SlateGetWing(slate, reduxWingList);
        return slate && wing && wing?.partyId === reduxClient?.party.id;
      }
    );

    // Mapea y ordena
    const mappedAndSortedSlates = filteredCircuitSlates
      .map((circuitSlate) => {
        const slateDetail = reduxSlateList.find(
          (slate) => slate.id === circuitSlate?.slateId
        );
        return {
          ...slateDetail,
          votes: circuitSlate.totalSlateVotes || 0, // Asegura que votes siempre sea un número
        };
      })
      .sort((a, b) => {
        // Primero por candidateId
        if (a.candidateId && b.candidateId) {
          if (a.candidateId < b.candidateId) return -1;
          if (a.candidateId > b.candidateId) return 1;
        } else if (a.candidateId && !b.candidateId) {
          return -1;
        } else if (!a.candidateId && b.candidateId) {
          return 1;
        }
        // Luego por nombre si candidateId es igual o solo si uno de los dos no tiene candidateId
        return b.name.localeCompare(a.name);
      });

    return mappedAndSortedSlates;
  };

  // Por cada Slate creo una card con su input votos
  const slateListJSX = filteredSlateList?.map((circuit, index) => {
    const candidate = SlateGetCandidate(circuit, reduxCandidateList);
    const partyImageURL = circuit.photoURL
      ? circuit.photoURL
      : reduxClient?.party?.photoShort?.url;
    const candidateImageURL = candidate?.photoURL
      ? candidate.photoURL
      : partyImageURL;

    return (
      <motion.div
        key={circuit.id}
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.8, delay: index * 0.1 }}
      >
        <ProfileCard
          key={circuit.id}
          id={String(circuit.id)}
          title={circuit.name}
          defaultValue={circuit.votes}
          onValidityChange={(isValid) => validityHandlerSlate(index, isValid)}
          onUpdateVotes={(newVotes) =>
            updateVotesHandlerSlate(circuit.id, +newVotes)
          }
          disabled={isDisabledSlate}
          otherVotes={Number(TOTALVotosGLOBAL)}
          name={circuit.name}
          partyImageURL={partyImageURL}
          candidateImageURL={candidateImageURL}
          maxValue={500}
          currentGlobalVotes={votosSlateTotal}
        />
      </motion.div>
    );
  });

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

  const formSubmitHandlerSlate = async (event) => {
    event.preventDefault();

    if (!isValidArraySlate.every(Boolean)) {
      setIsValidFormSlate(false);
      return;
    }

    setIsValidFormSlate(true);
    setIsDisabledSlate(true);

    // Actualizar SlateVotesList en reduxSelectedCircuit con los nuevos votos
    const updatedSlateVotesList = reduxSelectedCircuit?.listCircuitSlates?.map(
      (circuitSlate) => {
        const updatedVote = filteredSlateList.find(
          (slate) => slate.id === circuitSlate.slateId
        )?.votes;
        return updatedVote !== undefined
          ? { ...circuitSlate, totalSlateVotes: updatedVote }
          : circuitSlate;
      }
    );

    setIsLoadingSlate(true);

    let isSuccess = false;
    const circuitStep1DTO = preparePayload(updatedSlateVotesList);

    try {
      // HTTP Put (especial) a Circuits
      // debugger;
      await uploadData(
        JSON.stringify(circuitStep1DTO),
        // urlCircuitPut,
        urlCircuitUpdateStep1,
        true,
        reduxSelectedCircuit.id
      );

      if (isSuccess) {
      } else if (error) {
        console.error("Error al actualizar el circuito:", error);
      }

      setIsSuccessSlate(true);
      isSuccess = true;
    } catch (error) {
      console.error("Error al actualizar el circuito:", error);
      setIsSuccessSlate(false);
    }

    // Si el envío fue exitoso, intenta actualizar el circuito
    if (isSuccess) {
      setIsFormDirty(false);

      // Actualizar step en Redux
      dispatch(liveSettingsActions.setStepCompletedCircuit(1));

      fetchCircuitList();
      fetchSlateList();
      fetchPartyList();
    }

    setIsLoadingSlate(false);

    // SET REDUX ACA --> No es necesario acá, porque se guarda cada vez que cambia el valor de voto, verificar.
    dispatch(formActions.setReduxVotosStep1(votosSlateTotal));

    // Redux fetch DB
    dispatch(fetchSlateList()); // refresh DB data
  };

  const validityHandlerSlate = (index, isValid) => {
    setIsValidArraySlate((prevIsValidArray) => {
      const updatedIsValidArray = [...prevIsValidArray];
      updatedIsValidArray[index] = isValid;
      return updatedIsValidArray;
    });
  };

  const updateVotesHandlerSlate = (slateId, newVotes) => {
    setFilteredSlateList((prevSlates) =>
      prevSlates?.map((slate) =>
        slate.id === slateId ? { ...slate, votes: newVotes } : slate
      )
    );
  };

  //#endregion Events ***********************************

  //#region JSX props ***********************************

  const preparePayload = (updatedListCircuitSlates) => {
    // Update reduxSelectedCircuit: Actualizar circuito seleccionado en Redux - parte 1
    const updatedListCircuitParties =
      reduxSelectedCircuit.listCircuitParties.map((party) => {
        if (party.partyId === reduxClient.party?.id) {
          return {
            ...party,
            totalPartyVotes: votosSlateTotal,
            step1completed: true,
          }; // Actualiza los votos
        }
        return party; // Retorna los partidos sin cambios si no es el partido del cliente
      });

    // Update reduxSelectedCircuit: Actualizar circuito seleccionado en Redux - parte 2
    const updatedCircuit = {
      ...reduxSelectedCircuit,
      listCircuitParties: updatedListCircuitParties,
      listCircuitSlates: updatedListCircuitSlates.map((circuitSlate) => ({
        circuitId: circuitSlate.circuitId,
        slateId: circuitSlate.slateId,
        totalSlateVotes: circuitSlate.totalSlateVotes,
      })),
    };

    // Update reduxSelectedCircuit
    dispatch(liveSettingsActions.setSelectedCircuit(updatedCircuit));

    const circuitStep1DTO = {
      Id: reduxSelectedCircuit?.id,
      Number: reduxSelectedCircuit?.number,
      Name: reduxSelectedCircuit?.name,
      ListCircuitSlates: updatedCircuit?.listCircuitSlates,
      LastUpdateDelegadoId: delegadoId,
      ClientId: reduxClient.id,
    };

    return circuitStep1DTO;
  };

  const labelSelectCircuit = (
    <span style={{ color: "blue", fontStyle: "italic", width: "auto" }}>
      Seleccione un circuito.
    </span>
  );

  //#endregion JSX props ***********************************

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
            <CForm
              onSubmit={formSubmitHandlerSlate}
              style={{ paddingBottom: "4rem" }}
            >
              <CRow className="justify-content-center">
                {isLoadingSlate ? (
                  <LoadingSpinner />
                ) : reduxSelectedCircuit && reduxSelectedCircuit.id > 0 ? (
                  slateListJSX
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
  );
};

export default FormSlate;
