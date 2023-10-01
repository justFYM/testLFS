namespace TerriaMVC.DTO
{
    public class GenerateButtonsDTO
    {
        //public string NombreTipoInstrumento { get; set; }
        public int IdInstrumento { get; set; }
        
        public int IdTypeButtons { get; set; } /* 0: Sin iteración, 1: Con iteración.*/

        /*
        public void setIdInstrumento(int id)
        {
            this.IdInstrumento = id;
        }
        */
        public int getIdInstrumento()
        {
            return this.IdInstrumento;
        }
        public int getIdTypeButtons()
        {
            return this.IdTypeButtons;
        }
    }
}
