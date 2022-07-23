using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    private SoundObject soundObject; // Sound Object
    private Animator animator; // El animador de los cr�ditos
    private float time; // Tiempo transcurrido
    private float finalTime; // Tiempo final para skipear creditos

    // Start is called before the first frame update
    void Start()
    {
        soundObject = FindObjectOfType<SoundObject>(); // Obtenemos el Sound Object
        soundObject.GetAndPlayMusicBackground(3); // Reproduce la m�sica Cereza
        animator = GetComponent<Animator>(); // Obtiene el animator

        finalTime = 1000 / (2 / Time.deltaTime);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) MainMenu(); // Si termin� la animaci�n de los cr�ditos, devuelve al MainMenu
    }

    public void MainMenu()
    {
        if (time < finalTime) return;
        soundObject.GetAndPlayMusicBackground(0); // Reproduce la m�sica del main menu
        SceneManager.LoadScene(0); // Se devuelve al Main Menu
    }
}
