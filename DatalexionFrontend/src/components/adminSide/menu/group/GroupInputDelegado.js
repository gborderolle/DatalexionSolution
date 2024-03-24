import React from "react";
import { useState } from "react";

import {
  CRow,
  CButton,
  CCardFooter,
  CSpinner,
  CFormInput,
  CInputGroup,
  CInputGroupText,
  CAlert,
  CForm,
  CDropdown,
  CDropdownToggle,
  CDropdownMenu,
  CDropdownItem,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
} from "@coreui/react";
import useInput from "../../../../hooks/use-input";
// import useFirebase from "../../../../hooks/use-firebase";
import useAPI from "../../../../hooks/use-API";

// redux imports
import { useSelector } from "react-redux";

import "./GroupInput.css";

const GroupInputDelegado = (props) => {
  //#region Consts ***********************************

  const [isValidForm, setIsValidForm] = useState(true); // Declarar e inicializar isValidForm
  const [ddlSelectedDelegadoMunicipality, setDdlSelectDelegadoMunicipality] =
    useState(null);

  const { isLoading, isSuccess, uploadData } = useAPI(); //useFirebase();
  const [inputHasErrorMunicipality, setInputHasErrorMunicipality] =
    useState(false);

  // Redux
  const municipalityList = useSelector(
    (state) => state.generalData.municipalityList
  );

  const {
    value: delegadoName,
    isValid: inputIsValid1,
    hasError: inputHasError1,
    valueChangeHandler: inputChangeHandler1,
    inputBlurHandler: inputBlurHandler1,
    reset: inputReset1,
  } = useInput((value) => value.trim() !== "");

  const {
    value: delegadoCI,
    isValid: inputIsValid2,
    hasError: inputHasError2,
    valueChangeHandler: inputChangeHandler2,
    inputBlurHandler: inputBlurHandler2,
    reset: inputReset2,
  } = useInput((value) => /^[0-9]{8}$/.test(value.trim()));

  const {
    value: delegadoPhone,
    isValid: inputIsValid3,
    hasError: inputHasError3,
    valueChangeHandler: inputChangeHandler3,
    inputBlurHandler: inputBlurHandler3,
    reset: inputReset3,
  } = useInput((value) => /^[0-9]{9}$/.test(value.trim()));

  //#endregion Consts ***********************************

  //#region Functions ***********************************

  const inputResetMunicipality = () => {
    setDdlSelectDelegadoMunicipality(null);
    setInputHasErrorMunicipality(false);
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const formSubmitHandler = async (event) => {
    event.preventDefault();

    // Verificar si se seleccionó una provincia
    const inputIsValidMunicipality = setDdlSelectDelegadoMunicipality !== null;
    if (!inputIsValidMunicipality) {
      setInputHasErrorMunicipality(true);
      return;
    }

    setIsValidForm(
      inputIsValid1 &&
        inputIsValid2 &&
        inputIsValid3 &&
        inputIsValidMunicipality
    );

    if (!isValidForm) {
      return;
    }

    const dataToUpload = await props.createDataToUpload(
      delegadoName,
      delegadoCI,
      Number(delegadoPhone),
      ddlSelectedDelegadoMunicipality.provinceId,
      ddlSelectedDelegadoMunicipality.municipalityId
    );

    await uploadData(dataToUpload, props.firebaseUrlName);

    inputReset1();
    inputReset2();
    inputReset3();

    inputResetMunicipality();
  };

  const handleSelectDdlDelegadoMunicipality = (item) => {
    setDdlSelectDelegadoMunicipality(item);
  };

  //#endregion Events ***********************************

  return (
    <CForm onSubmit={formSubmitHandler}>
      <CAccordion>
        <CAccordionItem itemKey={1}>
          <CAccordionHeader className="custom-accordion-header">
            {props.title}
          </CAccordionHeader>
          <CAccordionBody>
            <CInputGroup>
              <CInputGroupText className="cardItem custom-input-group-text">
                {props.inputFullname}
              </CInputGroupText>
              <CFormInput
                type="text"
                className="cardItem"
                onChange={inputChangeHandler1}
                onBlur={inputBlurHandler1}
                value={delegadoName}
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
                {props.inputCI}
              </CInputGroupText>
              <CFormInput
                type="number"
                className="cardItem"
                onChange={inputChangeHandler2}
                onBlur={inputBlurHandler2}
                value={delegadoCI}
                placeholder="1234567-8"
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
                {props.inputPhone}
              </CInputGroupText>
              <CFormInput
                type="number"
                className="cardItem"
                onChange={inputChangeHandler3}
                onBlur={inputBlurHandler3}
                value={delegadoPhone}
                placeholder="099 123 456"
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
                {props.inputMunicipality}
              </CInputGroupText>
              {/*  */}
              <CDropdown>
                <CDropdownToggle id="ddlDelegadoMunicipality" color="secondary">
                  {ddlSelectedDelegadoMunicipality
                    ? ddlSelectedDelegadoMunicipality.name
                    : "Seleccionar"}
                </CDropdownToggle>
                <CDropdownMenu>
                  {municipalityList &&
                    municipalityList.map((municipality, index) => (
                      <CDropdownItem
                        key={municipality.id}
                        onClick={() =>
                          handleSelectDdlDelegadoMunicipality(municipality)
                        }
                        style={{ cursor: "pointer" }}
                        value={municipality.id}
                      >
                        {municipality.name}
                      </CDropdownItem>
                    ))}
                </CDropdownMenu>
              </CDropdown>
              {/*  */}
              {inputHasErrorMunicipality && (
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
            <CButton type="submit" color="dark">
              Confirmar
            </CButton>
            <br />
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
            </CCardFooter>
          </CAccordionBody>
        </CAccordionItem>
      </CAccordion>
    </CForm>
  );
};

export default GroupInputDelegado;
