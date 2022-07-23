using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CompletedLevelMenu : MonoBehaviour
{
    private Scripter scripter;

    private GameObject titleText;
    private GameObject timeText;
    private GameObject highScoreText;

    private GameObject recordText;
    private GameObject noHitText;

    private Button exitButton;

    private int finalHighscore;
    private float finalTime;
    private int loadingHighscore = 0;
    private int loadingHighscoreIncrement;
    private float loadingTime = 0f;
    private float loadingTimeIncrement;
    private float timeSinceActive;
    private bool isLoading = false;
    private bool isNewHighscore = false;
    private bool noOneHasReceivedDamage;

    // Start is called before the first frame update
    void Awake()
    {
        scripter = FindObjectOfType<Scripter>(); // Obtiene el Scripter

        titleText = transform.GetChild(0).gameObject;
        timeText = transform.GetChild(5).gameObject;
        highScoreText = transform.GetChild(4).gameObject;
        recordText = transform.GetChild(3).gameObject;
        noHitText = transform.GetChild(6).gameObject;
        exitButton = transform.GetChild(7).GetComponent<Button>();

        recordText.SetActive(false);
        noHitText.SetActive(false);
    }

    private void Update()
    {
        if (!isLoading) return;
        if (timeSinceActive + Time.fixedUnscaledDeltaTime < Time.fixedUnscaledDeltaTime) return;
        timeSinceActive += Time.fixedUnscaledDeltaTime;

        loadingHighscore = Mathf.Clamp(loadingHighscore + loadingHighscoreIncrement, 0, finalHighscore);
        loadingTime = Mathf.Clamp(loadingTime + loadingTimeIncrement, 0, finalTime);

        timeText.GetComponent<TextMeshProUGUI>().text = TimeSpan.FromSeconds(loadingTime).ToString(@"hh\:mm\:ss");
        highScoreText.GetComponent<TextMeshProUGUI>().text = loadingHighscore.ToString("D7");

        if (loadingHighscore == finalHighscore && loadingTime == finalTime)
        {
            if (isNewHighscore) recordText.SetActive(true);
            if (noOneHasReceivedDamage) noHitText.SetActive(true);
            exitButton.interactable = true;
            exitButton.Select();
            isLoading = false;
        }
    }

    public void ShowUp(int levelNumber, int score, int highscore, float time, bool noOneHasReceivedDamage)
    {
        if (levelNumber == 5) exitButton.onClick.AddListener(scripter.CargarCreditos); // Si es nivel 5, se va a los créditos
        else exitButton.onClick.AddListener(scripter.MainMenu); // En caso contrario, se va al mainmenu

        exitButton.interactable = false;
        titleText.GetComponent<TextMeshProUGUI>().text = titleText.GetComponent<TextMeshProUGUI>().text.Replace("x", levelNumber + "");

        finalHighscore = Mathf.Max(score, highscore);
        finalTime = time;

        loadingHighscoreIncrement = finalHighscore / (int) (2 / Time.fixedUnscaledDeltaTime);
        loadingTimeIncrement = finalTime / (2 / Time.fixedUnscaledDeltaTime);

        isNewHighscore = score > highscore;
        this.noOneHasReceivedDamage = noOneHasReceivedDamage;
        isLoading = true;
    }
}
