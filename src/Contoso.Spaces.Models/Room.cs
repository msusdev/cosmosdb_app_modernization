using System.ComponentModel.DataAnnotations;

namespace Contoso.Spaces.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal MonthlyRate { get; set; }

        public int Seats { get; set; }

        public bool PrivateFacilities { get; set; }

        public bool PhoneIncluded { get; set; }

        public bool Windows { get; set; }

        public bool Corner { get; set; }
    }
}