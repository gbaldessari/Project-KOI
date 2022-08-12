using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSettingsController : MonoBehaviour
{
    private TMP_InputField inputField;
    private TMP_Dropdown dropdown;
    private Image playerImage;

    private readonly Dictionary<string, string> opciones_colores = new()
    {
        { "White", "#FFFFFF" }, // Blanco
        { "Green", "#4DB030" }, // Verde
        { "Red", "#FF5656" }, // Rojo
        { "Orange", "#F3722C" }, // Naranja
    };

    private readonly Dictionary<string, string> opciones_colores_especiales = new()
    {
        { "Agito", "#72DDF7" }, // (Special) Pastel Blue Agito
        { "Nico", "#FFC8DD" }, // (Special) Rosa Pastel Nico
        { "Céxar", "#0AA0FF" }, // (Special) Azul Céxar
        { "Lucas", "#194780" }, // (Special) Azul Oscuro Lucas
        { "Voraz", "#D7CA3A" }, // (Special) Amarillo Voraz
        { "Based", "#151515" } // (Special) Negro-Like Based
    };

    // Start is called before the first frame update
    void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        playerImage = GetComponentsInChildren<Image>()[4];

        inputField.onValueChanged.AddListener(PlayerNameChecker);
        dropdown.onValueChanged.AddListener(ColorSelected);

        UpdateColorsList();
    }

    private void OnEnable()
    {
        UpdateColorsList();
        dropdown.value = 0;
        dropdown.RefreshShownValue();
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out Color color)) { playerImage.color = color; }
    }

    void PlayerNameChecker(string nombre)
    {
        inputField.text = nombre.Trim();
        if (nombre.Length > 5) inputField.text = nombre.Substring(0, 5);

        if (opciones_colores_especiales.ContainsKey(nombre)) dropdown.options.Add(new TMP_Dropdown.OptionData(nombre + "'s Special"));
        else UpdateColorsList();
    }

    void ColorSelected(int colorIndex)
    {
        string colorText;

        if (colorIndex >= opciones_colores.Keys.Count) colorText = opciones_colores_especiales[dropdown.options[colorIndex].text[0..^10]];
        else colorText = opciones_colores[dropdown.options[colorIndex].text];

        if (ColorUtility.TryParseHtmlString(colorText, out Color color)) { playerImage.color = color; }
    }

    void UpdateColorsList()
    {
        int currentOption = dropdown.value;
        dropdown.ClearOptions();
        foreach (string texto in new List<string>(opciones_colores.Keys)) { dropdown.options.Add(new TMP_Dropdown.OptionData(texto)); }
        dropdown.value = currentOption >= opciones_colores.Keys.Count ? 0 : currentOption;
        if (currentOption != dropdown.value) ColorSelected(dropdown.value);
        dropdown.RefreshShownValue();
    }
}