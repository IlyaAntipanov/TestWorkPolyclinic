namespace TestWorkPolyclinic.Models
{
    public class Cabinet
    {
        public int Id { get; set; }
        public int Number { set; get; }
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
