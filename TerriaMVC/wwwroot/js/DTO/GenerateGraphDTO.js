class GenerateGraphDTO {
    constructor(idInstrumento) {
        this.IdInstrumento = idInstrumento;
        this.HourToAverage = 24;
        this.AverageRequired = false;
        this.IdSensor = -1;
        this.ReDrawGraph = false;
        this.NombreSensor = ""
        this.NombreVariable = "";
        this.IdTypeGraph = 0;
    }
    async generateFirstGraph(nombreTipoInstrumento) {
        console.log("generateFirstGraph: " + nombreTipoInstrumento);
        console.log("generateFirstGraph: " + this.IdInstrumento);
        if (nombreTipoInstrumento == "Trigger") {
            this.IdTypeGraph = 1;
            this.NombreSensor = "Longitudinal-Transversal-Vertical"
        } else if (nombreTipoInstrumento == "Clinoextensometro") {
            this.IdTypeGraph = 1;
            this.NombreVariable = "tasa_mensual_de_asentamiento";
        }
        else if (nombreTipoInstrumento == "GNSS") {
            this.IdTypeGraph = 1;
        }
        else if (nombreTipoInstrumento == "Piezometro") {
            this.IdTypeGraph = 1;
        }
        else if (nombreTipoInstrumento == "Prisma") {
            this.IdTypeGraph = 0;
        }
        else if (nombreTipoInstrumento == "Sensor_Humedad") {
            this.IdTypeGraph = 1;
        }
        else if (nombreTipoInstrumento == "Radar01") {
            this.IdTypeGraph = 0;
            this.NombreSensor = "Velocidad%20en";
        }
        else if (nombreTipoInstrumento == "InSAR") {
            this.IdTypeGraph = 0;
            this.NombreSensor = "desp abs: 78794785"
        }

        const url = `https://localhost:7242/api/GenerateGraphController`;
        const data = JSON.stringify({
            idInstrumento: this.IdInstrumento,
            idTypeGraph: this.IdTypeGraph,
            averageRequired: this.AverageRequired,
            nombreVariable: this.NombreVariable,
            nombreSensor: this.NombreSensor,
            idSensor: this.IdSensor,
            reDrawGraph: this.ReDrawGraph,
            hourToAverage: this.HourToAverage
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

            return responseData;
        } catch (error) {
            console.error("Error en la solicitud HTTP:", error);
            return null; 
        }
        

    }
    async notificationTypeGraph(nombreTipoInstrumento) {
        if (nombreTipoInstrumento == "Trigger") {
            this.IdTypeGraph = 1;
        } else if (nombreTipoInstrumento == "Clinoextensometro") {
            this.IdTypeGraph = 1;
        }
        else if (nombreTipoInstrumento == "GNSS") {
            this.IdTypeGraph = 1;
        }
        else if (nombreTipoInstrumento == "Piezometro") {
            this.IdTypeGraph = 0;
        }
        else if (nombreTipoInstrumento == "Prisma") {
            this.IdTypeGraph = 0;
        }
        else if (nombreTipoInstrumento == "Sensor_Humedad") {
            this.IdTypeGraph = 0;
        }
        else if (nombreTipoInstrumento == "Radar01") {
            this.IdTypeGraph = 0;
        }
        else if (nombreTipoInstrumento == "InSAR") {
            this.IdTypeGraph = 0;
        }
    }

    async generateGraph(idInstrumento, idTypeGraph, averageRequired, nombreVariable, nombreSensor, idSensor, reDrawGraph) {
        this.IdInstrumento = idInstrumento;
        this.IdTypeGraph = idTypeGraph;
        this.AverageRequired = averageRequired;
        this.IdSensor = idSensor;
        this.ReDrawGraph = reDrawGraph;
        this.NombreSensor = nombreSensor,
        this.NombreVariable = nombreVariable;
        /*
        console.log(this.IdInstrumento)
        console.log(this.IdTypeGraph)
        console.log(this.AverageRequired)
        console.log(this.IdSensor)
        console.log(this.ReDrawGraph)
        console.log(this.NombreSensor)
        console.log(this.NombreVariable)
        */
        const url = `https://localhost:7242/api/GenerateGraphController`;
        const data = JSON.stringify({
            idInstrumento: this.IdInstrumento,
            idTypeGraph: this.IdTypeGraph,
            idSensor: this.IdSensor,
            averageRequired: this.AverageRequired,
            nombreVariable: this.NombreVariable,
            nombreSensor: this.NombreSensor,
            reDrawGraph: this.ReDrawGraph,
            hourToAverage: this.HourToAverage
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
    async generateGraphNotification(nombreTipoInstrumento, idSensor, nombreSensor) {
        await this.notificationTypeGraph(nombreTipoInstrumento);
        this.IdSensor = idSensor;
        this.NombreSensor = nombreSensor;

        /*
        console.log(this.IdInstrumento)
        console.log(this.IdTypeGraph)
        console.log(this.AverageRequired)
        console.log(this.IdSensor)
        console.log(this.ReDrawGraph)
        console.log(this.NombreSensor)
        console.log(this.NombreVariable)
        */
        const url = `https://localhost:7242/api/GenerateGraphController`;
        const data = JSON.stringify({
            idInstrumento: this.IdInstrumento,
            idTypeGraph: this.IdTypeGraph,
            idSensor: this.IdSensor,
            averageRequired: this.AverageRequired,
            nombreVariable: this.NombreVariable,
            nombreSensor: this.NombreSensor,
            reDrawGraph: this.ReDrawGraph,
            hourToAverage: this.HourToAverage
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
}

