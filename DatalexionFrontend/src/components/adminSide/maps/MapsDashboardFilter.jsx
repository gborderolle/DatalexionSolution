import React, { useState, useEffect } from "react";

import {
  CCol,
  CListGroup,
  CListGroupItem,
  CInputGroup,
  CFormInput,
  CPagination,
  CPaginationItem,
  CFormCheck,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
} from "@coreui/react";
import { motion } from "framer-motion";

// redux imports
import { useSelector } from "react-redux";

const MapsDashboardFilter = ({
  selectedCircuit,
  setSelectedCircuit,
  selectedMunicipality,
  setSelectedMunicipality,
  selectedProvince,
  setSelectedProvince,
  isExpanded,
  setIsExpanded,
}) => {
  //#region Consts ***********************************

  const [searchProvince, setSearchProvince] = useState("");
  const [searchMunicipality, setSearchMunicipality] = useState("");
  const [searchCircuit, setSearchCircuit] = useState("");

  const [processedProvinces, setProcessedProvinces] = useState([]);

  //#region Pagination   ***********************************

  const [currentPageCircuits, setCurrentPageCircuits] = useState(1);
  const itemsPerPageCircuits = 10;
  const [pageCountCircuits, setPageCountCircuits] = useState(0);

  const [currentPageProvinces, setCurrentPageProvinces] = useState(1);
  const itemsPerPageProvinces = 10;
  const [pageCountProvinces, setPageCountProvinces] = useState(0);

  const [currentPageMunicipalities, setCurrentPageMunicipalities] = useState(1);
  const itemsPerPageMunicipalities = 10;
  const [pageCountMunicipalities, setPageCountMunicipalities] = useState(0);

  //#endregion Pagination ***********************************

  const [filteredProvinceList, setFilteredProvinceList] = useState([]);
  const [completedCircuits, setCompletedCircuits] = useState(0);
  const [filterType, setFilterType] = useState("todos");

  const [activeKey, setActiveKey] = useState(1);
  const isMobile = JSON.parse(localStorage.getItem("isMobile"));

  // redux gets
  const reduxProvinceList = useSelector(
    (state) => state.generalData.provinceList
  );

  const [municipalityList, setMunicipalityList] = useState([]);
  const reduxMunicipalityList = useSelector(
    (state) => state.generalData.municipalityList
  );

  const [circuitList, setCircuitList] = useState([]);
  const reduxCircuitList = useSelector(
    (state) => state.generalData.circuitList
  );

  // Filtrar municipios y circuitos basados en las selecciones
  const filteredMunicipalityList = selectedProvince
    ? reduxMunicipalityList?.filter(
        (municipality) =>
          municipality.provinceId === selectedProvince.id &&
          municipality.name
            .toLowerCase()
            .includes(searchMunicipality.toLowerCase())
      )
    : [];

  const filteredCircuitList = selectedMunicipality
    ? reduxCircuitList?.filter(
        (circuit) =>
          (circuit.municipalityId === selectedMunicipality.id &&
            circuit.name.toLowerCase().includes(searchCircuit.toLowerCase())) ||
          (circuit.municipalityId === selectedMunicipality.id &&
            circuit.number.toString().includes(searchCircuit))
      )
    : [];

  const getFilteredCircuitList = () => {
    switch (filterType) {
      case "completados":
        return circuitList?.filter(
          (circuit) =>
            circuit.step1completed &&
            circuit.step2completed &&
            circuit.step3completed
        );
      case "sinCompletar":
        return circuitList?.filter(
          (circuit) =>
            !(
              circuit.step1completed &&
              circuit.step2completed &&
              circuit.step3completed
            )
        );
      default:
        return circuitList;
    }
  };

  const currentFilteredCircuitList = getFilteredCircuitList();

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    setActiveKey(isMobile ? 0 : 1); // Ajusta el '1' al key del acordeón que quieres abierto por defecto en modo no móvil
  }, [isMobile]);

  //#region Pagination ***********************************

  useEffect(() => {
    setPageCountCircuits(
      Math.ceil(filteredCircuitList.length / itemsPerPageCircuits)
    );
  }, [filteredCircuitList]);

  useEffect(() => {
    // Esto se ejecuta solo cuando `filteredProvinceList` o `itemsPerPageProvinces` cambian
    setPageCountProvinces(
      Math.ceil(filteredProvinceList.length / itemsPerPageProvinces)
    );

    const provincesWithVotes = filteredProvinceList.map((province) => ({
      ...province,
      totalVotes: getTotalVotesByProvince(province.id),
    }));

    // Ordenar primero por cantidad de votos y luego por nombre
    const sortedProvinces = provincesWithVotes.sort((a, b) => {
      return b.totalVotes - a.totalVotes || a.name.localeCompare(b.name);
    });

    // Actualizamos el estado con las provincias procesadas
    setProcessedProvinces(sortedProvinces);
  }, [filteredProvinceList, itemsPerPageProvinces]);

  useEffect(() => {
    setPageCountMunicipalities(
      Math.ceil(filteredMunicipalityList.length / itemsPerPageMunicipalities)
    );
  }, [filteredMunicipalityList]);

  //#endregion Pagination ***********************************

  // redux gets
  useEffect(() => {
    setMunicipalityList(reduxMunicipalityList);
    setCircuitList(reduxCircuitList);
  }, [reduxMunicipalityList, reduxCircuitList]);

  useEffect(() => {
    // Cuando cambia la provincia seleccionada, se limpian los estados de municipio y circuito seleccionados.
    setSelectedMunicipality(null);
    setSelectedCircuit(null);
  }, [selectedProvince]); // La dependencia es selectedProvince para que este efecto se ejecute solo cuando cambie.

  // Actualizar la lista de municipios filtrados cuando cambie selectedProvince o searchMunicipality
  useEffect(() => {
    const filtered = reduxMunicipalityList?.filter(
      (municipality) =>
        municipality.provinceId === selectedProvince?.id &&
        municipality?.name
          ?.toLowerCase()
          .includes(searchMunicipality.toLowerCase())
    );
    setMunicipalityList(filtered);
  }, [selectedProvince, searchMunicipality, reduxMunicipalityList]);

  // Actualizar la lista de circuitos filtrados cuando cambie selectedMunicipality o searchCircuit
  useEffect(() => {
    const filtered = reduxCircuitList?.filter(
      (circuit) =>
        circuit?.municipalityId === selectedMunicipality?.id &&
        circuit?.name?.toLowerCase().includes(searchCircuit.toLowerCase())
    );
    setCircuitList(filtered);
  }, [selectedMunicipality, searchCircuit, reduxCircuitList]);

  useEffect(() => {
    const filtered = reduxProvinceList?.filter((province) =>
      province?.name?.toLowerCase().includes(searchProvince.toLowerCase())
    );
    setFilteredProvinceList(filtered);
  }, [searchProvince]);

  // Efecto para actualizar municipios cuando cambia la provincia seleccionada
  useEffect(() => {
    const filteredMunicipalities = selectedProvince
      ? reduxMunicipalityList?.filter(
          (municipality) => municipality.provinceId === selectedProvince.id
        )
      : [];
    setMunicipalityList(filteredMunicipalities);
    // No resetear municipio o circuito aquí
  }, [selectedProvince, reduxMunicipalityList]);

  // Actualiza la lista de circuitos basada en la provincia o municipio seleccionado
  useEffect(() => {
    let filteredCircuits;
    if (selectedMunicipality) {
      filteredCircuits = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
    } else if (selectedProvince) {
      const municipalitiesInProvince = reduxMunicipalityList?.filter(
        (municipality) => municipality.provinceId === selectedProvince.id
      );
      filteredCircuits = reduxCircuitList?.filter((circuit) =>
        municipalitiesInProvince?.some(
          (municipality) => municipality.id === circuit.municipalityId
        )
      );
    } else {
      filteredCircuits = [...reduxCircuitList]; // Mantiene todos los circuitos si no hay selección
    }
    setCircuitList(filteredCircuits);
  }, [
    selectedProvince,
    selectedMunicipality,
    reduxCircuitList,
    reduxMunicipalityList,
  ]);

  useEffect(() => {
    let filtered = [];

    if (selectedCircuit) {
      // Si hay un circuito seleccionado, crea una lista que contenga solo ese circuito
      filtered = [selectedCircuit];
    } else if (selectedMunicipality) {
      // Filtra los circuitos basados en el municipio seleccionado
      filtered = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );
    } else if (selectedProvince) {
      // Primero, obtiene todos los municipios que pertenecen a la provincia seleccionada
      const municipalitiesInProvince = reduxMunicipalityList?.filter(
        (municipality) => municipality.provinceId === selectedProvince.id
      );

      // Luego, filtra los circuitos basados en los municipios de la provincia seleccionada
      filtered = reduxCircuitList?.filter((circuit) =>
        municipalitiesInProvince?.some(
          (municipality) => municipality.id === circuit.municipalityId
        )
      );
    } else {
      // Muestra todos los circuitos si no hay provincia ni municipio seleccionado
      filtered = [...reduxCircuitList];
    }

    setCircuitList(filtered);
  }, [
    selectedProvince,
    selectedMunicipality,
    selectedCircuit,
    reduxCircuitList,
    reduxMunicipalityList,
  ]);

  // Calcula el total de circuitos y los completados
  useEffect(() => {
    const totalCompleted = reduxCircuitList?.filter(
      (circuit) =>
        circuit.step1completed &&
        circuit.step2completed &&
        circuit.step3completed
    ).length;
    setCompletedCircuits(totalCompleted);
  }, [reduxCircuitList]);

  //#endregion Hooks ***********************************

  //#region Functions ***********************************

  //#region Pagination Circuits ***********************************

  // Determinar el rango de páginas a mostrar alrededor de la página actual
  let pagesToShowCircuits = 3; // Ajusta este número según sea necesario
  let startPageCircuits = Math.max(
    currentPageCircuits - Math.floor(pagesToShowCircuits / 2),
    1
  );
  let endPageCircuits = Math.min(
    startPageCircuits + pagesToShowCircuits - 1,
    pageCountCircuits
  );

  if (endPageCircuits - startPageCircuits + 1 < pagesToShowCircuits) {
    startPageCircuits = Math.max(endPageCircuits - pagesToShowCircuits + 1, 1);
  }

  let indexOfLastItemCircuits = currentPageCircuits * itemsPerPageCircuits;
  let indexOfFirstItemCircuits = indexOfLastItemCircuits - itemsPerPageCircuits;
  let currentCircuits = currentFilteredCircuitList?.slice(
    indexOfFirstItemCircuits,
    indexOfLastItemCircuits
  );

  //#endregion Pagination Circuits ***********************************

  // Función modificada para renderizar los circuitos de la página actual
  const renderCircuitList = () => {
    if (currentCircuits && currentCircuits.length > 0) {
      return currentCircuits.map((circuit, index) => {
        const totalVotes = getTotalVotesByCircuit(circuit.id);

        // Usar tanto partyId como slateId para formar una key compuesta
        const key = `${circuit.id}-${index}`;

        const isCompleted =
          circuit.step1completed &&
          circuit.step2completed &&
          circuit.step3completed;
        const listItemStyle = isCompleted ? { color: "green" } : {};

        return (
          <CListGroupItem
            component="button"
            key={key}
            onClick={() => handleCircuitClick(circuit)}
            className={
              selectedCircuit && selectedCircuit.id === circuit.id
                ? "selected-item"
                : ""
            }
          >
            <span
              style={listItemStyle}
            >{`#${circuit.number}: ${circuit.name} (${totalVotes} votos)`}</span>
          </CListGroupItem>
        );
      });
    } else {
      // Mostrar un mensaje cuando no haya circuitos disponibles
      return (
        <CListGroupItem>
          No hay circuitos disponibles para la selección actual.
        </CListGroupItem>
      );
    }
  };

  const getTotalVotesByProvince = (id) => {
    const municipalitiesInProvince = reduxMunicipalityList?.filter(
      (municipality) => municipality.id === id
    );

    let totalVotes = 0;
    municipalitiesInProvince.forEach((municipality) => {
      const circuitsInMunicipality = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === municipality.id
      );

      circuitsInMunicipality.forEach((circuit) => {
        if (circuit.listCircuitParties) {
          totalVotes += circuit.listCircuitParties.reduce(
            (sum, partyVote) => sum + partyVote.votes,
            0
          );
        }
      });
    });

    return totalVotes;
  };

  const getTotalVotesAllProvinces = () => {
    let totalVotes = 0;
    reduxProvinceList.forEach((province) => {
      totalVotes += getTotalVotesByProvince(province.id);
    });
    return totalVotes;
  };

  const getTotalVotesAllMunicipalities = () => {
    let totalVotes = 0;

    if (selectedProvince) {
      const municipalityInProvince = municipalityList?.filter(
        (municipality) => municipality.id === selectedProvince.id
      );

      municipalityInProvince.forEach((municipality) => {
        const circuitsInMunicipality = reduxCircuitList?.filter(
          (circuit) => circuit.municipalityId === municipality.id
        );

        circuitsInMunicipality.forEach((circuit) => {
          if (circuit.listCircuitParties) {
            totalVotes += circuit.listCircuitParties.reduce(
              (sum, partyVote) => sum + partyVote.votes,
              0
            );
          }
        });
      });
    }
    return totalVotes;
  };

  const getTotalVotesAllCircuits = () => {
    let totalVotes = 0;

    if (selectedMunicipality) {
      const circuitsInMunicipality = reduxCircuitList?.filter(
        (circuit) => circuit.municipalityId === selectedMunicipality.id
      );

      circuitsInMunicipality.forEach((circuit) => {
        if (circuit.listCircuitParties) {
          totalVotes += circuit.listCircuitParties.reduce(
            (sum, partyVote) => sum + partyVote.votes,
            0
          );
        }
      });
    }
    return totalVotes;
  };

  const getTotalVotesByMunicipality = (municipalityId) => {
    const circuitsInMunicipality = reduxCircuitList?.filter(
      (circuit) => circuit.municipalityId === municipalityId
    );

    let totalVotes = 0;
    circuitsInMunicipality.forEach((circuit) => {
      if (circuit.listCircuitParties) {
        totalVotes += circuit.listCircuitParties.reduce(
          (sum, partyVote) => sum + partyVote.votes,
          0
        );
      }
    });

    return totalVotes;
  };

  const getTotalVotesByCircuit = (circuitId) => {
    const circuit = reduxCircuitList.find((c) => c.id === circuitId);

    if (!circuit || !circuit.listCircuitParties) {
      return 0;
    }

    return circuit.listCircuitParties.reduce(
      (sum, partyVote) => sum + partyVote.votes,
      0
    );
  };

  //#endregion Functions ***********************************

  //#region Events ***********************************

  //#region Pagination ***********************************

  const handlePageChangeCircuits = (pageNumber) => {
    setCurrentPageCircuits(pageNumber);
  };
  const handlePageChangeProvinces = (pageNumber) => {
    setCurrentPageProvinces(pageNumber);
  };
  const handlePageChangeMunicipalities = (pageNumber) => {
    setCurrentPageMunicipalities(pageNumber);
  };

  //#endregion Pagination ***********************************

  const handleFilterChange = (event) => {
    setFilterType(event.target.value);
  };

  const handleProvinceClick = (province) => {
    setSelectedProvince(province);
    setSelectedMunicipality(null); // Resetea la selección de municipio
  };

  // Manejador para cuando se selecciona un municipio
  const handleMunicipalityClick = (municipality) => {
    setSelectedMunicipality(municipality);
    setSelectedCircuit(null); // Resetea la selección de municipio
  };

  const handleCircuitClick = (circuit) => {
    setSelectedCircuit(circuit);
  };

  //#endregion Events ***********************************

  //#region JSX ***********************************

  //#region Pagination Provinces ***********************************

  // Determinar el rango de páginas a mostrar alrededor de la página actual
  let pagesToShowProvinces = 3; // Ajusta este número según sea necesario
  let startPageProvinces = Math.max(
    currentPageProvinces - Math.floor(pagesToShowProvinces / 2),
    1
  );
  let endPageProvinces = Math.min(
    startPageProvinces + pagesToShowProvinces - 1,
    pageCountProvinces
  );

  if (endPageProvinces - startPageProvinces + 1 < pagesToShowProvinces) {
    startPageProvinces = Math.max(
      endPageProvinces - pagesToShowProvinces + 1,
      1
    );
  }

  let indexOfLastItemProvinces = currentPageProvinces * itemsPerPageProvinces;
  let indexOfFirstItemProvinces =
    indexOfLastItemProvinces - itemsPerPageProvinces;
  let currentProvinces = filteredProvinceList?.slice(
    indexOfFirstItemProvinces,
    indexOfLastItemProvinces
  );

  //#endregion Pagination Provinces ***********************************

  // Actualizar las funciones de renderizado para incorporar el manejo de clics
  const renderProvinceList = () => {
    if (currentProvinces && currentProvinces.length > 0) {
      return currentProvinces?.map((province) => {
        const totalVotes = getTotalVotesByProvince(province.id);
        return (
          <CListGroupItem
            component="button"
            key={province.id}
            onClick={() => handleProvinceClick(province)}
            className={
              selectedProvince && selectedProvince.id === province.id
                ? "selected-item"
                : ""
            }
          >
            {`${province.name} (${totalVotes} votos)`}
          </CListGroupItem>
        );
      });
    }
  };

  //#region Pagination Municipalities ***********************************

  // Determinar el rango de páginas a mostrar alrededor de la página actual
  let pagesToShowMunicipalities = 3; // Ajusta este número según sea necesario
  let startPageMunicipalities = Math.max(
    currentPageMunicipalities - Math.floor(pagesToShowMunicipalities / 2),
    1
  );
  let endPageMunicipalities = Math.min(
    startPageMunicipalities + pagesToShowMunicipalities - 1,
    pageCountMunicipalities
  );

  if (
    endPageMunicipalities - startPageMunicipalities + 1 <
    pagesToShowMunicipalities
  ) {
    startPageMunicipalities = Math.max(
      endPageMunicipalities - pagesToShowMunicipalities + 1,
      1
    );
  }

  let indexOfLastItemMunicipalities =
    currentPageMunicipalities * itemsPerPageMunicipalities;
  let indexOfFirstItemMunicipalities =
    indexOfLastItemMunicipalities - itemsPerPageMunicipalities;
  let currentMunicipalities = filteredMunicipalityList?.slice(
    indexOfFirstItemMunicipalities,
    indexOfLastItemMunicipalities
  );

  //#endregion Pagination Municipalities ***********************************

  const renderMunicipalityList = () => {
    if (currentMunicipalities && currentMunicipalities.length > 0) {
      return currentMunicipalities?.map((municipality) => {
        const totalVotes = getTotalVotesByMunicipality(municipality.id);
        return (
          <CListGroupItem
            component="button"
            key={municipality.id}
            onClick={() => handleMunicipalityClick(municipality)}
            className={
              selectedMunicipality &&
              selectedMunicipality.id === municipality.id
                ? "selected-item"
                : ""
            }
          >
            {`${municipality.name} (${totalVotes} votos)`}
          </CListGroupItem>
        );
      });
    }
  };

  const filterVariants = {
    hidden: {
      y: -20,
      opacity: 0,
    },
    visible: {
      y: 0,
      opacity: 1,
      transition: {
        duration: 0.3,
        ease: "easeInOut",
      },
    },
  };

  //#endregion JSX ***********************************

  return (
    <>
      <CCol
        xs={12}
        sm={!isExpanded ? 3 : 2}
        md={!isExpanded ? 3 : 2}
        lg={!isExpanded ? 3 : 2}
        xl={!isExpanded ? 3 : 2}
      >
        <CAccordion activeItemKey={activeKey}>
          <motion.div
            initial="hidden"
            animate="visible"
            variants={filterVariants}
          >
            <CAccordionItem itemKey={1}>
              <CAccordionHeader className="custom-accordion-header">
                Filtros
                {/* <div className="float-end">
                  <CFormCheck
                    type="checkbox"
                    id="expanded"
                    label="Min"
                    checked={isExpanded}
                    onChange={(e) => setIsExpanded(e.target.checked)}
                    style={{
                      display: "inline-block",
                      marginLeft: "10px",
                    }}
                  />
                </div> */}
              </CAccordionHeader>
              {!isExpanded ? (
                <CAccordionBody>
                  <CListGroup>
                    <CInputGroup>
                      <CFormInput
                        placeholder="Filtrar departamento..."
                        onChange={(e) => setSearchProvince(e.target.value)}
                        size="sm"
                      />
                    </CInputGroup>
                    <CListGroupItem active>
                      Departamentos ({getTotalVotesAllProvinces()} votos)
                    </CListGroupItem>
                    {renderProvinceList()}
                    <br />
                    <CPagination
                      align="center"
                      aria-label="Page navigation example"
                    >
                      {startPageProvinces > 1 && (
                        <CPaginationItem
                          onClick={() => handlePageChangeProvinces(1)}
                        >
                          1
                        </CPaginationItem>
                      )}
                      {startPageProvinces > 2 && (
                        <CPaginationItem>...</CPaginationItem>
                      )}
                      {[
                        ...Array(endPageProvinces - startPageProvinces + 1),
                      ].map((_, index) => (
                        <CPaginationItem
                          key={startPageProvinces + index}
                          active={
                            startPageProvinces + index === currentPageProvinces
                          }
                          onClick={() =>
                            handlePageChangeProvinces(
                              startPageProvinces + index
                            )
                          }
                        >
                          {startPageProvinces + index}
                        </CPaginationItem>
                      ))}
                      {endPageProvinces < pageCountProvinces - 1 && (
                        <CPaginationItem>...</CPaginationItem>
                      )}
                      {endPageProvinces < pageCountProvinces && (
                        <CPaginationItem
                          onClick={() =>
                            handlePageChangeProvinces(pageCountProvinces)
                          }
                        >
                          {pageCountProvinces}
                        </CPaginationItem>
                      )}
                    </CPagination>
                  </CListGroup>
                  <br />
                  <CListGroup>
                    <CInputGroup>
                      <CFormInput
                        placeholder="Filtrar municipio..."
                        onChange={(e) => setSearchMunicipality(e.target.value)}
                        size="sm"
                      />
                    </CInputGroup>
                    <CListGroupItem active>
                      Municipios ({getTotalVotesAllMunicipalities()} votos)
                    </CListGroupItem>
                    {renderMunicipalityList()}
                    <br />
                    <CPagination
                      align="center"
                      aria-label="Page navigation example"
                    >
                      {startPageMunicipalities > 1 && (
                        <CPaginationItem
                          onClick={() => handlePageChangeMunicipalities(1)}
                        >
                          1
                        </CPaginationItem>
                      )}
                      {startPageMunicipalities > 2 && (
                        <CPaginationItem>...</CPaginationItem>
                      )}
                      {[
                        ...Array(
                          endPageMunicipalities - startPageMunicipalities + 1
                        ),
                      ].map((_, index) => (
                        <CPaginationItem
                          key={startPageMunicipalities + index}
                          active={
                            startPageMunicipalities + index ===
                            currentPageMunicipalities
                          }
                          onClick={() =>
                            handlePageChangeMunicipalities(
                              startPageMunicipalities + index
                            )
                          }
                        >
                          {startPageMunicipalities + index}
                        </CPaginationItem>
                      ))}
                      {endPageMunicipalities < pageCountMunicipalities - 1 && (
                        <CPaginationItem>...</CPaginationItem>
                      )}
                      {endPageMunicipalities < pageCountMunicipalities && (
                        <CPaginationItem
                          onClick={() =>
                            handlePageChangeMunicipalities(
                              pageCountMunicipalities
                            )
                          }
                        >
                          {pageCountMunicipalities}
                        </CPaginationItem>
                      )}
                    </CPagination>
                  </CListGroup>
                  <br />
                  <CListGroup>
                    <CInputGroup>
                      <CFormInput
                        placeholder="Filtrar circuito..."
                        onChange={(e) => setSearchCircuit(e.target.value)}
                        size="sm"
                      />
                    </CInputGroup>
                    <CListGroupItem active>
                      Circuitos ({getTotalVotesAllCircuits()} votos)
                    </CListGroupItem>
                    {renderCircuitList()} <br />
                    <CPagination
                      align="center"
                      aria-label="Page navigation example"
                    >
                      {startPageCircuits > 1 && (
                        <CPaginationItem
                          onClick={() => handlePageChangeCircuits(1)}
                        >
                          1
                        </CPaginationItem>
                      )}
                      {startPageCircuits > 2 && (
                        <CPaginationItem>...</CPaginationItem>
                      )}
                      {[...Array(endPageCircuits - startPageCircuits + 1)].map(
                        (_, index) => (
                          <CPaginationItem
                            key={startPageCircuits + index}
                            active={
                              startPageCircuits + index === currentPageCircuits
                            }
                            onClick={() =>
                              handlePageChangeCircuits(
                                startPageCircuits + index
                              )
                            }
                          >
                            {startPageCircuits + index}
                          </CPaginationItem>
                        )
                      )}
                      {endPageCircuits < pageCountCircuits - 1 && (
                        <CPaginationItem>...</CPaginationItem>
                      )}
                      {endPageCircuits < pageCountCircuits && (
                        <CPaginationItem
                          onClick={() =>
                            handlePageChangeCircuits(pageCountCircuits)
                          }
                        >
                          {pageCountCircuits}
                        </CPaginationItem>
                      )}
                    </CPagination>
                  </CListGroup>
                  <br />

                  <hr className="mt-0" />

                  <div key={filterType}>
                    <CFormCheck
                      type="radio"
                      name="radFilterCircuits"
                      value="todos"
                      id="flexRadioDefault1"
                      label="Todos los circuitos"
                      onChange={handleFilterChange}
                      checked={filterType === "todos"}
                    />
                    <CFormCheck
                      type="radio"
                      name="radFilterCircuits"
                      value="completados"
                      id="flexRadioDefault2"
                      label="Sólo cerrados"
                      onChange={handleFilterChange}
                      checked={filterType === "completados"}
                    />
                    <CFormCheck
                      type="radio"
                      name="radFilterCircuits"
                      value="sinCompletar"
                      id="flexRadioDefault3"
                      label="Sólo abiertos"
                      onChange={handleFilterChange}
                      checked={filterType === "sinCompletar"}
                    />
                  </div>
                </CAccordionBody>
              ) : null}
            </CAccordionItem>
          </motion.div>
        </CAccordion>
      </CCol>
    </>
  );
};

export default MapsDashboardFilter;
