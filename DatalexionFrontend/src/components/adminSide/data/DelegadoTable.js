import React, { useState, useEffect, useMemo, useReducer } from "react";
import {
  CSpinner,
  CRow,
  CTable,
  CTableHead,
  CTableRow,
  CTableHeaderCell,
  CTableBody,
  CTableDataCell,
  CButton,
  CModal,
  CModalHeader,
  CModalTitle,
  CModalBody,
  CForm,
  CFormInput,
  CModalFooter,
  CInputGroup,
  CInputGroupText,
  CDropdown,
  CDropdownToggle,
  CDropdownMenu,
  CDropdownItem,
  CCard,
  CCardBody,
  CCardFooter,
  CAlert,
  CPagination,
  CPaginationItem,
  CContainer,
} from "@coreui/react";
import useInput from "../../../hooks/use-input";
import useAPI from "../../../hooks/use-API";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { fetchDelegadoListByClient } from "../../../store/generalData-actions";
import { urlDelegado, urlIsCIAlreadyRegistered } from "../../../endpoints";
import { MunicipalityGetProvince } from "src/utils/auxiliarFunctions";

const ciReducer = (state, action) => {
  if (action.type === "USER_INPUT") {
    const regex = /^[0-9]{7}-[0-9]$/;
    const isValid = regex.test(action.val);
    return { ...state, value: action.val, isValid };
  }
  if (action.type === "INPUT_BLUR") {
    const regex = /^[0-9]{7}-[0-9]$/;
    const isValid = regex.test(state.value);
    return { ...state, isValid };
  }
  if (action.type === "SET_ERROR") {
    return { ...state, errorMessage: action.errorMessage };
  }
  return { value: "", isValid: false, errorMessage: "" };
};

const phoneReducer = (state, action) => {
  if (action.type == "USER_INPUT") {
    // Expresión regular que valida un número de exactamente 7 dígitos seguido por un guión y un último dígito
    const regex = /^[0-9]{9}$/;
    return { value: action.val, isValid: regex.test(action.val) };
  }
  if (action.type == "INPUT_BLUR") {
    // Misma expresión regular para cuando el input pierde el foco
    const regex = /^[0-9]{9}$/;
    return { value: state.value, isValid: regex.test(state.value) };
  }
  return { value: "", isValid: false };
};

const DelegadoTable = (props) => {
  //#region Consts ***********************************

  const [isValidForm, setIsValidForm] = useState(true);
  const { isLoading, isSuccess, error, uploadData, removeData } = useAPI();

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentModel, setCurrentModel] = useState(null);

  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [idToDelete, setIdToDelete] = useState(null);

  const [ddlSelectedMunicipality, setDdlSelectedMunicipality] = useState(null);
  const [ddlSelectedProvince, setDdlSelectedProvince] = useState(null);

  const [filteredMunicipalities, setFilteredMunicipalities] = useState([]);
  const [selectedMunicipalities, setSelectedMunicipalities] = useState([]);

  const [ciState, dispatchCI] = useReducer(ciReducer, {
    value: "",
    isValid: false,
  });

  const [phoneState, dispatchPhone] = useReducer(phoneReducer, {
    value: "",
    isValid: false,
  });

  // Asegúrate de ajustar la función validateCI para no realizar la verificación si isEditing es true
  const validateCI = async (ci, isEditing = false) => {
    if (isEditing) {
      // Si estamos editando, asumimos que la cédula es válida porque ya estaba registrada
      dispatchCI({
        type: "SET_ERROR",
        errorMessage: "",
      });
    } else if (ci && ci.length > 0) {
      const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          "x-version": "1",
        },
      };

      try {
        const response = await fetch(
          `${urlIsCIAlreadyRegistered}?ci=${ci}`,
          requestOptions
        );
        if (!response.ok) {
          throw new Error("Error al validar la CI");
        }
        const isAvailable = await response.json();
        dispatchCI({
          type: "SET_ERROR",
          errorMessage: isAvailable ? "" : "La CI ya está registrada",
        });
      } catch (error) {
        console.error("Error durante la validación:", error);
        dispatchCI({
          type: "SET_ERROR",
          errorMessage: "Error al validar la CI",
        });
      }
    }
  };

  // redux
  const dispatch = useDispatch();

  const clientId = useSelector((state) => state.auth.clientId);

  // Redux
  const delegadoList = useSelector((state) => state.generalData.delegadoList);
  const municipalityList = useSelector(
    (state) => state.generalData.municipalityList
  );
  const provinceList = useSelector((state) => state.generalData.provinceList);

  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });

  //#region Pagination ***********************************

  const itemsPerPage = 25;
  const [currentPage, setCurrentPage] = useState(1);
  const [pageCount, setPageCount] = useState(0);

  useEffect(() => {
    setPageCount(Math.ceil(delegadoList.length / itemsPerPage));
  }, [delegadoList, itemsPerPage]);

  //#endregion Pagination ***********************************

  useEffect(() => {
    dispatch(fetchDelegadoListByClient());
  }, [dispatch]);

  const {
    value: name,
    isValid: inputIsValid1,
    hasError: inputHasError1,
    valueChangeHandler: inputChangeHandler1,
    inputBlurHandler: inputBlurHandler1,
    reset: inputReset1,
  } = useInput((value) => value.trim() !== "");

  const isValidEmail = (value) => {
    // Permite un string vacío o un email válido
    return (
      value.trim() === "" || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value.trim())
    );
  };

  const {
    value: email,
    isValid: inputIsValid4,
    hasError: inputHasError4,
    valueChangeHandler: inputChangeHandler4,
    inputBlurHandler: inputBlurHandler4,
    reset: inputReset4,
  } = useInput(isValidEmail);

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  const sortedList = useMemo(() => {
    let sortableList = [...delegadoList];
    if (sortConfig.key !== null) {
      sortableList.sort((a, b) => {
        // Comparación especial para el rol
        if (sortConfig.key === "municipality") {
          const roleA = a.role ? a.role.toLowerCase() : "";
          const roleB = b.role ? b.role.toLowerCase() : "";
          if (roleA < roleB) {
            return sortConfig.direction === "ascending" ? -1 : 1;
          }
          if (roleA > roleB) {
            return sortConfig.direction === "ascending" ? 1 : -1;
          }
          return 0;
        } else {
          // Ordenamiento estándar para otras propiedades
          if (a[sortConfig.key] < b[sortConfig.key]) {
            return sortConfig.direction === "ascending" ? -1 : 1;
          }
          if (a[sortConfig.key] > b[sortConfig.key]) {
            return sortConfig.direction === "ascending" ? 1 : -1;
          }
          return 0;
        }
      });
    }
    return sortableList;
  }, [delegadoList, sortConfig]);

  // Efecto para validar la CI cuando el input pierde el foco
  useEffect(() => {
    if (!ciState.isValid) {
      validateCI(ciState.value.replace(/-/g, ""));
    }
  }, [ciState.isValid]); // Dependiendo de cómo gestionas el evento onBlur, puedes necesitar ajustar esta dependencia

  // Modifica tu método validateCIHandler para gestionar onBlur
  const validateCIHandler = () => {
    dispatchCI({ type: "INPUT_BLUR" });
    if (ciState.isValid) {
      validateCI(ciState.value.replace(/-/g, ""));
    }
  };

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

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

  const phoneChangeHandler = (event) => {
    // Extrayendo el valor del input
    let inputValue = event.target.value;

    // Removiendo caracteres no numéricos
    inputValue = inputValue.replace(/\D/g, "");

    // Limitando la longitud a 8 dígitos
    inputValue = inputValue.substring(0, 9);

    // Despachando el evento con el valor formateado
    dispatchPhone({ type: "USER_INPUT", val: inputValue });
  };

  const validatePhoneHandler = () => {
    dispatchPhone({ type: "INPUT_BLUR" });
  };

  const requestSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  // Si la entidad es nula, se asume que se está creando una nueva, sino se está editando
  const openModal = (model = null) => {
    if (model) {
      // Caso para editar un delegado existente
      setCurrentModel(model);
      inputReset1(model.name);
      inputReset4(model.email);

      // Agregar el guión al formato correcto de la CI
      const formattedCI =
        model.ci.substring(0, 7) + "-" + model.ci.substring(7);

      dispatchCI({ type: "USER_INPUT", val: formattedCI }); // Initialize ciReducer with model.ci
      validateCI(model.ci, true); // Pasa true para indicar que se está editando

      dispatchPhone({ type: "USER_INPUT", val: model.phone }); // Initialize phoneReducer with model.phone

      // Suponiendo que delegado.municipalityIds es un arreglo de los IDs de los municipios asignados
      setSelectedMunicipalities(model.listMunicipalities || []);

      // Suponiendo que tienes una manera de obtener el departamento basado en los municipios
      // y que delegado tiene una propiedad departmentId
      if (model.listMunicipalities?.length > 0) {
        const firstMunicipality = model.listMunicipalities[0];
        if (firstMunicipality) {
          const province = MunicipalityGetProvince(
            firstMunicipality,
            provinceList
          );
          setDdlSelectedProvince(province || null);
          handleSelectProvince(province); // Esto ajustará `filteredMunicipalities` y mantendrá la coherencia
        }
      }
    } else {
      // Caso para agregar un nuevo delegado
      setCurrentModel(null);
      inputReset1();
      inputReset4();
      setSelectedMunicipalities([]);
      setDdlSelectedProvince(null);
      setFilteredMunicipalities([]);
    }
    setIsModalVisible(true);
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setCurrentModel(null);
  };

  const closeDeleteModal = () => {
    setIsDeleteModalVisible(false);
    setIdToDelete(null);
  };

  const openDeleteModal = (id) => {
    setIdToDelete(id);
    setIsDeleteModalVisible(true);
  };

  const confirmDelete = async () => {
    if (idToDelete) {
      await removeData(urlAccount, idToDelete);
      dispatch(fetchDelegadoListByClient());
      closeDeleteModal();
    }
  };

  const inputResetMunicipality = () => {
    setDdlSelectedMunicipality(null);
  };

  const inputResetProvince = () => {
    setDdlSelectedProvince(null);
  };

  const handleSelectProvince = (department) => {
    setDdlSelectedProvince(department);
    // Filtra los municipios basados en el departamento seleccionado
    const filtered = municipalityList.filter(
      (municipality) => municipality.provinceId === department.id // Asegúrate de que 'provinceId' sea la clave correcta
    );
    setFilteredMunicipalities(filtered);
  };

  const handleSelectDdlMunicipality = (municipality) => {
    setDdlSelectedMunicipality(municipality);
  };

  const handleMunicipalityChange = (municipalityId, isChecked) => {
    if (isChecked) {
      // Agregar el ID del municipio si el checkbox es marcado
      setSelectedMunicipalities((prev) => [...prev, municipalityId]);
    } else {
      // Remover el ID del municipio si el checkbox es desmarcado
      setSelectedMunicipalities((prev) =>
        prev.filter((id) => id !== municipalityId)
      );
    }
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const formSubmitHandler = async (event) => {
    event.preventDefault();

    setIsValidForm(inputIsValid1 && inputIsValid4);

    if (!isValidForm) {
      return;
    }

    const ciValue = ciState.value.replace(/-/g, "");

    const municipalitiesToSend = selectedMunicipalities; // Aquí asumimos que selectedMunicipalities ya contiene los ID de los municipios

    const dataToUpload = {
      Name: name,
      CI: ciValue,
      Phone: phoneState.value,
      Email: email,
      MunicipalityIds: municipalitiesToSend,
      ClientId: Number(clientId),
      ListCircuitDelegados: [],
    };

    try {
      let response;
      if (currentModel) {
        response = await uploadData(
          dataToUpload,
          urlDelegado,
          true,
          currentModel.id
        );
      } else {
        response = await uploadData(dataToUpload, urlDelegado);
      }
      if (response) {
        dispatch(fetchDelegadoListByClient());
        inputReset1();
        inputResetProvince();

        setTimeout(() => {
          closeModal();
        }, 1000);
      }
    } catch (error) {
      console.error("Error al enviar los datos:", error);
    }
  };

  const handleSelectDdlUserRole = (item) => {
    setDdlSelectedMunicipality(item);
  };

  //#endregion Events ***********************************

  //#region Pagination ***********************************

  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentList = sortedList.slice(indexOfFirstItem, indexOfLastItem);

  //#endregion Pagination ***********************************

  return (
    <div>
      <CContainer fluid>
        <CButton color="dark" size="sm" onClick={() => openModal()}>
          Agregar
        </CButton>
        <div className="table-responsive">
          <CTable striped>
            <CTableHead>
              <CTableRow>
                <CTableHeaderCell>#</CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("name")}>
                  Nombre completo
                </CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("ci")}>
                  CI
                </CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("phone")}>
                  Celular
                </CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("email")}>
                  Email
                </CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("province")}>
                  Departamento
                </CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("municipality")}>
                  Municipios
                </CTableHeaderCell>
                <CTableHeaderCell>Acciones</CTableHeaderCell>
              </CTableRow>
            </CTableHead>
            <CTableBody>
              {currentList.map((delegado, index) => {
                const firstMunicipality =
                  delegado.listMunicipalities &&
                  delegado.listMunicipalities.length > 0
                    ? delegado.listMunicipalities[0]
                    : null;
                const provinceName = firstMunicipality
                  ? MunicipalityGetProvince(firstMunicipality, provinceList)
                      ?.name
                  : "N/A";
                return (
                  <CTableRow key={delegado.id}>
                    <CTableDataCell>{index + 1}</CTableDataCell>
                    <CTableDataCell>{delegado.name}</CTableDataCell>
                    <CTableDataCell>{delegado.ci}</CTableDataCell>
                    <CTableDataCell>{delegado.phone}</CTableDataCell>
                    <CTableDataCell>{delegado.email}</CTableDataCell>
                    <CTableDataCell>{provinceName}</CTableDataCell>
                    <CTableDataCell>
                      {delegado.listMunicipalities &&
                      delegado.listMunicipalities.length > 0
                        ? delegado.listMunicipalities
                            .map((municipality) => municipality.name)
                            .join(", ") // Ajusta 'Name' al nombre correcto de tu propiedad
                        : "N/A"}
                    </CTableDataCell>
                    <CTableDataCell>
                      <CButton
                        color="dark"
                        size="sm"
                        onClick={() => openModal(delegado)}
                      >
                        Editar
                      </CButton>
                      <CButton
                        color="danger"
                        size="sm"
                        onClick={() => openDeleteModal(delegado.id)}
                        style={{ marginLeft: 10 }}
                      >
                        Eliminar
                      </CButton>
                    </CTableDataCell>
                  </CTableRow>
                );
              })}
            </CTableBody>
          </CTable>
        </div>

        <CPagination align="center">
          {currentPage > 1 && (
            <CPaginationItem onClick={() => handlePageChange(currentPage - 1)}>
              Anterior
            </CPaginationItem>
          )}
          {[...Array(pageCount)].map((_, i) => (
            <CPaginationItem
              key={i + 1}
              active={i + 1 === currentPage}
              onClick={() => handlePageChange(i + 1)}
            >
              {i + 1}
            </CPaginationItem>
          ))}
          {currentPage < pageCount && (
            <CPaginationItem onClick={() => handlePageChange(currentPage + 1)}>
              Siguiente
            </CPaginationItem>
          )}
        </CPagination>

        <CModal visible={isModalVisible} onClose={closeModal}>
          <CModalHeader>
            <CModalTitle>
              {currentModel ? "Editar entidad" : "Agregar entidad"}
            </CModalTitle>
          </CModalHeader>
          <CForm onSubmit={formSubmitHandler}>
            <CModalBody>
              <CCard>
                <CCardBody>
                  <CInputGroup>
                    <CInputGroupText className="cardItem custom-input-group-text">
                      {props.name}
                    </CInputGroupText>
                    <CFormInput
                      type="text"
                      className="cardItem"
                      onChange={inputChangeHandler1}
                      onBlur={inputBlurHandler1}
                      value={name}
                    />
                    {inputHasError1 && (
                      <CAlert color="danger" className="w-100">
                        Entrada inválida
                      </CAlert>
                    )}
                  </CInputGroup>
                  <br />
                  <CInputGroup>
                    <CInputGroupText className="cardItem custom-input-group-text">
                      {props.CI}
                    </CInputGroupText>
                    <CFormInput
                      placeholder="1234567-8"
                      onBlur={validateCIHandler}
                      onChange={ciChangeHandler}
                      value={ciState.value}
                      valid={ciState.isValid}
                      invalid={!ciState.isValid || ciState.errorMessage !== ""}
                      feedback={
                        ciState.isValid && !ciState.errorMessage
                          ? "El formato es correcto"
                          : ciState.errorMessage || "El formato es incorrecto"
                      }
                      required
                    />
                    {/* {inputHasError2 && (
                    <CAlert color="danger" className="w-100">
                      Entrada inválida
                    </CAlert>
                  )} */}
                  </CInputGroup>
                  <br />
                  <CInputGroup>
                    <CInputGroupText className="cardItem custom-input-group-text">
                      {props.phone}
                    </CInputGroupText>
                    {/* <CFormInput
                    type="number"
                    className="cardItem"
                    onChange={inputChangeHandler3}
                    onBlur={inputBlurHandler3}
                    value={phone}
                  /> */}
                    <CFormInput
                      placeholder="099000000"
                      onBlur={validatePhoneHandler}
                      onChange={phoneChangeHandler}
                      value={phoneState.value}
                      valid={phoneState.isValid}
                      invalid={!phoneState.isValid && phoneState.value !== ""}
                      feedback={
                        phoneState.isValid
                          ? "El formato es correcto"
                          : "El formato es incorrecto"
                      }
                      required
                    />
                    {/* {inputHasError3 && (
                    <CAlert color="danger" className="w-100">
                      Entrada inválida
                    </CAlert>
                  )} */}
                  </CInputGroup>
                  <br />
                  <CInputGroup>
                    <CInputGroupText className="cardItem custom-input-group-text">
                      {props.email}
                    </CInputGroupText>
                    <CFormInput
                      type="email"
                      className="cardItem"
                      onChange={inputChangeHandler4}
                      onBlur={inputBlurHandler4}
                      value={email}
                    />
                    {inputHasError4 && (
                      <CAlert color="danger" className="w-100">
                        Entrada inválida
                      </CAlert>
                    )}
                  </CInputGroup>
                  <br />
                  <CInputGroup>
                    <CInputGroupText className="cardItem custom-input-group-text">
                      {props.province}
                    </CInputGroupText>
                    <CDropdown>
                      <CDropdownToggle color="secondary">
                        {ddlSelectedProvince
                          ? ddlSelectedProvince.name
                          : "Seleccionar"}
                      </CDropdownToggle>
                      <CDropdownMenu>
                        {provinceList.map((province) => (
                          <CDropdownItem
                            key={province.id}
                            onClick={() => handleSelectProvince(province)}
                          >
                            {`${province.id}: ${province.name}`}
                          </CDropdownItem>
                        ))}
                      </CDropdownMenu>
                    </CDropdown>
                  </CInputGroup>
                  <br />
                  <CInputGroup>
                    <CInputGroupText>{props.municipality}</CInputGroupText>
                    <div>
                      {filteredMunicipalities.map((municipality) => (
                        <div key={municipality.id} style={{ padding: "5px" }}>
                          <input
                            type="checkbox"
                            id={`municipality-${municipality.id}`}
                            name="selectedMunicipalities"
                            value={municipality.id}
                            checked={selectedMunicipalities.includes(
                              municipality.id
                            )}
                            onChange={(e) =>
                              handleMunicipalityChange(
                                municipality.id,
                                e.target.checked
                              )
                            }
                          />
                          <label htmlFor={`municipality-${municipality.id}`}>
                            {municipality.name}
                          </label>
                        </div>
                      ))}
                    </div>
                  </CInputGroup>
                  <br />
                  <CRow className="justify-content-center">
                    {isLoading && (
                      <div className="text-center">
                        <CSpinner />
                      </div>
                    )}
                  </CRow>
                  <br />
                  <CCardFooter className="text-medium-emphasis">
                    {!isValidForm && (
                      <CAlert color="danger" className="w-100">
                        El formulario no es válido
                      </CAlert>
                    )}
                    {isSuccess && (
                      <CAlert color="success" className="w-100">
                        Datos ingresados correctamente
                      </CAlert>
                    )}
                    {error && (
                      <CAlert color="danger" className="w-100">
                        {error}
                      </CAlert>
                    )}
                  </CCardFooter>
                </CCardBody>
              </CCard>
            </CModalBody>
            <CModalFooter>
              <CButton type="submit" color="dark" size="sm">
                {currentModel ? "Actualizar" : "Guardar"}
              </CButton>
              <CButton color="secondary" size="sm" onClick={closeModal}>
                Cancelar
              </CButton>
            </CModalFooter>
          </CForm>
        </CModal>

        <CModal visible={isDeleteModalVisible} onClose={closeDeleteModal}>
          <CModalHeader>
            <CModalTitle>Confirmar</CModalTitle>
          </CModalHeader>
          <CModalBody>
            ¿Estás seguro de que deseas eliminar este elemento?
          </CModalBody>
          <CModalFooter>
            <CButton color="danger" size="sm" onClick={confirmDelete}>
              Eliminar
            </CButton>
            <CButton color="secondary" size="sm" onClick={closeDeleteModal}>
              Cancelar
            </CButton>
          </CModalFooter>
        </CModal>
      </CContainer>
    </div>
  );
};

export default DelegadoTable;
