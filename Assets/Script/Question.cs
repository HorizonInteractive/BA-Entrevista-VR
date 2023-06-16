using System;
using System.Linq;

[Serializable]
public class Question
{
    public string description;
    public Answer[] answers;

    public Question(string description, Answer[] answers)
    {
        this.description = description;

        var rng = new Random();
        this.answers = answers.OrderBy(e => rng.NextDouble()).ToArray();
    }
}
