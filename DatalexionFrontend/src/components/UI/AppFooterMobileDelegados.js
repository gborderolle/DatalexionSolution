import React from "react";

import { CFooter } from "@coreui/react";

import FooterEspecial from "../delegadosSide/footer/FooterEspecial";
import BtnSeleccionarCircuito from "../delegadosSide/selectDDL/SelectCircuitButton";
import FooterStepper from "../stepper/FooterStepper";

// redux imports
import { useSelector } from "react-redux";

import classes from "./AppFooterMobileDelegados.module.css";

const AppFooterMobileDelegados = () => {
  //#region Consts ***********************************

  // redux gets
  const reduxVotosTotalSteps = useSelector(
    (state) => state.form.reduxVotosTotalSteps
  );
  const reduxVotosStep1 = useSelector((state) => state.form.reduxVotosStep1);
  const reduxVotosStep2 = useSelector((state) => state.form.reduxVotosStep2);
  const reduxVotosStep3 = useSelector((state) => state.form.reduxVotosStep3);
  const stepperIsVisible = useSelector((state) => state.ui.stepperIsVisible);

  const reduxClient = useSelector((state) => state.generalData.client);

  // redux gets
  const reduxSelectedCircuit = useSelector(
    (state) => state.liveSettings.circuit
  );

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  //#endregion Hooks ***********************************

  let clientPartyVotes = 0;
  if (reduxSelectedCircuit) {
    clientPartyVotes = reduxSelectedCircuit?.listCircuitParties.find(
      (circuitParty) => circuitParty.partyId === reduxClient?.party?.id
    )?.votes;
  }

  const slateCount =
    !reduxVotosStep1 || isNaN(reduxVotosStep1) ? 0 : reduxVotosStep1;
  const partyCount =
    !reduxVotosStep2 || isNaN(reduxVotosStep2) ? 0 : reduxVotosStep2;
  const extrasCount =
    !reduxVotosStep3 || isNaN(reduxVotosStep3) ? 0 : reduxVotosStep3;
  const votosTotales = slateCount + partyCount + extrasCount - clientPartyVotes;
  return (
    <CFooter className={`${classes.footer} ${classes.fixedFooter}`}>
      {(!reduxSelectedCircuit || !stepperIsVisible) && (
        <div style={{ textAlign: "center" }}>
          <BtnSeleccionarCircuito />
        </div>
      )}

      {stepperIsVisible && (
        <>
          <FooterEspecial
            labelText1="Votos parciales:"
            labelVotesVALOR1={reduxVotosTotalSteps}
            labelStyle1={"success"}
            labelText2="Acumulado:"
            labelVotesVALOR2={votosTotales}
            labelStyle2={"info"}
            labelText3="Cantidad de votos totales:"
          />
          <div className="bg-gray-900 flex flex-row gap-10 items-center justify-center">
            <div className="bg-gray-900 flex flex-row gap-10 items-center justify-center">
              <FooterStepper />
            </div>
          </div>
        </>
      )}
    </CFooter>
  );
};

export default React.memo(AppFooterMobileDelegados);
