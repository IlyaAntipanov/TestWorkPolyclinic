using System.Numerics;

namespace TestWorkPolyclinic.Models
{
    public class Specialization
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
