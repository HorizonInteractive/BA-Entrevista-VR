using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

[Serializable]
public class Assets
{
    public Sprite backgroundSprite;
    public Sprite answerBackground;
    public Sprite answerHighlight;
    public Color textColor;
    public Color color;
    public Sprite horizonSprite;
    public Sprite nextButtonBackgroundSprite;
    public Color nextButtonColor;
    public Color highlightColor;
    public Color normalColor;

    public Assets(Sprite backgroundSprite, Sprite answerBackground, Color textColor, Color color, Sprite horizonSprite, Sprite nextButtonBackgroundSprite, Color nextButtonColor, Sprite answerHighlight, Color highlightColor, Color normalColor)
    {
        this.backgroundSprite = backgroundSprite;
        this.answerBackground = answerBackground;
        this.textColor = textColor;
        this.color = color;
        this.horizonSprite = horizonSprite;
        this.nextButtonBackgroundSprite = nextButtonBackgroundSprite;
        this.nextButtonColor = nextButtonColor;
        this.answerHighlight = answerHighlight;
        this.highlightColor = highlightColor;
        this.normalColor = normalColor;
    }
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

    [Header("UI Assets")]
    public Image background;
    public Image answer1;
    public Image answer1Highlight;
    public TextMeshProUGUI answer1Text;
    public Image answer2;
    public Image answer2Highlight;
    public TextMeshProUGUI answer2Text;
    public Image answer3;
    public Image answer3Highlight;
    public TextMeshProUGUI answer3Text;
    public Image clock;
    public Image clockFill;
    public Image horizonLogo;
    public Image nextButton;
    public TextMeshProUGUI nextButtonText;
    public TextMeshProUGUI counter;
    public Image loadingIcon;

    public Assets gastronomyAssets;
    public Assets itAssets;
    public Assets serviceAssets;
    public Assets economyAssets;

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
                UpdateAssets(gastronomyAssets);
                break;
            case SECTION.IT:
                enableObjects(itItems);
                UpdateAssets(itAssets);
                break;
            case SECTION.Economy:
                enableObjects(economyItems);
                UpdateAssets(economyAssets);
                break;
            case SECTION.Service:
                enableObjects(serviceItems);
                UpdateAssets(serviceAssets);
                break;
            default:
                break;
        }
        gameCanvas.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    private void UpdateAssets(Assets assets)
    {
        background.sprite = assets.backgroundSprite;
        answer1.sprite = assets.answerBackground;
        answer2.sprite = assets.answerBackground;
        answer3.sprite = assets.answerBackground;
        answer1Highlight.sprite = assets.answerHighlight;
        answer2Highlight.sprite = assets.answerHighlight;
        answer3Highlight.sprite = assets.answerHighlight;
        UpdateColors(answer1, assets.normalColor, assets.highlightColor);
        UpdateColors(answer2, assets.normalColor, assets.highlightColor);
        UpdateColors(answer3, assets.normalColor, assets.highlightColor);
        answer1Text.color = assets.textColor;
        answer2Text.color = assets.textColor;
        answer3Text.color = assets.textColor;
        clock.color = assets.color;
        clockFill.color = assets.color;
        horizonLogo.sprite = assets.horizonSprite;
        nextButton.sprite = assets.nextButtonBackgroundSprite;
        nextButtonText.color = assets.nextButtonColor;
        counter.color = assets.color;
        loadingIcon.color = assets.color;
    }

    private void UpdateColors(Image ImageButton, Color normalColor, Color color)
    {
        var colors = ImageButton.GetComponent<Button>();
        var colorReference = colors.colors;
        colorReference.normalColor = normalColor;
        colorReference.selectedColor = color;
        colors.colors = colorReference;
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
