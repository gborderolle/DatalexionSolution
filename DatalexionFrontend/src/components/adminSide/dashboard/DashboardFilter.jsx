import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";

import {
  CCol,
  CListGroup,
  CListGroupItem,
  CInputGroup,
  CFormInput,
  CPagination,
  CPaginationItem,
  CAccordion,
  CAccordionItem,
  CAccordionHeader,
  CAccordionBody,
  CFormCheck,
} from "@coreui/react";
import { motion } from "framer-motion";

const DashboardFilter = ({
  selectedCircuit,
  setSelectedCircuit,
  selectedMunicipality,
  setSelectedMunicipality,
  selectedProvince,
  setSelectedProvince,
  setPartyChartData,
  setSlateChartData,
  isExpanded,
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

  const [activeKey, setActiveKey] = useState(1);
  const isMobile = JSON.parse(localStorage.getItem("isMobile"));

  // redux gets
  const reduxSlateList = useSelector((state) => state.generalData.slateList);
  const reduxProvinceList = useSelector(
    (state) => state.generalData.provinceList
  );
  const reduxMunicipalityList = useSelector(
    (state) => state.generalData.municipalityList
  );
  const reduxCircuitList = useSelector(
    (state) => state.generalData.circuitList
  );

  const [slateColors, setSlateColors] = useState({});

  // Filtrar municipios y circuitos basados en las selecciones
  const filteredMunicipalityList = selectedProvince
    ? reduxMunicipalityList?.filter(
        (municipality) =>
          municipality.provinceId === selectedProvince.id &&
          (municipality.name
            ? municipality.name
                .toLowerCase()
                .includes(searchMunicipality.toLowerCase())
            : false)
      )
    : [];

  const filteredCircuitList = selectedMunicipality
    ? reduxCircuitList?.filter(
        (circuit) =>
          circuit.municipalityId === selectedMunicipality.id &&
          circuit.step1completed &&
          circuit.step2completed &&
          circuit.step3completed &&
          (circuit.name
            ? circuit.name.toLowerCase().includes(searchCircuit.toLowerCase())
            : false)
      )
    : [];

  function getFilteredCircuitList() {
    if (selectedMunicipality) {
      return reduxCircuitList?.filter(
        (circuit) =>
          circuit.municipalityId === selectedMunicipality.id && // Filtra por el ID del municipio seleccionado
          circuit.name.toLowerCase().includes(searchCircuit.toLowerCase()) && // Filtra por el texto de búsqueda en el nombre del circuito
          circuit.step1completed && // Solo incluye circuitos donde el paso 1 está completado
          circuit.step2completed && // Solo incluye circuitos donde el paso 2 está completado
          circuit.step3completed // Solo incluye circuitos donde el paso 3 está completado
      );
    }
  }

  const currentFilteredCircuitList = getFilteredCircuitList();

  //#endregion Consts ***********************************

  //#region Hooks ***********************************

  useEffect(() => {
    setActiveKey(isMobile ? null : 1); // Ajusta el '1' al key del acordeón que quieres abierto por defecto en modo no móvil
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

  // useEffect(() => {
  //   setPageCountProvinces(
  //     Math.ceil(filteredProvinceList.length / itemsPerPageProvinces)
  //   );

  //   const provincesWithVotes = filteredProvinceList.map((province) => ({
  //     ...province,
  //     totalVotes: getTotalVotesByProvince(province.id),
  //   }));

  //   // Ordenar primero por cantidad de votos y luego por nombre
  //   const sortedProvinces = provincesWithVotes.sort((a, b) => {
  //     if (b.totalVotes - a.totalVotes === 0) {
  //       // Si tienen la misma cantidad de votos
  //       return a.name.localeCompare(b.name); // Orden alfabético
  //     }
  //     return b.totalVotes - a.totalVotes; // Orden por cantidad de votos
  //   });
  //   setFilteredProvinceList(sortedProvinces);
  // }, [filteredProvinceList]);

  useEffect(() => {
    setPageCountMunicipalities(
      Math.ceil(filteredMunicipalityList.length / itemsPerPageMunicipalities)
    );
  }, [filteredMunicipalityList]);

  //#endregion Pagination ***********************************

  useEffect(() => {
    const filtered = reduxProvinceList?.filter((province) =>
      province.name
        ? province.name.toLowerCase().includes(searchProvince.toLowerCase())
        : false
    );
    setFilteredProvinceList(filtered);
  }, [searchProvince, reduxProvinceList]);

  // Genera un color aleatorio para cada lista cuando la lista de slates cambie
  useEffect(() => {
    const newSlateColors = reduxSlateList?.reduce((colors, slate) => {
      colors[slate.id] = slate.color || getRandomColor();
      return colors;
    }, {});

    setSlateColors(newSlateColors);
  }, [reduxSlateList]);

  useEffect(() => {
    // Asegúrate de que reduxSelectedCircuit y slateVotesList existen
    if (selectedCircuit && selectedCircuit?.listCircuitSlates) {
      selectedCircuit?.listCircuitSlates.forEach((slate, index) => {
        const slateColor = slateColors[slate.id] || getRandomColor();
        const element = document.getElementById(`idSlate-${index}`);
        if (element) {
          element.style.borderLeft = `4px solid ${slateColor} !important`;
        }
      });
    }
  }, [selectedCircuit, slateColors]);

  useEffect(() => {
    setPageCountCircuits(
      Math.ceil(filteredProvinceList.length / itemsPerPageCircuits)
    );
  }, [filteredProvinceList.length, itemsPerPageCircuits]);

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
            {`#${circuit.number}: ${circuit.name} (${totalVotes} votos)`}
          </CListGroupItem>
        );
      });
    }
  };

  function getRandomColor() {
    const letters = "0123456789ABCDEF";
    let color = "#";
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }

  const getTotalVotesByProvince = (provinceId) => {
    const municipalitiesInProvince = reduxMunicipalityList?.filter(
      (municipality) => municipality.provinceId === provinceId
    );

    let totalVotes = 0;
    municipalitiesInProvince.forEach((municipality) => {
      const circuitsInMunicipality = reduxCircuitList?.filter(
        (circuit) => circuit?.municipalityId === municipality.id
      );

      circuitsInMunicipality.forEach((circuit) => {
        if (circuit?.listCircuitParties) {
          totalVotes += circuit?.listCircuitParties?.reduce(
            (sum, partyVote) => sum + partyVote?.votes,
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
      const municipalityInProvince = reduxMunicipalityList?.filter(
        (municipality) => municipality.provinceId === selectedProvince.id
      );

      municipalityInProvince.forEach((municipality) => {
        const circuitsInMunicipality = reduxCircuitList?.filter(
          (circuit) => circuit?.municipalityId === municipality.id
        );

        circuitsInMunicipality.forEach((circuit) => {
          if (circuit?.listCircuitParties) {
            totalVotes += circuit?.listCircuitParties?.reduce(
              (sum, partyVote) => sum + partyVote?.votes,
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
        (circuit) => circuit?.municipalityId === selectedMunicipality.id
      );

      circuitsInMunicipality.forEach((circuit) => {
        if (circuit?.listCircuitParties) {
          totalVotes += circuit?.listCircuitParties?.reduce(
            (sum, partyVote) => sum + partyVote?.votes,
            0
          );
        }
      });
    }
    return totalVotes;
  };

  const getTotalVotesByMunicipality = (municipalityId) => {
    const circuitsInMunicipality = reduxCircuitList?.filter(
      (circuit) => circuit?.municipalityId === municipalityId
    );

    let totalVotes = 0;
    circuitsInMunicipality.forEach((circuit) => {
      if (circuit?.listCircuitParties) {
        totalVotes += circuit?.listCircuitParties?.reduce(
          (sum, partyVote) => sum + partyVote?.votes,
          0
        );
      }
    });

    return totalVotes;
  };

  const getTotalVotesByCircuit = (circuitId) => {
    const circuit = reduxCircuitList?.find((c) => c.id === circuitId);
    if (!circuit || !circuit?.listCircuitParties) {
      return 0;
    }

    return circuit?.listCircuitParties.reduce(
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

  const handleResetFilters = () => {
    setSelectedProvince(null);
    setSelectedMunicipality(null);
    setSelectedCircuit(null);
  };

  const handleResetMunicipalitySelection = () => {
    setSelectedMunicipality(null);
    setSelectedCircuit(null);
  };

  const handleResetCircuitSelection = () => {
    setSelectedCircuit(null);
  };

  const handleProvinceClick = (province) => {
    setSelectedProvince(province);
    setSelectedMunicipality(null);
    setSelectedCircuit(null);
  };

  // Manejador para cuando se selecciona un municipio
  const handleMunicipalityClick = (municipality) => {
    setSelectedMunicipality(municipality);
    setSelectedCircuit(null);
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
      return currentProvinces.map((province) => {
        const totalVotes = getTotalVotesByProvince(province.id);
        // Asegúrate de que cada CListGroupItem tenga una prop `key` única.
        return (
          <CListGroupItem
            component="button"
            key={province.id} // La prop `key` aquí usa el id de la provincia, lo cual es único
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
      {/* <CCol xs={12} sm={6} md={6} lg={4} xl={4}> */}
      <CCol
        xs={12}
        sm={!isExpanded ? 6 : 2}
        md={!isExpanded ? 6 : 2}
        lg={!isExpanded ? 4 : 2}
        xl={!isExpanded ? 4 : 2}
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
                    <CListGroupItem
                      active
                      component="button"
                      onClick={handleResetFilters}
                    >
                      Departamentos ({getTotalVotesAllProvinces()} votos)
                    </CListGroupItem>
                    {renderProvinceList()}
                    <br />
                    <CPagination align="center" aria-label="Page navigation">
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
                    <CListGroupItem
                      active
                      component="button"
                      onClick={handleResetMunicipalitySelection}
                    >
                      Municipios ({getTotalVotesAllMunicipalities()} votos)
                    </CListGroupItem>
                    {renderMunicipalityList()}
                    <br />
                    <CPagination align="center" aria-label="Page navigation">
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
                    <CListGroupItem
                      active
                      component="button"
                      onClick={handleResetCircuitSelection}
                    >
                      Sólo circuitos cerrados ({getTotalVotesAllCircuits()}{" "}
                      votos)
                    </CListGroupItem>
                    {renderCircuitList()} <br />
                    <CPagination align="center" aria-label="Page navigation">
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
                </CAccordionBody>
              ) : null}
            </CAccordionItem>
          </motion.div>
        </CAccordion>
      </CCol>
    </>
  );
};

export default DashboardFilter;
