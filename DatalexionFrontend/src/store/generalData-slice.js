import { createSlice } from "@reduxjs/toolkit";

const generalDataSlice = createSlice({
  name: "generalData",
  initialState: {
    client: [],
    partyList: [],
    partyListByClient: [],
    wingList: [],
    wingListByClient: [],
    slateList: [],
    slateListByClient: [],
    provinceList: [],
    municipalityList: [],
    circuitList: [],
    circuitListByClient: [],
    candidateList: [],
    userList: [],
    userRoleList: [],
    roleList: [],
    delegadoList: [],
    delegadoListByClient: [],
    logsList: [],
  },
  reducers: {
    setClient: (state, action) => {
      state.client = action.payload;
    },
    setPartyList: (state, action) => {
      state.partyList = action.payload;
    },
    setPartyListByClient: (state, action) => {
      state.partyListByClient = action.payload;
    },
    setWingList: (state, action) => {
      state.wingList = action.payload;
    },
    setWingListByClient: (state, action) => {
      state.wingListByClient = action.payload;
    },
    setSlateList: (state, action) => {
      state.slateList = action.payload;
    },
    setSlateListByClient: (state, action) => {
      state.slateListByClient = action.payload;
    },
    setProvinceList: (state, action) => {
      state.provinceList = action.payload;
    },
    setMunicipalityList: (state, action) => {
      state.municipalityList = action.payload;
    },
    setCircuitList: (state, action) => {
      state.circuitList = action.payload;
    },
    setCircuitListByClient: (state, action) => {
      state.circuitListByClient = action.payload;
    },
    setCandidateList: (state, action) => {
      state.candidateList = action.payload;
    },
    setRoleList: (state, action) => {
      state.roleList = action.payload;
    },
    setDelegadoList: (state, action) => {
      state.delegadoList = action.payload;
    },
    setUserList: (state, action) => {
      state.userList = action.payload;
    },
    setUserRoleList: (state, action) => {
      state.userRoleList = action.payload;
    },
    setLogsList: (state, action) => {
      state.logsList = action.payload;
    },
  },
});

export const generalDataActions = generalDataSlice.actions;

export default generalDataSlice;
