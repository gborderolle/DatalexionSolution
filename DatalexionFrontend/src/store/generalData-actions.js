import { generalDataActions } from "./generalData-slice";
import { formActions } from "./form-slice";

import { fetchApi } from "../hooks/use-API";

import {
  urlClient,
  urlParty,
  urlWing,
  urlSlate,
  urlProvince,
  urlMunicipality,
  urlCircuit,
  urlCandidate,
  urlDelegado,
  urlAccount,
} from "../endpoints";

import { selectUsername, selectClientId } from "./auth-slice";
import { selectClient } from "./generalData-slice";

export const fetchPartyList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlParty + "/GetParty");
      if (data && data.result) {
        dispatch(generalDataActions.setPartyList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener la lista de partidos:", error);
    }
  };
};

export const fetchPartyListByClient = () => {
  return async (dispatch, getState) => {
    try {
      const clientId = selectClientId(getState());
      if (!clientId) {
        console.error("ClientId no disponible.");
        return;
      }

      const urlWithParam = `${urlParty}/GetPartiesByClient?clientId=${clientId}`;
      const data = await fetchApi(urlWithParam);
      if (data && data.result) {
        dispatch(generalDataActions.setPartyListByClient(data.result));
      } else {
        console.error(
          "Error al obtener los usuarios: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener los usuarios:", error);
    }
  };
};

export const fetchWingList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlWing + "/GetWing");
      if (data && data.result) {
        dispatch(generalDataActions.setWingList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener la lista de agrupaciones:", error);
    }
  };
};

export const fetchWingListByClient = () => {
  return async (dispatch, getState) => {
    try {
      const clientId = selectClientId(getState());
      if (!clientId) {
        console.error("ClientId no disponible.");
        return;
      }

      const urlWithParam = `${urlWing}/GetWingsByClient?clientId=${clientId}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setWingListByClient(data.result));
      } else {
        console.error(
          "Error al obtener los usuarios: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener los usuarios:", error);
    }
  };
};

export const fetchSlateList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlSlate + "/GetSlate");
      if (data && data.result) {
        dispatch(generalDataActions.setSlateList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener la lista de listas:", error);
    }
  };
};

export const fetchSlateListByClient = () => {
  return async (dispatch, getState) => {
    try {
      const clientId = selectClientId(getState());
      if (!clientId) {
        console.error("ClientId no disponible.");
        return;
      }

      const urlWithParam = `${urlSlate}/GetSlatesByClient?clientId=${clientId}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setSlateListByClient(data.result));
      } else {
        console.error(
          "Error al obtener los usuarios: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener los usuarios:", error);
    }
  };
};

export const fetchProvinceList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlProvince + "/GetProvince");
      if (data && data.result) {
        dispatch(generalDataActions.setProvinceList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener la lista de provincias:", error);
    }
  };
};

export const fetchMunicipalityList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlMunicipality + "/GetMunicipality");
      if (data && data.result) {
        dispatch(generalDataActions.setMunicipalityList(data.result));
      }
    } catch (error) {
      console.error("Error al obtener la lista de municipios:", error);
    }
  };
};

// Todo: no está terminado el método
export const fetchMunicipalityListByDelegado = () => {
  return async (dispatch, getState) => {
    try {
      const clientId = selectClientId(getState());
      if (!clientId) {
        console.error("ClientId no disponible.");
        return;
      }

      const urlWithParam = `${urlCircuit}/GetMunicipalityByDelegado?delegadoId=${clientId}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setMunicipalityListByDelegado(data.result));
      } else {
        console.error(
          "Error al obtener los municipios: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener la lista de municipios:", error);
    }
  };
};

export const fetchCircuitList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlCircuit + "/GetCircuit");
      if (data && data.result) {
        dispatch(generalDataActions.setCircuitList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener la lista de circuitos:", error);
    }
  };
};

// Filtra los circuitos que incluye únicamente las listas de nuestro cliente
export const fetchCircuitListByClient = () => {
  return async (dispatch, getState) => {
    try {
      const clientId = selectClientId(getState());
      if (!clientId) {
        console.error("ClientId no disponible.");
        return;
      }

      const urlWithParam = `${urlCircuit}/GetCircuitsByClient?clientId=${clientId}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setCircuitListByClient(data.result));
      } else {
        console.error(
          "Error al obtener los circuitos: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener la lista de circuitos:", error);
    }
  };
};

export const fetchCandidateList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlCandidate + "/GetCandidate");
      if (data && data.result) {
        dispatch(generalDataActions.setCandidateList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener la lista de candidatos:", error);
    }
  };
};

export const fetchCandidateListByClient = () => {
  return async (dispatch, getState) => {
    try {
      // Intenta usar el parámetro username si se proporciona, de lo contrario, obtiene el username del estado Redux
      let clientId = selectClientId(getState());
      if (!clientId) {
        console.error("Nombre de usuario no proporcionado o inválido.");
        return; // Termina la ejecución si no hay un username válido
      }

      // Asegura que clientId sea un entero
      clientId = parseInt(clientId, 10); // El segundo argumento (10) especifica la base decimal
      if (isNaN(clientId)) {
        console.error("clientId debe ser un número.");
        return; // Termina la ejecución si clientId no es un número
      }

      const urlWithParam = `${urlCandidate}/GetCandidatesByClient?clientId=${clientId}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setCandidateList(data.result));
      } else {
        console.error(
          "Error al obtener los usuarios: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener los usuarios:", error);
    }
  };
};

export const fetchDelegadoList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlDelegado + "/GetDelegado");
      if (data && data.result) {
        dispatch(generalDataActions.setDelegadoList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener la lista de delegados:", error);
    }
  };
};

export const fetchDelegadoListByClient = () => {
  return async (dispatch, getState) => {
    try {
      const clientId = selectClientId(getState());
      if (!clientId) {
        console.error("ClientId no disponible.");
        return;
      }

      const urlWithParam = `${urlDelegado}/GetDelegadosByClient?clientId=${clientId}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setDelegadoList(data.result));
      } else {
        console.error(
          "Error al obtener los delegados: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener los delegados:", error);
    }
  };
};

export const fetchVotosTotal = (circuit) => {
  return async (dispatch, getState) => {
    try {
      const client = selectClient(getState());
      if (!client) {
        console.error("Client no disponible.");
        return;
      }

      const urlWithParam = `${urlCircuit}/GetCircuitByClient/${circuit.id}?clientId=${client.id}`;
      const data = await fetchApi(urlWithParam);

      // const data = await fetchApi(urlCircuit + "/" + circuit.id);
      if (data && data.result) {
        const circuitResult = data.result;
        let nSlates = 0;
        let nParties = 0;
        let nSlatesPartiesExtras = 0;

        // Sumar los votos de cada slate dentro del circuito
        circuitResult.listCircuitSlates.forEach((circuitSlate) => {
          nSlates += circuitSlate.totalSlateVotes || 0; // Asegurarse de que Votes sea un número
        });

        circuitResult.listCircuitParties.forEach((circuitParty) => {
          nParties += circuitParty.totalPartyVotes || 0; // Asegurarse de que Votes sea un número
        });

        // Encontrar el circuitParty específico basado en partyId
        const myCircuitParty = circuitResult.listCircuitParties.find(
          (party) => party.partyId === client.party.id
        );

        if (!myCircuitParty) {
          console.error("No se encontró el circuitParty especificado.");
          return; // Terminar si no se encuentra el circuitParty
        }

        // Sumar votos extras
        const nExtras =
          myCircuitParty.nullVotes +
          myCircuitParty.blankVotes +
          myCircuitParty.recurredVotes +
          myCircuitParty.observedVotes;

        nSlatesPartiesExtras += nParties + nExtras; // Sumar los votos extras al total

        // Actualizar el estado global con el total de votos
        dispatch(formActions.setReduxVotosStep1(nSlates));
        dispatch(formActions.setReduxVotosStep2(nParties));
        dispatch(formActions.setReduxVotosStep3(nExtras));
        dispatch(formActions.setReduxVotosTotalSteps(nSlatesPartiesExtras));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener los votos totales del circuito:", error);
    }
  };
};

export const fetchClient = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlClient + "/GetClient");
      if (data && data.result) {
        dispatch(generalDataActions.setClient(data.result[0]));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Error al obtener el cliente:", error);
    }
  };
};

export const fetchClientByUser = (usernameParam) => {
  return async (dispatch, getState) => {
    try {
      // Intenta usar el parámetro username si se proporciona, de lo contrario, obtiene el username del estado Redux
      const username = usernameParam || selectUsername(getState());
      if (!username) {
        console.error("Nombre de usuario no proporcionado o inválido.");
        return; // Termina la ejecución si no hay un username válido
      }

      const urlWithParam = `${urlClient}/GetUserClient?username=${encodeURIComponent(
        username
      )}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setClient(data.result));
      } else {
        console.error("Error al obtener el cliente: No se encontraron datos.");
      }
    } catch (error) {
      console.error("Error al obtener el cliente:", error);
      // Aquí podrías manejar el error, por ejemplo, actualizando el estado de Redux con el error recibido
    }
  };
};

export const fetchUserList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlAccount + "/GetUsers");
      if (data && data.result) {
        dispatch(generalDataActions.setUserList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Fetch error:", error);
    }
  };
};

export const fetchUserListByClient = () => {
  return async (dispatch, getState) => {
    try {
      const clientId = selectClientId(getState());
      if (!clientId) {
        console.error("ClientId no disponible.");
        return;
      }

      const urlWithParam = `${urlAccount}/GetUsersByClient?clientId=${encodeURIComponent(
        clientId
      )}`;
      const data = await fetchApi(urlWithParam);

      // Procesa los datos recibidos
      if (data && data.result) {
        dispatch(generalDataActions.setUserList(data.result));
      } else {
        console.error(
          "Error al obtener los usuarios: No se encontraron datos."
        );
      }
    } catch (error) {
      console.error("Error al obtener los usuarios:", error);
    }
  };
};

export const fetchUserRoleList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlAccount + "/GetRoles");
      if (data && data.result) {
        dispatch(generalDataActions.setUserRoleList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Fetch error:", error);
    }
  };
};

export const fetchUserRole = (id) => {
  return async () => {
    try {
      const data = await fetchApi(`${urlAccount}/GetUserRole/${id}`);
      if (data && data.result) {
        return data.result;
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Fetch error:", error);
    }
  };
};

export const fetchLogsList = () => {
  return async (dispatch) => {
    try {
      const data = await fetchApi(urlAccount + "/GetLogs");
      if (data && data.result) {
        dispatch(generalDataActions.setLogsList(data.result));
      } else {
        console.error("Fetch error:", data.errorMessages);
      }
    } catch (error) {
      console.error("Fetch error:", error);
    }
  };
};
