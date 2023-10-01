// Definición del objeto en JavaScript
class GenerateButtonsDTO {
    constructor(idInstrumento, nombreTipoInstrumento) {
        this.IdInstrumento = idInstrumento;
        if (nombreTipoInstrumento == "Trigger") {
            this.IdTypeButtons = 0;
        } else if (nombreTipoInstrumento == "Clinoextensometro") {
            this.IdTypeButtons = 0;
        }
        else if (nombreTipoInstrumento == "GNSS") {
            this.IdTypeButtons = 0;
        }
        else if (nombreTipoInstrumento == "Piezometro") {
            this.IdTypeButtons = 1;
        }
        else if (nombreTipoInstrumento == "Prisma") {
            this.IdTypeButtons = 0;
        }
        else if (nombreTipoInstrumento == "Sensor_Humedad") {
            this.IdTypeButtons = 1;
        }
        else if (nombreTipoInstrumento == "Radar01") {
            this.IdTypeButtons = 0;
        }
        else if (nombreTipoInstrumento == "InSAR") {
            this.IdTypeButtons = 0;
        }
    }
    async generateButtons() {
        const url = `https://localhost:7242/api/generateButtons`;
        const data = JSON.stringify({
            idInstrumento: this.IdInstrumento,
            idTypeButtons: this.IdTypeButtons
        });
       
        try {
            const respuesta = await fetch(url, {
                method: 'POST',
                body: data,
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            const responseData = await respuesta.text();

            return responseData; // Retorna la respuesta
        } catch (error) {
            // Manejar errores si la solicitud falla
            console.error("Error en la solicitud HTTP:", error);
            return null; // Retorna null en caso de error
        }
    
    }

    getIdInstrumento() {
        return this.IdInstrumento;
    }

    getIdTypeButtons() {
        return this.IdTypeButtons;
    }



}

