using System;

public enum Type
{
    right,
    neutral,
    wrong
}

[Serializable]
public class Answer
{
    public string description;
    public Type type;

    public Answer(string description, Type type)
    {
        this.type = type;
        this.description = description;
    }
}
