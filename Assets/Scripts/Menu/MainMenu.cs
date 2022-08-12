using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject FileSelectionMenu;
    [SerializeField] private GameObject LevelSelectionMenu;

    private readonly Dictionary<int, string> colores = new()
    { 
        { 1, "#FFFFFF" }, // Blanco
        { 2, "#00FF00" }, // Verde
        { 3, "#FF0000" }, // Rojo
        { 4, "#F3722C" }, // Naranja
        { 94, "#72DDF7" }, // (Special) Pastel Blue Agito
        { 95, "#FFC8DD" }, // (Special) Rosa Pastel Nico
        { 96, "#0AA0FF" }, // (Special) Azul Céxar
        { 97, "#023E8A" }, // (Special) Azul Oscuro Lucas
        { 98, "#FFEE32" }, // (Special) Amarillo Voraz
        { 99, "#0D060F" } // (Special) Negro-Like Based
    };

    private readonly Dictionary<int, string> slotNumberNames = new()
    {
        { 1, "FileOne" },
        { 2, "FileTwo" },
        { 3, "FileThree" }
    };

    readonly Dictionary<string, int> buttonNumberNames = new()
    {
        { "One", 1 },
        { "Two", 2 },
        { "Three", 3 },
        { "Four", 4 },
        { "Five", 5 }
    };

    private readonly Dictionary<int, Vector3> slotNumberPos = new()
    {
        { 1, new(0, 65, 0) },
        { 2, new(0, -25, 0) },
        { 3, new(0, -115, 0) }
    };

    GameObject blankFilePrefab;
    GameObject singlePlayerSlotPrefab;
    GameObject multiPlayerSlotPrefab;

    private void Awake()
    {
        Time.timeScale = 1f; // Pone el tiempo de vuelta a la normalidad, para evitar errores desde el menú de pausa o diálogos

        blankFilePrefab = Resources.Load<GameObject>("Prefabs/Menus/BlankFile"); // Obtiene el prefab de slot nulo
        singlePlayerSlotPrefab = Resources.Load<GameObject>("Prefabs/Menus/SinglePlayerFile"); // Obtiene el prefab de slot singleplayer
        multiPlayerSlotPrefab = Resources.Load<GameObject>("Prefabs/Menus/MultiPlayerFile"); // Obtiene el prefab de slot singleplayer
    }

    /// <summary>
    /// Se carga si se viene desde otro menú
    /// </summary>
    void OnEnable()
    {
        if (GlobalSettings.activeSlot != 0)
        {
            int fileProgress = PlayerPrefs.GetInt(GlobalSettings.activeSlot + "FileProgress");
            foreach (Button button in LevelSelectionMenu.GetComponentsInChildren<Button>())
            {
                string buttonName = button.gameObject.name;

                if (buttonName.StartsWith("Back"))
                {
                    button.onClick.AddListener(() => { GlobalSettings.UnsetActiveSlot(); });
                    continue;
                };

                int buttonNumber = buttonNumberNames[buttonName[5..^6]];
                if (buttonNumber <= fileProgress + 1) button.interactable = true;
                else button.interactable = false;
            }

            LevelSelectionMenu.SetActive(true);
            LevelSelectionMenu.GetComponentsInChildren<Button>()[0].Select(); // Selecciona el primer botón
            gameObject.SetActive(false);
        }
        else
        {
            GetComponentsInChildren<Button>()[1].Select(); // Selecciona el primer botón
        }

        LoadPlayerFiles();
    }

    /// <summary>
    /// Carga las partidas guardadas disponibles
    /// </summary>
    public void LoadPlayerFiles()
    {
        for (int dataKey = 1; dataKey <= 3; dataKey++) { // Por cada una de los 3 posibles savedatas
            if (!PlayerPrefs.HasKey(dataKey + "FileProgress")) 
            {
                GameObject slot = FileSelectionMenu.transform.Find(slotNumberNames[dataKey]).gameObject;

                if (slot.GetComponentsInChildren<TextMeshProUGUI>()[0].text == "NO SAVEDATA") continue;

                Destroy(slot);
                slot = Instantiate(blankFilePrefab, slotNumberPos[dataKey], Quaternion.identity, FileSelectionMenu.transform);

                slot.name = slotNumberNames[dataKey];
                slot.GetComponent<RectTransform>().localPosition = slotNumberPos[dataKey];
            }
            else
            {
                bool isMultiplayer = PlayerPrefs.GetInt(dataKey + "isMultiPlayer") == 1;
                if (!isMultiplayer) LoadSinglePlayerFile(dataKey);
                else LoadMultiPlayerFile(dataKey);
            }            
        }
    }

    void LoadSinglePlayerFile(int dataKey)
    {
        string difficulty = PlayerPrefs.GetInt(dataKey + "isHardMode") == 1 ? "Hard" : "Normal";
        string gameMode = "Singleplayer";
        string playerName = PlayerPrefs.GetString(dataKey + "PlayerOneName");
        string colorHex = colores[PlayerPrefs.GetInt(dataKey + "PlayerOneColor")];
        float timePlayed = PlayerPrefs.GetFloat(dataKey + "TimePlayed");

        GameObject fileSelectionMenu = transform.parent.Find("FileSelectionMenu").gameObject;
        GameObject slot = fileSelectionMenu.transform.Find(slotNumberNames[dataKey]).gameObject;
        Destroy(slot);
        slot = Instantiate(singlePlayerSlotPrefab, slotNumberPos[dataKey], Quaternion.identity, FileSelectionMenu.transform);

        slot.name = slotNumberNames[dataKey];
        slot.GetComponent<RectTransform>().localPosition = slotNumberPos[dataKey];

        slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerName;
        slot.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = difficulty;
        slot.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = gameMode;
        slot.transform.GetChild(6).GetComponentInChildren<TextMeshProUGUI>().text = ((int)TimeSpan.FromSeconds(timePlayed).TotalHours).ToString("D4") + ":" + TimeSpan.FromSeconds(timePlayed).ToString(@"mm\:ss");
        if (ColorUtility.TryParseHtmlString(colorHex, out Color color)) { slot.transform.GetChild(1).GetComponentsInChildren<Image>()[1].color = color; }

        Transform estrellasBasadas = slot.transform.GetChild(7);
        for (int numeroEstrella = 1; numeroEstrella < 6; numeroEstrella++)
        {
            bool noHit = PlayerPrefs.GetInt(dataKey + "NoHit" + numeroEstrella) == 1;
            GameObject estrella = estrellasBasadas.GetChild(numeroEstrella - 1).gameObject;

            if (noHit) estrella.SetActive(true);
            else estrella.SetActive(false);
        }
    }

    void LoadMultiPlayerFile(int dataKey)
    {
        string difficulty = PlayerPrefs.GetInt(dataKey + "isHardMode") == 1 ? "Hard" : "Normal";
        string gameMode = "Multiplayer";
        string playersName = PlayerPrefs.GetString(dataKey + "PlayerOneName") + " & " + PlayerPrefs.GetString(dataKey + "PlayerTwoName");
        string colorOne = colores[PlayerPrefs.GetInt(dataKey + "PlayerOneColor")];
        string colorTwo = colores[PlayerPrefs.GetInt(dataKey + "PlayerTwoColor")];
        float timePlayed = PlayerPrefs.GetFloat(dataKey + "TimePlayed");

        GameObject fileSelectionMenu = transform.parent.Find("FileSelectionMenu").gameObject;
        GameObject slot = fileSelectionMenu.transform.Find(slotNumberNames[dataKey]).gameObject;
        Destroy(slot);
        slot = Instantiate(multiPlayerSlotPrefab, slotNumberPos[dataKey], Quaternion.identity, FileSelectionMenu.transform);

        slot.name = slotNumberNames[dataKey];
        slot.GetComponent<RectTransform>().localPosition = slotNumberPos[dataKey];

        slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playersName;
        slot.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = difficulty;
        slot.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = gameMode;
        slot.transform.GetChild(7).GetComponentInChildren<TextMeshProUGUI>().text = ((int) TimeSpan.FromSeconds(timePlayed).TotalHours).ToString("D4") + ":" + TimeSpan.FromSeconds(timePlayed).ToString(@"mm\:ss");
        if (ColorUtility.TryParseHtmlString(colorOne, out Color colorOneParsed)) { slot.transform.GetChild(1).GetComponentsInChildren<Image>()[1].color = colorOneParsed; }
        if (ColorUtility.TryParseHtmlString(colorTwo, out Color colorTwoParsed)) { slot.transform.GetChild(2).GetComponentsInChildren<Image>()[1].color = colorTwoParsed; }

        Transform estrellasBasadas = slot.transform.GetChild(8);
        for (int numeroEstrella = 1; numeroEstrella < 6; numeroEstrella++)
        {
            bool noHit = PlayerPrefs.GetInt(dataKey + "NoHit" + numeroEstrella) == 1;
            GameObject estrella = estrellasBasadas.GetChild(numeroEstrella - 1).gameObject;

            if (noHit) estrella.SetActive(true);
            else estrella.SetActive(false);
        }
    }

    /// <summary>
    /// Muestra la secuencia de créditos
    /// </summary>
    public void LoadCredits()
    {
        SceneManager.LoadScene(6); // Carga secuencia de créditos
    }

    /// <summary>
    /// Permite salir del juego
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}