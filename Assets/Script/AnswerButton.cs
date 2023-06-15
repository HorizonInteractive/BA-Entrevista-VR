using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    private Answer answer;
    public Answer Answer
    {
        get
        { return answer; }
        set
        {
            answer = value;
            textUI.text = value.description;
        }

    }
}
