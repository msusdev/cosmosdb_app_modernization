using System.ComponentModel.DataAnnotations;

namespace Contoso.Spaces.Models
{
    public class CartLocation
    {
        [Key]
        public int Id { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Location Location { get; set; }
    }
}