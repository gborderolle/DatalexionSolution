import React, { useState, useEffect } from "react";
import { TiTick } from "react-icons/ti";

import "./RadioButtonStepper.css";

const RadioButtonStepper = ({ currentStep }) => {
  // Define los nombres de los pasos a mostrar
  const steps = ["Listas", "Partidos", "Extras"];

  // Estado que almacena la información de los pasos completados
  const [stepsSubmitted, setStepsSubmitted] = useState({
    step1: currentStep.step1completed,
    step2: currentStep.step2completed,
    step3: currentStep.step3completed,
  });

  // Estado que indica si todos los pasos han sido completados
  const [complete, setComplete] = useState(false);

  // Efecto para actualizar stepsSubmitted cuando cambia props.currentStep
  useEffect(() => {
    setStepsSubmitted({
      step1: currentStep.step1completed,
      step2: currentStep.step2completed,
      step3: currentStep.step3completed,
    });
  }, [currentStep]);

  // Efecto para verificar si todos los pasos están completos
  useEffect(() => {
    const allStepsCompleted = Object.values(stepsSubmitted).every(Boolean);
    setComplete(allStepsCompleted);
  }, [stepsSubmitted]);

  // Renderiza los pasos con indicadores visuales de su estado
  return (
    <>
      <div className="flex justify-between" style={{ display: "flex" }}>
        {steps.map((step, i) => (
          <div
            key={i}
            className={`step-item ${
              stepsSubmitted[`step${i + 1}`] && "complete"
            }`}
          >
            <div className="step">
              {stepsSubmitted[`step${i + 1}`] ? <TiTick size={24} /> : i + 1}
            </div>
            <p className="text-gray-500">{step}</p>
          </div>
        ))}
      </div>
      {complete && (
        <div style={{ textAlign: "center" }}>¡Proceso completo!</div>
      )}
    </>
  );
};

export default RadioButtonStepper;
