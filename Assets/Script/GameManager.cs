using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestionObject
{
    public string question;
    public string right;
    public string neutral;
    public string wrong;
}

[Serializable]
public class RootObject
{
    public QuestionObject[] questions;
}


public class GameManager : MonoBehaviour
{
    public QuestionPanel questionPanel;

    private Queue<Question> questions = new();

    [SerializeField]
    private float timeToSelect = 0.5f;

    private float selectTimer;
    private bool holding;

    private AnswerButton selectedAnswer;

    public void Answer()
    {
        NextQuestion();
    }

    private void Start()
    {
        var jsonQuestions = Resources.Load<TextAsset>("questions");
        var parsedQuestions = JsonUtility.FromJson<RootObject>("{\"questions\":" + jsonQuestions.text + "}");
        foreach (var question in parsedQuestions.questions)
        {
            questions.Enqueue(new Question(question.question, new Answer[] { new Answer(question.right, Type.right), new Answer(question.neutral, Type.neutral), new Answer(question.wrong, Type.wrong)}));
        }
        NextQuestion();
    }

    public void NextQuestion()
    {
        if(questions.Count > 0)
        {
            questionPanel.Question = questions.Dequeue();
        }
    }

    private void Update()
    {
        if (holding)
        {
            selectTimer += Time.deltaTime;
            if(selectTimer > timeToSelect)
            {
                Answer();
                holding = false;
            }
        }
    }

    public void PointerDown(AnswerButton answer) {
        holding = true;
        selectedAnswer = answer;
    }
    public void PointerUp() {
        holding = false;
        selectTimer = 0;
    }
}
