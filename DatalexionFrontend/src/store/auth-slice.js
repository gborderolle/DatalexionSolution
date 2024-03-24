import { createSlice } from "@reduxjs/toolkit";

// Función para manejar la deserialización segura desde localStorage
const getSafeJSONParse = (key, fallbackValue) => {
  const item = localStorage.getItem(key);
  try {
    // Verifica explícitamente si el item es null o "literalmente" undefined antes de intentar parsearlo
    if (item === null || typeof item === "undefined" || item === "undefined") {
      return fallbackValue;
    }
    return JSON.parse(item);
  } catch (error) {
    console.error(`Error parsing ${key} from localStorage:`, error);
    return fallbackValue;
  }
};

const getInitialState = () => {
  const loggedInData = JSON.parse(localStorage.getItem("loggedInData"));
  let isLoggedIn = false;

  if (loggedInData && new Date().getTime() < loggedInData.expiry) {
    isLoggedIn = loggedInData.value;
  } else {
    localStorage.removeItem("loggedInData");
    localStorage.removeItem("isLoggedIn");
    localStorage.removeItem("userProvinceId");
    localStorage.removeItem("userMunicipalityId");
    localStorage.removeItem("listMunicipalities");
    localStorage.removeItem("listCircuitDelegados");
    localStorage.removeItem("userRole");
    localStorage.removeItem("username");
    localStorage.removeItem("fullname");
    localStorage.removeItem("userId");
    localStorage.removeItem("clientId");
    localStorage.removeItem("isMobile");
    localStorage.removeItem("authToken");
  }

  return {
    loggedInData: loggedInData,
    isLoggedIn: localStorage.getItem("isLoggedIn") || "",
    userProvinceId: localStorage.getItem("userProvinceId") || "",
    userMunicipalityId: localStorage.getItem("userMunicipalityId") || "",
    listMunicipalities: getSafeJSONParse("listMunicipalities", []),
    listCircuitDelegados: getSafeJSONParse("listCircuitDelegados", []),
    userRole: localStorage.getItem("userRole") || "",
    username: localStorage.getItem("username") || "",
    fullname: localStorage.getItem("fullname") || "",
    userId: localStorage.getItem("userId") || "",
    clientId: localStorage.getItem("clientId") || "",
    isMobile: Boolean(localStorage.getItem("isMobile")) || false,
    authToken: localStorage.getItem("authToken") || "",
  };
};

const authSlice = createSlice({
  name: "auth",
  initialState: getInitialState(),
  reducers: {
    login(state, action) {
      state.isLoggedIn = true;
      state.userProvinceId = action.payload.userProvinceId;
      state.userMunicipalityId = action.payload.userMunicipalityId;
      state.listMunicipalities = action.payload.listMunicipalities;
      state.listCircuitDelegados = action.payload.listCircuitDelegados;
      state.userRole = action.payload.userRole;
      state.username = action.payload.username;
      state.fullname = action.payload.fullname;
      state.userId = action.payload.userId;
      state.clientId = action.payload.clientId;
      state.isMobile = action.payload.isMobile;
      state.authToken = action.payload.authToken; // Agrega esta línea

      // const expiry = new Date().getTime() + 5 * 1000; // 1 hora
      const expiry = new Date().getTime() + 1 * 60 * 60 * 1000; // 1 hora
      const data = {
        value: true,
        expiry,
      };
      state.loggedInData = data;

      localStorage.setItem("isLoggedIn", true);
      localStorage.setItem("loggedInData", JSON.stringify(data));
      localStorage.setItem("userProvinceId", state.userProvinceId);
      localStorage.setItem("userMunicipalityId", state.userMunicipalityId);
      localStorage.setItem(
        "listMunicipalities",
        JSON.stringify(state.listMunicipalities)
      );
      localStorage.setItem(
        "listCircuitDelegados",
        JSON.stringify(state.listCircuitDelegados)
      );
      localStorage.setItem("userRole", state.userRole);
      localStorage.setItem("username", state.username);
      localStorage.setItem("fullname", state.fullname);
      localStorage.setItem("userId", state.userId);
      localStorage.setItem("clientId", state.clientId);
      localStorage.setItem("isMobile", state.isMobile);
      localStorage.setItem("authToken", state.authToken); // Almacenar el token en localStorage
    },

    logout(state) {
      state.isLoggedIn = false;
      state.userProvinceId = "";
      state.userMunicipalityId = "";
      state.listMunicipalities = "";
      state.listCircuitDelegados = "";
      state.userRole = "";
      state.username = "";
      state.fullname = "";
      state.userId = "";
      state.clientId = "";
      state.authToken = ""; // Limpia el token

      const data = {
        value: false,
        expiry: new Date().getTime(),
      };
      localStorage.setItem("loggedInData", JSON.stringify(data));

      localStorage.removeItem("userProvinceId");
      localStorage.removeItem("userMunicipalityId");
      localStorage.removeItem("listMunicipalities");
      localStorage.removeItem("listCircuitDelegados");
      localStorage.removeItem("userRole");
      localStorage.removeItem("username");
      localStorage.removeItem("fullname");
      localStorage.removeItem("userId");
      localStorage.removeItem("clientId");
      localStorage.removeItem("isMobile");
      localStorage.removeItem("authToken");
    },

    setIsMobile(state, action) {
      state.isMobile = action.payload;
      localStorage.setItem("isMobile", action.payload);
    },

    initializeAuthState(state) {
      const loggedInData = getSafeJSONParse("loggedInData", null);

      if (loggedInData && new Date().getTime() < loggedInData.expiry) {
        state.loggedInData = loggedInData.value;
      } else {
        localStorage.removeItem("loggedInData");
        state.loggedInData = null;
      }

      state.isLoggedIn = localStorage.getItem("isLoggedIn") || "";
      state.userProvinceId = localStorage.getItem("userProvinceId") || "";
      state.userMunicipalityId =
        localStorage.getItem("userMunicipalityId") || "";
      state.listMunicipalities =
        JSON.parse(localStorage.getItem("listMunicipalities")) || [];
      state.listCircuitDelegados =
        JSON.parse(localStorage.getItem("listCircuitDelegados")) || [];
      state.userRole = localStorage.getItem("userRole") || "";
      state.username = localStorage.getItem("username") || "";
      state.fullname = localStorage.getItem("fullname") || "";
      state.userId = localStorage.getItem("userId") || "";
      state.clientId = localStorage.getItem("clientId") || "";
      state.isMobile =
        localStorage.getItem("isMobile") == "true" ? true : false;
      state.authToken = localStorage.getItem("authToken") || ""; // Recupera el token del localStorage
    },

    resetAuthState(state) {
      localStorage.removeItem("loggedInData");
      localStorage.removeItem("isLoggedIn");
      localStorage.removeItem("userProvinceId");
      localStorage.removeItem("userMunicipalityId");
      localStorage.removeItem("listMunicipalities");
      localStorage.removeItem("listCircuitDelegados");
      localStorage.removeItem("userRole");
      localStorage.removeItem("username");
      localStorage.removeItem("fullname");
      localStorage.removeItem("userId");
      localStorage.removeItem("clientId");
      localStorage.removeItem("isMobile");
      localStorage.removeItem("authToken");

      state.loggedInData = null;
      state.isLoggedIn = false;
      state.userProvinceId = null;
      state.userMunicipalityId = null;
      state.listMunicipalities = null;
      state.listCircuitDelegados = null;
      state.userRole = null;
      state.username = null;
      state.fullname = null;
      state.userId = null;
      state.clientId = null;
      state.isMobile = null;
    },
  },
});

export const authActions = authSlice.actions;

export const selectUsername = (state) => state.auth.username;
export const selectClientId = (state) => state.auth.clientId;

export default authSlice;
