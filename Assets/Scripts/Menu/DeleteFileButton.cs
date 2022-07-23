using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DeleteFileButton : MonoBehaviour
{
    private Button primaryButton;
    private GameObject deleteFileConfirmationScreen;

    // TODO añadir el tooltip de advertencia

    readonly Dictionary<string, int> slotNumberNames = new()
    {
        { "One", 1 },
        { "Two", 2 },
        { "Three", 3 }
    };

    private string slotNumberName;
    private int slotNumber;

    void Start()
    {
        deleteFileConfirmationScreen = transform.parent.parent.parent.GetChild(8).gameObject;

        slotNumberName = transform.parent.gameObject.name[4..^0];
        slotNumber = slotNumberNames[slotNumberName];

        primaryButton = GetComponent<Button>();
        primaryButton.onClick.AddListener(ShowDeleteFileConfirmationScreen);
    }

    private void ShowDeleteFileConfirmationScreen()
    {
        GlobalSettings.SetDeletingSlot(slotNumber);
        deleteFileConfirmationScreen.SetActive(true);
        transform.parent.parent.gameObject.SetActive(false);
    }
}
