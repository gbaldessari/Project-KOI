using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadFileButton : MonoBehaviour
{
    private GameObject fileCreationMenu;
    private GameObject levelSelectionMenu;
    private Button primaryButton;

    private string slotNumberName;
    private int slotNumber;

    readonly Dictionary<string, int> slotNumberNames = new()
    {
        { "One", 1 },
        { "Two", 2 },
        { "Three", 3 },
        { "Four", 4 }, // For button loading
        { "Five", 5 } // For button loading
    };

    private void Start()
    {
        slotNumberName = transform.parent.gameObject.name[4..^0];
        slotNumber = slotNumberNames[slotNumberName];

        fileCreationMenu = transform.parent.parent.parent.GetChild(5).gameObject;
        levelSelectionMenu = transform.parent.parent.parent.GetChild(6).gameObject;

        primaryButton = GetComponent<Button>();
        primaryButton.onClick.AddListener(LoadFile);
    }

    public void LoadFile()
    {
        if (PlayerPrefs.HasKey(slotNumber + "FileProgress"))
        {
            GlobalSettings.SetActiveSlot(slotNumber);
            int fileProgress = PlayerPrefs.GetInt(slotNumber + "FileProgress");

            foreach (Button button in levelSelectionMenu.GetComponentsInChildren<Button>())
            {
                string buttonName = button.gameObject.name;

                if (buttonName.StartsWith("Back"))
                {
                    button.onClick.AddListener(() => { GlobalSettings.UnsetActiveSlot(); });
                    continue;
                };

                int buttonNumber = slotNumberNames[buttonName[5..^6]];
                if (buttonNumber <= fileProgress + 1) button.interactable = true;
                else button.interactable = false;
            }

            levelSelectionMenu.SetActive(true);
            transform.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            GlobalSettings.SetCreationSlot(slotNumber);
            fileCreationMenu.SetActive(true);
            transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
