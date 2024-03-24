import React, { useState, useEffect, useReducer } from "react";
import { useNavigate, Link } from "react-router-dom";
import { motion } from "framer-motion";
import { isMobile } from "react-device-detect";

// Redux imports
import { useDispatch } from "react-redux";
import { loginAdminHandler } from "../../../store/auth-actions";
import { authActions } from "../../../store/auth-slice";

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
} from "@coreui/react";
import CIcon from "@coreui/icons-react";
import { cilLockLocked, cilUser } from "@coreui/icons";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";

import LoginFooter from "./LoginFooter";

import logo from "src/assets/images/datalexion-logo-big.png";
import { sygnet } from "src/assets/brand/sygnet";

import classes from "./Login.module.css";

//#region Functions

const emailReducer = (state, action) => {
  if (action.type == "USER_INPUT") {
    return { value: action.val };
    // return { value: action.val, isValid: action.val.includes("@") };
  }
  if (action.type == "INPUT_BLUR") {
    return { value: state.value };
    // return { value: state.value, isValid: state.value.includes("@") };
  }
  return { value: "", isValid: false };
};

const passwordReducer = (state, action) => {
  if (action.type == "USER_INPUT") {
    return { value: action.val, isValid: true };
    // return { value: action.val, isValid: action.val.trim().length > 6 };
  }
  if (action.type == "INPUT_BLUR") {
    return { value: state.value, isValid: true };
    // return { value: state.value, isValid: state.value.trim().length > 6 };
  }
  return { value: "", isValid: false };
};

//#endregion Functions

const buttonColor = "dark";

const LoginAdmin = () => {
  //#region Consts

  const [isLoggingIn, setIsLoggingIn] = useState(false);
  const [isMobileDevice, setIsMobileDevice] = useState(isMobile);
  const [errorMessage, setErrorMessage] = useState("");

  const navigate = useNavigate();

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

  // Redux call actions
  const dispatch = useDispatch();

  const [emailState, dispatchEmail] = useReducer(emailReducer, {
    value: "",
    isValid: false,
  });

  const [passwordState, dispatchPassword] = useReducer(passwordReducer, {
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

  //#endregion Hooks

  //#region Functions

  const emailChangeHandler = (event) => {
    dispatchEmail({ type: "USER_INPUT", val: event.target.value });
  };

  const passwordChangeHandler = (event) => {
    dispatchPassword({ type: "USER_INPUT", val: event.target.value });
  };

  const validateEmailHandler = () => {
    dispatchEmail({ type: "INPUT_BLUR" });
  };

  const validatePasswordHandler = () => {
    dispatchPassword({ type: "INPUT_BLUR" });
  };

  const submitHandler = (event) => {
    event.preventDefault();
    setIsLoggingIn(true); // Activar el spinner
    dispatch(
      loginAdminHandler(
        emailState.value,
        passwordState.value,
        navigate,
        setErrorMessage
      )
    ).then(() => {
      setIsLoggingIn(false); // Desactivar el spinner una vez completado el login
    });
  };

  //#endregion Functions

  return (
    <div className="bg-light min-vh-100 d-flex flex-row align-items-center">
      <CContainer>
        <CRow className="justify-content-center">
          <CCol md={8}>
            <CCardGroup>
              <CCard className="p-4">
                <CCardBody style={isMobileDevice ? { padding: 0 } : {}}>
                  <CForm onSubmit={submitHandler}>
                    <h1>Analistas</h1>
                    <p className="text-medium-emphasis">Ingrese su usuario</p>
                    <CInputGroup className="mb-3">
                      <CInputGroupText>
                        <CIcon icon={cilUser} />
                      </CInputGroupText>
                      <CFormInput
                        placeholder="Usuario"
                        onBlur={validateEmailHandler}
                        onChange={emailChangeHandler}
                        value={emailState.value}
                      />
                    </CInputGroup>
                    <CInputGroup className="mb-4">
                      <CInputGroupText>
                        <CIcon icon={cilLockLocked} />
                      </CInputGroupText>
                      <CFormInput
                        type="password"
                        placeholder="Contraseña"
                        onBlur={validatePasswordHandler}
                        onChange={passwordChangeHandler}
                        value={passwordState.value}
                      />
                    </CInputGroup>
                    <CRow>
                      <CCol
                        xs={12}
                        style={{ margin: "auto", textAlign: "center" }}
                      >
                        <CButton
                          color={buttonColor}
                          type="submit"
                          className="px-4"
                        >
                          {isLoggingIn ? (
                            <FontAwesomeIcon icon={faSpinner} spin />
                          ) : (
                            <FontAwesomeIcon icon={faCheck} />
                          )}
                          &nbsp;Login
                        </CButton>
                      </CCol>
                    </CRow>
                  </CForm>
                </CCardBody>
              </CCard>
              <CCard className={`bg ${classes.loginRight}`}>
                <motion.div
                  initial="hidden"
                  animate="visible"
                  transition={{ type: "spring", bounce: 0.7, duration: 2 }}
                  variants={
                    isMobileDevice
                      ? cardBodyVariantsMobile
                      : cardBodyVariantsDesktop
                  }
                >
                  <CCard className={`p-3 text-white bg-primary py-5`}>
                    <CCardBody
                      className="text-center"
                      style={isMobileDevice ? { padding: 0 } : {}}
                    >
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
                        <Link to="/login-delegados">
                          <CButton
                            color={buttonColor}
                            className={!isMobileDevice ? "mt-3" : ""}
                            tabIndex={-1}
                          >
                            Delegados
                          </CButton>
                        </Link>
                      </div>
                    </CCardBody>
                  </CCard>
                </motion.div>
              </CCard>
            </CCardGroup>
            <LoginFooter isMobileDevice={isMobileDevice} />
          </CCol>
        </CRow>
      </CContainer>
    </div>
  );
};

export default LoginAdmin;
