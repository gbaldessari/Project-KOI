using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Toggle vsyncToggle;
    public List<ResItem> resolutions = new();

    private int selectedResolution;

    public TMP_Text resolutionLabel;

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponentsInChildren<Button>()[0].Select(); // Selecciona el primer botón

        fullScreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }
    }

    private void Start()
    {
        for(int i = 0;i < resolutions.Count; i++)
        {
            if(Screen.height == resolutions[i].vertical)
            {
                selectedResolution = i;

                UpdateRes();
            }
        }
    }

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateRes();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count-1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateRes();
    }

    public void UpdateRes()
    {
        resolutionLabel.text = ((resolutions[selectedResolution].vertical*16)/9).ToString() + "x" + resolutions[selectedResolution].vertical.ToString();
    }
    public void ApplyGraphics()
    {

        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullScreenToggle.isOn);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}