const apiURL = process.env.REACT_APP_API_URL;

export const apiVersionName = process.env.REACT_APP_API_VERSION_NAME;
export const apiVersionNumber = process.env.REACT_APP_API_VERSION_NUMBER;
export const urlAccount = `${apiURL}/accounts`;
export const urlCircuit = `${apiURL}/circuits`;
export const urlCircuitUpdate = `${apiURL}/circuits/UpdateCircuit`;
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
export const urlAccountRegister = `${apiURL}/accounts/CreateUser`;
export const urlAccountUpdateUser = `${apiURL}/accounts/UpdateUser`;
export const urlUserRoleCreate = `${apiURL}/accounts/CreateUserRole`;
export const urlUserRoleUpdate = `${apiURL}/accounts/UpdateUserRole`;
export const urlAccountBiometricChallenge = `${apiURL}/accounts/auth/challenge`;
export const urlAccountBiometricValidate = `${apiURL}/accounts/auth/validateBiometricAuth`;
export const urlIsCIAlreadyRegistered = `${apiURL}/delegados/IsCIAlreadyRegistered`;
export const urlIsUsernameAlreadyRegistered = `${apiURL}/accounts/IsUsernameAlreadyRegistered`;

// Función para obtener ID autoincremental
export const getAutoIncrementedId = async (firebaseUrl) => {
  try {
  } catch (error) {
    console.error(`Error al obtener el ID autoincremental: ${error}`);
    return null; // O lanzar un error personalizado
  }
};
