using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Scripter scripter;
    private Slider slider;

    private Color color = Color.white;
    private string titulo = "Player X";
    private bool isCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        scripter = FindObjectOfType<Scripter>(); // Obtiene el Scripter
        slider = gameObject.GetComponent<Slider>();

        if (!scripter.isHardMode) slider.value = 0.5f; // Rellena la barra a la mitad si no es hardmode
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldown) SetColor(color);

        if (slider.value == 1f) SetTitleBased();
        else SetTitle(titulo);
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void AddValue(float value)
    {
        if (isCooldown || slider == null) return;
        slider.value += value;
    }

    public void SetTitle(string nuevoTitulo)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = titulo = nuevoTitulo;
    }

    private void SetTitleBased()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = titulo + " [B]";
    }

    public void SetColor(Color colorToSet)
    {
        transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color = colorToSet;
    }

    public float GetValue()
    {
        return slider == null ? 0f : slider.value;
    }

    public void SetCoolDown(bool value)
    {
        isCooldown = value;
        if (isCooldown) transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
    }
}
