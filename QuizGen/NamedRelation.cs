namespace QuizGen
{
    public struct NamedRelation
    {
        public string subject;
        public string name;
        public string target;

        public NamedRelation(string subject, string name, string target)
        {
            this.subject = subject;
            this.name = name;
            this.target = target;
        }
    }
}