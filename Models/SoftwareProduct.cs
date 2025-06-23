using System.ComponentModel.DataAnnotations;

namespace state_software_marketplace.Models
{
    public class SoftwareProduct
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Vendor { get; set; }

        [StringLength(100)]
        public string Category { get; set; }
    }
}
