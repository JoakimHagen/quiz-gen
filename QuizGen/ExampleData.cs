using System;
using System.Collections.Generic;
using System.Text;

namespace QuizGen
{
    public class ExampleData : Knowledge
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
        private static Feature RUM = new Feature
        {
            feature = "Real User Measurements (RUM)",
            action = "measure network latency to Azure regions from the end users' client web-applications"
        };

        private static FeatureCondition ATMCOND = new FeatureCondition
        {
            action = "generating a RUM key and embedding it and a javascript file into the webpage"
        };
        private static FeatureCondition ATMCOND2 = new FeatureCondition
        {
            action = "switching the \"Real User Measurements\" feature on in the diagnostic settings"
        };
        private static FeatureCondition ATMCOND3 = new FeatureCondition
        {
            action = "connecting the resource to Application Insights"
        };
        private static FeatureCondition ATMCOND4 = new FeatureCondition
        {
            action = "doing nothing. It's enabled by default"
        };

        public ExampleData()
        {
            Identities = new List<IdentityRelation>
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

            var atm_rum = new FeatureRelation(ATM, RUM);

            Features = new List<FeatureRelation>
            {
                new FeatureRelation(ATM, CDNSR),
                new FeatureRelation(AFD, CDNSR),
                new FeatureRelation(AFD, WAF),
                new FeatureRelation(AAG, WAF),
                new FeatureRelation(AFD, CDTR),
                new FeatureRelation(AFD, CQSR),
                atm_rum,
            };

            Conditions = new List<ConditionRelation>
            {
                new ConditionRelation(atm_rum, ATMCOND)
            };

            DistractConditions = new List<ConditionRelation>
            {
                new ConditionRelation(atm_rum, ATMCOND2),
                new ConditionRelation(atm_rum, ATMCOND3),
                new ConditionRelation(atm_rum, ATMCOND4)
            };
        }
    }
}
