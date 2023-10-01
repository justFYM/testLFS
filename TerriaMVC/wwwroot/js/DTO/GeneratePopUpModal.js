class GeneratePopUpModal {
    constructor(id,className,width,borderRadius,opacity) {
        this.Id = id;
        this.ClassName = className,
        this.Width = width,
        this.BorderRadius = borderRadius,
        this.Opacity = opacity
    }

    async generateModal(nombreTipoInstrumento, nombreInstrumento, buttons, graph) {
        const popupDiv = document.createElement("div");
        popupDiv.id = this.Id;
        popupDiv.className = this.ClassName;
        popupDiv.style.width = this.Width;
        popupDiv.style.borderRadius = this.BorderRadius;
        popupDiv.innerHTML = `
    <div class="modal-dialog draggable">
        <div class="modal-content" style="border-radius:${this.BorderRadius}; opacity:${this.Opacity}; width: ${this.Width} !important">
            <div class="modal-header" style="padding: 10px 20px; border-radius: 0;">
                <h5 class="modal-title" id="exampleModalLabel">${nombreTipoInstrumento} - ${nombreInstrumento}</h5>
                <button style="background-color: #3f4854" type="button" class="close" data-dismiss="modal"><h2 style="color:white">X</h2></button>
            </div>

            <div class="modal-body" style="padding: 10px 20px;">
                <!-- Contenido de la modal -->
                <div id="ModalButtons">
                ${buttons}
                </div>
                <div id="ModalGraph">
                  ${graph}
                </div>
            </div>
            <div class="modal-footer" style="padding: 20px 25px; border-radius: 0; opacity:0.95">
                
            </div>
        </div>
    </div>
`;
       
        return popupDiv;
    }


    async generateAOIModal(modalBody, container) {
        
        const popupAOIDiv = document.createElement("div");
        popupAOIDiv.id = this.Id;
        popupAOIDiv.className = this.ClassName;
        popupAOIDiv.style.width = this.Width;
        popupAOIDiv.style.borderRadius = this.BorderRadius;
        popupAOIDiv.innerHTML = `
    <div class="modal-dialog draggable">
        <div class="modal-content" style="border-radius:${this.BorderRadius}; opacity:${this.Opacity}; width: ${this.Width} !important">
            <div class="modal-header" style="padding: 10px 20px; border-radius: 0;">
                <h5 class="modal-title" id="exampleModalLabel">Generate AOI</h5>
                <button style="background-color: #3f4854" type="button" class="close" data-dismiss="modal"><h2 style="color:white">X</h2></button>
            </div>
             ${modalBody}
            <div class="modal-footer" style="padding: 20px 25px; border-radius: 0; opacity:0.95">
                
            </div>
        </div>
    </div>
`;
        const element = document.querySelector(container);
        //const element = document.querySelector(".tjs-map-interaction-window__content");
        // Verifica que se haya encontrado el elemento
        console.log("ContainerAOI: " + container)
        if (element) {
            // Agrega la clase "top-element" al elemento
            element.classList.add("top-element"); //Para que la modal esté en la "capa" superior.
        }
        element.appendChild(popupAOIDiv);
        return popupAOIDiv;
    }

    async showModal() {
        $(`#${this.Id}`).on('shown.bs.modal', async function () {
            // Eliminar la modal-backdrop una vez que se ha abierto la modal
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open');
            // Hacer que la modal sea arrastrable usando jQuery UI Draggable
            $('.modal-content').draggable({
                handle: ".modal-header" // Puedes cambiar el selector según tus necesidades
                /*start: function () {
                    const containerPopUp = document.querySelector(this.ClassName);
                    // Verifica que se haya encontrado el elemento
                    if (containerPopUp) {
                        containerPopUp.classList.add(this.ClassName);
                    }
                }
                */
            });

           
            const terriaMapNotification = document.getElementById("TerriaMapNotification");
            if (terriaMapNotification) {
                $('#TerriaMapNotification').css('overflow-y', 'hidden');
            }
        });
        $(`#${this.Id}`).on('hidden.bs.modal', async function () {
            $(this).remove();
        });
        
        $(`#${this.Id}`).modal('show');
     
    }


    async showModalAOI() {
        $(`#${this.Id}`).on('shown.bs.modal', async function () {
            // Eliminar la modal-backdrop una vez que se ha abierto la modal
            $('.modal-backdrop').remove();
            $('body').removeClass('modal-open');
            // Hacer que la modal sea arrastrable usando jQuery UI Draggable
            $('.modal-content').draggable({
                handle: ".modal-header", // Puedes cambiar el selector según tus necesidades
                start: function () {
                    const containerPopUpAOI = document.querySelector(".tjs-standard-user-interface__featureInfo");
                    // Verifica que se haya encontrado el elemento
                    if (containerPopUpAOI) {
                        containerPopUpAOI.classList.add("top-element");
                        console.log("Se agregó top-element a containerPopUpAOI");
                    } else {
                        console.log("Ya se agregó top-element a containerPopUpAOI")
                    }
                }
            });


            const terriaMapNotification = document.getElementById("TerriaMapNotification");
            if (terriaMapNotification) {
                $('#TerriaMapNotification').css('overflow-y', 'hidden');
            }
        });
        $(`#${this.Id}`).on('hidden.bs.modal', async function () {
            const containerPopUpAOI = document.querySelector(".tjs-standard-user-interface__featureInfo");
            // Verifica que se haya encontrado el elemento
            if (containerPopUpAOI) {
                containerPopUpAOI.classList.remove("top-element");
            }
            $(this).remove();
        });

        $(`#${this.Id}`).modal('show');

    }

}
