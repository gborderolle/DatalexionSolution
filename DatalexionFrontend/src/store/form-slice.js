import { createSlice } from "@reduxjs/toolkit";

const formSlice = createSlice({
  name: "form",
  initialState: {
    circuitDDLIsVisible: false,
    provinceDDLIsVisible: false,
    reduxVotosTotalSteps: 0,
    reduxVotosStep1: 0,
    reduxVotosStep2: 0,
    reduxVotosStep3: 0,
  },
  reducers: {
    setReduxVotosTotalSteps: (state, action) => {
      state.reduxVotosTotalSteps = action.payload;
    },
    setReduxVotosStep1: (state, action) => {
      state.reduxVotosStep1 = action.payload;
    },
    setReduxVotosStep2: (state, action) => {
      state.reduxVotosStep2 = action.payload;
    },
    setReduxVotosStep3: (state, action) => {
      state.reduxVotosStep3 = action.payload;
    },
    emptyAllVotos: (state) => {
      state.reduxVotosStep1 = 0;
      state.reduxVotosStep2 = 0;
      state.reduxVotosStep3 = 0;
    },
    setAllVotos: (state, action) => {
      // state.votosSlateTotalRedux = action.payload.circuit;
      // state.votosPartyTotalRedux = action.payload;
      // state.votosExtrasTotalRedux = action.payload;
    },
  },
});

// incluyo acá mismo el Actions (porque son métodos simples)
export const formActions = formSlice.actions;

export default formSlice;
