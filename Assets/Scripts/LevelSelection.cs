using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SECTION
{
    Gastronomy,
    IT,
    Economy,
    Service,
    Generic,
}
public class LevelSelection : MonoBehaviour
{

    public GameObject[] genericItems;
    public Button continueButton;
    public GameObject gameCanvas;

    public GameObject[] gastronomyItems;
    public GameObject[] itItems;
    public GameObject[] economyItems;
    public GameObject[] serviceItems;

    public GameObject[] itemsToDisable;

    public static SECTION selectedSection = SECTION.Generic;

    public void SelectSection(int section)
    {
        selectedSection = (SECTION)section;
        continueButton.interactable = true;
    }

    public void ConfirmSelection()
    {
        if (selectedSection == SECTION.Generic) return;
        switch (selectedSection)
        {
            case SECTION.Gastronomy:
                enableObjects(gastronomyItems);
                break;
            case SECTION.IT:
                enableObjects(itItems);
                break;
            case SECTION.Economy:
                enableObjects(economyItems);
                break;
            case SECTION.Service:
                enableObjects(serviceItems);
                break;
            default:
                break;
        }
        gameCanvas.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    private void enableObjects(GameObject[] items)
    {
        foreach (GameObject item in genericItems)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in items)
        {
            item.SetActive(true);
        }

        foreach (GameObject item in itemsToDisable)
        {
            item.SetActive(false);
        }
        GameManager.instance.InitiateGame();
    }
}
