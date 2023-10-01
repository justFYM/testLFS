

function calendar() {
    var enabledDates = "";
    var datePickerPromise = $.Deferred();
    document.addEventListener("DOMContentLoaded", function () {
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
            dateFormat: 'dd/mm/yy',
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
                enabledDates = ["10-08-2023", "11-08-2023", "12-08-2023"];
                var formattedDate = $.datepicker.formatDate("dd-mm-yy", date);

                if (enabledDates.includes(formattedDate)) {
                    // console.log("Día cliqueado.")
                    return [true, "ui-state-enabled"];
                } else {
                    return [false, ""];
                }
            },
            onSelect: function (dateText, inst) {
                var selectedDay = inst.selectedDay;
                var selectedMonth = (inst.selectedMonth + 1).toString().padStart(2, '0');
                var selectedYear = inst.selectedYear;
                var selectedDate = selectedDay + '-' + selectedMonth + '-' + selectedYear;
                console.log("Día seleccionado:", selectedDate);


                var selectElement = document.getElementById("selectFechas");
                while (selectElement.firstChild) {
                    selectElement.removeChild(selectElement.firstChild);
                }
                if (selectedDate == enabledDates[0]) {
                    selectElement.disabled = false;
                    console.log("1");
                    var option1 = document.createElement("option");
                    option1.value = "opcion1";
                    option1.text = "Opción 1";
                    selectElement.appendChild(option1);


                    var placeholderOption = document.createElement("option");
                    placeholderOption.disabled = true;
                    placeholderOption.selected = true;
                    placeholderOption.text = `¡${selectElement.options.length} eventos encontrados!`
                    selectElement.insertBefore(placeholderOption, selectElement.firstChild);


                } else
                    if (selectedDate == enabledDates[1]) {
                        selectElement.disabled = false;
                        console.log("2");

                        var option1 = document.createElement("option");
                        option1.value = "opcion1";
                        option1.text = "Opción 1";
                        selectElement.appendChild(option1);

                        var option2 = document.createElement("option");
                        option2.value = "opcion2";
                        option2.text = "Opción 2";
                        selectElement.appendChild(option2);
                        selectElement.addEventListener("change", updateSeriesData);


                        var placeholderOption = document.createElement("option");
                        placeholderOption.disabled = true;
                        placeholderOption.selected = true;
                        placeholderOption.text = `¡${selectElement.options.length} eventos encontrados!`
                        selectElement.insertBefore(placeholderOption, selectElement.firstChild);
                    } else
                        if (selectedDate == enabledDates[2]) {
                            selectElement.disabled = false;
                            // Agregar tres opciones al select
                            var option1 = document.createElement("option");
                            option1.value = "opcion1";
                            option1.text = "Opción 1";
                            selectElement.appendChild(option1);

                            var option2 = document.createElement("option");
                            option2.value = "opcion2";
                            option2.text = "Opción 2";
                            selectElement.appendChild(option2);

                            var option3 = document.createElement("option");
                            option3.value = "opcion3";
                            option3.text = "Opción 3";
                            selectElement.appendChild(option3);

                            selectElement.addEventListener("change", updateSeriesData);

                            var placeholderOption = document.createElement("option");
                            placeholderOption.disabled = true;
                            placeholderOption.selected = true;
                            placeholderOption.text = `¡${selectElement.options.length} eventos encontrados!`
                            selectElement.insertBefore(placeholderOption, selectElement.firstChild);

                        }


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
    })
}