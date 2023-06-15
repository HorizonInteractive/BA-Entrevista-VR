using System;

[Serializable]
public class Question
{
    public string description;
    public Answer[] answers;

    public Question(string description, Answer[] answers)
    {
        this.description = description;
        this.answers = answers;
    }
}
