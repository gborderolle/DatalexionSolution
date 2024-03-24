import React from "react";
import FooterAlert from "./FooterAlert";

import { CBadge, CCardFooter } from "@coreui/react";

import useHighlightedState from "../../../utils/useHighlightedState";

// redux imports
import { useSelector } from "react-redux";

import classes from "./Footer.module.css";

const FooterEspecial = ({
  isSuccess1,
  isSuccess2,
  isValidForm1,
  isValidForm2,
  labelText1,
  labelText2,
  labelText3,
  labelStyle1,
  labelStyle2,
  useAlert,
  labelVotesVALOR1,
  labelVotesVALOR2,
}) => {
  //#region Consts ***********************************

  // redux get
  const stepSummaryIsVisible = useSelector(
    (state) => state.ui.stepSummaryIsVisible
  );

  // bump effect 1
  const [isHighlighted1] = useHighlightedState(false, labelVotesVALOR1);
  const createBadgeClass1 = (isHighlighted) =>
    `headerBadge ${isHighlighted ? classes.bump : ""}`;
  const buttonClass1 = createBadgeClass1(isHighlighted1);
  // bump effect 2
  const [isHighlighted2] = useHighlightedState(false, labelVotesVALOR2);
  const createBadgeClass2 = (isHighlighted) =>
    `headerBadge ${isHighlighted ? classes.bump : ""}`;
  const buttonClass2 = createBadgeClass2(isHighlighted2);
  // bump effect 2

  //#endregion Consts ***********************************

  return (
    <>
      <CCardFooter
        className="text-medium-emphasis p-1"
        style={{
          textAlign: "center",
          display: "flex", // Establece el contenedor como flex
          justifyContent: "center", // Alinea los elementos hijos horizontalmente en el centro
          alignItems: "center", // Alinea los elementos hijos verticalmente en el centro
        }}
      >
        {!stepSummaryIsVisible && (
          <>
            <div style={{ margin: "0 10px" }}>
              {" "}
              {/* Añade margen a los div para separarlos */}
              {useAlert && (
                <FooterAlert
                  isValid={isValidForm1}
                  isSuccess={isSuccess1}
                  errorMsg="El formulario no es válido."
                  successMsg="Datos enviados correctamente."
                />
              )}
              <h6>
                {labelText1}&nbsp;
                <CBadge
                  color={labelStyle1}
                  className={buttonClass1}
                  style={{ fontSize: "inherit" }}
                >
                  {isNaN(labelVotesVALOR1) ? "0" : labelVotesVALOR1}
                </CBadge>
              </h6>
            </div>
            <div style={{ margin: "0 10px" }}>
              {" "}
              {/* Añade margen a los div para separarlos */}
              {useAlert && (
                <FooterAlert
                  isValid={isValidForm2}
                  isSuccess={isSuccess2}
                  errorMsg="El formulario no es válido."
                  successMsg="Datos enviados correctamente."
                />
              )}
              <h6>
                {labelText2}&nbsp;
                <CBadge
                  color={labelStyle2}
                  className={buttonClass2}
                  style={{ fontSize: "inherit" }}
                >
                  {labelVotesVALOR2}
                </CBadge>
              </h6>
            </div>
          </>
        )}

        {stepSummaryIsVisible && (
          <>
            <div style={{ margin: "0 10px" }}>
              {" "}
              {/* Añade margen a los div para separarlos */}
              {useAlert && (
                <FooterAlert
                  isValid={isValidForm2}
                  isSuccess={isSuccess2}
                  errorMsg="El formulario no es válido."
                  successMsg="Datos enviados correctamente."
                />
              )}
              <h6>
                {labelText3}&nbsp;
                <CBadge
                  color={labelStyle2}
                  className={buttonClass2}
                  style={{ fontSize: "inherit" }}
                >
                  {isNaN(labelVotesVALOR2) ? "0" : labelVotesVALOR2}
                </CBadge>
              </h6>
            </div>
          </>
        )}
      </CCardFooter>
    </>
  );
};

export default FooterEspecial;
