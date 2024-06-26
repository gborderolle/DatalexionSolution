import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

import { USER_ROLE_DELEGADO } from "../../../userRoles";

import {
  CCol,
  CRow,
  CCard,
  CCardBody,
  CCardHeader,
  CToast,
  CToastHeader,
  CToastBody,
  CInputGroup,
  CFormInput,
  CPagination,
  CPaginationItem,
  CFormCheck,
} from "@coreui/react";
import { RadioGroup, RadioButton } from "react-radio-buttons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRefresh } from "@fortawesome/free-solid-svg-icons";

import RadioButtonStepper from "../../stepper/RadioButtonStepper";

import { LoginGeneral } from "../../../utils/navigationPaths";

// redux imports
import { authActions } from "../../../store/auth-slice";
import { liveSettingsActions } from "../../../store/liveSettings-slice";
import { uiActions } from "../../../store/ui-slice";
import { fetchCircuitListByClient } from "../../../store/generalData-actions";

import { getCircuitParty } from "../../../utils/auxiliarFunctions";

import useBumpEffect from "../../../utils/useBumpEffect";

import "./FormStart.css";

const FormStart = (props) => {
  //#region Consts ***********************************

  // redux
  const dispatch = useDispatch();
  const navigate = useNavigate();

  // useSelector
  const reduxListMunicipalitiesFromDelegado = useSelector(
    (state) => state.auth.listMunicipalities
  );
  const userFullname = useSelector((state) => state.auth.fullname);
  const reduxCircuitList = useSelector(
    (state) => state.generalData.circuitListByClient
  );
  const reduxClient = useSelector((state) => state.generalData.client);
  const isToastAlreadyShown = useSelector((state) => state.ui.isToastShown);
  const userRole = useSelector((state) => state.auth.userRole);

  // useStates
  const [selectedCircuit, setSelectedCircuit] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedForBump, setSelectedForBump] = useState(null);
  const [circuitList, setCircuitList] = useState([]);
  const [isBumped, triggerBump] = useBumpEffect();
  const [onlyOpen, setOnlyOpen] = useState(false); // Nuevo estado para el checkbox
  const [applyAnimation, setApplyAnimation] = useState(false);
  const [containerClass, setContainerClass] = useState("");

  //#region Pagination   ***********************************

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const [pageCount, setPageCount] = useState(0);

  //#endregion Pagination ***********************************

  // Antes de filtrar, asegúrate de que circuitList no sea null ni undefined.
  const filteredCircuitList = circuitList
    ? circuitList.filter(
        (circuit) =>
          circuit.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          circuit.number.toString().includes(searchTerm)
      )
    : [];

  // Ordenar lista de circuitos por número de circuito antes de mostrarla
  const sortedList = [...filteredCircuitList]?.sort(
    (a, b) => a.number - b.number
  );

  //#region Pagination ***********************************

  const handlePageChange = (newPage) => {
    setCurrentPage(newPage);
  };

  // Determinar el rango de páginas a mostrar alrededor de la página actual
  const pagesToShow = 3; // Ajusta este número según sea necesario
  let startPage = Math.max(currentPage - Math.floor(pagesToShow / 2), 1);
  let endPage = Math.min(startPage + pagesToShow - 1, pageCount);

  if (endPage - startPage + 1 < pagesToShow) {
    startPage = Math.max(endPage - pagesToShow + 1, 1);
  }

  // Definir currentItems aquí, después de definir los estados y antes del JSX
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentList = sortedList?.slice(indexOfFirstItem, indexOfLastItem);

  //#endregion Pagination ***********************************

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    if (userRole != USER_ROLE_DELEGADO) {
      dispatch(authActions.logout());
      navigate(LoginGeneral);
    }
  }, [userRole, navigate, dispatch]);

  useEffect(() => {
    dispatch(fetchCircuitListByClient());
    dispatch(uiActions.hideStepper());
    dispatch(uiActions.setStepsSubmittedEmpty());

    return () => {
      dispatch(uiActions.showStepper());
    };
  }, [dispatch]);

  useEffect(() => {
    if (Array.isArray(reduxListMunicipalitiesFromDelegado)) {
      const municipalityIdsFromDelegado =
        reduxListMunicipalitiesFromDelegado.map(
          (municipality) => municipality.id
        );

      const filteredList = reduxCircuitList.filter((circuit) =>
        municipalityIdsFromDelegado.includes(circuit?.municipalityId)
      );

      const enrichedList = filteredList.map((circuit) => {
        const circuitParty = getCircuitParty(circuit, reduxClient);
        return {
          ...circuit,
          step1completed: circuitParty?.step1completed ?? false,
          step2completed: circuitParty?.step2completed ?? false,
          step3completed: circuitParty?.step3completed ?? false,
        };
      });

      setCircuitList(enrichedList);
    }
  }, [reduxCircuitList, reduxListMunicipalitiesFromDelegado, reduxClient]); // Agregar reduxClient a las dependencias si su estado o disponibilidad puede cambiar

  //#region Pagination ***********************************

  useEffect(() => {
    setPageCount(
      Math.ceil(
        filteredCircuitList && filteredCircuitList.length / itemsPerPage
      )
    );
  }, [filteredCircuitList]);

  //#endregion Pagination ***********************************

  useEffect(() => {
    let filteredList = reduxCircuitList;

    // Extraer IDs de municipios del usuario si es necesario
    const userMunicipalityIds = reduxListMunicipalitiesFromDelegado.map(
      (municipality) => municipality.id
    );

    // Filtrar por municipio si es necesario
    if (userMunicipalityIds && userMunicipalityIds.length > 0) {
      filteredList = filteredList.filter((circuit) =>
        userMunicipalityIds.includes(circuit.municipalityId)
      );
    }

    // Dentro de tu componente o función donde manejas los circuitos
    if (onlyOpen) {
      filteredList = filteredList.filter((circuit) => {
        const circuitParty = getCircuitParty(circuit, reduxClient);
        if (!circuitParty) {
          return true; // Si no hay circuitParty correspondiente, considera que el circuito no está completo
        }
        // Devuelve true si alguno de los pasos no está completado
        return !(
          circuitParty.step1completed &&
          circuitParty.step2completed &&
          circuitParty.step3completed
        );
      });
    }

    setCircuitList(filteredList);
  }, [reduxCircuitList, reduxListMunicipalitiesFromDelegado, onlyOpen]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  // Lista de circuitos JSX
  const circuitListJSX =
    currentList &&
    currentList.map((circuit, index) => {
      const isSelected = selectedCircuit?.id == circuit.id;
      const selectedStyle = {
        border: "4px solid blue",
        fontWeight: "bold",
        boxShadow: "0px 0px 12px rgba(42, 113, 222, 0.5)",
        transform: "scale(1.05)",
        transition: "transform 0.3s ease-in-out",
      };
      const radioButtonStyle = isSelected ? selectedStyle : {};
      const isBumped = selectedForBump == circuit.id.toString();

      const radioButtonStyles = {
        marginBottom: "20px !important",
      };

      const delay = index * 100; // 100ms por elemento
      const animationStyle = applyAnimation
        ? { animationDelay: `${delay}ms` }
        : {};

      const circuitParty = getCircuitParty(circuit, reduxClient);
      if (circuitParty) {
        return (
          <RadioButton
            iconSize={20}
            padding={14}
            key={circuit.id}
            value={circuit.id.toString()}
            rootColor={isSelected ? "rgb(136 131 131)" : "rgb(136 131 131)"}
            pointColor={isSelected ? "rgb(42 113 222)" : "rgb(136 131 131)"}
            onClick={() => onChangeHandler(circuit.id.toString())}
            style={{ ...radioButtonStyles, ...animationStyle }}
            className={`radio-button-margin ${
              applyAnimation ? "fade-in-element" : ""
            }`}
          >
            <div className={`${isBumped ? "bump" : ""}`}>
              <CRow style={radioButtonStyle}>
                <h2># {circuit.number}</h2>
              </CRow>
              <CRow style={radioButtonStyle}>{circuit.name}</CRow>
              <br />
              <CRow>
                {/* <RadioButtonStepper currentStep={circuit} /> */}
                <RadioButtonStepper currentStep={circuitParty} />
              </CRow>
            </div>
          </RadioButton>
        );
      }
    });

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const handleCheckboxChange = (e) => {
    setOnlyOpen(e.target.checked);
    setContainerClass(e.target.checked ? "animate-items" : "");
  };

  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  const onChangeHandler = (value) => {
    const circuit = filteredCircuitList.find(
      (circuit) => circuit.id.toString() == value
    );

    if (circuit) {
      setSelectedCircuit(circuit);
    }
    // SET REDUX ACA
    dispatch(liveSettingsActions.setSelectedCircuit(circuit));

    setSelectedForBump(value);

    const timer = setTimeout(() => {
      setSelectedForBump(null);
    }, 300);
    return () => clearTimeout(timer);
  };

  const bumpHandler = () => {
    triggerBump();

    dispatch(fetchCircuitListByClient());
  };

  //#endregion Events ***********************************

  //#region JSX ***********************************

  const labelNoCircuits = (
    <span
      key="no-circuits"
      style={{ color: "blue", fontStyle: "italic", width: "auto" }}
    >
      No hay circuitos que cumplan con la condición.
    </span>
  );

  const toastContainerStyles = {
    position: "fixed",
    top: "20%",
    zIndex: 1050,
    width: "70%",
    left: "50%",
    transform: "translateX(-50%)",
  };

  //#endregion JSX ***********************************

  return (
    <>
      {!isToastAlreadyShown && (
        <div style={toastContainerStyles}>
          <CToast autohide={false} visible={true}>
            <CToastHeader closeButton>
              <svg
                className="rounded me-2"
                width="20"
                height="20"
                xmlns="http://www.w3.org/2000/svg"
                preserveAspectRatio="xMidYMid slice"
                focusable="false"
                role="img"
              >
                <rect width="100%" height="100%" fill="#007aff"></rect>
              </svg>
              <div className="fw-bold me-auto">Datalexion dice:</div>
              <small>Ahora</small>
            </CToastHeader>
            <CToastBody>¡Bienvenido {userFullname}!</CToastBody>
          </CToast>
        </div>
      )}

      <CCard className="mb-4" style={{ paddingBottom: "3rem" }}>
        <CCardHeader>
          Seleccione un circuito
          {/* <br /> */}
          <button
            onClick={bumpHandler}
            style={{ border: "none", background: "none", float: "right" }}
            className={isBumped ? "bump" : ""}
          >
            <FontAwesomeIcon icon={faRefresh} color="#697588" />{" "}
          </button>
        </CCardHeader>
        <CCardBody>
          <CRow>
            <CCol xs={12} sm={12} md={12} lg={12} xl={12}>
              <CFormCheck
                reverse
                id="chkOnlyOpen"
                label="Sólo abiertos"
                onChange={handleCheckboxChange}
                checked={onlyOpen}
              />
              <CInputGroup>
                <CFormInput
                  placeholder="Filtrar circuito..."
                  onChange={handleSearchChange}
                  size="sm"
                />
              </CInputGroup>
              <br />
              <RadioGroup
                onChange={onChangeHandler}
                className={`classRadio ${containerClass}`}
              >
                {props.isLoading
                  ? [<LoadingSpinner key="loading" />]
                  : filteredCircuitList && filteredCircuitList.length > 0
                  ? circuitListJSX
                  : [labelNoCircuits]}
              </RadioGroup>
              <br />

              <CPagination align="center" aria-label="Page navigation">
                {startPage > 1 && (
                  <CPaginationItem onClick={() => handlePageChange(1)}>
                    1
                  </CPaginationItem>
                )}
                {startPage > 2 && <CPaginationItem>...</CPaginationItem>}
                {[...Array(endPage - startPage + 1)].map((_, index) => (
                  <CPaginationItem
                    key={startPage + index}
                    active={startPage + index == currentPage}
                    onClick={() => handlePageChange(startPage + index)}
                  >
                    {startPage + index}
                  </CPaginationItem>
                ))}
                {endPage < pageCount - 1 && (
                  <CPaginationItem>...</CPaginationItem>
                )}
                {endPage < pageCount && (
                  <CPaginationItem onClick={() => handlePageChange(pageCount)}>
                    {pageCount}
                  </CPaginationItem>
                )}
              </CPagination>
            </CCol>
          </CRow>
        </CCardBody>
      </CCard>
    </>
  );
};

export default FormStart;
