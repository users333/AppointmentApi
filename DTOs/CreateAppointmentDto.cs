using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApi.Dtos
{
    public class CreateAppointmentDto
    {
        [Required]
        [Range(1, 1000, ErrorMessage = "ID-ul trebuie să fie între 1 și 1000.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între 3 și 50 de caractere.")]
        public string Name { get; set; } = "";

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email-ul nu este valid.")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(30)]
        public string DoctorName { get; set; } = "";
    }
}
