class GenerateNotificationDTO {
    constructor(idElement, width) {
    
    }

    async generateNotifications() {
        const url = `https://localhost:7242/api/GenerateNotificationsController`;
        try {
            const respuesta = await fetch(url, {
                method: 'GET',
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

}

