using System;
using System.Collections.Generic;
using System.Text;

namespace QuizGen
{
    public static class ExampleData
    {
        private const string INDUSTRY = "Industry Trademark";

        private const string CAR = "Car Brand";
        private const string HW = "Hardware Store";
        private const string FOOD = "Food Chain";
        private const string TECH = "Tech Brand";

        private const string BMW = "BMW";
        private const string OPEL = "Opel";
        private const string PRNC = "Princess";
        private const string AIRBUS = "Airbus";
        private const string HARLEY = "Harley Davidson";


        public static List<Relation> relations = new List<Relation>
        {
            new Relation(CAR, "is", INDUSTRY),
            new Relation(BMW, "is", CAR),
            new Relation(OPEL, "is", CAR)
        };

        public static List<Relation> distractions = new List<Relation>
        {
            new Relation(HW, "is", INDUSTRY),
            new Relation(FOOD, "is", INDUSTRY),
            new Relation(TECH, "is", INDUSTRY),
            new Relation(PRNC, "is not", CAR),
            new Relation(AIRBUS, "is not", CAR),
            new Relation(HARLEY, "is not", CAR),
            new Relation(BMW, "is not", "British Monarchy Warships")
        };
    }
}
