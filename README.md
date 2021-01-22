# quiz-gen
Quiz Generator based on a knowledge database

The engine operates on two types of input data;
- A directed graph between strings where edges are named
- Templates, containing patterns that both imply substitutions from the graph and potential answers.

## Distractors
Questions are currently limited to a multiple-choice format. Given a set of correct answers, the engine will search the graph for similar nodes, aided by commonality in edges' direction, name and targets. Similar nodes serves as distractors, i.e. incorrect but hopefully plausable options to a given question. The creator of the graph can add distractors explicitly by prepending edge names with `!` to denote a negative relation.

## Template format
`"What contains {<in} and is in turn contained in {in>}?"`

The above example template has two patterns. Patterns consist of the name of a graph edge and a direction, `<` or `>`. The direction of the edge is always pointing from the implied answer. There are no restriction on either names or direction. When the arrow points to the right, the answer is the subject or source of the directed edge and the substitution to be entered in the question is the target of the edge. Vice versa with left arrow.

The following graph shows the full set of connections that satisfies the above template.

substitution-1 --in--> implied-answer --in--> substitution-2

When generating questions, the engine looks for candidates in the graph for filling out the template and its answers. If no matching section of the graph is found or answers have insufficient distractors, a new round is attempted, picking a new template.