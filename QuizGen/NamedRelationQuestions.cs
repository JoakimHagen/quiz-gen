using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QuizGen
{
    public class NamedRelationQuestions
    {
        Knowledge knowledge;

        public NamedRelationQuestions(Knowledge knowledge)
        {
            this.knowledge = knowledge;
        }

        public Question Question(Random seed, NamedRelation relation)
        {
            /*
            if (relation.name == "id")
            {
                return AskForIdentity(seed, relation);
            }
            else if (relation.name == "feature")
            {
                return AskForFeature(seed, relation);
            }
            else if (relation.name == "ability")
            {
                return AskForAbilitySubjects(seed, relation);
            }
            else if (relation.name == "condition")
            {
                return AskForCondition(seed, relation);
            }
            else return null;
            */

            return ExpandTemplate(seed, "{<id} belong under which option?");
        }

        private Question AskForIdentity(Random seed, NamedRelation relation)
        {
            var direction = seed.Next(0, 2) > 0;

            var answers = direction
                ? knowledge.Relations
                    .Where(x => x.subject == relation.subject && x.name == relation.name)
                    .Select(x => x.target)
                    .ToArray()
                : knowledge.Relations
                    .Where(x => x.target == relation.target && x.name == relation.name)
                    .Select(x => x.subject)
                    .ToArray();

            var stem = "";
            if (direction)
            {
                stem = relation.subject + " is which of the following?";
            }
            else
            {
                var an = Regex.IsMatch(relation.target, "^[aeiouyæøåAEIOUYÆØÅ]") ? "an" : "a";
                stem = $"Which of the following is {an} {relation.target}?";
            }

            return FillDistractors(seed, stem, answers);
        }

        private Question AskForFeature(Random seed, NamedRelation relation)
        {
            var direction = seed.Next(0, 2) > 0;

            var answers = direction
                ? knowledge.Relations
                    .Where(x => x.subject == relation.subject && x.name == relation.name)
                    .Select(x => x.target)
                    .ToArray()
                : knowledge.Relations
                    .Where(x => x.target == relation.target && x.name == relation.name)
                    .Select(x => x.subject)
                    .ToArray();

            var stem = "";
            if (direction)
            {
                stem = $"Which of the following is a feature of {relation.subject}?";
            }
            else
            {
                stem = $"Which of the following support {relation.target}?";
            }

            return FillDistractors(seed, stem, answers);
        }

        private Question AskForAbilitySubjects(Random seed, NamedRelation relation)
        {
            var features = knowledge.Relations
                .Where(x => x.target == relation.target && x.name == relation.name)
                .Select(x => x.subject)
                .ToArray();

            var answers = knowledge.Relations
                .Where(x => x.name == "feature" && features.Contains(x.target))
                .Select(x => x.subject)
                .ToArray();

            var stem = $"Which of the following can {relation.target}?";

            return FillDistractors(seed, stem, answers);
        }

        private Question AskForCondition(Random seed, NamedRelation relation)
        {
            var answers = knowledge.Relations
                .Where(x => x.subject == relation.subject && x.name == relation.name)
                .Select(x => x.target)
                .ToArray();

            var stem = $"How would you make sure {relation.subject} is enabled?";

            return FillDistractors(seed, stem, answers);
        }

        private Question FillDistractors(Random seed, string stem, string[] answers)
        {
            var distractors = knowledge.FindSimilar(answers);

            if (distractors.Length == 0)
            {
                return null;
            }

            distractors.Shuffle(seed);
            distractors = distractors.Take(5).ToArray();

            return new Question
            {
                Stem = stem,
                Answers = answers,
                Distractors = distractors
            };
        }

        private Question ExpandTemplate(Random seed, string template)
        {
            // NOT WORKING YET
            // keep at it

            var str = new StringBuilder();
            var start = 0;
            string[] intersection = null;

            var relations = new List<string>();

            while (start < template.Length)
            {
                var index = template.IndexOf('{', start);
                if (index == -1)
                {
                    str.Append(template.Substring(start, template.Length - start));
                    break;
                }

                str.Append(template.Substring(start, index - start));
                start = index + 1;

                index = template.IndexOf('}', start);
                if (index == -1)
                    index = template.Length;

                var pattern = template.Substring(start, index - start);
                str.Append($"{{{relations.Count}}}");
                start = index + 1;

                relations.Add(pattern);

                IEnumerable<string> potential;
                if (pattern.StartsWith("<"))
                {
                    var relation = pattern.Substring(1);
                    potential = knowledge.Relations
                        .Where(x => x.name == relation)
                        .Select(x => x.target);
                }
                else
                {
                    var relation = pattern.Substring(0, pattern.Length - 1);
                    potential = knowledge.Relations
                        .Where(x => x.name == relation)
                        .Select(x => x.subject);
                }

                if (intersection == null)
                {
                    intersection = potential.ToArray();
                }
                else
                {
                    intersection = intersection.Intersect(potential).ToArray();
                    if (intersection.Length == 0)
                        return null;
                }
            }

            var selected = seed.Choose(intersection);

            var substitutions = relations.Select(pattern =>
            {
                if (pattern.StartsWith("<"))
                {
                    var relation = pattern.Substring(1);
                    return knowledge.Relations
                        .Where(x => x.name == relation && x.target == selected)
                        .Select(x => x.subject)
                        .FirstOrDefault();
                }
                else
                {
                    var relation = pattern.Substring(0, pattern.Length - 1);
                    return knowledge.Relations
                        .Where(x => x.name == relation && x.subject == selected)
                        .Select(x => x.target)
                        .FirstOrDefault();
                }
            });

            var stem = string.Format(str.ToString(), selected);

            return FillDistractors(seed, stem, substitutions.Take(3).ToArray());
        }
    }
}
