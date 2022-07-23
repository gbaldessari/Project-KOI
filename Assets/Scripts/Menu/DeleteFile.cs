using UnityEngine;
using UnityEngine.UI;

public class DeleteFile : MonoBehaviour
{
    private Button primaryButton;
    // Start is called before the first frame update
    void Start()
    {
        primaryButton = GetComponent<Button>();
        primaryButton.onClick.AddListener(Delete);
    }

    // Update is called once per frame
    private void Delete()
    {
        int slotNumber = GlobalSettings.deletingSlot;

        PlayerPrefs.DeleteKey(slotNumber + "FileProgress");

        if (PlayerPrefs.GetInt(slotNumber + "isMultiPlayer") == 1)
        {
            PlayerPrefs.DeleteKey(slotNumber + "PlayerTwoName");
            PlayerPrefs.DeleteKey(slotNumber + "PlayerTwoColor");
        }

        PlayerPrefs.DeleteKey(slotNumber + "isMultiPlayer");
        PlayerPrefs.DeleteKey(slotNumber + "isHardMode");
        PlayerPrefs.DeleteKey(slotNumber + "TimePlayed");

        PlayerPrefs.DeleteKey(slotNumber + "PlayerOneName");
        PlayerPrefs.DeleteKey(slotNumber + "PlayerOneColor");

        PlayerPrefs.DeleteKey(slotNumber + "HiScore1");
        PlayerPrefs.DeleteKey(slotNumber + "HiScore2");
        PlayerPrefs.DeleteKey(slotNumber + "HiScore3");
        PlayerPrefs.DeleteKey(slotNumber + "HiScore4");
        PlayerPrefs.DeleteKey(slotNumber + "HiScore5");

        PlayerPrefs.DeleteKey(slotNumber + "NoHit1");
        PlayerPrefs.DeleteKey(slotNumber + "NoHit2");
        PlayerPrefs.DeleteKey(slotNumber + "NoHit3");
        PlayerPrefs.DeleteKey(slotNumber + "NoHit4");
        PlayerPrefs.DeleteKey(slotNumber + "NoHit5");

        transform.parent.parent.GetChild(1).GetComponent<MainMenu>().LoadPlayerFiles();
        GlobalSettings.UnsetDeletingSlot();
    }
}
