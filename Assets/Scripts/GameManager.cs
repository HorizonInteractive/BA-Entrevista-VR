using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public static GameManager instance;

    public QuestionPanel questionPanel;

    public GameObject boti;
    private Material materialBoti;

    private Queue<Question> questions = new();

    private Answer selectedAnswer;

    public GameObject[] UiElements;
    public GameObject loadingUI;

    private float totalTime = 45f;
    private float timeLeft;
    private bool timeRunning;

    private int answerIndex;
    private int AnswerIndex
    {
        get { return answerIndex; }
        set { 
            answerIndex = value;
            answerStep.text = value.ToString() + "/7";
        }
    }
    public TextMeshProUGUI answerStep;

    public Image clock;

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

    public GameObject GameCanvas;
    public GameObject EndCanvas;
    private int rightCount;
    public TextMeshProUGUI rightText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }

        timeLeft = totalTime;
    }

    public IEnumerator HideAnswers()
    {
        foreach (GameObject element in UiElements)
        {
            LeanTween.scale(element, Vector3.zero, 0.4f).setEaseOutCubic();
        }
        yield return new WaitForSeconds(0.4f);
        clock.fillAmount = 1;
        LeanTween.scale(loadingUI, Vector3.one, 0.6f).setEaseOutCubic();
    }

    public IEnumerator ShowAnswers()
    {
        LeanTween.scale(loadingUI, Vector3.zero, 0.4f).setEaseOutCubic();
        yield return new WaitForSeconds(0.4f);
        foreach (GameObject button in UiElements)
        {
            LeanTween.scale(button, Vector3.one, 0.6f).setEaseOutCubic();
        }
        yield return new WaitForSeconds(0.6f);
        timeRunning = true;
    }

    public void InitiateGame()
    {
        StartCoroutine(StartQuestions());
    }

    public IEnumerator StartQuestions()
    {
        yield return new WaitForSeconds(5f);
        materialBoti = boti.GetComponent<Renderer>().material;

        var jsonQuestions = Resources.Load<TextAsset>("questions");
        var parsedQuestions = JsonUtility.FromJson<RootObject>("{\"questions\":" + jsonQuestions.text + "}");
        foreach (var question in parsedQuestions.questions)
        {
            if (!(question.category == SECTION.Generic || question.category == LevelSelection.selectedSection)) continue;
            questions.Enqueue(new Question(question.question, new Answer[] { new Answer(question.right, Type.right), new Answer(question.neutral, Type.neutral), new Answer(question.wrong, Type.wrong)}, question.bonus, question.category));
        }
        NextQuestion();
    }

    public void NextQuestion()
    {
        if (AnswerIndex >= 7)
        {
            GameCanvas.SetActive(false);
            timeRunning = false;
            EndCanvas.SetActive(true);
            rightText.text = "Contestaste " + rightCount.ToString() + "/7 preguntas correctamente";
        }
        if (questions.Count > 0 && AnswerIndex < 7)
        {
            AnswerIndex++;
            questionPanel.Question = questions.Dequeue();
        }
    }

    public void SelectAnswer(AnswerButton answer) {
        selectedAnswer = answer.Answer;
    }

    public void ConfirmAnswer()
    {
        if (LeanTween.isTweening(loadingUI) || selectedAnswer == null) return;
        timeRunning = false;
        timeLeft = totalTime;
        StartCoroutine(CheckAnswer(selectedAnswer));
    }

    private void Update()
    {
        if (timeRunning)
        {
            if(timeLeft <= 0)
            {
                timeLeft = totalTime;
                timeRunning = false;
                StartCoroutine(CheckAnswer(new Answer("", Type.wrong)));
            }
            else
            {
                clock.fillAmount = timeLeft / totalTime;
                timeLeft -= Time.deltaTime;
            }
        }
    }

    private IEnumerator CheckAnswer(Answer answer)
    {
        questionPanel.HideQuestion();
        StartCoroutine(HideAnswers());
        yield return new WaitForSeconds(1f);
        switch (answer.type)
        {
            case Type.right:
                rightCount++;
                Score++;
                break;
            case Type.neutral:
                Score += 0;
                break;
            case Type.wrong:
                Score--;
                break;
            default:
                break;
        }
        selectedAnswer = null;
        yield return new WaitForSeconds(4f);
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
