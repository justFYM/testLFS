class GenerateAOIDTO {
    constructor(idInstrumentos, defaultContent, isForTop, isForTopConsolidado, isForBotConsolidado) {
        this.IdInstrumentos = idInstrumentos;
        this.DefaultContent = defaultContent;
        this.IsForTop = isForTop;
        this.IsForTopConsolidado = isForTopConsolidado;
        this.IsForBotConsolidado = isForBotConsolidado;
    }
    async readValues() {
        console.log("this.IdInstrumentos: " + this.IdInstrumentos);
        console.log("this.DefaultContent: " + this.DefaultContent)
        console.log("this.IsForTop: " + this.IsForTop)
        console.log("this.IsForTopConsolidado: " + this.IsForTopConsolidado)
        console.log("this.IsForBotConsolidado: " + this.IsForBotConsolidado)
    }
}
