using System.Numerics;

namespace TestWorkPolyclinic.Models
{
    public class Plot
    {
        public int Id { set; get; }
        public int Number { set; get; }

        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
