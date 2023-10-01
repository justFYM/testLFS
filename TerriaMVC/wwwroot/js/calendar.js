

function eliminarEspacios(cadena) {
    return cadena.replace(/\s+/g, "");
}
function calendar(fechas, eventos, idInstrumento, idSensor, nombreSensores) {
    console.log("idInstrumento: " + idInstrumento)
    console.log("nombreSensores: " + nombreSensores)
    console.log("idSensor: " + idSensor)
    //idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect
    var fechasBd = fechas.split(",");

    // Crear un nuevo array para almacenar las letras individualmente
    var fechasArray = [];

    // Iterar a través del array de letras y agregarlas al nuevo array
    for (var i = 0; i < fechasBd.length; i++) {
        fechasArray.push(fechasBd[i]);
    }
    var eventosBd = eventos.split(",");

    // Crear un nuevo array para almacenar las letras individualmente
    var eventosArray = [];

    // Iterar a través del array de letras y agregarlas al nuevo array
    for (var i = 0; i < eventosBd.length; i++) {
        eventosArray.push(eventosBd[i]);
        console.log(eventosBd[i])
    }
    var selectElement = document.getElementById("selectFechas");
    //var chart = document.getElementById("container");
    selectElement.addEventListener("change", function () {
        const nombreEvento = selectElement.options[selectElement.selectedIndex].textContent;
        if (selectElement.selectedIndex !== 0) {
            var datepickerElement = document.getElementById('datepicker'); // Obtiene el valor de la fecha
            var selectedDate = datepickerElement.value;
            console.log(selectedDate);
            var eventos = nombreEvento.split("-");
            var selectedDateToQuery = datepickerElement.value.replace(/\//g, "%2F");
            var fechaCalendario = selectedDateToQuery + "%20" + eliminarEspacios(eventos[0]) + "-" + selectedDateToQuery +"%20"+eliminarEspacios(eventos[1]);
           console.log(fechaCalendario);
            //idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect, fechaCalendario
            reDrawGraph(idInstrumento, 1, false, '', nombreSensores, idSensor, true, false, fechaCalendario);
        }
    });



    var enabledDates = "";
    var datePickerPromise = $.Deferred();
    function onOpen() {
        setTimeout(function () {
            const buttonPane = document.querySelector('.ui-datepicker-buttonpane.ui-widget-content');
            if (buttonPane) {
                const buttons = buttonPane.querySelectorAll('button');
                $(".ui-datepicker-current").prop("disabled", false);

                buttons.forEach((button) => {
                    if (button.textContent === "Hoy" && !button.onclick) {
                        button.onclick = function () {
                            const currentDate = new Date();
                            $("#datepicker").datepicker('setDate', currentDate);
                            console.log("Una vez.");
                            onOpen(); // Llamada a la función onOpen al abrir el selector de fechas
                        };
                    }
                });
            } else {
                console.log("El elemento no se encontró.");
            }
        }, 50);
    }


    // Agregar evento al botón "Hoy"
    $(".ui-datepicker-current").on("click", function () {
        const currentDate = new Date();
        $("#datepicker").datepicker('setDate', currentDate);
        onOpen(); // Llamada a la función onOpen al abrir el selector de fechas
    });

    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        currentText: 'Hoy',
        dateFormat: 'yy/mm/dd',
        yearRange: '1900:c',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],

        beforeShow: function (input, inst) {
            datePickerPromise.resolve();
            onOpen(); // Llamada a la función onOpen al abrir el selector de fechas
        },
        onChangeMonthYear: function (year, month, inst) {
            onOpen(); // Llamada a la función onOpen al cambiar de mes y año
        },
        beforeShowDay: function (date) {
            enabledDates = fechasArray;
            var formattedDate = $.datepicker.formatDate("yy-mm-dd", date);

            if (enabledDates.includes(formattedDate)) {
                // console.log("Día cliqueado.")
                return [true, "ui-state-enabled"];
            } else {
                return [false, ""];
            }
        },
        onSelect: function (dateText, inst) {

            var selectedDay = inst.selectedDay.toString().padStart(2, '0');
            var selectedMonth = (inst.selectedMonth + 1).toString().padStart(2, '0');
            var selectedYear = inst.selectedYear;
            var selectedDate = selectedYear + '-' + selectedMonth + '-' + selectedDay;
            //console.log("Día seleccionado:", selectedDate);




            //idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect, fechaCalendario
            reDrawSelect(idInstrumento, 1, false, '', nombreSensores, idSensor, false, true, selectedDate);




            if (inst.selectedDay === new Date().getDate() &&
                inst.selectedMonth === new Date().getMonth() &&
                inst.selectedYear === new Date().getFullYear()) {
                $(this).datepicker('setDate', new Date());
                $("#Hola").prop("disabled", true); // Habilitar el botón "Hola"

            }
        },

        onClose: function (dateText, inst) {
            $(this).blur(); // Remover el foco del input
            return false;
        },
    });
}

function generateSelect(eventos) {
    // Separar el parámetro en un array de eventos
    const eventosArray = eventos.split(',');

    // Obtener el elemento select
    const selectFechas = document.getElementById("selectFechas");

    // Limpiar opciones anteriores (si las hay)
    selectFechas.innerHTML = '';

    // Crear la opción de mensaje deshabilitada
    const optionMensaje = document.createElement('option');
    optionMensaje.value = "mensaje";
    optionMensaje.disabled = true;
    optionMensaje.textContent = `${eventosArray.length} eventos disponibles`;
    selectFechas.appendChild(optionMensaje);

    // Agregar cada evento como opción al select
    eventosArray.forEach(evento => {
        const optionEvento = document.createElement('option');
        optionEvento.value = evento;
        optionEvento.textContent = evento;
        selectFechas.appendChild(optionEvento);
    });

    // Desactivar la opción de mensaje
    optionMensaje.disabled = true;

    // Seleccionar el último evento
    selectFechas.value = eventosArray[eventosArray.length - 1];

    // Agregar event listener para el cambio de opción

}

async function reDrawSelect(idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect, fechaCalendario) {
    console.log(idInstrumento);
    console.log(idSensor);
    console.log(fechaCalendario);
    console.log(nombreSensor);
   
    // Obtener el div de la vista 1
    var divTest = document.getElementById('divTest');

    // Eliminar el select existente
    var selectToDelete = divTest.querySelector('select');
    if (selectToDelete) {
        selectToDelete.remove();
    }

    // Crear y agregar el spinner
    var spinner = document.createElement('span');
    spinner.className = 'mt-2 spinner-border text-success';
    divTest.appendChild(spinner);

    const url = `https://localhost:7242/api/GenerateGraphController`;
    const data = JSON.stringify({ idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect, fechaCalendario });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const responseData = await respuesta.text();

    if (respuesta.ok) {
        spinner.remove();

        // Crear y agregar el nuevo select
        
        divTest.innerHTML = responseData;
        var newSelect = divTest.querySelector('select');
       
        newSelect.addEventListener("change", async function () {
            const nombreEvento = newSelect.options[newSelect.selectedIndex].textContent;
            if (newSelect.selectedIndex !== 0) {
                var datepickerElement = document.getElementById('datepicker'); // Obtiene el valor de la fecha
                var selectedDate = datepickerElement.value;
                console.log(selectedDate);
                var eventos = nombreEvento.split("-");
                var selectedDateToQuery = datepickerElement.value.replace(/\//g, "%2F");
                var fechaCalendario = selectedDateToQuery + "%20" + eliminarEspacios(eventos[0]) + "-" + selectedDateToQuery + "%20" + eliminarEspacios(eventos[1]);
                console.log(fechaCalendario);
                //reDrawGraph(idInstrumento, 1, false, '', nombreSensor, idSensor, true, false, fechaCalendario);
                console.log("Agregar ReDrawGraph acá");
                console.log(idInstrumento);
                console.log(1);
                console.log(false);
                console.log(nombreSensor);
                console.log(idSensor);
                console.log(true)
                console.log(false)
                console.log(fechaCalendario)
                var reDrawGraph = true;
                var reDrawSelect = false;
                var mainDiv = document.getElementById('mainDiv');
                var containerDiv = mainDiv.querySelector('div#container');
                containerDiv.innerHTML = `<div class="d-flex justify-content-center mt-20 mb-4"><span class="spinner-border text-success"></span><div style="color:#212529;"class="mt-2 mx-2">Cargando gráfico...</div></div>`
                const urlReDrawGraph = `https://localhost:7242/api/GenerateGraphController`;
                const dataReDrawGraph = JSON.stringify({ idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect, fechaCalendario });
                const respuestaReDrawGraph = await fetch(urlReDrawGraph, {
                    method: 'POST',
                    body: dataReDrawGraph,
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
                const responseDataReDrawGraph = await respuestaReDrawGraph.text();

                if (respuestaReDrawGraph.ok) {
                    //spinner.remove();
                    containerDiv.innerHTML = `${responseDataReDrawGraph}`
                    var fromTerriaMap = document.getElementById("myNewDiv");
                    var fromJS = document.getElementById("ModalGraph");
                    if (fromJS && fromTerriaMap) {
                        var executeHighChartsScript = fromJS.querySelector('script#executeHighCharts');
                        var scriptContent = executeHighChartsScript.textContent;
                        eval(scriptContent);
                    } else if (fromJS) {
                        var executeHighChartsScript = fromJS.querySelector('script#executeHighCharts');
                        var scriptContent = executeHighChartsScript.textContent;
                        eval(scriptContent);
                    }
                    else {
                        var executeHighChartsScript = myNewDiv.querySelector('script#executeHighCharts');
                        var scriptContent = executeHighChartsScript.textContent;
                        eval(scriptContent);
                    }
                 
                }


            }
        });
  
        /*
        newSelect.addEventListener("change", function () {
            const nombreEvento = newSelect.options[newSelect.selectedIndex].textContent;
            if (newSelect.selectedIndex !== 0) {
                console.log(nombreEvento);
                var mainDiv = document.getElementById('mainDiv');
                var myNewDiv = document.getElementById('myNewDiv');
                var executeHighChartsScript = myNewDiv.querySelector('script#executeHighCharts');
                if (executeHighChartsScript) {
                    var scriptContent = executeHighChartsScript.textContent;
                    console.log('Contenido del script "executeHighCharts":', scriptContent);
                } else {
                    console.log('No se encontró el script "executeHighCharts" dentro de "myNewDiv".');
                }
                var containerDiv = mainDiv.querySelector('div#container');
                if (containerDiv) {
                    containerDiv.remove();
                    // Crea un nuevo elemento <div> con id "container"
                    var newContainerDiv = document.createElement('div');
                    newContainerDiv.id = 'container';
                    mainDiv.appendChild(newContainerDiv);
                    setTimeout(function () {
                        eval(scriptContent);
                    }, 2000);

                } else {
                    console.log('No se encontró el div con id "container" dentro de "myNewDiv".');
                }
            }
        });
        */

    }
}
async function reDrawGraph(idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect, fechaCalendario) {
    console.log(idInstrumento);
    console.log(idSensor);
    console.log(fechaCalendario);
    console.log(nombreSensor);

    var mainDiv = document.getElementById('mainDiv');
    var containerDiv = mainDiv.querySelector('div#container');
    containerDiv.innerHTML = `<div class="d-flex justify-content-center mt-20 mb-4"><span class="spinner-border text-success"></span><div style="color:#212529;"class="mt-2 mx-2">Cargando gráfico...</div></div>`

    const url = `https://localhost:7242/api/GenerateGraphController`;
    const data = JSON.stringify({ idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph, reDrawSelect, fechaCalendario });
    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const responseData = await respuesta.text();

    if (respuesta.ok) {
        //spinner.remove();
        containerDiv.innerHTML = `${responseData}`
        var fromTerriaMap = document.getElementById("myNewDiv");
        var fromJS = document.getElementById("ModalGraph");
        if (fromJS && fromTerriaMap) {
            var executeHighChartsScript = fromJS.querySelector('script#executeHighCharts');
            var scriptContent = executeHighChartsScript.textContent;
            eval(scriptContent);
        } else if (fromJS) {
            var executeHighChartsScript = fromJS.querySelector('script#executeHighCharts');
            var scriptContent = executeHighChartsScript.textContent;
            eval(scriptContent);
        }
        else {
            var executeHighChartsScript = myNewDiv.querySelector('script#executeHighCharts');
            var scriptContent = executeHighChartsScript.textContent;
            eval(scriptContent);
        }
    }

}