namespace AppointmentApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty; 
        public bool IsConfirmed { get; set; }
    }
}
