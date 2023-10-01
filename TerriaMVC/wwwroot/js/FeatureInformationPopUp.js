

async function notificationButton() {
    console.log("Funcionó.");
}

/*
Está configurado para que desde React se llame a la función button cuando se cliquea un punto en el mapa. La función recibe como parámetro la id del 
instrumento y el tipo de instrumento. Se puede configurar en el archivo dentro de la carpeta wwwroot/lib/TerriaMap/build/index.TerriaMap.js.
*/
async function GenerateButtonsController(idInstrumento, nombreTipoInstrumento) {
    /*
    Detectar container, si es JavaScript o de TerriaMap.
    Con el container detectado, generar botones y además el primer gráfico. Colocando los botónes y gráfico en el elemento correspondiente.
    */
    await wait();
    const spinner = new GenerateSpinnerDTO("Loading sensors and graph");
    const generateContainerDTO = new GenerateContainersDTO();
    const selectorContainer = await generateContainerDTO.findContainer();
    const selectorButtonsContainer = await generateContainerDTO.findButtonsContainer();
    const selectorGraphContainer = await generateContainerDTO.findGraphContainer();
    await changeSelectorContainerHTMLContent(selectorButtonsContainer, spinner.getSpinner());

    const generateButtonsDTO = new GenerateButtonsDTO(idInstrumento, nombreTipoInstrumento);
    const buttons = await generateButtonsDTO.generateButtons();

    //Genera el gráfico.
    const generateGraphDTO = new GenerateGraphDTO(idInstrumento);
    const responseFirstGraph = await generateGraphDTO.generateFirstGraph(nombreTipoInstrumento);
    await changeSelectorContainerHTMLContent(selectorGraphContainer, responseFirstGraph);
    await changeSelectorContainerHTMLContent(selectorButtonsContainer, buttons);
    await activeTab(selectorButtonsContainer, "Serie de Tiempo");
}
async function GenerateGraphController(idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, tab) {
    await wait();

    const generateContainerDTO = new GenerateContainersDTO();
    const container = await generateContainerDTO.findContainer();
    // console.log("Container activo: " + container)
    const graphContainer = await generateContainerDTO.findGraphContainer();
    await activeTab(container, tab);
    const spinner = new GenerateSpinnerDTO("Cargando gráfico");
    const generateGraphDTO = new GenerateGraphDTO(idInstrumento);
    graphContainer.innerHTML = spinner.getSpinner();
    const graph = await generateGraphDTO.generateGraph(idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph);
    graphContainer.innerHTML = graph;
    await executeScript(container);
}
async function NotificationPopUp() {
  
    let containerNotifications = document.getElementById("TerriaMapNotification");
    if (containerNotifications) {
        console.log("Ya se generaron las notificaciones.")
    }
    else {
        await wait();
        const generateContainerDTO = new GenerateContainersDTO();
        const container = await generateContainerDTO.findContainer();
        console.log(container);
        if (container) {
            //Se prepara el contenedor de notificaciones
            containerNotifications = document.getElementById("TerriaMapNotification");
            await modifyWidthByElementId('100%', 'TerriaMapNotification');
            await modifyWidthByClassName('400px', 'tjs-related-maps__dropdown-inner');
            await removeElementByClassName('tjs-panel__inner-close-btn');

            //Se crea el objeto de transferencia.
            const generateNotificationDTO = new GenerateNotificationDTO();

            const spinner = new GenerateSpinnerDTO("Loading notifications");
            //Se coloca el spinner
            containerNotifications.innerHTML = spinner.getSpinner();

            const notifications = await generateNotificationDTO.generateNotifications();
            //Se colocan las notificaciones.
            containerNotifications.innerHTML = notifications;
        } 
    }
}
async function testButtonHola(idButton, nombreTipoInstrumento, nombreInstrumento, idInstrumento, sensorId, nombreSensor) {
    await wait();
    const generateContainerDTO = new GenerateContainersDTO();
    const container = await generateContainerDTO.findContainer();
    console.log("Container activo: " + container)
    var openPopUpButton = document.getElementById(idButton);
    openPopUpButton.innerHTML = `Cargando`;
    //Se crea la modal.
    const generatePopUpModal = new GeneratePopUpModal("popup", "modal fade", "800px", 0, 0.95);
    const generateButtonsDTO = new GenerateButtonsDTO(idInstrumento, nombreTipoInstrumento);
    const buttons = await generateButtonsDTO.generateButtons();
    const generateGraphDTO = new GenerateGraphDTO(idInstrumento);
    const graph = await generateGraphDTO.generateGraphNotification(nombreTipoInstrumento, sensorId, nombreSensor);

    const popUp = await generatePopUpModal.generateModal(nombreTipoInstrumento, nombreInstrumento, buttons, graph);
    openPopUpButton.parentNode.insertBefore(popUp, openPopUpButton.nextSibling);
    await generatePopUpModal.showModal();

    await executeScriptByElementId("popup");
    openPopUpButton.innerHTML = "Detalles";
    //Marca la primera pestaña.
    await activeTabById("popup", nombreSensor);
}

//Recibe los elementos dentro del polígono desde TerriaMap.
async function MeasureToolElements(array) {
    const generateContainerDTO = new GenerateContainersDTO();
    const selectorContainer = await generateContainerDTO.findContainer();
    const configurateAOI = new ConfigurateAOI(selectorContainer);
    configurateAOI.updateOrCreateRowWithElementsInsidePolygon(array);
    configurateAOI.activateOrDesactivateCalculateAOIButton(array);
}

async function CalculateAOI(array) {
    /* 
    * Crear la modal
    * - en AOI/DefaultContent.html se crea la modal por default, la cual hay que modificarla para que solo retorne "modal-body".
    * - Luego con el objeto GeneratePopUpModal, crear la modal para AOI, pasarle el contenido de modal-body y se creará: ModalTopButtons, ForTopButtons, ModalGraph ,
    * ModalBotButtons.
    * con el objeto de container, se puede obtener ModalTopButtons, ModalBotButtons, ModalGraph.
    * Ver la función CalculateAOI en VisualStudio Code porque ahí se le pasa el contenido html a la modal popupAOI.
    */
    console.log("Elementos dentro del polígono: " + array.length);
    array.forEach(elemento => {
        console.log(elemento);
    });
    /* 
    
    
    */
    const generateContainerDTO = new GenerateContainersDTO();
    const container = await generateContainerDTO.findAOIContainer();

    const generateAOIPopUpModal = new GeneratePopUpModal("popupAOI", "modal fade", "800px", 0, 0.95);

    const idInstrumentos = array;
    const defaultContent = true;
    const url = `https://localhost:7242/api/generateAOI`;
    const data = JSON.stringify({ idInstrumentos, defaultContent });

    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const responseData = await respuesta.text();
    const popUpAOI = await generateAOIPopUpModal.generateAOIModal(responseData, container);
    await generateAOIPopUpModal.showModalAOI();
    await executeScript(container);

}



async function closeModal(id) {
    const checkModal = document.getElementById(`popup` + id);
    if (checkModal) {
        checkModal.remove();
        // $('.tjs-_base__scrollbars').css('overflow-y', 'auto');
    } else {
        console.log("No se encontró la modal.");
    }
}

async function onClickButtonsFromAOIPopUpModal() {
   // const generateAOIDTO = new GenerateAOIDTO();


}

async function GenerateChartAndButtons(array, defaultContent, tab) {
    array.forEach(element => {
        console.log(element);
    });
    const idInstrumentos = array;

    //
    var buttonsModal = document.getElementById("ModalTopButtons");
    var modalGraph = document.getElementById("ForTopButtons");
    // Cambiar su contenido innerHTML
    modalGraph.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando...</div></div>`;
    const elements = buttonsModal.querySelectorAll("*");
    elements.forEach(element => {
        // Verificar si el elemento es un <a> y si su texto coincide con nombreSensor
        if (element.tagName === 'A' && element.textContent === tab) {
            // Realizar la acción que necesitas con el elemento <a>
            // console.log("Elemento <a> encontrado:", element);
            // Por ejemplo, agregar una clase para resaltarlo
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }
    })
    //
    const url = `https://localhost:7242/api/generateAOI`;
    const data = JSON.stringify({ idInstrumentos, defaultContent });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const forTopButtons = document.getElementById("ForTopButtons");

    const responseData = await respuesta.text();
    forTopButtons.innerHTML = responseData;
    console.log(responseData);
    let scriptElements = forTopButtons.querySelectorAll("script");

    scriptElements.forEach(scriptElement => {
        eval(scriptElement.textContent);
    });
    console.log(tab);
    console.log(defaultContent);
}

async function GenerateChartAndButtons(array, defaultContent, isForTop, tab) {
    array.forEach(element => {
        console.log(element);
    });
    const idInstrumentos = array;

    //
    var buttonsModal = document.getElementById("ModalTopButtons");
    var modalGraph = document.getElementById("ForTopButtons");
    // Cambiar su contenido innerHTML
    modalGraph.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando...</div></div>`;
    const elements = buttonsModal.querySelectorAll("*");
    elements.forEach(element => {
        // Verificar si el elemento es un <a> y si su texto coincide con nombreSensor
        if (element.tagName === 'A' && element.textContent === tab) {
            // Realizar la acción que necesitas con el elemento <a>
            // console.log("Elemento <a> encontrado:", element);
            // Por ejemplo, agregar una clase para resaltarlo
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }
    })
    //
    const url = `https://localhost:7242/api/generateAOI`;
    const data = JSON.stringify({ idInstrumentos, defaultContent, isForTop });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const forTopButtons = document.getElementById("ForTopButtons");

    const responseData = await respuesta.text();
    forTopButtons.innerHTML = responseData;
    console.log(responseData);
    let scriptElements = forTopButtons.querySelectorAll("script");

    scriptElements.forEach(scriptElement => {
        eval(scriptElement.textContent);
    });
    console.log(tab);
    console.log(defaultContent);
}
async function GenerateGraphAOIContent(idInstrumento, defaultContent, isForTop, tab) {
    //console.log(idInstrumento);
    const idInstrumentos = [idInstrumento];
    console.log(defaultContent);
    console.log(isForTop);
    console.log(tab);
    var buttonsModal = document.getElementById("ModalBotButtons");
    var modalGraph = document.getElementById("ModalGraph");
    // Cambiar su contenido innerHTML
    modalGraph.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando...</div></div>`;
    const elements = buttonsModal.querySelectorAll("*");
    elements.forEach(element => {
        // Verificar si el elemento es un <a> y si su texto coincide con nombreSensor
        if (element.tagName === 'A' && element.textContent === tab) {
            // Realizar la acción que necesitas con el elemento <a>
            // console.log("Elemento <a> encontrado:", element);
            // Por ejemplo, agregar una clase para resaltarlo
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }
    })
    //
    const url = `https://localhost:7242/api/generateAOI`;
    const data = JSON.stringify({ idInstrumentos, defaultContent, isForTop });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    // const forTopButtons = document.getElementById("ForTopButtons");

    const responseData = await respuesta.text();
    modalGraph.innerHTML = responseData;
    console.log(responseData);
    let scriptElements = modalGraph.querySelectorAll("script");

    scriptElements.forEach(scriptElement => {
        eval(scriptElement.textContent);
    });
    console.log(tab);
    console.log(defaultContent);
}


async function GenerateChartAndButtonsConsolidado(idInstrumento, defaultContent, isForTop, isForTopConsolidado, tab) {

    const idInstrumentos = idInstrumento;

    //
    var buttonsModal = document.getElementById("ModalTopButtons");
    var modalGraph = document.getElementById("ForTopButtons");
    // Cambiar su contenido innerHTML
    modalGraph.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando...</div></div>`;
    const elements = buttonsModal.querySelectorAll("*");
    elements.forEach(element => {
        // Verificar si el elemento es un <a> y si su texto coincide con nombreSensor
        if (element.tagName === 'A' && element.textContent === tab) {
            // Realizar la acción que necesitas con el elemento <a>
            // console.log("Elemento <a> encontrado:", element);
            // Por ejemplo, agregar una clase para resaltarlo
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }
    })
    //
    const url = `https://localhost:7242/api/generateAOI`;
    const data = JSON.stringify({ idInstrumentos, defaultContent, isForTop, isForTopConsolidado });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const forTopButtons = document.getElementById("ForTopButtons");

    const responseData = await respuesta.text();
    forTopButtons.innerHTML = responseData;
    console.log(responseData);
    let scriptElements = forTopButtons.querySelectorAll("script");

    scriptElements.forEach(scriptElement => {
        eval(scriptElement.textContent);
    });
}



async function GenerateGraphAOIConsolidadoContent(idInstrumentos, defaultContent, isForTop, isForTopConsolidado, isForBotConsolidado, tab) {
    idInstrumentos.forEach(iterador => {
        console.log(iterador);
    })
    var buttonsModal = document.getElementById("ModalBotButtons");
    var modalGraph = document.getElementById("ModalGraph");
    // Cambiar su contenido innerHTML
    modalGraph.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando...</div></div>`;
    const elements = buttonsModal.querySelectorAll("*");
    elements.forEach(element => {
        // Verificar si el elemento es un <a> y si su texto coincide con nombreSensor
        if (element.tagName === 'A' && element.textContent === tab) {
            // Realizar la acción que necesitas con el elemento <a>
            // console.log("Elemento <a> encontrado:", element);
            // Por ejemplo, agregar una clase para resaltarlo
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }
    })
    //
    const url = `https://localhost:7242/api/generateAOI`;
    const data = JSON.stringify({ idInstrumentos, defaultContent, isForTop, isForTopConsolidado, isForBotConsolidado });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const responseData = await respuesta.text();
    modalGraph.innerHTML = responseData;
    console.log(responseData);
    let scriptElements = modalGraph.querySelectorAll("script");

    scriptElements.forEach(scriptElement => {
        eval(scriptElement.textContent);
    });
    console.log(tab);
    console.log(defaultContent);
}

async function GenerateLogBookController(idInstrumento, tab) {
    console.log(idInstrumento);
    console.log(tab);
    //Identificar si el div de las notificaciones está abierto, en caso de que si, se ocupará el popup de la modal JS.
    //En caso de que no, entonces se ocupará lo de TerriaMap.
    let terriaMapNotification = document.getElementById("TerriaMapNotification");
    let elementToRender = "";
    let fromJS = true;
    let modalGraph = "";
    if (terriaMapNotification) {
        console.log("Está TerriaMapNotification")
        elementToRender = document.getElementById("popup");
        modalGraph = document.getElementById("ModalGraph");
        modalGraph.innerHTML = "";
        //console.log(elementToRender);
    } else {
        console.log("No está TerriaMapNotification")
        elementToRender = document.getElementsByClassName("tjs-feature-info-section__renderOne")[0];
        fromJS = false;
    }
    // let renderOneDiv = document.getElementsByClassName("tjs-feature-info-section__renderOne")[0];


    const elements = elementToRender.querySelectorAll("*");
    elements.forEach(element => {
        // Verificar si el elemento es un <a> y si su texto coincide con nombreSensor
        if (element.tagName === 'A' && element.textContent === tab) {
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }
    });
    let newDiv = "";
    if (fromJS) {
        modalGraph.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando gráfico...</div></div>`;
    } else {
        newDiv = document.getElementById("myNewDiv");

        if (!newDiv) {
            // Si newDiv no existe, crear uno nuevo
            newDiv = document.createElement("div");
            newDiv.id = "myNewDiv";
            newDiv.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando gráfico...</div></div>`;
            //   elementToRender.appendChild(newDiv);

        } else {
            // Si newDiv ya existe, borra su contenido
            newDiv.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando gráfico...</div></div>`;
        }
    }


    const url = `https://localhost:7242/api/GenerateLogBookController`;
    const data = JSON.stringify({ idInstrumento });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const responseData = await respuesta.text();
    if (fromJS) {
        modalGraph.innerHTML = responseData;
    } else {
        newDiv.innerHTML = responseData;
    }



}

async function SaveLogBook(_idInstrumento) {
    console.log("Se presionó el botón, idInstrumento: " + _idInstrumento);

    // Obtener el valor seleccionado del elemento <select>
    var selectSensorLogBook = document.getElementById("selectSensorLogBook");
    var selectedOption = selectSensorLogBook.options[selectSensorLogBook.selectedIndex].value;

    // Obtener el valor del <input>
    var inputLogBook = document.getElementById("inputLogBook").value;

    // Mostrar los valores en la consola
    console.log("Valor seleccionado del select: " + selectedOption);
    console.log("Valor del input: " + inputLogBook);

    // Crear una instancia del objeto Date
    var fechaActual = new Date();

    // Obtener los componentes de la fecha y la hora
    var year = fechaActual.getFullYear();
    var month = ('0' + (fechaActual.getMonth() + 1)).slice(-2); // Sumar 1 al mes porque en JavaScript los meses van de 0 a 11
    var day = ('0' + fechaActual.getDate()).slice(-2);
    var hour = ('0' + fechaActual.getHours()).slice(-2);
    var minutes = ('0' + fechaActual.getMinutes()).slice(-2);
    var seconds = ('0' + fechaActual.getSeconds()).slice(-2);

    // Formatear la fecha y la hora como una cadena
    var fechaHoraFormateada = year + '-' + month + '-' + day + ' ' + hour + ':' + minutes + ':' + seconds;

    // Mostrar la fecha y hora formateadas


    var DateTimeString = fechaHoraFormateada;
    var SaveNote = true;
    var IdSensor = parseInt(selectedOption);
    var Note = inputLogBook;
    var IdInstrumento = parseInt(_idInstrumento);
    console.log("Fecha y hora actual: " + DateTimeString);

    const url = `https://localhost:7242/api/GenerateLogBookController`;
    //var DateTime = fechaHoraFormateada;
    const data = JSON.stringify({ IdInstrumento, SaveNote, IdSensor, Note, DateTimeString });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    if (respuesta.ok) {
        let terriaMapNotification = document.getElementById("TerriaMapNotification");
        let fromJS = true;
        let modalGraph = "";
        let newDiv = "";
        if (terriaMapNotification) {
            modalGraph = document.getElementById("ModalGraph");
            modalGraph.innerHTML = "";
        } else {
            fromJS = false;
        }

        if (fromJS) {
            modalGraph.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando gráfico...</div></div>`;

        } else {
            newDiv = document.getElementById("myNewDiv");
            newDiv.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando gráfico...</div></div>`;

        }

        const url = `https://localhost:7242/api/GenerateLogBookController`;
        const data = JSON.stringify({ IdInstrumento });
        const respuesta = await fetch(url, {
            method: 'POST',
            body: data,
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const responseData = await respuesta.text();
        if (fromJS) {
            modalGraph.innerHTML = responseData;
        } else {
            newDiv.innerHTML = responseData;
        }


    }


    // const responseData = await respuesta.text();

    //  newDiv.innerHTML = responseData;



}










