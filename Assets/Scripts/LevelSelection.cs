using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameManager gameManager;

    public GameObject[] genericItems;

    public GameObject[] gastronomyItems;
    public GameObject[] itItems;
    public GameObject[] economyItems;
    public GameObject[] serviceItems;

    public GameObject[] itemsToDisable;

    public static SECTION selectedSection = SECTION.IT;

    public void SelectSection(int section)
    {
        selectedSection = (SECTION)section;
    }

    public void ConfirmSelection()
    {
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
        gameManager.StartQuestions();
    }
}
