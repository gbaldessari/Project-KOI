using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class DialogueManager : MonoBehaviour
{
    public Scripter scripter;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public Button mainButton;
    public Image image;
    public Sprite playerImage;

    [HideInInspector] public bool isDialogueActive = false;

    private Queue<Dialogue> dialogues;
    private readonly float typingSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        scripter = FindObjectOfType<Scripter>();

        dialogues = new Queue<Dialogue>();
    }

    public async void StartDialogue(Dialogue[] dialogues)
    {
        try
        {
            if (dialogues.Length == 0) return;
            await Task.Delay(1850); // Para el tiempo por 1500ms por delay

            isDialogueActive = true; // Flag de que el diálogo está activo
            Time.timeScale = 0; // Para el tiempo (pausa el juego)
            animator.SetBool("IsOpen", true); // Animación de aparición del cuadro de diálogo
            await Task.Delay(500); // Para el tiempo por 500ms para dar tiempo a la animación
            mainButton.Select(); // Selecciona el botón de "Continue >>"

            this.dialogues.Clear(); //Limpia la lista de dialogos
            foreach (Dialogue dialogue in dialogues) { this.dialogues.Enqueue(dialogue); }

            DisplayNextSentence();
        }
        catch { };
    }

    public void DisplayNextSentence()
    {
        if (dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();
        Dialogue dialogue = dialogues.Dequeue();
        StartCoroutine(TypeSentence(dialogue));
    }

    IEnumerator TypeSentence(Dialogue dialogue)
    {
        // Font Color
        Color color = dialogue.name == "" ? scripter.playerOneColor : dialogue.color;
        color.a = 1f;
        nameText.colorGradientPreset = new(Color.white, Color.white, color, color);
        dialogueText.colorGradientPreset = new(Color.white, Color.white, color, color);

        // Name
        nameText.text = dialogue.name == "" ? scripter.playerOneName : dialogue.name;

        // Sprite
        image.sprite = dialogue.name == "" ? playerImage : dialogue.image;
        image.color = dialogue.name == "" ? scripter.playerOneColor : Color.white;

        // Sentence
        dialogueText.text = "";
        foreach (char letter in dialogue.sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    private async void EndDialogue()
    {
        EventSystem.current.SetSelectedGameObject(null);
        animator.SetBool("IsOpen", false);
        await Task.Delay(750);
        Time.timeScale = 1;
        isDialogueActive = false;
    }
}
