using System;
using System.Linq;

[Serializable]
public class Question
{
    public bool bonus;
    public SECTION category;
    public string description;
    public Answer[] answers;

    public Question(string description, Answer[] answers, bool bonus, SECTION category)
    {
        this.description = description;

        var rng = new Random();
        this.answers = answers.OrderBy(e => rng.NextDouble()).ToArray();
        this.bonus = bonus;
        this.category = category;
    }
}
