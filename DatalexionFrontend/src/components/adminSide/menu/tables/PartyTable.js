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
  CCard,
  CCardBody,
  CCardFooter,
  CAlert,
  CPagination,
  CPaginationItem,
  CContainer,
} from "@coreui/react";
import useInput from "../../../../hooks/use-input";
import useAPI from "../../../../hooks/use-API";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import { fetchPartyListByClient } from "../../../../store/generalData-actions";
import { urlParty } from "../../../../endpoints";

const PartyTable = (props) => {
  //#region Consts ***********************************

  const [isValidForm, setIsValidForm] = useState(true);
  const { isLoading, isSuccess, error, uploadData, removeData } = useAPI();

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);

  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [idToDelete, setIdToDelete] = useState(null);

  const [currentImageLong, setCurrentImageLong] = useState(null);
  const [imagePreviewLong, setImagePreviewLong] = useState(null);

  const [currentImageShort, setCurrentImageShort] = useState(null);
  const [imagePreviewShort, setImagePreviewShort] = useState(null);

  // redux
  const dispatch = useDispatch();

  // Redux
  const partyListByClient = useSelector(
    (state) => state.generalData.partyListByClient
  );

  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });

  //#region Pagination ***********************************

  const itemsPerPage = 25;
  const [currentPage, setCurrentPage] = useState(1);
  const [pageCount, setPageCount] = useState(0);

  useEffect(() => {
    setPageCount(Math.ceil(partyListByClient.length / itemsPerPage));
  }, [partyListByClient, itemsPerPage]);

  //#endregion Pagination ***********************************

  useEffect(() => {
    dispatch(fetchPartyListByClient());
  }, [dispatch]);

  const {
    value: name,
    isValid: inputIsValid1,
    hasError: inputHasError1,
    valueChangeHandler: inputChangeHandler1,
    inputBlurHandler: inputBlurHandler1,
    reset: inputReset1,
  } = useInput((value) => value.trim() !== "");

  const {
    value: shortName,
    isValid: inputIsValid2,
    hasError: inputHasError2,
    valueChangeHandler: inputChangeHandler2,
    inputBlurHandler: inputBlurHandler2,
    reset: inputReset2,
  } = useInput((value) => value.trim() !== "");

  const {
    value: color,
    isValid: inputIsValid3,
    hasError: inputHasError3,
    valueChangeHandler: inputChangeHandler3,
    inputBlurHandler: inputBlurHandler3,
    reset: inputReset3,
  } = useInput((value) => value.trim() !== "");

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  const sortedList = useMemo(() => {
    let sortableList = [...partyListByClient];
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
  }, [partyListByClient, sortConfig]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const requestSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  // Manejo de la imagen
  const handleImageChange = (e, setImage, setCurrentImage) => {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0];
      const fileUrl = URL.createObjectURL(file);
      setImage(fileUrl);
      setCurrentImage(file);
    }
  };

  // Si la entidad es nula, se asume que se está creando una nueva, sino se está editando
  const openModal = (party = null) => {
    setCurrentUser(party);
    if (party) {
      // Establece los valores iniciales directamente
      inputReset1(party.name ?? "");
      inputReset2(party.shortName ?? "");
      inputReset3(party.color ?? "");

      setCurrentImageLong(party.photoLong);
      setImagePreviewLong(null);
      setCurrentImageShort(party.photoShort);
      setImagePreviewShort(null);
    } else {
      // Resetea los campos si es un nuevo usuario
      inputReset1();
      inputReset2();
      inputReset3();

      setCurrentImageLong(null);
      setImagePreviewLong(null);
      setCurrentImageShort(null);
      setImagePreviewShort(null);
    }
    setIsModalVisible(true);
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setCurrentUser(null);

    setCurrentImageLong(null);
    setImagePreviewLong(null);
    setCurrentImageShort(null);
    setImagePreviewShort(null);
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
      await removeData(urlParty, idToDelete);
      dispatch(fetchPartyListByClient());
      closeDeleteModal();
    }
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const formSubmitHandler = async (event) => {
    event.preventDefault();

    setIsValidForm(inputIsValid1 && inputIsValid2 && inputIsValid3);

    if (!isValidForm) {
      return;
    }

    const dataToUpload = new FormData();
    dataToUpload.append("Name", name);
    dataToUpload.append("ShortName", shortName);
    dataToUpload.append("Color", color);

    // Asegurar que el input de imagen larga contiene un archivo
    const fileLongInput = event.target.elements.fileLong;
    if (fileLongInput && fileLongInput.files.length > 0) {
      dataToUpload.append("fileLong", fileLongInput.files[0]);
    }

    // Asegurar que el input de imagen corta contiene un archivo
    const fileShortInput = event.target.elements.fileShort;
    if (fileShortInput && fileShortInput.files.length > 0) {
      dataToUpload.append("fileShort", fileShortInput.files[0]);
    }

    try {
      let response;
      if (currentUser) {
        response = await uploadData(
          dataToUpload,
          urlParty,
          true,
          currentUser.id
        );
      } else {
        response = await uploadData(dataToUpload, urlParty);
      }
      if (response) {
        dispatch(fetchPartyListByClient());
        inputReset1();
        inputReset2();
        inputReset3();

        setTimeout(() => {
          closeModal();
        }, 1000);
      }
    } catch (error) {
      console.error("Error al enviar los datos:", error);
    }
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
                  Nombre
                </CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("shortName")}>
                  Nombre corto
                </CTableHeaderCell>
                <CTableHeaderCell onClick={() => requestSort("color")}>
                  Color
                </CTableHeaderCell>
                <CTableHeaderCell>Acciones</CTableHeaderCell>
              </CTableRow>
            </CTableHead>
            <CTableBody>
              {currentList.map((party, index) => {
                return (
                  <CTableRow key={party.id}>
                    <CTableDataCell>{index + 1}</CTableDataCell>
                    <CTableDataCell>{party.name}</CTableDataCell>
                    <CTableDataCell>{party.shortName}</CTableDataCell>
                    <CTableDataCell>{party.color}</CTableDataCell>
                    <CTableDataCell>
                      <CButton
                        color="dark"
                        size="sm"
                        onClick={() => openModal(party)}
                      >
                        Editar
                      </CButton>
                      <CButton
                        color="danger"
                        size="sm"
                        onClick={() => openDeleteModal(party.id)}
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
                      {props.shortName}
                    </CInputGroupText>
                    <CFormInput
                      type="text"
                      className="cardItem"
                      onChange={inputChangeHandler2}
                      onBlur={inputBlurHandler2}
                      value={shortName}
                    />
                    {inputHasError2 && (
                      <CAlert color="danger" className="w-100">
                        Entrada inválida
                      </CAlert>
                    )}
                  </CInputGroup>
                  <br />
                  <CInputGroup>
                    <CInputGroupText className="cardItem custom-input-group-text">
                      {props.color}
                    </CInputGroupText>
                    <CFormInput
                      type="text"
                      className="cardItem"
                      onChange={inputChangeHandler3}
                      onBlur={inputBlurHandler3}
                      value={color}
                    />
                    {inputHasError3 && (
                      <CAlert color="danger" className="w-100">
                        Entrada inválida
                      </CAlert>
                    )}
                  </CInputGroup>
                  <br />
                  <CInputGroup>
                    <CInputGroupText>Imagen Larga</CInputGroupText>
                    <CFormInput
                      type="file"
                      onChange={(e) =>
                        handleImageChange(
                          e,
                          setImagePreviewLong,
                          setCurrentImageLong
                        )
                      }
                      name="fileLong"
                    />
                  </CInputGroup>
                  {imagePreviewLong || currentImageLong ? (
                    <img
                      src={imagePreviewLong || currentImageLong}
                      alt="preview"
                      style={{
                        width: "100px",
                        height: "100px",
                        marginTop: "10px",
                      }}
                    />
                  ) : null}
                  <br />
                  <CInputGroup>
                    <CInputGroupText>Imagen Corta</CInputGroupText>
                    <CFormInput
                      type="file"
                      onChange={(e) =>
                        handleImageChange(
                          e,
                          setImagePreviewShort,
                          setCurrentImageShort
                        )
                      }
                      name="fileShort"
                    />
                  </CInputGroup>
                  {imagePreviewShort || currentImageShort ? (
                    <img
                      src={imagePreviewShort || currentImageShort}
                      alt="preview"
                      style={{
                        width: "100px",
                        height: "100px",
                        marginTop: "10px",
                      }}
                    />
                  ) : null}
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
      </CContainer>
    </div>
  );
};

export default PartyTable;
