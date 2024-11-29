namespace apiwork.Models
{
    public class HistoriqueModification
    {
        public int HistoriqueID { get; set; }  // Clé primaire
        public DateTime DateModification { get; set; }
        public string Action { get; set; }
        public int AdminID { get; set; }
        public int SalarieID { get; set; }

        public Admin Admin { get; set; }
        public Salarie Salarie { get; set; }
    }
}
