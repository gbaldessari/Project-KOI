
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FileCreationMenu : MonoBehaviour
{
    [SerializeField] private GameObject gamemodeTitle;
    [SerializeField] private GameObject diffTitle;
    [SerializeField] private GameObject playerTwoSettings;
    [SerializeField] private Button backButton;
    [SerializeField] private Button sumbitButtom;

    private readonly string normalDifficulty = "First time trying a Bullet Hell?\nMultiple lifes and classic rules.";
    private readonly string hardDifficulty = "The ultimate Project-KOI challenge!\nOne life only and slower cooldowns, may you have luck.";
    private readonly string singleGamemode = "Singleplayer mode!\nPlay alone against the world!";
    private readonly string multiGamemode = "Multiplayer mode!\nGet a friend and save the world!";

    Slider[] sliders;
    TMP_InputField[] inputs;
    TMP_Dropdown[] dropdowns;

    private readonly Dictionary<string, int> opciones_colores = new()
    {
        { "White", 1 }, // Blanco
        { "Green", 2 }, // Verde
        { "Red", 3 }, // Rojo
        { "Orange", 4 }, // Naranja
        { "Agito", 94 }, // (Special) Pastel Blue Agito
        { "Nico", 95 }, // (Special) Rosa Pastel Nico
        { "Cexar", 96 }, // (Special) Azul Cexar
        { "Lucas", 97 }, // (Special) Azul Oscuro Lucas
        { "Voraz", 98 }, // (Special) Amarillo Voraz
        { "Based", 99 } // (Special) Negro-Like Based
    };

    // Start is called before the first frame update
    void Awake()
    {
        sliders = GetComponentsInChildren<Slider>();
        inputs = GetComponentsInChildren<TMP_InputField>(true);
        dropdowns = GetComponentsInChildren<TMP_Dropdown>(true);
        sliders[0].onValueChanged.AddListener(value => OnGameModeToggleChange(value)); // Slider Gamemode
        sliders[1].onValueChanged.AddListener(value => OnDifficultyToggleChange(value)); // Slider Difficulty
        sumbitButtom.onClick.AddListener(CreateFile);
        backButton.onClick.AddListener(GlobalSettings.UnsetCreationSlot);
    }

    void CreateFile()
    {
        if (sliders[0].value == 0f) CreateSinglePlayerFile();
        else CreateMultiPlayerFile();
        transform.parent.GetChild(1).GetComponent<MainMenu>().LoadPlayerFiles();
    }

    void CreateSinglePlayerFile()
    {
        int dataKey = GlobalSettings.creationSlot;

        string colorSelected = dropdowns[0].options[dropdowns[0].value].text;
        if (colorSelected.EndsWith("'s Special")) colorSelected = colorSelected[0..^10];

        PlayerPrefs.SetInt(dataKey + "isMultiPlayer", (int) sliders[0].value);
        PlayerPrefs.SetInt(dataKey + "isHardMode", (int) sliders[1].value);
        PlayerPrefs.SetFloat(dataKey + "TimePlayed", 0f);

        PlayerPrefs.SetString(dataKey + "PlayerOneName", inputs[0].text);
        PlayerPrefs.SetInt(dataKey + "PlayerOneColor", opciones_colores[colorSelected]);

        PlayerPrefs.SetInt(dataKey + "FileProgress", 0);

        PlayerPrefs.SetInt(dataKey + "HiScore1", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore2", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore3", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore4", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore5", 0);

        PlayerPrefs.SetInt(dataKey + "NoHit1", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit2", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit3", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit4", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit5", 0);
    }

    void CreateMultiPlayerFile()
    {
        int dataKey = GlobalSettings.creationSlot;

        string colorOneSelected = dropdowns[0].options[dropdowns[0].value].text;
        if (colorOneSelected.EndsWith("'s Special")) colorOneSelected = colorOneSelected[0..^10];

        string colorTwoSelected = dropdowns[1].options[dropdowns[1].value].text;
        if (colorTwoSelected.EndsWith("'s Special")) colorTwoSelected = colorTwoSelected[0..^10];

        PlayerPrefs.SetInt(dataKey + "isMultiPlayer", (int) sliders[0].value);
        PlayerPrefs.SetInt(dataKey + "isHardMode", (int) sliders[1].value);
        PlayerPrefs.SetFloat(dataKey + "TimePlayed", 0f);

        PlayerPrefs.SetString(dataKey + "PlayerOneName", inputs[0].text);
        PlayerPrefs.SetInt(dataKey + "PlayerOneColor", opciones_colores[colorOneSelected]);

        PlayerPrefs.SetString(dataKey + "PlayerTwoName", inputs[1].text);
        PlayerPrefs.SetInt(dataKey + "PlayerTwoColor", opciones_colores[colorTwoSelected]);

        PlayerPrefs.SetInt(dataKey + "FileProgress", 0);

        PlayerPrefs.SetInt(dataKey + "HiScore1", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore2", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore3", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore4", 0);
        PlayerPrefs.SetInt(dataKey + "HiScore5", 0);

        PlayerPrefs.SetInt(dataKey + "NoHit1", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit2", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit3", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit4", 0);
        PlayerPrefs.SetInt(dataKey + "NoHit5", 0);
    }

    void OnEnable()
    {
        foreach (Slider slider in sliders) { slider.value = 0; }
        foreach (TMP_InputField input in inputs) { input.text = ""; }
        gamemodeTitle.GetComponent<TextMeshProUGUI>().text = "Singleplayer";
        diffTitle.GetComponent<TextMeshProUGUI>().text = "Normal";
        sumbitButtom.interactable = false;
    }

    private void Update()
    {
        if (sliders[0].value == 0f && inputs[0].text.Length != 0)
        {
            sumbitButtom.interactable = true;
        }
        else if (sliders[0].value == 1f && inputs[1].text.Length != 0 && inputs[0].text.Length != 0)
        {
            sumbitButtom.interactable = true;
        }
        else
        {
            sumbitButtom.interactable = false;
        }
    }

    void OnGameModeToggleChange(float value)
    {
        if (sliders[0].value == 0f)
        {
            ToolTip.UpdateToolTip(singleGamemode);
            gamemodeTitle.GetComponent<TextMeshProUGUI>().text = "Singleplayer";
            playerTwoSettings.SetActive(false);
        }
        else
        {
            ToolTip.UpdateToolTip(multiGamemode);
            gamemodeTitle.GetComponent<TextMeshProUGUI>().text = "Multiplayer";
            playerTwoSettings.SetActive(true);
        }
    }

    void OnDifficultyToggleChange(float value)
    {
        if (sliders[1].value == 0f)
        {
            ToolTip.UpdateToolTip(normalDifficulty);
            diffTitle.GetComponent<TextMeshProUGUI>().text = "Normal";
        }
        else
        {
            ToolTip.UpdateToolTip(hardDifficulty);
            diffTitle.GetComponent<TextMeshProUGUI>().text = "Hard";
        }
    }

    public void ShowToolTip(bool diffText)
    {
        string selectedString = normalDifficulty;

        if (diffText && sliders[1].value == 1f) selectedString = hardDifficulty;
        else if (!diffText && sliders[0].value == 0f) selectedString = singleGamemode;
        else if (!diffText && sliders[0].value == 1f) selectedString = multiGamemode;

        ToolTip.ShowToolTip(selectedString);
    }

    public void HideToolTip()
    {
        ToolTip.HideToolTip();
    }
}