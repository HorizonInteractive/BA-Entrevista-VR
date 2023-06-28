using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LastCanvas : MonoBehaviour
{
    public TextMeshProUGUI counter;
    public GameObject title;
    public Image image;

    private void OnEnable()
    {
        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        Color fromColor = new Color(0, 0, 0, 0);
        Color toColor = new Color(0, 0, 0, 1);

        LeanTween.value(image.gameObject, setColorCallback, fromColor, toColor, 1.5f);
        yield return new WaitForSeconds(1.5f);
        title.SetActive(true);
        counter.gameObject.SetActive(true);
        for (int i = 10; i > 0; i--)
        {
            counter.text = "El juego se reiniciara en " + i;
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void setColorCallback(Color c)
    {
        image.color = c;
        var tempColor = image.color;
        image.color = tempColor;
    }
}
