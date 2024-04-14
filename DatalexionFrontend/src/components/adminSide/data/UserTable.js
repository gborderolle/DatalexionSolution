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
import useInput from "../../../hooks/use-input";
import useAPI from "../../../hooks/use-API";

// redux imports
import { useSelector, useDispatch } from "react-redux";
import {
  fetchUserListByClient,
  fetchUserRole,
  fetchUserRoleList,
} from "../../../store/generalData-actions";
import {
  urlAccount,
  urlAccountRegister,
  urlAccountUpdateUser,
  urlIsUsernameAlreadyRegistered,
} from "../../../endpoints";

const UserTable = (props) => {
  //#region Consts ***********************************

  const [isValidForm, setIsValidForm] = useState(true);
  const { isLoading, isSuccess, error, uploadData, removeData } = useAPI();

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);

  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [idToDelete, setIdToDelete] = useState(null);

  const [ddlSelectedUserRole, setDdlSelectedUserRole] = useState(null);
  const [inputHasErrorUserRole, setInputHasErrorUserRole] = useState(false);

  const [userRoles, setUserRoles] = useState({});
  const [combinedUserList, setCombinedUserList] = useState([]);

  const [usernameError, setUsernameError] = useState("");

  // redux
  const dispatch = useDispatch();

  const clientId = useSelector((state) => state.auth.clientId);

  // Redux
  const userList = useSelector((state) => state.generalData.userList);
  const userRoleList = useSelector((state) => state.generalData.userRoleList);

  const [sortConfig, setSortConfig] = useState({
    key: null,
    direction: "ascending",
  });

  //#region Pagination ***********************************

  const itemsPerPage = 25;
  const [currentPage, setCurrentPage] = useState(1);
  const [pageCount, setPageCount] = useState(0);

  useEffect(() => {
    setPageCount(Math.ceil(userList.length / itemsPerPage));
  }, [userList, itemsPerPage]);

  //#endregion Pagination ***********************************

  useEffect(() => {
    dispatch(fetchUserListByClient());
    dispatch(fetchUserRoleList());
  }, [dispatch]);

  useEffect(() => {
    const updatedList = userList.map((user) => {
      return {
        ...user,
        role: userRoles[user.id] ? userRoles[user.id][0] : "Loading...",
      };
    });
    setCombinedUserList(updatedList);
  }, [userList, userRoles]);

  const {
    value: name,
    isValid: inputIsValid1,
    hasError: inputHasError1,
    valueChangeHandler: inputChangeHandler1,
    inputBlurHandler: inputBlurHandler1,
    reset: inputReset1,
  } = useInput((value) => value.trim() !== "");

  const {
    value: username,
    isValid: inputIsValid2,
    hasError: inputHasError2,
    valueChangeHandler: inputChangeHandler2,
    inputBlurHandler: inputBlurHandler2,
    reset: inputReset2,
  } = useInput((value) => value.trim() !== "");

  const {
    value: password,
    isValid: inputIsValid3,
    hasError: inputHasError3,
    valueChangeHandler: inputChangeHandler3,
    inputBlurHandler: inputBlurHandler3,
    reset: inputReset3,
  } = useInput((value) => value.trim() !== "");

  const {
    value: email,
    isValid: inputIsValid4,
    hasError: inputHasError4,
    valueChangeHandler: inputChangeHandler4,
    inputBlurHandler: inputBlurHandler4,
    reset: inputReset4,
  } = useInput((value) => value.trim());

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    userList.forEach((user) => {
      dispatch(fetchUserRole(user.id)).then((role) => {
        setUserRoles((prevRoles) => ({ ...prevRoles, [user.id]: role }));
      });
    });
  }, [userList, dispatch]);

  const sortedList = useMemo(() => {
    let sortableList = [...combinedUserList];
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
  }, [combinedUserList, sortConfig]);

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
  const openModal = (user = null) => {
    setCurrentUser(user);
    if (user) {
      // Establece los valores iniciales directamente
      inputReset1(user.name);
      inputReset2(user.username);
      inputReset4(user.email);
      const userRoleName = userRoles[user.id][0]; // Obtén el nombre del rol del usuario
      const role = userRoleList.find((r) => r.name === userRoleName);

      // const role = UserGetUserRole(user, userRoleList);
      setDdlSelectedUserRole(role || null);
    } else {
      // Resetea los campos si es un nuevo usuario
      inputReset1();
      inputReset2();
      inputReset3();
      inputReset4();
      setDdlSelectedUserRole(null);
    }
    setIsModalVisible(true);
  };

  const closeModal = () => {
    setIsModalVisible(false);
    setCurrentUser(null);
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
      dispatch(fetchUserListByClient());
      closeDeleteModal();
    }
  };

  const inputResetUserRole = () => {
    setDdlSelectedUserRole(null);
    setInputHasErrorUserRole(false);
  };

  const validateUsername = async (username) => {
    if (username && username.trim() !== "") {
      const requestOptions = {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          "x-version": "1",
        },
      };

      try {
        const response = await fetch(
          `${urlIsUsernameAlreadyRegistered}?username=${username}`,
          requestOptions
        );

        if (!response.ok) {
          throw new Error("Error al validar el nombre de usuario");
        }

        const isAvailable = await response.json();
        setUsernameError(
          isAvailable ? "" : "El nombre de usuario ya está registrado"
        );
      } catch (error) {
        console.error("Error durante la validación:", error);
        setUsernameError("Error al validar el nombre de usuario");
      }
    }
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const formSubmitHandler = async (event) => {
    event.preventDefault();

    const inputIsValidUserRole = ddlSelectedUserRole !== null;
    if (!inputIsValidUserRole) {
      setInputHasErrorUserRole(true);
      return;
    }

    setIsValidForm(inputIsValid1 && inputIsValid2 && inputIsValid3);

    if (!isValidForm) {
      return;
    }

    const dataToUpload = {
      Name: name,
      Username: username,
      Password: password,
      Email: email,
      UserRoleId: ddlSelectedUserRole.id,
      UserRoleName: ddlSelectedUserRole.name,
      ClientId: Number(clientId),
    };

    try {
      let response;
      if (currentUser) {
        response = await uploadData(
          dataToUpload,
          urlAccountUpdateUser,
          true,
          currentUser.id
        );
      } else {
        response = await uploadData(dataToUpload, urlAccountRegister);
      }
      if (response) {
        dispatch(fetchUserListByClient());
        inputReset1();
        inputReset2();
        inputResetUserRole();

        setTimeout(() => {
          closeModal();
        }, 1000);
      }
    } catch (error) {
      console.error("Error al enviar los datos:", error);
    }
  };

  const handleSelectDdlUserRole = (item) => {
    setDdlSelectedUserRole(item);
  };

  const usernameBlurHandler = () => {
    inputBlurHandler2(); // Llama al manejador de onBlur existente si lo tienes
    validateUsername(username);
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
              Nombre completo
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("username")}>
              Usuario
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("email")}>
              Email
            </CTableHeaderCell>
            <CTableHeaderCell onClick={() => requestSort("role")}>
              Rol
            </CTableHeaderCell>
            <CTableHeaderCell>Acciones</CTableHeaderCell>
          </CTableRow>
        </CTableHead>
        <CTableBody>
          {currentList.map((user, index) => {
            return (
              <CTableRow key={user.id}>
                <CTableDataCell>{index + 1}</CTableDataCell>
                <CTableDataCell>{user.name}</CTableDataCell>
                <CTableDataCell>{user.userName}</CTableDataCell>
                <CTableDataCell>{user.email}</CTableDataCell>
                <CTableDataCell>
                  {userRoles[user.id] ? (
                    userRoles[user.id]
                  ) : (
                    <CSpinner size="sm" />
                  )}
                </CTableDataCell>
                <CTableDataCell>
                  <CButton
                    color="dark"
                    size="sm"
                    onClick={() => openModal(user)}
                  >
                    Editar
                  </CButton>
                  <CButton
                    color="danger"
                    size="sm"
                    onClick={() => openDeleteModal(user.id)}
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
                    {props.username}
                  </CInputGroupText>
                  <CFormInput
                    type="text"
                    className="cardItem"
                    onChange={inputChangeHandler2}
                    onBlur={usernameBlurHandler}
                    value={username}
                  />
                  {usernameError && (
                    <CAlert color="danger" className="w-100">
                      {usernameError}
                    </CAlert>
                  )}
                </CInputGroup>
                <br />
                <CInputGroup>
                  <CInputGroupText className="cardItem custom-input-group-text">
                    {props.password}
                  </CInputGroupText>
                  <CFormInput
                    type="password"
                    className="cardItem"
                    onChange={inputChangeHandler3}
                    onBlur={inputBlurHandler3}
                    value={password}
                  />
                  {inputHasError3 && (
                    <CAlert color="danger" className="w-100">
                      Entrada inválida
                    </CAlert>
                  )}
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
                    Rol de usuario
                  </CInputGroupText>
                  <CDropdown>
                    <CDropdownToggle id="ddlUserRole" color="secondary">
                      {ddlSelectedUserRole
                        ? ddlSelectedUserRole.name
                        : "Seleccionar"}
                    </CDropdownToggle>
                    <CDropdownMenu>
                      {userRoleList &&
                        userRoleList.length > 0 &&
                        userRoleList.map((userRole, index) => (
                          <CDropdownItem
                            key={userRole.id}
                            onClick={() => handleSelectDdlUserRole(userRole)}
                            style={{ cursor: "pointer" }}
                            value={userRole.id}
                          >
                            {`${index + 1}: ${userRole.name}`}
                          </CDropdownItem>
                        ))}
                    </CDropdownMenu>
                  </CDropdown>
                  {inputHasErrorUserRole && (
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

export default UserTable;
