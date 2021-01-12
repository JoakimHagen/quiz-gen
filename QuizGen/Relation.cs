using System;
using System.Collections.Generic;
using System.Text;

namespace QuizGen
{
    public class Relation
    {
        public string item1;
        public string property;
        public string item2;
        /*
        public List<string> distractors1;
        public List<string> distractors2;
        */
        public Relation(string v1, string v2, string v3)
        {
            item1 = v1;
            property = v2;
            item2 = v3;
        }
    }
}
