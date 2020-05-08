using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contoso.Spaces.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string MailingAddress { get; set; }

        public virtual List<Room> Rooms { get; set; }

        public bool ParkingIncluded { get; set; }

        public bool ConferenceRoomsIncluded { get; set; }

        public bool ReceptionIncluded { get; set; }

        public bool PublicAccess { get; set; }

        public DateTimeOffset LastRenovationDate { get; set; }

        public string Image { get; set; }
    }
}