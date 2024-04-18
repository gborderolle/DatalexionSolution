import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { urlCircuit } from "../../../endpoints";
import useAPI from "../../../hooks/use-API";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome } from "@fortawesome/free-solid-svg-icons";
import { CCard, CCardBody, CCol, CCardHeader, CRow } from "@coreui/react";

import FormExtras2 from "./FormExtras2";
import useBumpEffect from "../../../utils/useBumpEffect";
import "./FormStart.css";

import { FormStart } from "../../../utils/navigationPaths";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { formActions } from "../../../store/form-slice";
import { uiActions } from "../../../store/ui-slice";
import { liveSettingsActions } from "../../../store/liveSettings-slice";
import { fetchCircuitList } from "../../../store/generalData-actions";

const FormExtras1 = () => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // redux get
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );

  // redux gets
  const [isLoadingExtras, setIsLoadingExtras] = useState(false);
  const TOTALVotosGLOBAL = useSelector(
    (state) => state.form.reduxVotosTotalSteps
  );
  const [TOTALVotosExtras, setTOTALVotosExtras] = useState(0);
  const [isBumped, triggerBump] = useBumpEffect();

  const { isSuccess, patchData } = useAPI();

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Scroll to top of the page on startup
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);

  useEffect(() => {
    dispatch(formActions.setReduxVotosTotalSteps(TOTALVotosExtras));
  }, [TOTALVotosExtras, dispatch]);

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
    list, // Este argumento puede ser tu lista de votos extras o cualquier otra lista relevante
    collectionName, // No usado en tu código de ejemplo, considera si es necesario
    setIsSuccess,
    setLoading,
    updatedSelectedCircuit,
    imageFile
  ) => {
    event.preventDefault();

    if (!isValidArray.every(Boolean)) {
      setIsValidForm(false);
      return;
    }

    setIsValidForm(true);
    setIsDisabled(true);
    setLoading(true);

    const formData = new FormData();

    // Agrega los votos y cualquier otro campo que desees actualizar
    formData.append("BlankVotes", updatedSelectedCircuit.blankVotes);
    formData.append("NullVotes", updatedSelectedCircuit.nullVotes);
    formData.append("ObservedVotes", updatedSelectedCircuit.observedVotes);
    formData.append("RecurredVotes", updatedSelectedCircuit.recurredVotes);

    formData.append(
      "updatedCircuitData",
      JSON.stringify(updatedSelectedCircuit)
    );

    // Define 'count' aquí para que sea accesible en todo el cuerpo de la función
    let count = reduxSelectedCircuit.imagesUploadedCount || 0;

    if (imageFile) {
      formData.append("Photos", imageFile, imageFile.name);
      count++; // Incrementa el contador localmente
    }

    try {
      // HTTP Patch a Circuits
      await patchData(formData, urlCircuit, reduxSelectedCircuit.id);

      setIsSuccess(true);
    } catch (error) {
      console.error("Error al actualizar el circuito:", error);
      setIsSuccess(false);
    }

    setLoading(false);

    // Si el envío fue exitoso, intenta actualizar el circuito
    if (isSuccess) {
      dispatch(liveSettingsActions.setStepCompletedCircuit(3));
      fetchCircuitList();
    }
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

  return (
    <>
      <CCard className="mb-4">
        <CCardHeader>
          Paso 3. Datos extras del circuito
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
            <CCol xs={12} sm={12} md={12} lg={12} xl={12}>
              <FormExtras2
                formHandlerGeneric={formHandlerGeneric}
                isLoading={isLoadingExtras}
                setIsLoading={setIsLoadingExtras}
                TOTALVotosExtras={TOTALVotosExtras ? TOTALVotosExtras : 0}
                setTOTALVotosExtras={
                  setTOTALVotosExtras ? setTOTALVotosExtras : 0
                }
                TOTALVotosGLOBAL={TOTALVotosGLOBAL ? TOTALVotosGLOBAL : 0}
              />
            </CCol>
          </CRow>
        </CCardBody>
      </CCard>
    </>
  );
};

export default FormExtras1;
