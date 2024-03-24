import React, { useState, useEffect, useReducer } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";
import { isMobile } from "react-device-detect";

// Redux imports
import { useDispatch } from "react-redux";
import { loginDelegadosHandler } from "../../../store/auth-actions";
import { authActions } from "../../../store/auth-slice";

import useBumpEffect from "../../../utils/useBumpEffect";

import {
  CButton,
  CCard,
  CCardBody,
  CCardGroup,
  CCol,
  CContainer,
  CForm,
  CFormInput,
  CInputGroup,
  CInputGroupText,
  CRow,
  CSidebarBrand,
  CImage,
  CAlert,
} from "@coreui/react";
import CIcon from "@coreui/icons-react";
import { cilAddressBook } from "@coreui/icons";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faShuffle } from "@fortawesome/free-solid-svg-icons";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";

import LoginFooter from "./LoginFooter";

import logo from "src/assets/images/datalexion-logo-big.png";
import intro from "src/assets/images/dx-motion.png";
import { sygnet } from "src/assets/brand/sygnet";

import classes from "./Login.module.css";
import "./LoginDelegados.css";

//#region Functions

const ciReducer = (state, action) => {
  if (action.type == "USER_INPUT") {
    // Expresión regular que valida un número de exactamente 7 dígitos seguido por un guión y un último dígito
    const regex = /^[0-9]{7}-[0-9]$/;
    return { value: action.val, isValid: regex.test(action.val) };
  }
  if (action.type == "INPUT_BLUR") {
    // Misma expresión regular para cuando el input pierde el foco
    const regex = /^[0-9]{7}-[0-9]$/;
    return { value: state.value, isValid: regex.test(state.value) };
  }
  return { value: "", isValid: false };
};

//#endregion Functions

const buttonColor = "dark";

const LoginDelegados = () => {
  //#region Consts

  const [isLoggingIn, setIsLoggingIn] = useState(false);
  const [isMobileDevice, setIsMobileDevice] = useState(isMobile);
  const [isBumped, triggerBump] = useBumpEffect();
  const [errorMessage, setErrorMessage] = useState("");

  const [cardState, setCardState] = useState("expanded");
  const [showCCard4, setShowCCard4] = useState(true);
  const [showEffectCC3, setShowEffectCC3] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    dispatch(authActions.setIsMobile(isMobile)); // Suponiendo que tienes una acción para esto
  }, [isMobile]);

  // Redux call actions
  const dispatch = useDispatch();

  const [ciState, dispatchCI] = useReducer(ciReducer, {
    value: "",
    isValid: false,
  });

  const cardBodyVariantsDesktop = {
    hidden: { x: "-100%" },
    visible: { x: 0 },
  };

  const cardBodyVariantsMobile = {
    hidden: { y: "-100%" },
    visible: { y: 0 },
  };

  //#endregion Consts

  //#region Hooks

  useEffect(() => {
    // redux call
    dispatch(authActions.resetAuthState());
  }, []);

  useEffect(() => {
    // Función para actualizar el estado en caso de cambio de orientación o tamaño
    const handleResize = () => {
      setIsMobileDevice(isMobile);
    };

    // Agregar listeners
    window.addEventListener("resize", handleResize);
    window.addEventListener("orientationchange", handleResize);

    // Remover listeners al desmontar el componente
    return () => {
      window.removeEventListener("resize", handleResize);
      window.removeEventListener("orientationchange", handleResize);
    };
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      setShowCCard4(false); // Inicia el fade out después de un tiempo determinado
    }, 2000); // Aquí puedes ajustar el tiempo según necesites

    return () => clearTimeout(timer); // Limpia el temporizador si el componente se desmonta
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      setShowEffectCC3(true); // Inicia el efecto después de 3 segundos
    }, 2050);

    return () => clearTimeout(timer); // Limpia el temporizador si el componente se desmonta
  }, []);

  //#endregion Hooks

  //#region Functions

  const ciChangeHandler = (event) => {
    // Extrayendo el valor del input
    let inputValue = event.target.value;

    // Removiendo caracteres no numéricos
    inputValue = inputValue.replace(/\D/g, "");

    // Limitando la longitud a 8 dígitos
    inputValue = inputValue.substring(0, 8);

    // Agregando el guión antes del último dígito
    if (inputValue.length == 8) {
      inputValue = `${inputValue.substring(0, 7)}-${inputValue.substring(
        7,
        8
      )}`;
    }

    // Despachando el evento con el valor formateado
    dispatchCI({ type: "USER_INPUT", val: inputValue });
  };

  const validateCIHandler = () => {
    dispatchCI({ type: "INPUT_BLUR" });
  };

  const submitHandler = (event) => {
    event.preventDefault();
    setIsLoggingIn(true); // Activar el spinner
    const ciValue = ciState.value.replace(/-/g, "");
    dispatch(loginDelegadosHandler(ciValue, navigate)).then(() => {
      setIsLoggingIn(false); // Desactivar el spinner una vez completado el login
    });
  };

  const bumpHandler = () => {
    triggerBump();

    setTimeout(() => {
      navigate("/login-general");
    }, 200);
  };

  //#endregion Functions

  return (
    <div className="bg-light min-vh-100 d-flex flex-row align-items-center">
      <CContainer>
        <CRow className="justify-content-center">
          <CCol md={8}>
            <CCardGroup>
              <CCard id="cc1" className="p-4">
                <CCardBody>
                  <CForm onSubmit={submitHandler}>
                    <h1>Delegados</h1>
                    <p className="text-medium-emphasis">Ingrese su cédula</p>
                    <CInputGroup className="mb-3">
                      <CInputGroupText>
                        <CIcon icon={cilAddressBook} />
                      </CInputGroupText>
                      <CFormInput
                        placeholder="1234567-8"
                        onBlur={validateCIHandler}
                        onChange={ciChangeHandler}
                        value={ciState.value}
                        valid={ciState.isValid}
                        invalid={!ciState.isValid && ciState.value !== ""}
                        feedback={
                          ciState.isValid
                            ? "El formato es correcto"
                            : "El formato es incorrecto"
                        }
                        required
                      />
                    </CInputGroup>

                    {errorMessage && (
                      <div className="error-message">{errorMessage}</div>
                    )}

                    <CRow>
                      <CCol
                        xs={12}
                        style={{ margin: "auto", textAlign: "center" }}
                      >
                        <CButton
                          color={buttonColor}
                          type="submit"
                          className="px-4"
                          disabled={!isMobileDevice} // Se deshabilita si isMobile es true
                        >
                          {isLoggingIn ? (
                            <FontAwesomeIcon icon={faSpinner} spin />
                          ) : (
                            <FontAwesomeIcon icon={faCheck} />
                          )}
                          &nbsp;Login
                        </CButton>
                        {!isMobileDevice && (
                          <CAlert color="danger" style={{ marginTop: "10px" }}>
                            La aplicación Delegados solo está disponible para
                            dispositivos móviles.
                          </CAlert>
                        )}
                      </CCol>
                    </CRow>
                  </CForm>
                </CCardBody>
              </CCard>
              <CCard id="cc2" className={`bg ${classes.loginRight}`}>
                {showEffectCC3 && (
                  <motion.div
                    initial="hidden"
                    animate="visible"
                    // transition={{ type: "spring", bounce: 0.7, duration: 2 }}
                    transition={{ type: "spring", bounce: 0.4, duration: 1 }}
                    variants={
                      isMobile
                        ? cardBodyVariantsMobile
                        : cardBodyVariantsDesktop
                    }
                  >
                    <CCard id="cc3" className={`p-3 text-white bg-dark py-5`}>
                      <CCardBody className="text-center">
                        <button
                          onClick={bumpHandler}
                          style={{
                            border: "none",
                            background: "none",
                            position: "absolute",
                            top: "10px",
                            right: "10px",
                          }}
                          className={isBumped ? "bump" : ""}
                        >
                          <FontAwesomeIcon icon={faShuffle} color="#ffff" />{" "}
                        </button>
                        <div>
                          <CSidebarBrand className="d-md-flex" to="/">
                            <CImage fluid src={logo} width={150} />
                            <CIcon
                              className="sidebar-brand-narrow"
                              icon={sygnet}
                              height={35}
                            />
                          </CSidebarBrand>
                          <p className={classes.p}>
                            Inteligencia Artificial analítica para optimizar y
                            procesar la información electoral en tiempo real.
                          </p>
                        </div>
                      </CCardBody>
                    </CCard>
                  </motion.div>
                )}

                {showCCard4 && (
                  <motion.div
                    initial={{ opacity: 1 }}
                    animate={{ opacity: 0 }}
                    transition={{ ease: "easeOut", delay: 2 }}
                  >
                    <CCard
                      id="cc4"
                      className={`p-3 text-white bg-dark py-5 fullViewportCard`}
                    >
                      <CCardBody className="text-center cardBodyCentered">
                        <div>
                          <CSidebarBrand className="d-md-flex" to="/">
                            <motion.div
                              initial={{ rotate: 0 }}
                              animate={{ rotate: 720 }}
                              transition={{ duration: 1.6, ease: "easeInOut" }}
                              onAnimationComplete={() =>
                                setCardState("collapsed")
                              } // Asume que gestionas el estado con `setCardState`
                            >
                              <CImage fluid src={intro} width={150} />
                            </motion.div>
                            <CIcon
                              className="sidebar-brand-narrow"
                              icon={sygnet}
                              height={35}
                            />
                          </CSidebarBrand>
                        </div>
                      </CCardBody>
                    </CCard>
                  </motion.div>
                )}
              </CCard>
            </CCardGroup>
            <LoginFooter isMobileDevice={isMobileDevice} />
          </CCol>
        </CRow>
      </CContainer>
    </div>
  );
};

export default LoginDelegados;
