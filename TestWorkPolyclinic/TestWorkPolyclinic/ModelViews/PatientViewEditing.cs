﻿namespace TestWorkPolyclinic.ModelViews
{
    public class PatientViewEditing
    {
        public string Surname { set; get; }
        public string Name { get; set; }
        public string Patronymic { set; get; }
        public string Address { set; get; }
        public DateTime BirthDate { set; get; }
        public bool IsMan { set; get; }
        public int PlotId { set; get; }
    }
}
