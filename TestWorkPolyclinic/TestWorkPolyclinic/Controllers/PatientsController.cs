using Microsoft.AspNetCore.Mvc;
using TestWorkPolyclinic.Models;
using TestWorkPolyclinic.ModelViews;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWorkPolyclinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        // GET: api/<PatientsController>
        [HttpGet]
        public ActionResult<List<PatientView>> Get([FromQuery] Sort sort)
        {
            using PolyclinicDB db = new PolyclinicDB();
            if (sort.MaxResultInPage > 1 && sort.Page >= 1)
            {
                IOrderedQueryable<Patient> orderBy = sort.PropSort.ToLower() switch
                {
                    "id" => db.Patients.OrderBy(o => o.Id),
                    "surname" => db.Patients.OrderBy(o => o.Surname),
                    "name" => db.Patients.OrderBy(o => o.Name),
                    "patronymic" => db.Patients.OrderBy(o => o.Patronymic),
                    "address" => db.Patients.OrderBy(o => o.Address),
                    "birthdate" => db.Patients.OrderBy(o => o.BirthDate),
                    "isman" => db.Patients.OrderBy(o => o.IsMan),
                    "plot" => db.Patients.OrderBy(o => o.Plot.Number),
                    _ => db.Patients.OrderBy(o => o.Id)
                };
                var result = orderBy.Skip((sort.Page - 1) * sort.MaxResultInPage).Take(sort.Page * sort.MaxResultInPage)
                    .Select(s => new PatientView()
                    {
                        Id = s.Id,
                        Surname = s.Surname,
                        Name = s.Name,
                        Patronymic = s.Patronymic,
                        Address = s.Address,
                        BirthDate = s.BirthDate,
                        IsMan = s.IsMan,
                        Plot = s.Plot.Number
                    }).ToList();
                return result;
            }
            return new BadRequestResult();
        }

        // GET api/<PatientsController>/5
        [HttpGet("{id}")]
        public ActionResult<PatientViewEditing> Get(int id)
        {
            using PolyclinicDB db = new PolyclinicDB();
            var patient = db.Patients.Where(w => w.Id == id).FirstOrDefault();
            if (patient == null)
                return new NotFoundResult();
            return Ok(new PatientViewEditing()
            {
                Surname = patient.Surname,
                Name = patient.Name,
                Patronymic = patient.Patronymic,
                Address = patient.Address,
                BirthDate = patient.BirthDate,
                IsMan = patient.IsMan,
                PlotId = patient.PlotId
            });
        }

        // POST api/<PatientsController>
        [HttpPost]
        public ActionResult<int> Post([FromBody] PatientViewEditing newPatient)
        {
            using PolyclinicDB db = new PolyclinicDB();
            if (db.Plots.Any(a => a.Id == newPatient.PlotId))
            {
                var date = new DateTime(newPatient.BirthDate.Ticks, DateTimeKind.Utc);
                var patient = new Patient()
                {
                    Surname = newPatient.Surname,
                    Name = newPatient.Name,
                    Patronymic = newPatient.Patronymic,
                    Address = newPatient.Address,
                    BirthDate = date,
                    IsMan = newPatient.IsMan,
                    PlotId = newPatient.PlotId
                };
                db.Patients.Add(patient);
                db.SaveChanges();
                return Ok(patient.Id);
            }
            return new BadRequestResult();
        }

        // PUT api/<PatientsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] PatientViewEditing newPatient)
        {
            using PolyclinicDB db = new PolyclinicDB();
            if (db.Plots.Any(a => a.Id == newPatient.PlotId))
            {
                var patient = db.Patients.Where(w => w.Id == id).FirstOrDefault();
                if (patient != null)
                {
                    var date = new DateTime(newPatient.BirthDate.Ticks, DateTimeKind.Utc);

                    patient.Surname = newPatient.Surname;
                    patient.Name = newPatient.Name;
                    patient.Patronymic = newPatient.Patronymic;
                    patient.Address = newPatient.Address;
                    patient.BirthDate = date;
                    patient.IsMan = newPatient.IsMan;
                    patient.PlotId = newPatient.PlotId;
                    db.SaveChanges();
                    return Ok();
                }
            }
            return new BadRequestResult();
        }

        // DELETE api/<PatientsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using PolyclinicDB db = new PolyclinicDB();
            var patient = db.Patients.FirstOrDefault(w => w.Id == id);
            if (patient != null)
            {
                db.Patients.Remove(patient);
                db.SaveChanges();
                return Ok();
            }
            return new BadRequestResult();
        }
    }
}
