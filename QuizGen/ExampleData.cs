using System;
using System.Collections.Generic;
using System.Text;

namespace QuizGen
{
    public static class ExampleData
    {
        private const string AZSRVC = "Azure Service";

        private const string DNSLB = "DNS Load Balancer";
        private const string L4LB = "Layer 4 (Transport) Load Balancer";
        private const string L7LB = "Layer 7 (Application) Load Balancer";
        private const string L5LB = "Layer 5 (Session) Load Balancer";

        private const string ALB = "Azure Load Balancer";
        private const string ATM = "Azure Traffic Manager";
        private const string AAG = "Azure Application Gateway";
        private const string AFD = "Azure Front Door";
        private const string ACDN = "Azure Content Delivery Network (CDN)";


        public static List<Relation> relations = new List<Relation>
        {
            new Relation(DNSLB, "is", AZSRVC),
            new Relation(L4LB, "is", AZSRVC),
            new Relation(L7LB, "is", AZSRVC),
            new Relation(ALB, "is", L4LB),
            new Relation(ATM, "is", DNSLB),
            new Relation(AAG, "is", L7LB),
            new Relation(AFD, "is", L7LB),
            new Relation(AFD, "is", DNSLB),
            new Relation(AFD, "is", ACDN),
        };

        public static List<Relation> distractions = new List<Relation>
        {
            new Relation(L5LB, "is", AZSRVC)
        };
    }
}
