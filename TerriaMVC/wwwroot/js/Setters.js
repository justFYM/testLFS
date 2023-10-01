async function SetterProperties(idInstrumento, nombreTipoInstrumento) {
    const generateButtons = new GenerateButtonsDTO(idInstrumento, nombreTipoInstrumento);
    console.log(generateButtons.getIdInstrumento)
    console.log(generateButtons.getIdTypeButtons);
}