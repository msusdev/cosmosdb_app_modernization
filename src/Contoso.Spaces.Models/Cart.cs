using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Spaces.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual List<CartLocation> Locations { get; set; }
    }
}