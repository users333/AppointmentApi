using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApi.Dtos
{
    public class UpdateAppointmentDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [Required]
        public DateTime Date { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? DoctorName { get; set; }

        [Range(0, 5, ErrorMessage = "Prioritatea trebuie să fie între 0 și 5.")]
        public int? Priority { get; set; }
    }
}
