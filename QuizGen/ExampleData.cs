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

        public static List<IdentityRelation> idrelations = new List<IdentityRelation>
        {
            new IdentityRelation(DNSLB, AZSRVC),
            new IdentityRelation(L4LB, AZSRVC),
            new IdentityRelation(L7LB, AZSRVC),
            new IdentityRelation(ALB, L4LB),
            new IdentityRelation(ATM, DNSLB),
            new IdentityRelation(AAG, L7LB),
            new IdentityRelation(AFD, L7LB),
            new IdentityRelation(AFD, DNSLB),
            new IdentityRelation(AFD, ACDN),
        };

        private static Feature WAF = new Feature
        {
            feature = "web-application firewall",
            action  = "filter network traffic based on OSI Layer 7 (Application) data"
        };
        private static Feature CDTR = new Feature
        {
            feature = "conditional device-type routing",
            action = "route network traffic based on the device type of the requester"
        };
        private static Feature CQSR = new Feature
        {
            feature = "conditional query-string routing",
            action = "route network traffic based on the content of the request url query-string"
        };
        private static Feature CDNSR = new Feature
        {
            feature = "conditional domain name service (DNS) routing",
            action = "direct dns requests to frontends based on location and latency"
        };

        public static List<FeatureRelation> featureRelations = new List<FeatureRelation>
        {
            new FeatureRelation(ATM, CDNSR),
            new FeatureRelation( AFD, CDNSR ),
            new FeatureRelation( AFD, WAF ),
            new FeatureRelation( AAG, WAF ),
            new FeatureRelation( AFD, CDTR ),
            new FeatureRelation( AFD, CQSR ),
        };
    }
}
