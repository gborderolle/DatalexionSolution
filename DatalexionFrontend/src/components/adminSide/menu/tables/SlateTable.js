import React, { useState, useEffect, useMemo } from "react";
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
} from "@coreui/react";
import useInput from "../../../../hooks/use-input";
import useAPI from "../../../../hooks/use-API";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import {
  fetchSlateListByClient,
  fetchWingListByClient,
  fetchCandidateListByClient,
  fetchProvinceList,
} from "../../../../store/generalData-actions";
import { urlSlate } from "../../../../endpoints";
import {
  SlateGetWing,
  SlateGetParty,
  SlateGetCandidate,
  SlateGetProvince,
} from "../../../../utils/auxiliarFunctions";

const SlateTable = (props) => {
  //#region Consts ***********************************

  const [isValidForm, setIsValidForm] = useState(true);
  const { isLoading, isSuccess, error, uploadData, removeData } = useAPI();

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentUser, setCurrentSlate] = useState(null);

  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [idToDelete, setIdToDelete] = useState(null);

  const [ddlSelectedWing, setDdlSelectdWing] = useState(null);
  const [inputHasErrordWing, setInputHasErrorWing] = useState(false);

  const [ddlSelectedProvince, setDdlSelectdProvince] = useState(null);
  const [inputHasErrordProvince, setInputHasErrorProvince] = useState(false);

  const [ddlSelectedCandidate, setDdlSelectdCandidate] = useState(null);
  const [inputHasErrordCandidate, setInputHasErrorCandidate] = useState(false);

  // redux
  const dispatch = useDispatch();

  // Redux
  const slateList = useSelector((state) => state.generalData.slateListByClient);
  const wingList = useSelector((state) => state.generalData.wingListByClient);
  const partyList = useSelector((state) => state.generalData.partyListByClient);
  const candidateList = useSelector((state) => state.generalData.candidateList);
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
    setPageCount(Math.ceil(slateList.length / itemsPerPage));
  }, [slateList, itemsPerPage]);

  //#endregion Pagination ***********************************

  useEffect(() => {
    dispatch(fetchSlateListByClient());
    dispatch(fetchWingListByClient());
    dispatch(fetchCandidateListByClient());
    dispatch(fetchProvinceList());
  }, [dispatch]);

  const {
    value: name,
    isValid: inputIsValid1,
    hasError: inputHasError1,
    valueChangeHandler: inputChangeHandler1,
    inputBlurHandler: inputBlurHandler1,
    reset: inputReset1,
  } = useInput((value) => value.trim() !== "");

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  const sortedList = useMemo(() => {
    let sortableList = [...slateList];
    if (sortConfig.key !== null) {
      sortableList.sort((a, b) => {
        // Comparación especial para el rol
        if (sortConfig.key === "role") {
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
  }, [slateList, sortConfig]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const requestSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  const openModal = (slate = null) => {
    setCurrentSlate(slate);
    if (slate) {
      // Establece los valores iniciales directamente
      inputReset1(slate.name);
      const wing = SlateGetWing(slate, wingList);
      setDdlSelectdWing(wing || null);
      const province = SlateGetProvince(slate, provinceList);
      setDdlSelectdProvince(province || null);
      const candidate = SlateGetCandidate(slate, candidateList);
      setDdlSelectdCandidate(candidate || null);
    } else {
      // Resetea los campos si es un nuevo usuario
      inputReset1();
      setDdlSelectdWing(null);
      setDdlSelectdProvince(null);
      setDdlSelectdCandidate(null);
    }
    setIsModalVisible(true);
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setCurrentSlate(null);
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
      await removeData(urlMunicipality, idToDelete);
      dispatch(fetchSlateListByClient());
      closeDeleteModal();
    }
  };

  const inputResetWing = () => {
    setDdlSelectdWing(null);
    setInputHasErrorWing(false);
  };

  const inputResetProvince = () => {
    setDdlSelectdProvince(null);
    setInputHasErrorProvince(false);
  };

  const inputResetCandidate = () => {
    setDdlSelectdCandidate(null);
    setInputHasErrorCandidate(false);
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const formSubmitHandler = async (event) => {
    event.preventDefault();

    const inputIsValidWing = ddlSelectedWing !== null;
    if (!inputIsValidWing) {
      setInputHasErrorWing(true);
      return;
    }

    const inputIsValidProvince = ddlSelectedProvince !== null;
    if (!inputIsValidProvince) {
      setInputHasErrorProvince(true);
      return;
    }

    const inputIsValidCandidate = ddlSelectedCandidate !== null;
    if (!inputIsValidCandidate) {
      setInputHasErrorCandidate(true);
      return;
    }

    setIsValidForm(inputIsValid1);

    if (!isValidForm) {
      return;
    }

    const dataToUpload = {
      Name: name,
      WingId: ddlSelectedWing.id,
      ProvinceId: ddlSelectedProvince.id,
      CandidateId: ddlSelectedCandidate.id,
    };

    try {
      let response;
      if (currentUser) {
        response = await uploadData(
          dataToUpload,
          urlSlate,
          true,
          currentUser.id
        );
      } else {
        response = await uploadData(dataToUpload, urlSlate);
      }
      if (response) {
        dispatch(fetchSlateListByClient());
        inputReset1();
        inputResetWing();
        inputResetProvince();
        inputResetCandidate();

        setTimeout(() => {
          closeModal();
        }, 1000);
      }
    } catch (error) {
      console.error("Error al enviar los datos:", error);
    }
  };

  const handleSelectDdlWing = (item) => {
    setDdlSelectdWing(item);
  };

  const handleSelectDdlProvince = (item) => {
    setDdlSelectdProvince(item);
  };

  const handleSelectDdlCandidate = (item) => {
    setDdlSelectdCandidate(item);
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
      <CButton color="dark" size="sm" onClick={() => openModal()}>
        Agregar
      </CButton>
      <CTable striped>
        <CTableHead>
          <CTableRow>
            <CTableHeaderCell>#</CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("name")}>
              Nombre
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("wing")}>
              Candidato
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("wing")}>
              Agrupación
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("party")}>
              Partido
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("province")}>
              Departamento
            </CTableHeaderCell>
            <CTableHeaderCell>Acciones</CTableHeaderCell>
          </CTableRow>
        </CTableHead>
        <CTableBody>
          {currentList.map((slate, index) => {
            return (
              <CTableRow key={slate.id}>
                <CTableDataCell>{index + 1}</CTableDataCell>
                <CTableDataCell>{slate.name}</CTableDataCell>
                <CTableDataCell>
                  {SlateGetCandidate(slate, candidateList)?.name}
                </CTableDataCell>
                <CTableDataCell>
                  {SlateGetWing(slate, wingList)?.name}
                </CTableDataCell>
                <CTableDataCell>
                  {SlateGetParty(slate, wingList, partyList)?.name}
                </CTableDataCell>
                <CTableDataCell>
                  {SlateGetProvince(slate, provinceList)?.name}
                </CTableDataCell>
                <CTableDataCell>
                  <CButton
                    color="dark"
                    size="sm"
                    onClick={() => openModal(slate)}
                  >
                    Editar
                  </CButton>
                  <CButton
                    color="danger"
                    size="sm"
                    onClick={() => openDeleteModal(slate.id)}
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
            {currentUser ? "Editar entidad" : "Agregar entidad"}
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
                    {props.candidate}
                  </CInputGroupText>
                  <CDropdown>
                    <CDropdownToggle id="ddlCandidate" color="secondary">
                      {ddlSelectedWing
                        ? ddlSelectedCandidate.name
                        : "Seleccionar"}
                    </CDropdownToggle>
                    <CDropdownMenu>
                      {candidateList &&
                        candidateList.length > 0 &&
                        candidateList.map((candidate) => (
                          <CDropdownItem
                            key={candidate.id}
                            onClick={() => handleSelectDdlCandidate(candidate)}
                            style={{ cursor: "pointer" }}
                            value={candidate.id}
                          >
                            {`${candidate.id}: ${candidate.name}`}
                          </CDropdownItem>
                        ))}
                    </CDropdownMenu>
                  </CDropdown>
                  {inputHasErrordCandidate && (
                    <CAlert color="danger" className="w-100">
                      Entrada inválida
                    </CAlert>
                  )}
                </CInputGroup>
                <br />
                <CInputGroup>
                  <CInputGroupText className="cardItem custom-input-group-text">
                    {props.wing}
                  </CInputGroupText>
                  <CDropdown>
                    <CDropdownToggle id="ddlWing" color="secondary">
                      {ddlSelectedWing ? ddlSelectedWing.name : "Seleccionar"}
                    </CDropdownToggle>
                    <CDropdownMenu>
                      {wingList &&
                        wingList.length > 0 &&
                        wingList.map((wing) => (
                          <CDropdownItem
                            key={wing.id}
                            onClick={() => handleSelectDdlWing(wing)}
                            style={{ cursor: "pointer" }}
                            value={wing.id}
                          >
                            {`${wing.id}: ${wing.name}`}
                          </CDropdownItem>
                        ))}
                    </CDropdownMenu>
                  </CDropdown>
                  {inputHasErrordWing && (
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
                    <CDropdownToggle id="ddlProvince" color="secondary">
                      {ddlSelectedProvince
                        ? ddlSelectedProvince.name
                        : "Seleccionar"}
                    </CDropdownToggle>
                    <CDropdownMenu>
                      {provinceList &&
                        provinceList.length > 0 &&
                        provinceList.map((province) => (
                          <CDropdownItem
                            key={province.id}
                            onClick={() => handleSelectDdlProvince(province)}
                            style={{ cursor: "pointer" }}
                            value={province.id}
                          >
                            {`${province.id}: ${province.name}`}
                          </CDropdownItem>
                        ))}
                    </CDropdownMenu>
                  </CDropdown>
                  {inputHasErrordProvince && (
                    <CAlert color="danger" className="w-100">
                      Entrada inválida
                    </CAlert>
                  )}
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
              {currentUser ? "Actualizar" : "Guardar"}
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
    </div>
  );
};

export default SlateTable;