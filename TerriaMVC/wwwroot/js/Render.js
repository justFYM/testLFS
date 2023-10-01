async function renderHTMLByClassOfElement(classElement, content) {
    await new Promise(resolve => setTimeout(resolve, 10));
    console.log(classElement)
    console.log(content)
    let htmlElement = document.getElementsByClassName(classElement)[0];
    htmlElement.innerHTML = content;
    return htmlElement;
}
async function renderHTMLByIdOfElement(idElement, content) {
    await new Promise(resolve => setTimeout(resolve, 10));
    let htmlElement = document.getElementById(idElement);
    htmlElement.innerHTML = content;
}

async function insertNewElementByClassName(classElement, element) {
    await new Promise(resolve => setTimeout(resolve, 10));
    let htmlElement = document.getElementsByClassName(classElement)[0];
    let newDiv = document.createElement("div");
    newDiv.id = element;
    htmlElement.appendChild(newDiv);
    return newDiv;
}

async function executeScriptByElementId(idElement) {
    let element = document.getElementById(idElement);
    let scriptElements = element.querySelectorAll("script");
    scriptElements.forEach(scriptElement => {
        eval(scriptElement.textContent);
    });
}

async function activeTabByClassName(classElement, nameTabToActive) {
    console.log(nameTabToActive)
    nameTabToActive.toString();
    var test = nameTabToActive.toString();
    let htmlElement = document.getElementsByClassName(classElement)[0];
    let liElements = htmlElement.getElementsByTagName("a");
    var isTab = false;
    var tab = "";
    var firstTab = "";
    for (let i = 0; i < liElements.length; i++) {
        if (liElements[i].innerText === test) {
            isTab = true;
            tab = liElements[i]
        } else {
            liElements[i].classList.remove("active");
        }
        firstTab = liElements[0];
    }
    if (isTab) {
        console.log("Se encontró: "+nameTabToActive);
        tab.classList.add("active");
    } else {
        console.log("No se encontró: " + nameTabToActive);
        console.log("Primera pestaña: " + firstTab.innerText);
        firstTab.classList.add("active");
    }
}

async function activeTabById(idElement, nameTabToActive) {
    console.log(nameTabToActive);
    nameTabToActive.toString();
    var test = nameTabToActive.toString();
    let htmlElement = document.getElementById(idElement);
    if (htmlElement) {
        let aElements = htmlElement.getElementsByTagName("a");
        var isTab = false;
        var tab = null;
        var firstTab = null;

        for (let i = 0; i < aElements.length; i++) {
            if (aElements[i].innerText === test) {
                isTab = true;
                tab = aElements[i];
            } else {
                aElements[i].classList.remove("active");
            }
            if (i === 0) {
                firstTab = aElements[i];
            }
        }

        if (isTab) {
            console.log("Se encontró: " + nameTabToActive);
            tab.classList.add("active");
        } else {
            console.log("No se encontró: " + nameTabToActive);
            if (firstTab) {
                console.log("Primera pestaña: " + firstTab.innerText);
                firstTab.classList.add("active");
            }
        }
    } else {
        console.log("No se encontró ningún elemento con el ID: " + idElement);
    }
}


async function findElementByClassName(classElement, idElement) {
    // Obtén la colección de elementos con la clase "classElement"
    var containerElements = document.getElementsByClassName(classElement);

    // Si solo esperas un elemento con esa clase, selecciona el primero (índice 0)
    var containerElement = containerElements[0];

    // Asegúrate de que containerElement no sea nulo antes de continuar
    if (containerElement) {
        // Busca el elemento con el ID "myNewDiv" dentro del contenedor
        var myNewDivElement = containerElement.querySelector(`#${idElement}`);
        //console.log(myNewDivElement);
        return myNewDivElement;
    } else {
        console.log("No se encontró ningún elemento con la clase especificada.");
        return null;
    }

}
async function findElementById(elementById, className) {
    // Obtén el elemento por su ID
    var containerElement = document.getElementById(elementById);

    if (containerElement) {
        // Busca el elemento con la clase especificada dentro de containerElement
        var elementWithClass = containerElement.querySelector("." + className);

        if (elementWithClass) {
            // Haz algo con el elemento encontrado
            console.log("Elemento encontrado:", elementWithClass);
            return elementWithClass;
        } else {
            console.log("No se encontró ningún elemento con la clase especificada.");
            return null;
        }
    } else {
        console.log("No se encontró ningún elemento con el ID especificado.");
    }
}
async function wait() {
    await new Promise(resolve => setTimeout(resolve, 10));
    console.log("La función se ha completado después del retraso de 10 milisegundos.");
}

async function modifyWidthByClassName(width, className) {
    $(`.${className}`).css('width', `${width}`);
}

async function modifyWidthByElementId(width, elementId) {
    $(`#${elementId}`).css('width', `${width}`);
}
async function removeElementByClassName(className) {
    var element = document.querySelector("."+className);
    if (element) {
        element.remove();
    }
}
/*
async function findActualContainers(selectors) {
    const elementsInDOM = [];

    // Recorre cada selector en el array
    selectors.forEach(selector => {
        if (selector.startsWith('#')) {
            const element = document.getElementById(selector.substring(1)); // Obtiene el elemento por ID
            if (element) {
                elementsInDOM.push(selector); 
            }
        } else if (selector.startsWith('.')) {
            const elements = document.getElementsByClassName(selector.substring(1)); // Obtiene elementos por clase

            // Verifica si se encontraron elementos y si la colección no está vacía
            if (elements.length > 0) {
                elementsInDOM.push(selector); // Agrega el selector al array de elementos encontrados
            }
        }
    });

    return elementsInDOM;
}
*/
async function findActualContainers(selectors) {
    const elementsInDOM = [];

    // Recorre cada selector en el array
    selectors.forEach(selector => {
        const elements = $(selector); // Utiliza jQuery para buscar elementos

        // Verifica si se encontraron elementos y si la colección no está vacía
        if (elements.length > 0) {
            elementsInDOM.push(selector); // Agrega el selector al array de elementos encontrados
        }
    });

    return elementsInDOM;
}


async function executeScript(container) {
    console.log(container);
    if (container.startsWith('#')) {
        const element = document.getElementById(container.substring(1));
        let scriptElements = element.querySelectorAll("script");
        scriptElements.forEach(scriptElement => {
            eval(scriptElement.textContent);
        });
    } else if (container.startsWith('.')) {
        const elements = document.querySelectorAll(container); // Cambia a querySelectorAll para buscar por clase
        elements.forEach(element => {
            let scriptElements = element.querySelectorAll("script");
            scriptElements.forEach(scriptElement => {
                eval(scriptElement.textContent);
            });
        });
    }

}



async function activeTab(container, nameTabToActive) {
    nameTabToActive.toString();
    var test = nameTabToActive.toString();
    if (container.startsWith('#')) {
        let htmlElement = document.getElementById(container.substring(1));
        if (htmlElement) {
            let aElements = htmlElement.getElementsByTagName("a");
            var isTab = false;
            var tab = null;
            var firstTab = null;
            for (let i = 0; i < aElements.length; i++) {
                if (aElements[i].innerText === test) {
                    isTab = true;
                    tab = aElements[i];
                } else {
                    aElements[i].classList.remove("active");
                }
                if (i === 0) {
                    firstTab = aElements[i];
                }
            }

            if (isTab) {
                console.log("Se encontró: " + nameTabToActive);
                tab.classList.add("active");
            } else {
                console.log("No se encontró: " + nameTabToActive);
                if (firstTab) {
                    console.log("Primera pestaña: " + firstTab.innerText);
                    firstTab.classList.add("active");
                }
            }
        } else {
            console.log("No se encontró ningún elemento con el ID: " + idElement);
        }







    } else if (container.startsWith('.')) {
        let htmlElement = document.getElementsByClassName(container.substring(1))[0];
        let liElements = htmlElement.getElementsByTagName("a");
        var isTab = false;
        var tab = "";
        var firstTab = "";
        for (let i = 0; i < liElements.length; i++) {
            if (liElements[i].innerText === test) {
                isTab = true;
                tab = liElements[i]
            } else {
                liElements[i].classList.remove("active");
            }
            firstTab = liElements[0];
        }
        if (isTab) {
            console.log("Se encontró: " + nameTabToActive);
            tab.classList.add("active");
        } else {
            console.log("No se encontró: " + nameTabToActive);
            console.log("Primera pestaña: " + firstTab.innerText);
            firstTab.classList.add("active");
        }



    }

  
}


async function changeSelectorContainerHTMLContent(container, content) {
    if (container.startsWith('#')) {
        $(container).html(content);
    } else if (container.startsWith('.')) {
        $(container).html(content);
    }
   
}
