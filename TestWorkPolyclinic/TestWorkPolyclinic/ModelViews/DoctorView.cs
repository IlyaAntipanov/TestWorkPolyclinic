namespace TestWorkPolyclinic.ModelViews
{
    public class DoctorView
    {
        public int Id { get; set; }
        public string Surname { set; get; }
        public string Name { get; set; }
        public string Patronymic { set; get; }
        public int Cabinet { set; get; }
        public string Specialization { set; get; }
        public int? Plot { set; get; }
    }
}
