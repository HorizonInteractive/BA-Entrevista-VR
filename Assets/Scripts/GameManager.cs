using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    public Animator chairAnimator;

    public static GameManager instance;

    public QuestionPanel questionPanel;

    private Queue<Question> questions = new();

    private Answer selectedAnswer;

    public GameObject[] UiElements;
    public GameObject loadingUI;

    private bool isChecking;

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
    public Animator botiAnim;
    // Particles
    [SerializeField] ParticleSystem P_Happy;
    [SerializeField] ParticleSystem P_Confused;
    [SerializeField] ParticleSystem P_Angry;
    //Audio Clips
    [SerializeField] AudioSource C_Happy;
    [SerializeField] AudioSource C_Confused;
    [SerializeField] AudioSource C_Angry;
 
    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            var pointsWon = (value + (questionPanel.Question.bonus ? 1 : 0)) - score;
            switch (pointsWon)
            {
                case 0:
                    botiAnim.SetTrigger("Confused");
                    if (C_Confused != null) C_Confused.Play();
                    P_Confused.Play();
                    break;

                case int n when n > 0:
                    botiAnim.SetTrigger("Happy");
                    if (C_Happy != null) C_Happy.Play();
                    P_Happy.Play();
                    break;

                case int n when n < 0:
                    botiAnim.SetTrigger("Angry");
                    if (C_Angry != null) C_Angry.Play();
                    P_Angry.Play();
                    break;

                default:
                    break;
            }
            score = value + (questionPanel.Question.bonus ? 1 : 0);
            scoreText.text = score.ToString();
        }
    }

    public GameObject GameCanvas;
    public GameObject EndCanvas;
    private int rightCount;
    public TextMeshProUGUI rightText;
    public TextMeshProUGUI endText;

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

    public Color originalColor;

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
        clock.color = originalColor;
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

        var jsonQuestions = Resources.Load<TextAsset>("questions");
        var parsedQuestions = JsonUtility.FromJson<RootObject>("{\"questions\":" + jsonQuestions.text + "}");
        foreach (var question in parsedQuestions.questions)
        {
            if (!(question.category == SECTION.Generic || question.category == LevelSelection.selectedSection)) continue;
            questions.Enqueue(new Question(question.question, new Answer[] { new Answer(question.right, Type.right), new Answer(question.neutral, Type.neutral), new Answer(question.wrong, Type.wrong)}, question.bonus, question.category));
        }
        var randomizedQueue = new Queue<Question>(questions.OrderBy(_ => Guid.NewGuid()));

        // Filter the queue to get 2 elements with Bonus = true
        var trueQueue = new Queue<Question>(randomizedQueue.Where(item => item.bonus).Take(2));

        // Filter the queue to get 5 elements with Bonus = false
        var falseQueue = new Queue<Question>(randomizedQueue.Where(item => !item.bonus).Take(5));

        // Combine the true and false queues
        var combinedQueue = new Queue<Question>(trueQueue.Concat(falseQueue).OrderBy(_ => Guid.NewGuid()));
        questions = combinedQueue;
        NextQuestion();
    }

    [SerializeField] AudioSource C_Win;
    [SerializeField] AudioSource C_Lose;
    [SerializeField] AudioSource C_RocketChair;

    public void NextQuestion()
    {
        if (AnswerIndex >= 7)
        {
            GameCanvas.SetActive(false);
            timeRunning = false;
            EndCanvas.SetActive(true);
            rightText.text = rightCount.ToString() + "/7";
            if(rightCount >= 4 && Score > 3)
            {
                if (C_Win != null) C_Win.Play();
                StartCoroutine(Win());
            }
            switch (Score)
            {
                case int n when n < 3 || rightCount < 4:
                    endText.text = "Nosotros te llamamos. Buen viaje.";
                    chairAnimator.SetTrigger("Fly");
                    if (C_Lose != null)  C_Lose.Play();
                    if (C_RocketChair != null)  C_RocketChair.Play();
                    break;
                case int n when n == 3 || n == 4:
                    endText.text = "Casi te vas volando. Bien hecho pero puedes mejorar.";
                    break;
                case int n when n == 5 || n == 6 || n == 7:
                    endText.text = "Muy bien, pero podrías hacerlo mejor. Te vamos a dejar en la base de datos.";
                    break;
                case 8:
                    endText.text = "Casi perfecto, te vamos a llamar para otra entrevista. ¡Segui mejorando!";
                    break;
                case 9:
                    endText.text = "¡Felicidades! Quedaste contratado.";
                    break;
                default:
                    break;
            }
        }
        if (questions.Count > 0 && AnswerIndex < 7)
        {
            AnswerIndex++;
            questionPanel.Question = questions.Dequeue();
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(3);
        botiAnim.SetTrigger("Handshake");
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
                if (timeLeft < totalTime / 2)
                {
                    clock.color = Color.yellow;
                }
                if (timeLeft < totalTime / 4)
                {
                    clock.color = Color.red;
                }
                clock.fillAmount = timeLeft / totalTime;
                timeLeft -= Time.deltaTime;
            }
        }
    }

    private IEnumerator CheckAnswer(Answer answer)
    {
        if (!isChecking)
        {
            isChecking = true;
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
            isChecking = false;
        }
    }

    //private void setColorCallback(Color c)
    //{
    //    materialBoti.color = c;

    //    var tempColor = materialBoti.color;
    //    tempColor.a = 1f;
    //    materialBoti.color = tempColor;
    //}
}
