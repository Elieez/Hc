namespace HealthCareABApi.DTO
{
    public class AvailableSlotDto
    { 
        public int AvailabilityId { get; set; }
        public DateTime Date { get; set; }
        public int CaregiverId { get; set; }
    }
}
