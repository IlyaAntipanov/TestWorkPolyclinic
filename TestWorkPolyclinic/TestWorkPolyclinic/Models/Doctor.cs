namespace TestWorkPolyclinic.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Surname { set; get; }
        public string Name { get; set; }
        public string Patronymic { set; get; }
        public int CabinetId { set; get; }
        public int SpecializationId { set; get; }
        public int? PlotId { set; get; }

        public virtual Cabinet Cabinet { set; get; }
        public virtual Specialization Specialization { set; get; }
        public virtual Plot? Plot { set; get; }
    }
}
