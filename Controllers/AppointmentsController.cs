using Microsoft.AspNetCore.Mvc;
using AppointmentApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [Route("appointments")]
    public class AppointmentsController : ControllerBase
    {
        private static readonly List<Appointment> Appointments = new()
        {
            new Appointment { Id = 1, Name = "Andrei Popescu", Date = DateTime.Now.AddDays(1), Email = "andrei@email.com", DoctorName = "Dr. Vasilescu" },
            new Appointment { Id = 2, Name = "Maria Ionescu", Date = DateTime.Now.AddDays(2), Email = "maria@email.com", DoctorName = "Dr. Popa" }
        };

        [HttpGet("list")]
        public IActionResult GetAll() => Ok(Appointments);

        [HttpGet("details/{id}")]
        public IActionResult GetDetails(int id)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("add")]
        public IActionResult Add([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Appointments.Any(a => a.Id == dto.Id))
                return BadRequest("ID already exists.");

            var newAppointment = new Appointment
            {
                Id = dto.Id,
                Name = dto.Name,
                Date = dto.Date,
                Email = dto.Email,
                DoctorName = dto.DoctorName
            };

            Appointments.Add(newAppointment);
            return CreatedAtAction(nameof(GetDetails), new { id = newAppointment.Id }, newAppointment);
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound($"Nu exista programare cu ID = {id}.");

            item.Name = dto.Name;
            item.Date = dto.Date;
            item.Email = dto.Email ?? item.Email;
            item.DoctorName = dto.DoctorName ?? item.DoctorName;

            return Ok(item);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery, StringLength(50)] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Trebuie să specifici un nume.");

            var results = Appointments
                .Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!results.Any())
                return NotFound("Nu s-au găsit programări pentru acest nume.");

            return Ok(results);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();

            Appointments.Remove(item);
            return Ok($"Appointment with ID {id} was deleted.");
        }
        [HttpPost("admin/add")]
        public IActionResult AdminAdd([FromBody] CreateAppointmentDto newAppointment)
        {
            if (Appointments.Any(a => a.Id == newAppointment.Id))
                return BadRequest("ID already exists.");

            var appointment = new Appointment
            {
                Id = newAppointment.Id,
                Name = newAppointment.Name,
                Date = newAppointment.Date,
                Email = newAppointment.Email,
                DoctorName = newAppointment.DoctorName,
            };

            Appointments.Add(appointment);
            return CreatedAtAction(nameof(GetDetails), new { id = appointment.Id }, appointment);
        }

    }

    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Date { get; set; }
        public string Email { get; set; } = "";
        public string DoctorName { get; set; } = "";
    }
}
