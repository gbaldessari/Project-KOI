using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadLevel : MonoBehaviour
{
    [SerializeField, Range(1, 5)] private int levelNumber;
    [SerializeField] private string levelName;
    [SerializeField] private GameObject loadingLevelScreen;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnLoadLevel);
    }

    /// <summary>
    /// Carga la escena del nivel especificado:
    /// </summary>
    /// <param name="level">Número de nivel especificado</param>
    public void OnLoadLevel() // 
    {
        loadingLevelScreen.SetActive(true); // Activa el menú de carga
        loadingLevelScreen.GetComponent<LoadingLevelProgress>().OnLoadingEvent(levelNumber, levelName);
        gameObject.transform.parent.gameObject.SetActive(false); // Desactiva este gameobject
    }
}
