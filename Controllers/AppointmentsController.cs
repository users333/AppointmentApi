using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [Route("appointments")]
    public class AppointmentsController : ControllerBase
    {

        private static readonly List<Appointment> Appointments = new()
        {
            new Appointment { Id = 1, Name = "Andrei Popescu", Date = DateTime.Now.AddDays(1) },
            new Appointment { Id = 2, Name = "Maria Ionescu", Date = DateTime.Now.AddDays(2) },
            new Appointment { Id = 3, Name = "Radu Marinescu", Date = DateTime.Now.AddDays(3) },
            new Appointment { Id = 4, Name = "Elena Gheorghe", Date = DateTime.Now.AddDays(4) },
            new Appointment { Id = 5, Name = "Ioana Dumitrescu", Date = DateTime.Now.AddDays(5) },
            new Appointment { Id = 6, Name = "Vasile Petrescu", Date = DateTime.Now.AddDays(6) },
            new Appointment { Id = 7, Name = "Cristina Stan", Date = DateTime.Now.AddDays(7) },
            new Appointment { Id = 8, Name = "George Mihai", Date = DateTime.Now.AddDays(8) },
            new Appointment { Id = 9, Name = "Diana Ene", Date = DateTime.Now.AddDays(9) },
            new Appointment { Id = 10, Name = "Florin Dobre", Date = DateTime.Now.AddDays(10) }
        };



        [HttpGet("public/list")]
        public IActionResult PublicList() => Ok(Appointments);

        [HttpGet("details/{id}")]
        public IActionResult GetDetails(int id)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }


        [HttpPost("admin/add")]
        public IActionResult AdminAdd([FromBody] Appointment newAppointment)
        {
            if (Appointments.Any(a => a.Id == newAppointment.Id))
            {
                return BadRequest("ID already exists.");
            }

            Appointments.Add(newAppointment);
            return CreatedAtAction(nameof(GetDetails), new { id = newAppointment.Id }, newAppointment);
        }

        [HttpDelete("admin/delete/{id}")]
        public IActionResult AdminDelete(int id)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();

            Appointments.Remove(item);
            return Ok($"Appointment with ID {id} was deleted.");
        }
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name)
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

        [HttpPut("update-up/{id}")]
        public IActionResult UpdateAppointment(int id, [FromBody] Appointment update)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound($"Nu exista programare cu ID = {id}.");

            item.Name = update.Name;
            item.Date = update.Date;

            return Ok(item);
        }




    }



    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
