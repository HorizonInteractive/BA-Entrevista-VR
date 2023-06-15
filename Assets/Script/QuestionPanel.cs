using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;
using System;


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
        if(transform.localScale.x > 0)
        {
            LeanTween.scale(gameObject, Vector3.zero, 0.4f).setEaseOutCubic();
            foreach (AnswerButton button in answerButtons)
            {
                LeanTween.scale(button.gameObject, Vector3.zero, 0.4f).setEaseOutCubic();
            }
            yield return new WaitForSeconds(0.6f);
        }
        UpdateAnswers();
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutCubic();
        yield return new WaitForSeconds(0.4f);
        foreach (char character in question.description)
        {
            textUI.text += character;
            yield return new WaitForSeconds(0.05f);
        }
        foreach (AnswerButton button in answerButtons)
        {
            LeanTween.scale(button.gameObject, Vector3.one, 0.4f).setEaseOutCubic();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
