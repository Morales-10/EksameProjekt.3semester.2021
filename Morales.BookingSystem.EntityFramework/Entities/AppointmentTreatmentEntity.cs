namespace Morales.BookingSystem.EntityFramework.Entities
{
    public class AppointmentTreatmentEntity
    {
        public int AppointmentId { get; set; }
        public int TreatmentId { get; set; }
        public AppointmentEntity Appointment { get; set; }
        public TreatmentEntity Treatment { get; set; }
    }
}