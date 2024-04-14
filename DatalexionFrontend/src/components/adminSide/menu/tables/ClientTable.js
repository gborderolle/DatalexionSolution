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
} from "@coreui/react";
import useInput from "../../../../hooks/use-input";
import useAPI from "../../../../hooks/use-API";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import {
  fetchClient,
  fetchPartyList,
} from "../../../../store/generalData-actions";
import { urlClient } from "../../../../endpoints";
import { ClientGetParty } from "src/utils/auxiliarFunctions";

const ClientTable = (props) => {
  //#region Consts ***********************************

  const [isValidForm, setIsValidForm] = useState(true);
  const { isLoading, isSuccess, error, uploadData } = useAPI();

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);

  const [ddlSelectedParty, setDdlSelectedParty] = useState(null);
  const [inputHasErrorParty, setInputHasErrorParty] = useState(false);

  // redux
  const dispatch = useDispatch();

  // Redux
  const clientRedux = useSelector((state) => state.generalData.client);
  const partyList = useSelector((state) => state.generalData.partyList);

  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });

  useEffect(() => {
    dispatch(fetchClient());
    dispatch(fetchPartyList());
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

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  const requestSort = (key) => {
    let direction = "ascending";
    if (sortConfig.key === key && sortConfig.direction === "ascending") {
      direction = "descending";
    }
    setSortConfig({ key, direction });
  };

  // Si la entidad es nula, se asume que se está creando una nueva, sino se está editando
  const openModal = (client = null) => {
    setCurrentUser(client);
    if (client) {
      // Establece los valores iniciales directamente
      inputReset1(client.name);
      const party = ClientGetParty(client, partyList);
      setDdlSelectedParty(party || null);
    } else {
      // Resetea los campos si es un nuevo usuario
      inputReset1();
      setDdlSelectedParty(null);
    }
    setIsModalVisible(true);
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setCurrentUser(null);
  };

  const inputResetParty = () => {
    setDdlSelectedParty(null);
    setInputHasErrorParty(false);
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const formSubmitHandler = async (event) => {
    event.preventDefault();

    const inputIsValidParty = ddlSelectedParty !== null;
    if (!inputIsValidParty) {
      setInputHasErrorParty(true);
      return;
    }

    setIsValidForm(inputIsValid1);

    if (!isValidForm) {
      return;
    }

    const dataToUpload = {
      Name: name,
      PartyId: ddlSelectedParty.id,
    };

    try {
      let response;
      if (currentUser) {
        response = await uploadData(
          dataToUpload,
          urlClient,
          true,
          currentUser.id
        );
      } else {
        response = await uploadData(dataToUpload, urlClient);
      }
      if (response) {
        dispatch(fetchClient());
        inputReset1();
        inputResetParty();

        setTimeout(() => {
          closeModal();
        }, 1000);
      }
    } catch (error) {
      console.error("Error al enviar los datos:", error);
    }
  };

  const handleSelectDdlParty = (item) => {
    setDdlSelectedParty(item);
  };

  //#endregion Events ***********************************

  return (
    <div>
      <CTable striped>
        <CTableHead>
          <CTableRow>
            <CTableHeaderCell onClick={() => requestSort("name")}>
              Nombre
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("party")}>
              Partido
            </CTableHeaderCell>
            <CTableHeaderCell>Acciones</CTableHeaderCell>
          </CTableRow>
        </CTableHead>
        <CTableBody>
          <CTableRow>
            <CTableDataCell>{clientRedux.name}</CTableDataCell>
            <CTableDataCell>
              {ClientGetParty(clientRedux, partyList)?.name}
            </CTableDataCell>
            <CTableDataCell>
              <CButton
                color="dark"
                size="sm"
                onClick={() => openModal(clientRedux)}
              >
                Editar
              </CButton>
            </CTableDataCell>
          </CTableRow>
        </CTableBody>
      </CTable>

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
                    {props.party}
                  </CInputGroupText>
                  <CDropdown>
                    <CDropdownToggle id="ddlParty" color="secondary">
                      {ddlSelectedParty ? ddlSelectedParty.name : "Seleccionar"}
                    </CDropdownToggle>
                    <CDropdownMenu>
                      {partyList &&
                        partyList.length > 0 &&
                        partyList.map((party) => (
                          <CDropdownItem
                            key={party.id}
                            onClick={() => handleSelectDdlParty(party)}
                            style={{ cursor: "pointer" }}
                            value={party.id}
                          >
                            {`${party.id}: ${party.name}`}
                          </CDropdownItem>
                        ))}
                    </CDropdownMenu>
                  </CDropdown>
                  {inputHasErrorParty && (
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
    </div>
  );
};

export default ClientTable;
