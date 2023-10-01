
/*
async function callClinoextensometroGraphController(idInstrumento,nombreVariable, idTypeGraph,tab) {
    
    await new Promise(resolve => setTimeout(resolve, 100));
    let renderOneDiv = document.getElementsByClassName("tjs-feature-info-section__renderOne")[0];

    const elements = renderOneDiv.querySelectorAll("*");
    elements.forEach(element => {
        // Verificar si el elemento es un <a> y si su texto coincide con nombreSensor
        if (element.tagName === 'A' && element.textContent === tab) {
            // Realizar la acción que necesitas con el elemento <a>
            console.log("Elemento <a> encontrado:", element);
            // Por ejemplo, agregar una clase para resaltarlo
            element.classList.add("active");
        } else {
            element.classList.remove("active");
        }
    });

    // Verifica si newDiv ya existe
    let newDiv = document.getElementById("myNewDiv");
    if (!newDiv) {
        // Si newDiv no existe, crear uno nuevo
        newDiv = document.createElement("div");
        newDiv.id = "myNewDiv"; // Asigna una ID para futuras verificaciones
        newDiv.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando gráfico...</div></div>`;
        renderOneDiv.appendChild(newDiv);
    } else {
        // Si newDiv ya existe, borra su contenido
        newDiv.innerHTML = `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">Cargando gráfico...</div></div>`;
    }
    const url = `https://localhost:7242/api/GenerateGraphController`;
   
    const idServicio = parseInt(2);


    const data = JSON.stringify({ idInstrumento, nombreVariable, idTypeGraph });

    const respuesta = await fetch(url, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const responseData = await respuesta.text();

    // Agrega el contenido HTML al newDiv
    if (respuesta.ok) {
        newDiv.innerHTML = responseData;
    }


    // Ejecuta el script de Highcharts para inicializar el gráfico
    let scriptElements = newDiv.querySelectorAll("script");
    scriptElements.forEach(scriptElement => {
        console.log(scriptElement);
        eval(scriptElement.textContent);
    });
}
*/