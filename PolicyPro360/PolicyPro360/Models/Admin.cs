using System.ComponentModel.DataAnnotations;

namespace PolicyPro360.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }                                                                                    
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
