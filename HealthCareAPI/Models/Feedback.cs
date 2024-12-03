namespace HealthCareABApi.Models
{
	public class Feedback
	{
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string Comment { get; set; }
    }
}

