import React, { useEffect, useMemo } from "react";
import { MapContainer, TileLayer, useMap, Popup, Marker } from "react-leaflet";
import L, { divIcon } from "leaflet";

import MarkerClusterGroup from "react-leaflet-cluster";

import iconBlue from "leaflet/dist/images/marker-icon.png";
import iconGreen from "leaflet/dist/images/marker-icon-completed.png";
import iconShadow from "leaflet/dist/images/marker-shadow.png";
import "leaflet/dist/leaflet.css";
import classes from "./CircuitMap.module.css";

// redux imports
import { useSelector } from "react-redux";

import { getCircuitParty, calculatePercentage } from "../../../utils/auxiliarFunctions";

// Componente Popup personalizado
const CustomPopup = ({ circuit }) => {
  let totalCircuitPartiesVotes = getCircuitPartiesVotes(circuit);

  // Calcular la suma total de votos de tu partido
  const totalSlateVotes = circuit.listCircuitSlates.reduce(
    (acc, circuitSlate) => acc + circuitSlate.totalSlateVotes,
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
      {totalCircuitPartiesVotes.toLocaleString("de-DE")}
      <br />
      <strong>Mi partido: </strong>
      {totalSlateVotes.toLocaleString("de-DE")} (
      {totalCircuitPartiesVotes > 0
        ? calculatePercentage(totalSlateVotes, totalCircuitPartiesVotes)
        : 0}
      %)
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

function getCircuitPartiesVotes(circuit) {
  let totalVotes = 0;
  if (circuit && circuit.listCircuitParties) {
    circuit.listCircuitParties.forEach((circuitParty) => {
      totalVotes += circuitParty.totalPartyVotes || 0;
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
  const reduxClient = useSelector((state) => state.generalData.client);

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
              // Obtener el circuitParty correspondiente al cliente
              const circuitParty = getCircuitParty(circuit, reduxClient);

              // Determinar si todos los pasos están completados
              const allStepsCompleted =
                circuitParty &&
                circuitParty.step1completed &&
                circuitParty.step2completed &&
                circuitParty.step3completed;

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
