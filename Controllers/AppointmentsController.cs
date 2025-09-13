using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controllers
{
    [ApiController]
    [Route("appointments")]
    public class AppointmentsController : ControllerBase
    {
        // LISTA STATICĂ DE PROGRAMĂRI (Nivel 5)
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

        // ===================== PUBLIC =====================

    
        [HttpGet("public/list")]
        public IActionResult PublicList() => Ok(Appointments);

        // GET /appointments/details/{id}  (Nivel 6)
        [HttpGet("details/{id}")]
        public IActionResult GetDetails(int id)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

       
        [HttpGet("search")]
        public IActionResult Search(string? name, DateTime? dateFrom, DateTime? dateTo)
        {
            var results = Appointments.AsEnumerable();

            if (!string.IsNullOrEmpty(name))
                results = results.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (dateFrom.HasValue)
                results = results.Where(a => a.Date >= dateFrom.Value);
            if (dateTo.HasValue)
                results = results.Where(a => a.Date <= dateTo.Value);

            return Ok(results);
        }

      
        [HttpPut("admin/edit/{id}")]
        public IActionResult AdminEdit(int id, [FromBody] Appointment update)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();

            item.Name = update.Name;
            item.Date = update.Date;
            return Ok(item);
        }

        // POST /appointments/admin/add  (Nivel 9 + 10)
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

        // DELETE /appointments/admin/delete/{id}  (Nivel 9 + 10)
        [HttpDelete("admin/delete/{id}")]
        public IActionResult AdminDelete(int id)
        {
            var item = Appointments.FirstOrDefault(a => a.Id == id);
            if (item == null) return NotFound();

            Appointments.Remove(item);
            return Ok($"Appointment with ID {id} was deleted.");
        }
    }

    
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
