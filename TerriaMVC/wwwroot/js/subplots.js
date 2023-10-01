

var verticalLines = [];

function removeVerticalLines() {
    verticalLines.forEach(function (line) {
        line.destroy();
    });
    verticalLines = [];
}
/*
function drawVerticalLines() {
    removeVerticalLines(); // Elimina líneas previas

    var yAxis = chart.yAxis;
    var renderer = chart.renderer;

    // Coordenadas x comunes para las líneas verticales
    var xPosition = (chart.plotSizeX / 1000000000) + chart.plotLeft;

    // Dibuja una sola línea vertical en cada gráfico
    verticalLines.push(renderer.rect(xPosition - 1, yAxis[0].top, 2, yAxis[0].height)
        .attr({
            fill: '#e6e6e6',
            stroke: '#e6e6e6'
        })
        .add());

    verticalLines.push(renderer.rect(xPosition - 1, yAxis[1].top, 2, yAxis[1].height)
        .attr({
            fill: '#e6e6e6',
            stroke: '#e6e6e6'
        })
        .add());

    verticalLines.push(renderer.rect(xPosition - 1, yAxis[2].top, 2, yAxis[2].height)
        .attr({
            fill: '#e6e6e6',
            stroke: '#e6e6e6'

        })
        .add());
}
*/
function drawVerticalLines(cantidadSensores) {
    console.log(cantidadSensores);
    removeVerticalLines(); // Elimina líneas previas
    var yAxis = chart.yAxis;
    var renderer = chart.renderer;
    var xPosition = (chart.plotSizeX / 1000000000) + chart.plotLeft;
    for (var i = 0; i < cantidadSensores; i++) {
        verticalLines.push(renderer.rect(xPosition - 1, yAxis[i].top, 2, yAxis[i].height)
            .attr({
                fill: '#e6e6e6',
                stroke: '#e6e6e6'
            })
            .add());
    }
}
/*
function updateSeriesData() {
    seriesData.series1.name = 'Nuevo Nombre Serie 1';
    seriesData.series1.data = generateRandomData(3);
    seriesData.series1.yAxisTitle = 'Nuevo Nombre Serie 1';

    seriesData.series2.name = 'Nuevo Nombre Serie 2';
    seriesData.series2.data = generateRandomData(3);
    seriesData.series2.yAxisTitle = 'Nuevo Nombre Serie 2';

    seriesData.series3.name = 'Nuevo Nombre Serie 3';
    seriesData.series3.data = generateRandomData(3);
    seriesData.series3.yAxisTitle = 'Nuevo Nombre Serie 3';
    drawChart();
}
function generateRandomData(length) {
    var data = [];
    for (var i = 0; i < length; i++) {
        data.push(Math.floor(Math.random() * 101)); // Genera un número aleatorio entre 0 y 100
    }
    return data;
}
*/



