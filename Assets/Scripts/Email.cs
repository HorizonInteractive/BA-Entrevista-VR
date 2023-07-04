using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Email : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _input;
    [SerializeField]
    private Button _confirmBtn;

    public GameObject emailCanvas, tutorialCanvas;

    private void Awake()
    {
        _input.onSubmit.AddListener(ConfirmInput);
    }

    public void ConfirmInput(string _)
    {
        if (string.IsNullOrEmpty(_input.text)) return;
        GameManager.instance.Email = _input.text;
        LoadNextStage();
    }

    private void LoadNextStage()
    {
        emailCanvas.SetActive(false);
        tutorialCanvas.SetActive(true);
    }

    public void OnEmailValueChange(string email)
    {
        _confirmBtn.interactable = email != "";
    }

    private void OnDisable()
    {
        _input.onSubmit.RemoveListener(ConfirmInput);
    }
}
