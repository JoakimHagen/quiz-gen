using System.Collections.Generic;

namespace QuizGen
{
    public class ExampleData : Knowledge
    {
        private const string DNSLB = "DNS Load Balancer";
        private const string L4LB  = "Layer 4 (Transport) Load Balancer";
        private const string L7LB  = "Layer 7 (Application) Load Balancer";

        private const string ALB  = "Azure Load Balancer";
        private const string ATM  = "Azure Traffic Manager";
        private const string AAG  = "Azure Application Gateway";
        private const string AFD  = "Azure Front Door";
        private const string ACDN = "Azure Content Delivery Network (CDN)";

        private static string WAF   = "web-application firewall";
        private static string CDTR  = "conditional device-type routing";
        private static string CQSR  = "conditional query-string routing";
        private static string CDNSR = "conditional domain name service (DNS) routing";
        private static string RUM   = "Real User Measurements (RUM)";

        public ExampleData()
        {
            QuestionTemplates = new List<string>
            {
                "{<id} belong in which categories?",
                "Which options belong in category {id>}?",
                "Which options are a feature of {<feature}?",
                "Which options support {feature>}?",
                "Which options support both {feature>} and {feature>}?",
                "Which options can {ability>}?",
                "How would you make sure {<condition} is enabled?",
                "What feature of {<feature} can {ability>}?"
            };

            Relations = new List<NamedRelation>
            {
                new NamedRelation(ALB, "id", L4LB),
                new NamedRelation(ATM, "id", DNSLB),
                new NamedRelation(AAG, "id", L7LB),
                new NamedRelation(AFD, "id", L7LB),
                new NamedRelation(AFD, "id", DNSLB),
                new NamedRelation(AFD, "id", ACDN),

                new NamedRelation(ATM, "feature", CDNSR),
                new NamedRelation(AFD, "feature", CDNSR),
                new NamedRelation(AFD, "feature", WAF),
                new NamedRelation(AAG, "feature", WAF),
                new NamedRelation(AFD, "feature", CDTR),
                new NamedRelation(AFD, "feature", CQSR),
                new NamedRelation(ATM, "feature", RUM),

                new NamedRelation(CDNSR, "ability", "direct dns requests to frontends based on location and latency"),
                new NamedRelation(WAF,   "ability", "filter network traffic based on OSI Layer 7 (Application) data"),
                new NamedRelation(CDTR,  "ability", "route network traffic based on the device type of the requester"),
                new NamedRelation(CQSR,  "ability", "route network traffic based on the content of the request url query-string"),
                new NamedRelation(RUM,   "ability", "measure network latency to Azure regions from the end users' client web-applications"),

                new NamedRelation(RUM, "condition",  "generating a RUM key and embedding it and a javascript file into the webpage"),
                new NamedRelation(RUM, "!condition", "switching the \"Real User Measurements\" feature on in the diagnostic settings"),
                new NamedRelation(RUM, "!condition", "connecting the resource to Application Insights"),
                new NamedRelation(RUM, "!condition", "doing nothing. It's enabled by default"),
            };
        }
    }
}
