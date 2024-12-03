namespace HealthCareABApi.Models
{
    public class Availability
    {
        public int Id { get; set; }
        public int CaregiverId { get; set; }
        public List<AvailableSlot> AvailableSlots { get; set; } = new List<AvailableSlot>();
    }

    public class AvailableSlot
    {
        public DateTime Date { get; set; }      // Holds both date and time
    }

}