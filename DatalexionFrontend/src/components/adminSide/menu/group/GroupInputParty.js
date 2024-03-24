import React from "react";
import { useState } from "react";

import {
  CRow,
  CButton,
  CCardFooter,
  CSpinner,
  CCardTitle,
  CCardBody,
  CFormInput,
  CInputGroup,
  CInputGroupText,
  CAlert,
  CForm,
  CCard,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
} from "@coreui/react";
import useInput from "../../../../hooks/use-input";
// import useFirebase from "../../../../hooks/use-firebase";
import useAPI from "../../../../hooks/use-API";

// import { getStorage, ref, uploadBytes, getDownloadURL } from "firebase/storage";

import "./GroupInput.css";

const GroupInputParty = (props) => {
  //#region Const ***********************************

  const [isValidForm, setIsValidForm] = useState(true); // Declarar e inicializar isValidForm
  const [color, setColor] = useState("#ffffff");
  const [image, setImage] = useState(null);
  const [imageURL, setImageURL] = useState("");

  const { isLoading, isSuccess, uploadData } = useAPI(); //useFirebase();
  const fileInputRef = React.useRef();

  const {
    value: partyName,
    isValid: inputIsValid1,
    hasError: inputHasError1,
    valueChangeHandler: inputChangeHandler1,
    inputBlurHandler: inputBlurHandler1,
    reset: inputReset1,
  } = useInput((value) => value.trim() !== "");

  const {
    value: partyNameLong,
    isValid: inputIsValid2,
    hasError: inputHasError2,
    valueChangeHandler: inputChangeHandler2,
    inputBlurHandler: inputBlurHandler2,
    reset: inputReset2,
  } = useInput((value) => value.trim() !== "");

  //#endregion Const ***********************************

  //#region Functions ***********************************

  const resetForm = () => {
    setImage(null);
    setImageURL("");

    // Restablece el valor del input de tipo 'file'
    if (fileInputRef.current) {
      fileInputRef.current.value = "";
    }

    // Llama a cualquier otro reset que necesites realizar
    inputReset1();
    inputReset2();
  };

  // Función para cargar la imagen a Firebase Storage
  const uploadImage = async () => {
    if (image) {
      // const storage = getStorage();
      // const storageRef = ref(storage, `assets/images/parties/${image.name}`);
      // await uploadBytes(storageRef, image);
      // const url = await getDownloadURL(storageRef);
      // return url; // Retorna la URL de la imagen
    }
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  const imageChangeHandler = (event) => {
    const file = event.target.files[0];
    if (file) {
      setImage(file);
    }
  };

  const formSubmitHandler = async (event) => {
    event.preventDefault();

    setIsValidForm(inputIsValid1 && inputIsValid2);

    if (!isValidForm) {
      return;
    }

    // Espera a que la imagen se cargue y obtén la URL
    const uploadedImageURL = await uploadImage();
    // Asegúrate de que la imagen se haya cargado antes de continuar
    if (uploadedImageURL) {
      setImageURL(uploadedImageURL); // Establece la URL de la imagen en el estado

      const dataToUpload = await props.createDataToUpload(
        partyName,
        partyNameLong,
        uploadedImageURL,
        color
      ); // Usa la URL cargada

      await uploadData(dataToUpload, props.firebaseUrlName);

      resetForm();
    } else {
      // Manejar el caso en que la imagen no se cargó
      console.error("La carga de la imagen falló");
    }
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
                {props.labelName}
              </CInputGroupText>
              <CFormInput
                type="text"
                className="cardItem"
                onChange={inputChangeHandler1}
                onBlur={inputBlurHandler1}
                value={partyName}
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
                {props.labelNameLong}
              </CInputGroupText>
              <CFormInput
                type="text"
                className="cardItem"
                onChange={inputChangeHandler2}
                onBlur={inputBlurHandler2}
                value={partyNameLong}
              />
              {inputHasError2 && (
                <CAlert color="danger" className="w-100">
                  Entrada inválida
                </CAlert>
              )}
            </CInputGroup>
            <br />
            <CInputGroup>
              <CInputGroupText>Imagen</CInputGroupText>
              <CFormInput
                type="file"
                onChange={imageChangeHandler}
                ref={fileInputRef}
              />
            </CInputGroup>
            <br />
            <CInputGroup>
              <CInputGroupText>Color distintivo</CInputGroupText>
              <CFormInput
                type="color"
                value={color}
                onChange={(e) => setColor(e.target.value)}
              />
            </CInputGroup>

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

export default GroupInputParty;
