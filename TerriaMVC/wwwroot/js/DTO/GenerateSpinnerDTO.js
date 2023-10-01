class GenerateSpinnerDTO {
    constructor(text) {
        this.Spinner = this.generateSpinnerHTML(text);
    }

     generateSpinnerHTML(text) {
        if (text != 'empty') {
            return `<div class="d-flex justify-content-center mt-4 mb-4"><span class="spinner-border text-success"></span><div class="mt-2 mx-2">${text}...</div></div>`;

        } else {
            return `<div class="d-flex justify-content-center"><span class="spinner-border text-success"></span></div>`;

        }
       }

    getSpinner() {
        return this.Spinner;
    }

    changeText(text) {
        this.Spinner = this.generateSpinnerHTML(text);
    }
}
