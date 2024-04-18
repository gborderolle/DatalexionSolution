import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";

import { CForm, CCardFooter, CButton, CRow } from "@coreui/react";

import { LoadingSpinner } from "../../../utils/LoadingSpinner";
import WidgetCard from "../widgets/WidgetCard";

import { FormExtras } from "../../../utils/navigationPaths";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { fetchPartyList } from "../../../store/generalData-actions";
import { uiActions } from "../../../store/ui-slice";
import { formActions } from "../../../store/form-slice";

const buttonColor = "dark";

const FormParty2_OLD = ({
  formHandlerGeneric,
  isLoadingParty,
  setIsLoadingParty,
  setVotosPartyTotal,
  TOTALVotosGLOBAL = 0,
}) => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [isDisabledParty, setIsDisabledParty] = useState(false);
  const [isValidArrayParty, setIsValidArrayParty] = useState([true]);
  const [isValidFormParty, setIsValidFormParty] = useState(true);
  const [isSuccessParty, setIsSuccessParty] = useState(false);
  const [filteredPartyList, setFilteredPartyList] = useState([]);

  // const [votosPartyTotal, setVotosPartyTotal] = useState(0);
  const votosPartyTotal = 0;

  // redux gets
  const reduxPartyList = useSelector((state) => state.generalData.partyList);
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    let list = getFilteredParties();
    setFilteredPartyList(list);

    // Filtrar los votos del partido del cliente
    const totalVotosParty = reduxSelectedCircuit?.listCircuitParties
      // .filter((circuitParty) => circuitParty.partyId !== reduxClient?.party.id) // Excluir el partido del cliente
      .reduce((total, circuitParty) => {
        return total + Number(circuitParty.votes || 0); // Asegurar que party.votes sea un número
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

  //#region Events ***********************************

  const formSubmitHandlerParty = async (event) => {
    await formHandlerGeneric(
      event,
      isValidArrayParty,
      setIsValidFormParty,
      setIsDisabledParty,
      filteredPartyList,
      "partyList",
      setIsSuccessParty,
      setIsLoadingParty,
      reduxSelectedCircuit
    );

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
          votes: circuitParty?.votes || 0,
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

  //#region JSX props ***********************************

  const labelSelectCircuit = (
    <span style={{ color: "blue", fontStyle: "italic", width: "auto" }}>
      Seleccione un circuito.
    </span>
  );

  //#endregion JSX props ***********************************

  return (
    <>
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
              Siguiente
            </CButton>
          </div>
        </CCardFooter>
      </CForm>
    </>
  );
};
export default FormParty2_OLD;
