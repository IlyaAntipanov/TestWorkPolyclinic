using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestWorkPolyclinic.Models;
using TestWorkPolyclinic.ModelViews;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestWorkPolyclinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        // GET: api/<DoctorsController>
        [HttpGet]
        public ActionResult<List<DoctorView>> Get([FromQuery] Sort sort)
        {
            using PolyclinicDB db = new PolyclinicDB();
            if (sort.MaxResultInPage > 1 && sort.Page >= 1)
            {
                IOrderedQueryable<Doctor> orderBy = sort.PropSort.ToLower() switch
                {
                    "id" => db.Doctors.OrderBy(o => o.Id),
                    "surname" => db.Doctors.OrderBy(o => o.Surname),
                    "name" => db.Doctors.OrderBy(o => o.Name),
                    "patronymic" => db.Doctors.OrderBy(o => o.Patronymic),
                    "cabinet" => db.Doctors.OrderBy(o => o.Cabinet.Number),
                    "specialization" => db.Doctors.OrderBy(o => o.Specialization.Name),
                    "plot"=> db.Doctors.OrderBy(o => o.Plot.Number),
                    _ => db.Doctors.OrderBy(o => o.Id)
                };
                var result = orderBy.Skip((sort.Page - 1) * sort.MaxResultInPage).Take(sort.Page * sort.MaxResultInPage)
                    .Select(s => new DoctorView()
                    {
                        Id = s.Id,
                        Surname = s.Surname,
                        Name = s.Name,
                        Patronymic = s.Patronymic,
                        Cabinet = s.Cabinet.Number,
                        Specialization = s.Specialization.Name,
                        Plot = s.Plot == null ? null : s.Plot.Number
                    }).ToList();
                return result;
            }
            return new BadRequestResult();
        }

        // GET api/<DoctorsController>/5
        [HttpGet("{id}")]
        public ActionResult<DoctorViewEditing> Get(int id)
        {
            using PolyclinicDB db = new PolyclinicDB();
            var doctor = db.Doctors.Where(w => w.Id == id).FirstOrDefault();
            if (doctor == null)
                return new NotFoundResult();
            return Ok(new DoctorViewEditing()
            {
                Surname = doctor.Surname,
                Name = doctor.Name,
                Patronymic = doctor.Patronymic,
                CabinetId = doctor.CabinetId,
                SpecializationId = doctor.SpecializationId,
                PlotId = doctor.PlotId
            });
        }

        // POST api/<DoctorsController>
        [HttpPost]
        public ActionResult<int> Post([FromBody] DoctorViewEditing newDoctor)
        {
            using PolyclinicDB db = new PolyclinicDB();
            if (db.Cabinets.Any(a => a.Id == newDoctor.CabinetId) && db.Specializations.Any(a => a.Id == newDoctor.SpecializationId) &&
                (newDoctor.PlotId == null || db.Plots.Any(a => a.Id == newDoctor.PlotId)))
            {
                var doctor = new Doctor()
                {
                    Surname = newDoctor.Surname,
                    Name = newDoctor.Name,
                    Patronymic = newDoctor.Patronymic,
                    CabinetId = newDoctor.CabinetId,
                    SpecializationId = newDoctor.SpecializationId,
                    PlotId = newDoctor.PlotId
                };
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return Ok(doctor.Id);
            }
            return new BadRequestResult();
        }

        // PUT api/<DoctorsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] DoctorViewEditing newDoctor)
        {
            using PolyclinicDB db = new PolyclinicDB();
            if (db.Cabinets.Any(a => a.Id == newDoctor.CabinetId) && db.Specializations.Any(a => a.Id == newDoctor.SpecializationId) &&
                (newDoctor.PlotId == null || db.Plots.Any(a => a.Id == newDoctor.PlotId)))
            {
                var doctor = db.Doctors.Where(w => w.Id == id).FirstOrDefault();
                if(doctor != null)
                {
                    doctor.Surname = newDoctor.Surname;
                    doctor.Name = newDoctor.Name;
                    doctor.Patronymic = newDoctor.Patronymic;
                    doctor.CabinetId = newDoctor.CabinetId;
                    doctor.SpecializationId = newDoctor.SpecializationId;
                    doctor.PlotId = newDoctor.PlotId;
                    db.SaveChanges();
                    return Ok();
                }
            }
            return new BadRequestResult();
        }

        // DELETE api/<DoctorsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using PolyclinicDB db = new PolyclinicDB();
            var doctor = db.Doctors.FirstOrDefault(w => w.Id == id);
            if (doctor != null)
            {
                db.Doctors.Remove(doctor); 
                db.SaveChanges(); 
                return Ok();   
            }
            return new BadRequestResult();
        }
    }
}
