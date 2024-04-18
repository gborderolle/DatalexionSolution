import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";

let backendURL = process.env.REACT_APP_URL;

import {
  CForm,
  CButton,
  CCol,
  CRow,
  CCard,
  CCardTitle,
  CCardBody,
  CCardFooter,
  CFormTextarea,
} from "@coreui/react";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes } from "@fortawesome/free-solid-svg-icons";

import { LoadingSpinner } from "../../../utils/LoadingSpinner";
import WidgetCard from "../widgets/WidgetCard";

import { FormSummary } from "../../../utils/navigationPaths";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { liveSettingsActions } from "../../../store/liveSettings-slice";
import { uiActions } from "../../../store/ui-slice";
import { formActions } from "../../../store/form-slice";

import "./FormExtras2.css";

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

const FormExtras2_OLD = ({
  formHandlerGeneric,
  isLoadingExtras,
  setIsLoadingExtras,
  setTOTALVotosExtras,
  TOTALVotosGLOBAL = 0,
}) => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [isDisabledExtras, setIsDisabledExtras] = useState(false);
  const [isValidArrayExtras, setIsValidArrayExtras] = useState([true]);
  const [isValidFormExtras, setIsValidFormExtras] = useState(true);
  const [isSuccessExtras, setIsSuccessExtras] = useState(false);
  const [imageFile, setImageFile] = useState(null);
  const [imagesList, setImagesList] = useState([]);
  const [fixedCards, setFixedCards] = useState(initialFixedCards);
  const [votosExtrasTotal, setVotosExtrasTotal] = useState(0);
  const [txbComments, setTxbComments] = useState("");

  // redux gets
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  // Esta función suma todos los votos extras de las tarjetas
  const sumarVotosExtras = (tarjetas) => {
    return tarjetas.reduce(
      (acumulado, tarjeta) => acumulado + tarjeta.votes,
      0
    );
  };

  useEffect(() => {
    // Calcula el total de votos extras cada vez que 'fixedCards' cambie
    const totalVotosExtras = sumarVotosExtras(fixedCards);
    setVotosExtrasTotal(totalVotosExtras);
    setTOTALVotosExtras(totalVotosExtras);

    // SET REDUX ACA
    dispatch(formActions.setReduxVotosStep3(totalVotosExtras));
  }, [fixedCards, setTOTALVotosExtras]); // Dependencia 'fixedCards' asegura que el efecto se ejecute cada vez que cambian los votos

  useEffect(() => {
    if (reduxSelectedCircuit) {
      // Define los datos del circuito en función de reduxSelectedCircuit
      const circuitData = {
        pushId: reduxSelectedCircuit.id,
        nullVotes: reduxSelectedCircuit.nullVotes,
        blankVotes: reduxSelectedCircuit.blankVotes,
        recurredVotes: reduxSelectedCircuit.recurredVotes,
        observedVotes: reduxSelectedCircuit.observedVotes,
      };

      setTxbComments(reduxSelectedCircuit.comments);

      // Calcula y establece el total de votos extras.
      const totalVotes =
        circuitData.nullVotes +
        circuitData.blankVotes +
        circuitData.recurredVotes +
        circuitData.observedVotes;
      setTOTALVotosExtras(totalVotes);
    }
  }, []);

  useEffect(() => {
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

  useEffect(() => {
    if (isSuccessExtras) {
      dispatch(
        uiActions.setStepsSubmitted({ step: "step3", isSubmitted: true })
      );

      setTimeout(() => {
        // Poner step Resumen en azul (activo)
        navigate(FormSummary);
      }, 100);
    }
  }, [isSuccessExtras, dispatch]);

  //#endregion Hooks ***********************************

  //#region Events ***********************************

  const handleDeleteImage = async (imageURL) => {};

  const handleImageChange = (e) => {
    if (e.target.files[0]) {
      const file = e.target.files[0];
      setImageFile(file); // Aquí faltaba asignar el archivo a la variable de estado imageFile
      const reader = new FileReader();
      reader.onloadend = () => {
        // setImageUrl(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const formSubmitHandlerExtras = async (event) => {
    event.preventDefault();

    if (reduxSelectedCircuit && reduxSelectedCircuit.id) {
      const updatedVotes = fixedCards.reduce((acc, card) => {
        const voteKey = card.id.replace("circuit", ""); // Transforma "circuitNullVotes" en "NullVotes"
        acc[voteKey.charAt(0).toLowerCase() + voteKey.slice(1)] = card.votes; // Convierte "NullVotes" en "nullVotes" y asigna el valor
        return acc;
      }, {});

      // Verifica la validez del formulario antes de proceder
      const allValid = isValidArrayExtras.every(Boolean);
      if (!allValid) {
        setIsValidFormExtras(false);
        return;
      }

      setIsValidFormExtras(true);
      setIsDisabledExtras(true);
      setIsLoadingExtras(true);

      const updatedSelectedCircuit = {
        ...reduxSelectedCircuit,
        step3completed: true,
        imagesUploadedCount: 0,
        comments: txbComments,

        nullVotes: updatedVotes.nullVotes,
        blankVotes: updatedVotes.blankVotes,
        recurredVotes: updatedVotes.recurredVotes,
        observedVotes: updatedVotes.observedVotes,
      };
      dispatch(liveSettingsActions.setSelectedCircuit(updatedSelectedCircuit));

      await formHandlerGeneric(
        event,
        isValidArrayExtras,
        setIsValidFormExtras,
        setIsDisabledExtras,
        null,
        "circuit",
        setIsSuccessExtras,
        setIsLoadingExtras,
        updatedSelectedCircuit,
        imageFile
      );
    }
  };

  const validityHandlerExtras = (index, isValid) => {
    setIsValidArrayExtras((prevIsValidArray) => {
      const updatedIsValidArray = [...prevIsValidArray];
      updatedIsValidArray[index] = isValid;
      return updatedIsValidArray;
    });
  };

  const updateVotesHandlerExtras = (cardId, newVotes) => {
    setFixedCards((currentFixedCards) => {
      return currentFixedCards?.map((card) => {
        if (card.id == cardId) {
          // Solo actualiza el cardVotes del card correspondiente
          return { ...card, votes: newVotes };
        } else {
          // Para los demás cards, devuelve el objeto como está
          return card;
        }
      });
    });
  };

  //#endregion Events ***********************************

  //#region Functions ***********************************

  const cardList = fixedCards?.map((card, index) => {
    return (
      <motion.div
        key={card.id}
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.8, delay: index * 0.1 }}
      >
        <WidgetCard
          key={index}
          id={String(card.id)}
          title={card.name}
          defaultValue={card.votes}
          onValidityChange={(isValid, votes) =>
            validityHandlerExtras(index, isValid)
          }
          onUpdateVotes={(newVotes) =>
            updateVotesHandlerExtras(card.id, +newVotes)
          }
          disabled={isDisabledExtras}
          otherVotes={Number(TOTALVotosGLOBAL)}
          name={card.name}
          photoURL={card.photoURL}
          maxValue={500}
          currentGlobalVotes={votosExtrasTotal}
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
        onSubmit={formSubmitHandlerExtras}
        style={{ paddingBottom: "4rem" }}
      >
        <CRow className="justify-content-center">
          {isLoadingExtras ? (
            <LoadingSpinner />
          ) : reduxSelectedCircuit ? (
            cardList
          ) : (
            <>
              {labelSelectCircuit}
              <br />
              <br />
            </>
          )}
        </CRow>
        <CRow className="justify-content-center">
          <CCol sm={6} lg={3}>
            <CCard className="text-center">
              &nbsp;
              <CCardTitle>Comentarios</CCardTitle>
              <CCardBody>
                <CFormTextarea
                  id="txbComments"
                  value={txbComments || ""}
                  onChange={(e) => setTxbComments(e.target.value)}
                />
              </CCardBody>
            </CCard>
          </CCol>
        </CRow>
        <br />
        <CRow className="justify-content-center">
          <CCol sm={6} lg={3}>
            <CCard className="text-center">
              &nbsp;
              <CCardTitle>Carga del Acta Oficial</CCardTitle>
              <CCardBody>
                <ul
                  className="left-aligned-list"
                  style={{ paddingLeft: "12px" }}
                >
                  {imagesList &&
                    imagesList.map((image, index) => (
                      <li key={index}>
                        {image.name}
                        &nbsp;
                        <a
                          href={image.url}
                          target="_blank"
                          rel="noopener noreferrer"
                        >
                          Descargar
                        </a>
                        &nbsp;
                        <FontAwesomeIcon
                          icon={faTimes}
                          onClick={() => handleDeleteImage(image.url)}
                          className="text-danger"
                        />
                      </li>
                    ))}
                </ul>

                <br />

                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImageChange}
                />
              </CCardBody>
            </CCard>
          </CCol>
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

export default FormExtras2_OLD;
