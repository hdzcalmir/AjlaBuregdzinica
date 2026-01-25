using System.ComponentModel.DataAnnotations;

namespace BuregdzinicaAjla.Model
{
    public class EmailForm
    {
        [Required]
        public string ImePrezime { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Naslov { get; set; }
        [Required]
        public string Poruka { get; set; }
    }
}
