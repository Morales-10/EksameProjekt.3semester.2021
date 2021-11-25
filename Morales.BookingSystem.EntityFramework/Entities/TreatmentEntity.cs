using System;

namespace Morales.BookingSystem.EntityFramework.Entities
{
    public class TreatmentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
    }
}