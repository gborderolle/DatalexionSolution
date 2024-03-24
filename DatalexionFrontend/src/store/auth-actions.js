import axios from "axios";
import { useSelector, useDispatch } from "react-redux";

import { authActions } from "./auth-slice";
import { urlAccount } from "../endpoints";
import showToastMessage from "../components/messages/ShowSuccess";

// Redux imports
import { fetchClientByUser } from "../store/generalData-actions";

export const loginDelegadosHandler = (ci, navigate) => async (dispatch) => {
  try {
    const response = await axios.post(
      `${urlAccount}/loginDelegados`,
      { ci },
      { headers: { "x-version": "1" } }
    );

    // Asegúrate de que la respuesta contenga el resultado esperado
    if (response.data && response.data.result && response.data.result.token) {
      const {
        token,
        userRoles,
        fullname,
        userId,
        listMunicipalities,
        listCircuitDelegados,
        clientId,
      } = response.data.result;

      // Manejo exitoso del login
      await showToastMessage({
        title: "Login correcto",
        icon: "success",
        callback: () => {
          setTimeout(() => {
            dispatch(
              authActions.login({
                username: ci,
                isMobile: isMobileDevice(),
                authToken: token.token,
                userRole: userRoles,
                fullname: fullname,
                userId: userId,
                listMunicipalities: listMunicipalities,
                listCircuitDelegados: listCircuitDelegados,
                clientId: clientId,
              })
            );
            dispatch(fetchClientByUser(ci));
            navigate("/formStart");
          }, 500);
        },
      });
    } else {
      // Manejar el caso donde no se recibe un token válido
      // setErrorMessage("Login incorrecto. No se recibió un token válido.");
      await showToastMessage({
        title: "Login incorrecto",
        icon: "error",
      });
    }
  } catch (error) {
    console.error("Error al autenticar:", error);
    showToastMessage({
      title: "Error al autenticar",
      icon: "error",
    });
  }
};

export const loginAdminHandler =
  (username, password, navigate, setErrorMessage) => async (dispatch) => {
    try {
      const response = await axios.post(
        `${urlAccount}/loginGeneral`,
        { username, password },
        { headers: { "x-version": "1" } }
      );

      // Asegúrate de que la respuesta contenga el resultado esperado
      if (response.data && response.data.result && response.data.result.token) {
        const { token, userRoles, clientId, fullname } = response.data.result;

        // Manejo exitoso del login
        await showToastMessage({
          title: "Login correcto",
          icon: "success",
          callback: () => {
            setTimeout(() => {
              dispatch(
                authActions.login({
                  username,
                  isMobile: isMobileDevice(),
                  authToken: token.token,
                  userRole: userRoles[0],
                  clientId: clientId,
                  fullname: fullname,
                })
              );
              dispatch(fetchClientByUser(username));
              navigate("/dashboard");
            }, 500);
          },
        });
      } else {
        // Manejar el caso donde no se recibe un token válido
        setErrorMessage("Login incorrecto. No se recibió un token válido.");
        await showToastMessage({
          title: "Login incorrecto",
          icon: "error",
        });
      }
    } catch (error) {
      console.error("Error al autenticar:", error);
      showToastMessage({
        title: "Error al autenticar",
        icon: "error",
      });
      setErrorMessage(
        "Error al autenticar. Por favor, revisa tu conexión a Internet."
      );
    }
  };

const isMobileDevice = () => {
  return (
    typeof window.orientation !== "undefined" ||
    navigator.userAgent.indexOf("IEMobile") !== -1
  );
};
