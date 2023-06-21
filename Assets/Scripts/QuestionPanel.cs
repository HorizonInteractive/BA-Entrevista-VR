using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class QuestionPanel : MonoBehaviour
{
    public AnswerButton[] answerButtons;
    public TextMeshProUGUI textUI;
    private Question question;
    public Question Question
    {
        get
        { return question; }
        set
        {
            question = value;
            textUI.text = "";
            StartCoroutine(AnimateQuestion());
        }
    }

    private void UpdateAnswers()
    {
        for (int index = 0; index < 3; index++)
        {
            answerButtons[index].Answer = question.answers[index];
        }
    }

    IEnumerator AnimateQuestion()
    {
        UpdateAnswers();
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutCubic();
        yield return new WaitForSeconds(0.4f);
        foreach (char character in question.description)
        {
            textUI.text += character;
            yield return new WaitForSeconds(0.10f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GameManager.instance.ShowAnswers());
    }

    public void HideQuestion()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.4f).setEaseOutCubic();
    }
}
