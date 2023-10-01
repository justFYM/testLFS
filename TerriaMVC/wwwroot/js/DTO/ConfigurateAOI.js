class ConfigurateAOI {
    constructor(selector) {
        this.Selector = selector;
        this.setCloseModalToMapIconButon();
       
    }
    getSelector() {
        return this.Selector;
    }
     setCloseModalToMapIconButon() {
        const iconButton = document.querySelector('.MapIconButton___StyledDiv-sc-12ve1j3-6.bkXrkN');
        if (iconButton) {
            const boton = iconButton.querySelector('button');
            // Comprueba si se encontró un botón
            if (boton) {
                // Verifica si el atributo onClick ya está asignado
                if (!boton.hasAttribute("onClick")) {
                    // Asigna el atributo onClick al botón
                    boton.setAttribute("onClick", "closeModal('AOI')");
                } else {
                    console.log('La función ya está asignada al botón.');
                }
            } else {
                console.log('No se encontró un botón dentro del div.');
            }
        } else {
            console.log('No se encontró el div con la clase especificada.');
        }
    }

    updateOrCreateRowWithElementsInsidePolygon(elementsInsidePolygon) {
        if (elementsInsidePolygon != null) {
            const uniquePolygonCoordinates = Array.from(new Set(elementsInsidePolygon));
            const interactionContentDiv = document.querySelector(".tjs-map-interaction-window__content");
            if (interactionContentDiv) {
                const firstDivInsideContent = interactionContentDiv.querySelector("div");
                if (firstDivInsideContent) {
                    // Verifica si ya existe la fila que contiene los elementos dentro del polígono y el enlace "see elements"
                    const existingRow = firstDivInsideContent.querySelector(".row");
                    if (!existingRow) {
                        // Si no existe la fila, se crea
                        const row = document.createElement("div");
                        row.classList.add("row");

                        // Columna para el texto "see elements" cliqueable
                        const column1 = document.createElement("div");
                        column1.classList.add("col-md-12"); 

                        // Agrega los elementos dentro del polígono a la columna 1
                        const horizontalLine = document.createElement("hr");
                        column1.appendChild(horizontalLine);
                        const elementsCountText = document.createTextNode(`Elements inside the polygon: ${uniquePolygonCoordinates.length} `);
                        column1.appendChild(elementsCountText);

                        // Crea el enlace "see elements" cliqueable como un enlace
                        const seeElementsLink = document.createElement("a");
                        seeElementsLink.href = "#"; // Agrega un enlace ficticio
                        seeElementsLink.textContent = "[see elements]"; // Cambia el texto a minúsculas

                        // Agrega la función al hacer clic en el enlace
                        seeElementsLink.addEventListener("click", function (event) {
                            event.preventDefault(); // Evita que el enlace navegue a una nueva página
                            console.log("Colocar función que reciba la id cada elemento y que abra un PopUp con el nombre del tipo de instrumento al que pertenece.")
                        });

                        // Agrega la columna a la fila
                        row.appendChild(column1);

                        // Agrega la fila al primer div dentro del contenido
                        firstDivInsideContent.appendChild(row);

                            if (uniquePolygonCoordinates.length > 0) {
                                column1.appendChild(seeElementsLink);
                            }
                      } else {
                        // Si la fila ya existe, actualiza el contenido de "Elements inside the polygon"
                        const elementsCountText = existingRow.querySelector("div.col-md-12").childNodes[2];
                        elementsCountText.textContent = `elements inside the polygon: ${uniquePolygonCoordinates.length}, `;
                    }
                }
            }
        } else {
            console.log("No hay elementos dentro del polígono.");
        }






    }

    activateOrDesactivateCalculateAOIButton(elementsInsidePolygon) {
        if (elementsInsidePolygon != null) {
            const uniquePolygonCoordinates = Array.from(new Set(elementsInsidePolygon));
       
        const measureToolButtonsContainer = document.getElementById("measureToolButtonsContainer");
        if (measureToolButtonsContainer) {
            const buttons = measureToolButtonsContainer.getElementsByTagName("button");
            for (let i = 0; i < buttons.length; i++) {
                if (buttons[i].textContent === "Calculate AOI") {
                    //calculateAOIButton = buttons[i];
                    buttons[i].setAttribute("onClick", `CalculateAOI(${JSON.stringify(uniquePolygonCoordinates)})`);
                    if (uniquePolygonCoordinates.length > 0) {
                        buttons[i].disabled = false;
                        buttons[i].title = "";
                    }
                   
                }
                if (buttons[i].textContent === "Done" || buttons[i].textContent === "Cancel") {
                   // const closeModalAOIButton = buttons[i];
                    buttons[i].setAttribute("onClick", "closeModal('AOI')");
                }
            }
        }

        }




    }

}