using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public class QuestionObject
{
    public string question;
    public string right;
    public string neutral;
    public string wrong;
    public SECTION category;
    public bool bonus;
}

[Serializable]
public class RootObject
{
    public QuestionObject[] questions;
}


public class GameManager : MonoBehaviour
{
    public QuestionPanel questionPanel;

    public GameObject boti;
    private Material materialBoti;
    private Color originalColor;

    private Queue<Question> questions = new();

    [SerializeField]
    private float timeToSelect = 0.5f;

    private float selectTimer;
    private bool holding;

    private Answer selectedAnswer;

    public TextMeshProUGUI scoreText;
    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value + (questionPanel.Question.bonus ? 1 : 0);
            scoreText.text = score.ToString();
        }
    }

    public void StartQuestions()
    {
        print("in the end");
        materialBoti = boti.GetComponent<Renderer>().material;
        originalColor = materialBoti.color;

        var jsonQuestions = Resources.Load<TextAsset>("questions");
        var parsedQuestions = JsonUtility.FromJson<RootObject>("{\"questions\":" + jsonQuestions.text + "}");
        foreach (var question in parsedQuestions.questions)
        {
            print(question.category);
            if (!(question.category == SECTION.Generic || question.category == LevelSelection.selectedSection)) continue;
            questions.Enqueue(new Question(question.question, new Answer[] { new Answer(question.right, Type.right), new Answer(question.neutral, Type.neutral), new Answer(question.wrong, Type.wrong)}, question.bonus, question.category));
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
                StartCoroutine(CheckAnswer(selectedAnswer));
                holding = false;
            }
        }
    }

    public void PointerDown(AnswerButton answer) {
        holding = true;
        selectedAnswer = answer.Answer;
    }
    public void PointerUp() {
        holding = false;
        selectTimer = 0;
    }

    private IEnumerator CheckAnswer(Answer answer)
    {
        StartCoroutine(questionPanel.HideAnswers());
        switch (answer.type)
        {
            case Type.right:
                LeanTween.value(gameObject, setColorCallback, originalColor, Color.green, .5f);
                Score++;
                print("Color Green");
                break;
            case Type.neutral:
                LeanTween.value(gameObject, setColorCallback, originalColor, Color.blue, .5f);
                print("Color Blue");
                Score += 0;
                break;
            case Type.wrong:
                LeanTween.value(gameObject, setColorCallback, originalColor, Color.red, .5f);
                print("Color Red");
                Score--;
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1f);
        LeanTween.value(gameObject, setColorCallback, materialBoti.color, originalColor, .5f);
        yield return new WaitForSeconds(1f);
        NextQuestion();
    }

    private void setColorCallback(Color c)
    {
        materialBoti.color = c;

        var tempColor = materialBoti.color;
        tempColor.a = 1f;
        materialBoti.color = tempColor;
    }
}
