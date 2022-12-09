namespace VisitingsApp
{
    [Serializable]
    class Person
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
        public Person()
        {
            Name = "";
            Checked = false;
        }
        public Person(string name, bool chkd)
        {
            Name = name;
            Checked = chkd;
        }
    }
}
