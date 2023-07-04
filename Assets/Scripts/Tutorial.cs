using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public GameObject tutorialCanvas;
    public GameObject game;

    public void Continue()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(true);
    }

    public void Continue2()
    {
        tutorial2.SetActive(false);
        tutorial3.SetActive(true);
    }

    public void Finish()
    {
        tutorialCanvas.SetActive(false);
        game.SetActive(true);
    }
}
