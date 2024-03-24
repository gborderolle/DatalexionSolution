//#region Gets Slate

export function SlateGetWing(slate, wingList) {
  if (slate.wing) return slate.wing;

  return slate.wingId
    ? wingList.find((wing) => wing.id === slate.wingId)
    : undefined;
}

export function SlateGetParty(slate, wingList, partyList) {
  const wing =
    slate.wing || (slate.wingId && wingList.find((r) => r.id === slate.wingId));

  if (wing && wing.party) return wing.party;

  return wing && wing.partyId
    ? partyList.find((p) => p.id === wing.partyId)
    : undefined;
}

export function SlateGetCandidate(slate, candidateList) {
  if (slate.candidate && slate.candidate.photoURL) return slate.candidate;

  return slate.candidateId
    ? candidateList.find((candidate) => candidate.id === slate.candidateId)
    : undefined;
}

export function SlateGetProvince(slate, provinceList) {
  if (slate.province) return slate.province;

  return slate.provinceId
    ? provinceList.find((province) => province.id === slate.provinceId)
    : undefined;
}

//#endregion Gets Slate

//#region Gets Wing

export function WingGetParty(wing, partyList) {
  if (wing.party) return wing.party;

  return wing.partyId
    ? partyList.find((party) => party.id === wing.partyId)
    : undefined;
}

//#endregion Gets Wing

//#region Gets Municipality

export function MunicipalityGetProvince(municipality, provinceList) {
  if (municipality.province) return municipality.province;

  return municipality.provinceId
    ? provinceList.find((province) => province.id === municipality.provinceId)
    : undefined;
}

//#endregion Gets Municipality

//#region Gets Circuit

export function CircuitGetMunicipality(circuit, municipalityList) {
  if (circuit.municipality) return circuit.municipality;

  return circuit.municipalityId
    ? municipalityList.find(
        (municipality) => municipality.id === circuit.municipalityId
      )
    : undefined;
}

//#endregion Gets Circuit

//#region Gets Client

export function ClientGetParty(client, partyList) {
  if (client.party) return client.party;

  return client.partyId
    ? partyList.find((party) => party.id === client.partyId)
    : undefined;
}

//#endregion Gets Client

//#region Gets Delegado

export function DelegadoGetMunicipality(delegado, municipalityList) {
  if (delegado.municipality) return delegado.municipality;

  return delegado.municipalityId
    ? municipalityList.find(
        (municipality) => municipality.id === delegado.municipalityId
      )
    : undefined;
}

//#endregion Gets Delegado

//#region Gets User

export function UserGetUserRole(user, userRoleList) {
  if (user.userRole) return user.userRole;

  return user.userRoleId
    ? userRoleList.find((userRole) => userRole.id === user.userRoleId)
    : undefined;
}

//#endregion Gets User

// Utilidad para el cálculo del porcentaje.
export const calculatePercentage = (partialValue, totalValue) => {
  if (totalValue === 0) {
    return 0; // O cualquier valor que consideres apropiado para divisiones por cero
  }
  return Math.round((partialValue / totalValue) * 100);
};

export function getRandomColor() {
  const letters = "0123456789ABCDEF";
  let color = "#";
  for (let i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
}

export function hexToRGBA(hex, alpha = 1) {
  let r = parseInt(hex.slice(1, 3), 16);
  let g = parseInt(hex.slice(3, 5), 16);
  let b = parseInt(hex.slice(5, 7), 16);

  return `rgba(${r}, ${g}, ${b}, ${alpha})`;
}


export const getDynamicClassName = (color) => {
  // Verifica si el color existe y es un string antes de llamar a replace.
  if (typeof color === "string") {
    const className = `progress-bar-${color.replace("#", "")}`;
    addStyle(`.${className} { background-color: ${color}; }`);
    return className;
  } else {
    console.log("Color no proporcionado o no es un string", color);
    // Devuelve una clase predeterminada o maneja el error de alguna manera.
    return "progress-bar-default";
  }
};

export const setDynamicBorderStyle = (color, index, element) => {
  if (typeof color === "string") {
    const colorCode = color.replace("#", "");
    addStyle(
      `#${element}-${index} { border-color: #${colorCode} !important; }`
    );
  } else {
    console.log(
      `Color no proporcionado o no es un string para el elemento ${element}-${index}`,
      color
    );
    // Maneja el caso en que color no es un string, por ejemplo, asignando un color predeterminado o ignorando el estilo.
  }
};

export const addStyle = (styleString) => {
  const style = document.createElement("style");
  style.textContent = styleString;
  document.head.append(style);
};
