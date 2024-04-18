import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";

import { CForm, CCardFooter, CButton, CRow } from "@coreui/react";

import { LoadingSpinner } from "../../../utils/LoadingSpinner";
import ProfileCard from "../profileCard/ProfileCard";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import {
  fetchSlateList,
  fetchVotosTotal,
} from "../../../store/generalData-actions";
import { uiActions } from "../../../store/ui-slice";
import { formActions } from "../../../store/form-slice";
import { SlateGetWing, SlateGetCandidate } from "src/utils/auxiliarFunctions";

const buttonColor = "dark";
const FormSlate2_OLD = ({
  setVotosSlateTotal,
  formHandlerGeneric,
  isLoadingSlate,
  setIsLoadingSlate,
  myPartyImageURL,
  TOTALVotosGLOBAL = 0,
}) => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [isDisabledSlate, setIsDisabledSlate] = useState(false);
  const [isValidArraySlate, setIsValidArraySlate] = useState([true]);
  const [isValidFormSlate, setIsValidFormSlate] = useState(true);
  const [isSuccessSlate, setIsSuccessSlate] = useState(false);
  const [filteredSlateList, setFilteredSlateList] = useState([]);

  // const [votosSlateTotal, setVotosSlateTotal] = useState(0);
  const votosSlateTotal = 0;

  // redux gets
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxSlateList = useSelector(
    (state) => state.generalData.slateList || []
  );
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );
  const reduxCandidateList = useSelector(
    (state) => state.generalData.candidateList || []
  );
  const reduxWingList = useSelector(
    (state) => state.generalData.wingList || []
  );

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    const fetchData = async () => {
      if (reduxSelectedCircuit) {
        await dispatch(fetchVotosTotal(reduxSelectedCircuit));
      }
    };

    fetchData();

    let list = getFilteredSlates();
    setFilteredSlateList(list);

    const totalVotosSlate = list.reduce(
      (total, slate) => total + Number(slate.votes),
      0
    );

    setVotosSlateTotal(totalVotosSlate);

    // SET REDUX ACA
    dispatch(formActions.setReduxVotosStep1(totalVotosSlate));
  }, [reduxSelectedCircuit, setVotosSlateTotal]);

  // Actualizo la cantidad de votos total para todas las listas (footer label)
  useEffect(() => {
    const totalVotosSlate = filteredSlateList.reduce(
      (total, slate) => total + Number(slate.votes),
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

  //#endregion Hooks ***********************************

  //#region Events ***********************************

  const formSubmitHandlerSlate = async (event) => {
    await formHandlerGeneric(
      event,
      isValidArraySlate,
      setIsValidFormSlate,
      setIsDisabledSlate,
      filteredSlateList,
      "slateList",
      setIsSuccessSlate,
      setIsLoadingSlate,
      reduxSelectedCircuit
    );
    setIsLoadingSlate(false);

    // SET REDUX ACA
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
          votes: circuitSlate.votes || 0, // Asegura que votes siempre sea un número
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
  const slateList1 = filteredSlateList?.map((slate, index) => {
    const candidate = SlateGetCandidate(slate, reduxCandidateList);
    const partyImageURL = slate.photoURL ? slate.photoURL : myPartyImageURL;
    const candidateImageURL = candidate?.photoURL
      ? candidate.photoURL
      : partyImageURL;

    return (
      <motion.div
        key={slate.id}
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.8, delay: index * 0.1 }}
      >
        <ProfileCard
          key={slate.id}
          id={String(slate.id)}
          title={slate.name}
          defaultValue={slate.votes}
          onValidityChange={(isValid) => validityHandlerSlate(index, isValid)}
          onUpdateVotes={(newVotes) =>
            updateVotesHandlerSlate(slate.id, +newVotes)
          }
          disabled={isDisabledSlate}
          otherVotes={Number(TOTALVotosGLOBAL)}
          name={slate.name}
          partyImageURL={partyImageURL}
          candidateImageURL={candidateImageURL}
          maxValue={500}
          currentGlobalVotes={votosSlateTotal}
        />
      </motion.div>
    );
  });

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
        onSubmit={formSubmitHandlerSlate}
        style={{ paddingBottom: "4rem" }}
      >
        <CRow className="justify-content-center">
          {isLoadingSlate ? (
            <LoadingSpinner />
          ) : reduxSelectedCircuit && reduxSelectedCircuit.id > 0 ? (
            slateList1
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
export default FormSlate2_OLD;
