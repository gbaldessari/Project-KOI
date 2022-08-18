using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LoadingLevelProgress : MonoBehaviour
{
    private SoundObject soundObject; // Sound Object
    private GameObject levelTitle;
    private GameObject levelNameText;
    private GameObject progressText;
    private GameObject hintText;
    private readonly int loadTime = 1500; // Cantidad de tiempo m�nimo para cargar el nivel (tiempo de carga forzado)
    private bool isTimePassed = false;
    private bool sceneLoadedEventTriggered = false;

    private readonly List<string> hints = new()
    {
        "Get an attack boost by collecting 40 Based Coins without dying!",
        "Press and hold [Slowdown] for extra accuracy",
        "Your life bar recharges over time",
        "If your life bar is completed press [Bomb] to release your special attack, but be careful, it has consequences!",
        "No-hit players are rewarded with a special award",
        "Remember that you can always change the controls!",
        "The vast majority of wildlife has become extinct due to war, only a few species are visible to the naked eye",
        "Bosses can drop an orb of power that will give you an attack boost",
        "Also try Pureya!",
        "Also try Flatworld!",
        "Also try Karlson...if it was already released!",
        "Moai are monolithic human figures carved by the Rapa Nui people on Easter Island in eastern Polynesia between the years 1250 and 1500",
        "The Pudu is a species of South American deer, and is the smallest deer in the world",
        "If you are reading this, you are special to us!",
        "Hi mom, I made a game!",
        "Their own technology allowed us to face them",
        "We believe that they come from the planet KOI-456",
        "They...are like us...they are human",
        "Only you can save the earth...we trust you",
        "Sorry if some translations are wrong, we use google translate xd",
        "It is incredible that they have such advanced technology, can we replicate it?",
        "We believe that its base is on Easter Island, that would explain the moai that are appearing in the rest of the world",
        "The best hint? Enjoy the game",
        "Why are they attacking us?",
        "Be better, be Based",
        "The name of certain warriors unlocks special colors",
        "We didn't start this war... or did we?",
        "See our trailer!! https://www.youtube.com/watch?v=dQw4w9WgXcQ",
        "If you want to play with a gamepad, you can bind the controls!",
        "On hard difficulty you only have one life and your life bar recharges slower, be careful!"
    };


    void Awake()
    {
        soundObject = FindObjectOfType<SoundObject>(); // Obtenemos el Sound Object
        levelTitle = transform.GetChild(0).gameObject;
        levelNameText = transform.GetChild(1).gameObject;
        progressText = transform.GetChild(2).gameObject;
        hintText = transform.GetChild(3).gameObject;
    }

    /// <summary>
    /// Evento de cuando este GameObject es ejecutado
    /// </summary>
    /// <param name="levelNumber">N�mero de nivel a cargar</param>
    /// <param name="levelName">Nombre del nivel a cargar</param>
    public void OnLoadingEvent(int levelNumber, string levelName)
    {
        levelTitle.GetComponent<TextMeshProUGUI>().text = "Mission " + levelNumber; // Canbia el t�tulo del nivel (Ex: Mission 1)
        levelNameText.GetComponent<TextMeshProUGUI>().text = levelName; // Cambia el nombre del nivel
        hintText.GetComponent<TextMeshProUGUI>().text = hints[Random.Range(0, hints.Count)];
        StartCoroutine(LoadScene(levelNumber)); // Carga la escena asincr�nicamente y actualiza el progress bar
    }

    /// <summary>
    /// Convertir� un booleano luego de la cantidad de ms se�alados
    /// </summary>
    private async void LoadingScreenTime()
    {
        await Task.Delay(loadTime); // Pausar� el thread por la cantidad de ms se�alado
        isTimePassed = true; // Permite que la carga se muestre
    }

    /// <summary>
    /// Carga el evento de cuando el nivel se termina de cargar.
    /// </summary>
    private void OnSceneLoaded()
    {
        if (sceneLoadedEventTriggered) return;

        sceneLoadedEventTriggered = true; // Permite que no se reproduzca esta funci�n otra vez
        progressText.GetComponent<Animator>().SetBool("Boing", true); // Hace que el texto de carga tenga una animaci�n de salto
        soundObject.PlayUISpecial(); // Reproduce sonido de carga terminada
    }

    /// <summary>
    /// (Coroutine) Carga una escena asincr�nicamente y muestra su progreso de carga
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadScene(int levelNumber)
    {
        yield return null; // Retorna null en primera carga
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelNumber); // Empieza a cargar la escena por detr�s de c�mara
        asyncOperation.allowSceneActivation = false; // La escena no es mostrar� autom�ticamente una vez cargado
        LoadingScreenTime();

        while (!asyncOperation.isDone || !isTimePassed) // Cuando la carga aun no se haya completado
        {
            progressText.GetComponent<TextMeshProUGUI>().text = "Loading progress: " + (asyncOperation.progress * 100) + "%"; // Muestra la carga actual

            if (asyncOperation.progress >= 0.9f && isTimePassed) // Si la carga ha terminado pr�cticamente
            {
                progressText.GetComponent<TextMeshProUGUI>().text = "Press any key to continue"; // Cambiar al texto de carga terminada
                OnSceneLoaded();

                if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed) // Espera que presiones una tecla para continuar
                    asyncOperation.allowSceneActivation = true; // Activa la escena
            }

            yield return null; // Retorna null para pr�xima carga
        }
    }
}
