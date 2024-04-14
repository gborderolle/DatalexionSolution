import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";

import {
  CCard,
  CCardBody,
  CCardHeader,
  CButton,
  CRow,
  CFormInput,
  CTable,
  CPagination,
  CPaginationItem,
  CModal,
  CModalBody,
  CModalHeader,
  CModalTitle,
  CModalFooter,
  CContainer,
} from "@coreui/react";

import { USER_ROLE_ADMIN, USER_ROLE_ANALYST } from "../../../userRoles";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faRefresh,
  faMobileScreenButton,
  faBullhorn,
} from "@fortawesome/free-solid-svg-icons";

import useBumpEffect from "../../../utils/useBumpEffect";

// redux imports
import { batch, useDispatch, useSelector } from "react-redux";
import { authActions } from "../../../store/auth-slice";
import {
  fetchProvinceList,
  fetchMunicipalityList,
  fetchCircuitList,
  fetchDelegadoListByClient,
} from "../../../store/generalData-actions";

import "./DelegadosMenu.css";

const DelegadosMenu = () => {
  //#region Consts ***********************************

  const [isBumped, triggerBump] = useBumpEffect();
  const [searchTerm, setSearchTerm] = useState("");

  const [showModal, setShowModal] = useState(false);
  const [diffusionMessages, setDiffusionMessages] = useState([]);
  const [diffusionValue, setDiffusionValue] = useState("");
  const [ws, setWs] = useState(null);

  // redux init
  const dispatch = useDispatch();

  //#region RUTA PROTEGIDA
  const navigate = useNavigate();
  const userRole = useSelector((state) => state.auth.userRole);
  useEffect(() => {
    if (userRole != USER_ROLE_ADMIN && userRole != USER_ROLE_ANALYST) {
      dispatch(authActions.logout());
      navigate("/login-general");
    }
  }, [userRole, navigate, dispatch]);
  //#endregion RUTA PROTEGIDA

  // redux gets
  const reduxProvinceList = useSelector(
    (state) => state.generalData.provinceList
  );

  const reduxMunicipalityList = useSelector(
    (state) => state.generalData.municipalityList
  );

  const reduxCircuitList = useSelector(
    (state) => state.generalData.circuitList
  );

  const reduxDelegadoList = useSelector(
    (state) => state.generalData.delegadoList
  );

  // pagination logic
  const itemsPerPage = 5;
  const [currentPage, setCurrentPage] = useState(1);
  const [pageCount, setPageCount] = useState(0);

  const getCircuitNumbersForDelegate = (delegadoId) => {
    return reduxCircuitList
      ?.filter((circuit) => circuit.lastUpdateDelegadoId === delegadoId)
      .map((circuit) => circuit.number.toString());
  };

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    const newWs = new WebSocket("wss://localhost:8015/ws");

    newWs.onmessage = (event) => {
      // Directamente maneja el mensaje como texto en lugar de parsearlo como JSON
      const message = event.data;
      setDiffusionMessages((prevMessages) => [...prevMessages, message]);
    };

    setWs(newWs);

    return () => {
      newWs.close();
    };
  }, [showModal]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  // Función para obtener el nombre de la provincia por su ID
  const getProvinceNameById = (provinceId) => {
    const province = reduxProvinceList.find((p) => p.provinceId === provinceId);
    return province ? province.name : "N/A";
  };

  // Función para obtener el nombre del municipio por su ID
  const getMunicipalitiesNameById = (municipalityId) => {
    const municipality = reduxMunicipalityList.find(
      (m) => m.id === municipalityId
    );
    return municipality ? municipality.name : "N/A";
  };

  // Filtra los delegados en función del término de búsqueda
  const filteredDelegadoList = reduxDelegadoList?.filter((delegate) => {
    const ciMatch = delegate.ci?.includes(searchTerm);
    const nameMatch = delegate.name
      ?.toLowerCase()
      .includes(searchTerm.toLowerCase());
    const phoneStr = String(delegate.phone); // Convertir a string
    const phoneMatch = phoneStr.includes(searchTerm);

    const provinceName = getProvinceNameById(
      delegate.provinceId
    )?.toLowerCase();
    const municipalityName = getMunicipalitiesNameById(
      delegate.municipalityId
    )?.toLowerCase();

    const provinceMatch = provinceName?.includes(searchTerm.toLowerCase());
    const municipalityMatch = municipalityName?.includes(
      searchTerm.toLowerCase()
    );

    // Nueva lógica para circuitNumber
    const circuitNumbers = getCircuitNumbersForDelegate(delegate.id);
    const circuitMatch = circuitNumbers.some((number) =>
      number.includes(searchTerm)
    );

    return (
      ciMatch ||
      nameMatch ||
      phoneMatch ||
      provinceMatch ||
      municipalityMatch ||
      circuitMatch
    );
  });

  // pagination logic
  useEffect(() => {
    if (filteredDelegadoList) {
      setPageCount(Math.ceil(filteredDelegadoList.length / itemsPerPage));
    }
  }, [filteredDelegadoList]);

  // Determinar el rango de páginas a mostrar alrededor de la página actual
  const pagesToShow = 3; // Ajusta este número según sea necesario
  let startPage = Math.max(currentPage - Math.floor(pagesToShow / 2), 1);
  let endPage = Math.min(startPage + pagesToShow - 1, pageCount);

  if (endPage - startPage + 1 < pagesToShow) {
    startPage = Math.max(endPage - pagesToShow + 1, 1);
  }

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentDelegates = filteredDelegadoList?.slice(
    indexOfFirstItem,
    indexOfLastItem
  );

  // Función para obtener los números de circuito asignados a un delegado específico
  const getCircuitsAssignedForDelegado = (delegado) => {
    // Asume que `delegado.listCircuitDelegados` es una lista de objetos { circuitId, delegadoId }
    if (!delegado.listCircuitDelegados) {
      return "N/A"; // O maneja este caso según sea necesario
    }

    // Utiliza `some()` para verificar si el id de un circuito está en la lista de circuitos del delegado
    const assignedCircuits = reduxCircuitList.filter((circuit) =>
      delegado.listCircuitDelegados.some(
        (circuitDelegado) => circuitDelegado.circuitId === circuit.id
      )
    );

    // Mapea los circuitos asignados para extraer el número de cada circuito y luego une esos números en una cadena
    const circuitNumbers = assignedCircuits
      .map((circuit) => circuit.number)
      .join(", ");

    return circuitNumbers || "N/A"; // Devuelve los números de circuito o "N/A" si no hay circuitos asignados
  };

  const getCircuitsCompletedForDelegado = (delegado) => {
    // Filtra `reduxCircuitList` para encontrar circuitos modificados y completados por el delegado especificado
    const completedCircuits = reduxCircuitList.filter(
      (circuit) =>
        circuit.lastUpdateDelegadoId === delegado.id &&
        circuit.step1completed &&
        circuit.step2completed &&
        circuit.step3completed
    );

    // Mapea los circuitos completados para extraer información relevante, por ejemplo, el número de cada circuito
    const circuitInfo = completedCircuits.map((circuit) => ({
      id: circuit.id, // Asegúrate de que 'Id' es el nombre correcto de la propiedad
      number: circuit.number, // Asume que 'number' es una propiedad que quieres mostrar
      name: circuit.name, // Asume que 'name' es otra propiedad que quieres mostrar
    }));

    return circuitInfo; // Devuelve un array de objetos con los datos de los circuitos completados
  };

  const handleActionClick = (phone) => {
    let whatsappLink;
    let phoneString = String(phone);

    // Si el número tiene 9 dígitos, quita el primer dígito y agrega el prefijo
    if (phoneString.length === 9) {
      whatsappLink = `https://api.whatsapp.com/send/?phone=598${phoneString.substring(
        1
      )}`;
    }
    // Si el número tiene 8 dígitos, se usa como viene
    else if (phoneString.length === 8) {
      whatsappLink = `https://api.whatsapp.com/send/?phone=598${phoneString}`;
    }
    // En caso de que el número no tenga 8 o 9 dígitos, muestra un mensaje de error
    else {
      console.log("Número de teléfono inválido para WhatsApp");
      return;
    }

    // Abrir el enlace de WhatsApp
    window.open(whatsappLink, "_blank");
  };

  const isPhoneValid = (phone) => {
    if (!phone) return false; // Verifica si el teléfono es null, undefined o una cadena vacía

    const phoneStr = String(phone); // Convertir a string
    return phoneStr.length === 8 || phoneStr.length === 9;
  };

  const sendDiffusion = () => {
    if (ws && diffusionValue.trim()) {
      ws.send(JSON.stringify(diffusionValue));
      setDiffusionValue("");
    }
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  // Función para manejar el cambio en el campo de búsqueda
  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };

  // pagination logic
  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const bumpHandler = () => {
    triggerBump();

    const fetchGeneralData = async () => {
      batch(() => {
        dispatch(fetchCircuitList());
        dispatch(fetchDelegadoListByClient());
        dispatch(fetchProvinceList());
        dispatch(fetchMunicipalityList());
      });
    };
    fetchGeneralData();
  };

  const diffusionHandler = () => {
    setShowModal(true);
  };

  const handleClose = () => {
    setShowModal(false);
  };

  //#endregion Events ***********************************

  //#region JSX ***********************************

  // Renderiza cada fila de la tabla para los delegados filtrados
  const renderDelegadoRows = () => {
    return currentDelegates?.map((delegado, index) => {
      const isPhoneValidFlag = isPhoneValid(delegado.phone);
      const iconColor = isPhoneValidFlag ? "#697588" : "#CCCCCC"; // Color gris claro cuando está deshabilitado

      const circuitCompletados = getCircuitsCompletedForDelegado(delegado);
      const circuitAsignados = getCircuitsAssignedForDelegado(delegado);
      return (
        <tr key={index}>
          <td>{index + 1}</td>
          <td>{delegado.name}</td>
          <td>{getProvinceNameById(delegado.provinceId)}</td>
          <td>
            {delegado.listMunicipalities
              ? delegado.listMunicipalities
                  .map((municipality) => municipality.name)
                  .join(", ")
              : "N/A"}
          </td>
          <td>
            {`${circuitCompletados
              .map((circuit) => circuit.number)
              .join(", ")} (${circuitCompletados.length})`}
          </td>
          <td>{`${circuitAsignados} (${
            delegado.listCircuitDelegados?.length || 0
          })`}</td>
          <td>{delegado.email}</td>
          <td>{delegado.phone}</td>
          <td style={{ textAlign: "center" }}>
            <button
              onClick={() => handleActionClick(delegado.phone)}
              style={{ border: "none", background: "none" }}
              title="Contactar por WhatsApp"
              disabled={!isPhoneValidFlag}
            >
              <FontAwesomeIcon icon={faMobileScreenButton} color={iconColor} />
            </button>
          </td>
        </tr>
      );
    });
  };

  //#endregion JSX ***********************************

  return (
    <CContainer fluid>
      <CCard className="mb-4">
        <CCardHeader>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
            }}
          >
            Panel general
            <div style={{ display: "flex", justifyContent: "flex-end" }}>
              <button
                onClick={diffusionHandler}
                style={{ border: "none", background: "none", margin: "2px" }}
              >
                <FontAwesomeIcon icon={faBullhorn} color="#697588" />
              </button>
              <CFormInput
                type="text"
                placeholder="Buscar..."
                value={searchTerm}
                onChange={handleSearchChange}
                style={{ maxWidth: "300px" }} // O el ancho que prefieras
              />
              &nbsp;
              <button
                onClick={bumpHandler}
                style={{ border: "none", background: "none" }}
                className={isBumped ? "bump" : ""}
              >
                <FontAwesomeIcon icon={faRefresh} color="#697588" />
              </button>
            </div>
          </div>
        </CCardHeader>
        <CCardBody>
          <CRow>
            <div className="custom-table-responsive">
              <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ duration: 0.3 }}
              >
                <div className="table-responsive">
                  <CTable striped>
                    <thead>
                      <tr>
                        <th>#</th>
                        {/* <th>Cédula</th> */}
                        <th>Nombre</th>
                        <th>Departamento</th>
                        <th>Municipios</th>
                        <th>C.Cerrados</th>
                        <th>C.Asignados</th>
                        <th>Email</th>
                        <th>Celular</th>
                        <th>Acciones</th>
                      </tr>
                    </thead>
                    <tbody>{renderDelegadoRows()}</tbody>
                  </CTable>
                </div>
              </motion.div>
            </div>
          </CRow>
          <CPagination align="center" aria-label="Page navigation example">
            {startPage > 1 && (
              <CPaginationItem onClick={() => handlePageChange(1)}>
                1
              </CPaginationItem>
            )}
            {startPage > 2 && <CPaginationItem>...</CPaginationItem>}
            {[...Array(endPage - startPage + 1)].map((_, index) => (
              <CPaginationItem
                key={startPage + index}
                active={startPage + index === currentPage}
                onClick={() => handlePageChange(startPage + index)}
              >
                {startPage + index}
              </CPaginationItem>
            ))}
            {endPage < pageCount - 1 && <CPaginationItem>...</CPaginationItem>}
            {endPage < pageCount && (
              <CPaginationItem onClick={() => handlePageChange(pageCount)}>
                {pageCount}
              </CPaginationItem>
            )}
          </CPagination>
        </CCardBody>
      </CCard>

      <CModal visible={showModal} onClose={handleClose}>
        <CModalHeader>
          <CModalTitle>Mensajes de difusión</CModalTitle>
        </CModalHeader>
        <CModalBody>
          {diffusionMessages.map((msg, index) => (
            <div key={index}>{msg}</div>
          ))}
          <CFormInput
            type="text"
            value={diffusionValue}
            onChange={(e) => setDiffusionValue(e.target.value)}
            onKeyPress={(e) => e.key === "Enter" && sendDiffusion()}
            placeholder="Escribe un mensaje..."
          />
          <br />
          <CButton color="primary" onClick={sendDiffusion}>
            Enviar
          </CButton>
        </CModalBody>
        <CModalFooter>
          <CButton color="secondary" onClick={handleClose}>
            Cerrar
          </CButton>
        </CModalFooter>
      </CModal>
    </CContainer>
  );
};

export default DelegadosMenu;
