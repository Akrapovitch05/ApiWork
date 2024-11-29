namespace apiwork.Models
{
    public class Salarie
    {
        public int SalarieID { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string? TelephoneFixe { get; set; }
        public string? TelephonePortable { get; set; }
        public string Email { get; set; }

        public int ServiceID { get; set; }
        public Service Service { get; set; }  // Relation avec Service

        public int SiteID { get; set; }
        public Site Site { get; set; }  // Relation avec Site
    }
}
