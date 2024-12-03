namespace HealthCareABApi.Models
{
	public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public DateTime DateTime { get; set; }
        public AppointmentStatus Status { get; set; }
    }

    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }
}

