class GenerateContainersDTO {
    constructor() {
       // this.ContainerNotifications = '#TerriaMapNotifications';
        this.ModalPopUpNotifications = '#popup';
        this.PopUpNotifications = '#TerriaMapNotification'
        this.TerriaMapPopUp = '.tjs-feature-info-section__renderOne';
        this.AOIContainer = '.MapInteractionWindow__MapInteractionWindowWrapper-sc-11tk3s1-0.tjs-map-interaction-window__window.tjs-map-interaction-window__isActive';
        this.AOIPopUpContainer = '.tjs-standard-user-interface__featureInfo'; //Este contenedor estará si o si, es en dónde se renderizan los popup de React.
    }
    
    async getPopUpNotifications() {
        return this.ModalPopUpNotifications;
    }
    async TerriaMapPopUp() {
        return this.TerriaMapPopUp;
    }
    async getTotalContainers() {
        return [
            this.ModalPopUpNotifications,
            this.PopUpNotifications,
            this.TerriaMapPopUp,
            this.AOIContainer
        ];
    }

    async findGraphContainer() {
        const containers = await this.getTotalContainers();
        const activeContainers = await findActualContainers(containers);
        let response = '';
        activeContainers.forEach(iterador => {
            if (iterador === this.ModalPopUpNotifications) {
                response = this.ModalPopUpNotifications;
            } else if (!response) {
                response = iterador;
            }
        });
    
        if (response == this.TerriaMapPopUp) {
            response = document.getElementById('myNewDiv');
            if (response == null) {
                await insertNewElementByClassName(this.TerriaMapPopUp.substring(1), "myNewDiv");
                response = '#myNewDiv';
            }
        } else if (response == this.ModalPopUpNotifications) {
            response = document.getElementById('ModalGraph');
        }
     

        return response;
    }

    async findButtonsContainer() {
        const containers = await this.getTotalContainers();
        const activeContainers = await findActualContainers(containers);
        let response = '';
        activeContainers.forEach(iterador => {
            if (iterador === this.ModalPopUpNotifications) {
                response = this.ModalPopUpNotifications;
            } else if (!response) {
                response = iterador;
            }
        });

        if (response == this.TerriaMapPopUp) {
            response = document.getElementById('myNewDivButtons');
            if (response == null) {
                await insertNewElementByClassName(this.TerriaMapPopUp.substring(1), "myNewDivButtons");
                response = '#myNewDivButtons';
            }
        } else if (response == this.ModalPopUpNotifications) {
            response = document.getElementById('ModalGraph');
        }


        return response;
    }

    async findContainer() {
        const containers = await this.getTotalContainers();
        const activeContainers = await findActualContainers(containers);
        let response = '';
        let priorityOne = false;
        let priorityTwo = false;

        activeContainers.forEach(iterador => {
            if (iterador === this.ModalPopUpNotifications && !priorityOne) {
                priorityOne = true;
                response = this.ModalPopUpNotifications;
            } else if (iterador === this.PopUpNotifications && !priorityTwo) {
                priorityTwo = true;
                response = iterador;
            } else if (!response) {
                response = iterador;
            }
        });
        if (priorityTwo == true) {
            const element = document.querySelector(this.AOIPopUpContainer);
            if (element) {
                element.classList.remove("top-element"); //Para que la modal esté en la "capa" superior.
            }
        }
        console.log("PriorityOne: #popup: " + priorityOne);
        console.log("PriorityTwo: #TerriaMapNotification: " + priorityTwo);
        console.log("El contenedor actual es: " + response);

        return response;
    }


    async findAOIContainer() {
        const containers = await this.getTotalContainers();
        const activeContainers = await findActualContainers(containers);
        let response = '';
        activeContainers.forEach(iterador => {
            if (iterador === this.ModalPopUpNotifications) {
                response = this.ModalPopUpNotifications;
            } else {
                response = this.AOIPopUpContainer;
            }
        });
        console.log("El contenedor actual es: " + response)
        return response;
    }




}
