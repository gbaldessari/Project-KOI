using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    private SoundObject soundObject; // Sound Object
    private Animator animator; // El animador de los créditos
    private float time; // Tiempo transcurrido
    private float finalTime; // Tiempo final para skipear creditos

    // Start is called before the first frame update
    void Start()
    {
        soundObject = FindObjectOfType<SoundObject>(); // Obtenemos el Sound Object
        soundObject.GetAndPlayMusicBackground(3); // Reproduce la música Cereza
        animator = GetComponent<Animator>(); // Obtiene el animator

        finalTime = 1000 / (2 / Time.deltaTime);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) MainMenu(); // Si terminó la animación de los créditos, devuelve al MainMenu
    }

    public void MainMenu()
    {
        if (time < finalTime) return;
        soundObject.GetAndPlayMusicBackground(0); // Reproduce la música del main menu
        SceneManager.LoadScene(0); // Se devuelve al Main Menu
    }
}
