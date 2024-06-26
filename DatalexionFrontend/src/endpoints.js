const apiURL = process.env.REACT_APP_API_URL;

export const apiVersionName = process.env.REACT_APP_API_VERSION_NAME;
export const apiVersionNumber = process.env.REACT_APP_API_VERSION_NUMBER;
export const urlAccount = `${apiURL}/accounts`;
export const urlLoginGeneral = `${apiURL}/accounts/LoginGeneral`;
export const urlLoginDelegados = `${apiURL}/accounts/LoginDelegados`;
export const urlCircuit = `${apiURL}/circuits`; // POST (CircuitTable.js), PUT () y PATCH (FormExtras.js)
export const urlCircuitUpdate = `${apiURL}/circuits/CircuitUpdate`; // PUT (CircuitTable.js) Circuits
export const urlCircuitUpdateStep1 = `${apiURL}/circuits/UpdateStep1`; // PUT (FormSlate.js) Circuits
export const urlCircuitUpdateStep2 = `${apiURL}/circuits/UpdateStep2`; // PUT (FormParty.js) Circuits
export const urlCircuitUpdateStep3 = `${apiURL}/circuits/UpdateStep3`; // PUT (FormExtras.js) Circuits
export const urlCircuitPut = `${apiURL}/circuits/CircuitPut`; // PUT (CircuitTable.js) Circuits
export const urlClient = `${apiURL}/clients`;
export const urlDelegado = `${apiURL}/delegados`;
export const urlCandidate = `${apiURL}/candidates`;
export const urlParty = `${apiURL}/parties`;
export const urlMyParty = `${apiURL}/parties`;
export const urlProvince = `${apiURL}/provinces`;
export const urlMunicipality = `${apiURL}/municipalities`;
export const urlWing = `${apiURL}/wings`;
export const urlSlate = `${apiURL}/slates`;
export const urlUserRole = `${apiURL}/accounts`;
export const urlAccountRegister = `${apiURL}/accounts/CreateUser`; // POST Accounts
export const urlAccountUpdateUser = `${apiURL}/accounts/UpdateUser`; // PUT Accounts
export const urlUserRoleCreate = `${apiURL}/accounts/CreateUserRole`; // POST Accounts
export const urlUserRoleUpdate = `${apiURL}/accounts/UpdateUserRole`; // PUT Accounts
export const urlIsCIAlreadyRegistered = `${apiURL}/delegados/IsCIAlreadyRegistered`;
export const urlIsUsernameAlreadyRegistered = `${apiURL}/accounts/IsUsernameAlreadyRegistered`;
