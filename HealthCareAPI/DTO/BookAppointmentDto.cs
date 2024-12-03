namespace HealthCareABApi.DTO
{
    public class BookAppointmentDto
    {
        public int AvailabilityId { get; set; }
        public DateTime SelectedSlot { get; set; }
        public int CaregiverId { get; set; }
        public int PatientId { get; set; }
        public string Username { get; set; }
    }

}
