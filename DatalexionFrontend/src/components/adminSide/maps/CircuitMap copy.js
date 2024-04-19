import React, { useEffect, useMemo } from "react";
import { MapContainer, TileLayer, useMap, Popup, Marker } from "react-leaflet";
import MarkerClusterGroup from "react-leaflet-cluster";
import iconBlue from "leaflet/dist/images/marker-icon.png";
import iconGreen from "leaflet/dist/images/marker-icon-completed.png";
import iconShadow from "leaflet/dist/images/marker-shadow.png";
import "leaflet/dist/leaflet.css";
import L, { divIcon } from "leaflet";

// redux imports
import { useSelector } from "react-redux";

import classes from "./CircuitMap.module.css";

// Utilidad para el cálculo del porcentaje.
const calculatePercentage = (partialValue, totalValue) => {
  return ((partialValue / (totalValue + partialValue)) * 100).toFixed(2);
};

// Componente Popup personalizado
const CustomPopup = ({ circuit }) => {
  // redux gets
  const reduxClient = useSelector((state) => state.generalData.client);
  const reduxPartyList = useSelector((state) => state.generalData.partyList);
  const reduxWingList = useSelector((state) => state.generalData.wingList);
  const reduxSlateList = useSelector((state) => state.generalData.slateList);

  // Obtener total de votos para el partido del cliente
  let totalPartyVotes = getClientPartyVotes(
    circuit,
    reduxClient,
    reduxSlateList
  );

  // Calcular la suma total de votos de tu partido
  const totalSlateVotes = circuit.listCircuitSlates.reduce(
    (acc, slate) => acc + slate.votes,
    0
  );

  return (
    <Popup>
      <strong>Circuito: </strong>#{circuit.number}
      <br />
      <strong>Nombre: </strong>
      {circuit.name}
      <br />
      <strong>Votación: </strong>
      {totalPartyVotes.toLocaleString("de-DE")}
      <br />
      <strong>Mi partido: </strong>
      {totalSlateVotes.toLocaleString("de-DE")} (
      {calculatePercentage(totalSlateVotes, totalPartyVotes)}%)
    </Popup>
  );
};

L.icon({
  iconUrl: iconBlue,
  shadowUrl: iconShadow,
  iconAnchor: [16, 37],
  iconSize: [20, 30],
});

// Opciones de marcador por defecto con icono azul
const defaultIcon = L.icon({
  iconUrl: iconBlue,
  shadowUrl: iconShadow,
  iconAnchor: [16, 37],
  iconSize: [20, 30],
});

// Opciones de marcador con icono rojo
const completedIcon = L.icon({
  iconUrl: iconGreen,
  shadowUrl: iconShadow,
  iconAnchor: [16, 37],
  iconSize: [20, 30],
});

function getClientPartyVotes(circuit, reduxClient, reduxSlateList) {
  let totalVotes = 0;

  if (reduxClient && circuit && circuit.listCircuitSlates && reduxSlateList) {
    const clientPartyId = reduxClient.party?.id;

    circuit.listCircuitSlates.forEach((circuitSlate) => {
      const slate = reduxSlateList.find(
        (slate) => slate.id === circuitSlate.slateId
      );

      if (slate && slate.wing && slate.wing.partyId === clientPartyId) {
        totalVotes += circuitSlate.totalSlateVotes || 0;
      }
    });
  }

  return totalVotes;
}

function getAllSlateVotes(circuit) {
  let totalVotes = 0;

  // Verifica si el circuito tiene slates asignados
  if (circuit && circuit.listCircuitSlates) {
    // Itera sobre cada slate del circuito
    circuit.listCircuitSlates.forEach((circuitSlate) => {
      // Suma los votos de cada slate al total
      totalVotes += circuitSlate.totalSlateVotes || 0;
    });
  }

  return totalVotes;
}

const CircuitMap = (props) => {
  //#region Consts ***********************************

  // Redux gets
  const provinceCenter = useSelector(
    (state) => state.liveSettings.provinceCenter
  );
  const provinceZoom = useSelector((state) => state.liveSettings.provinceZoom);
  const isMobile = useSelector((state) => state.auth.isMobile);

  const defaultCenter = [-34.91911763324771, -56.15673823330682];
  const defaultZoom = provinceZoom ? provinceZoom : 13;

  const mapStyle = {
    height: isMobile ? "370px" : "450px",
    width: "100%",
  };

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    L.Marker.prototype.options.icon = L.icon({
      iconUrl: iconBlue,
      shadowUrl: iconShadow,
      iconAnchor: [16, 37],
      iconSize: [20, 30],
    });
  }, []);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  // Hook para ajustar la vista del mapa
  const ChangeView = ({ bounds }) => {
    const map = useMap();

    useEffect(() => {
      if (map && bounds) {
        map.fitBounds(bounds); // Ajustar la vista del mapa a los límites
      }
    }, [map, bounds]);

    return null;
  };

  // Calcula los límites para incluir todos los circuitos
  const bounds = useMemo(() => {
    const latLngBounds = L.latLngBounds();
    props.circuitList.forEach((circuit) => {
      if (circuit.latLong) {
        const [lat, lon] = circuit.latLong.split(",").map(Number);
        if (!isNaN(lat) && !isNaN(lon)) {
          latLngBounds.extend([lat, lon]);
        }
      }
    });
    return latLngBounds.isValid() ? latLngBounds : null; // Verificar que los límites son válidos
  }, [props.circuitList]);

  // Función para crear el ícono del cluster
  const createCustomClusterIcon = (cluster) => {
    return new divIcon({
      html: `<div class=${
        classes.clusterIcon
      }>${cluster.getChildCount()}</div>`,
      className: "custom-marker-cluster",
      iconSize: L.point(33, 33, true),
    });
  };

  // Calcula el centro de todos los circuitos
  const centerOfCircuits = useMemo(() => {
    if (props.circuitList && props.circuitList.length > 0) {
      let totalLat = 0;
      let totalLon = 0;
      let count = 0;

      props.circuitList.forEach((circuit) => {
        if (circuit.latLong) {
          const [lat, lon] = circuit.latLong.split(",").map(Number);
          if (!isNaN(lat) && !isNaN(lon)) {
            totalLat += lat;
            totalLon += lon;
            count++;
          }
        }
      });

      if (count > 0) {
        return [totalLat / count, totalLon / count];
      }
    }
    return defaultCenter; // Utiliza un centro por defecto si no hay circuitos
  }, [props.circuitList]);

  //#endregion Functions ***********************************

  return (
    <MapContainer style={mapStyle}>
      <ChangeView bounds={bounds} />
      <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
      <MarkerClusterGroup
        chunkedLoading
        iconCreateFunction={createCustomClusterIcon}
      >
        {props.circuitList?.map((circuit) => {
          if (circuit.latLong) {
            const [lat, lon] = circuit.latLong.split(",").map(Number);
            if (!isNaN(lat) && !isNaN(lon)) {
              // Determinar si todos los pasos están completados
              const allStepsCompleted =
                circuit.step1completed &&
                circuit.step2completed &&
                circuit.step3completed;

              return (
                <Marker
                  key={circuit.id}
                  position={[lat, lon]}
                  icon={allStepsCompleted ? completedIcon : defaultIcon}
                >
                  <CustomPopup circuit={circuit} />
                </Marker>
              );
            } else {
              console.log("Coordenadas no válidas:", circuit.latLong);
            }
          }
          return null;
        })}
      </MarkerClusterGroup>
    </MapContainer>
  );
};

export default CircuitMap;
