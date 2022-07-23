using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class ButtonBinding : MonoBehaviour
{
    private GlobalSettings globalSettings; // global settings object
    private static bool isBindingRunning;

    [Range(1, 2)] public int playerIndex;
    [Range(0, 3)] public int actionIndex;
    [Range(0, 3)] public int compositeValue;
    [SerializeField] private bool isComposite;

    public readonly IReadOnlyList<string> COMPOSITEKEYS = new List<string>() {
        "up",
        "down",
        "left",
        "right"
    };

    private InputActionMap accionEntradaJugador; // Obtiene los controles
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private TextMeshProUGUI componenteTexto;
    private Button button;

    private readonly string textoEspera = "Awaiting...";
    private string textoOriginal;


    /// Action 0: Movement
    /// Action 1: Fire
    /// Action 2: BasedMode
    /// Action 3: Slowdown

    private void Start()
    {
        isBindingRunning = false;
        globalSettings = FindObjectOfType<GlobalSettings>(); // Obtenemos el global settings object
        button = GetComponent<Button>();
        componenteTexto = GetComponentInChildren<TextMeshProUGUI>();

        accionEntradaJugador = globalSettings.ObtenerMapaJugador(playerIndex); // Añadimos el mapa de controles del jugador
        componenteTexto.text = textoOriginal = accionEntradaJugador.actions[actionIndex].GetBindingDisplayString(isComposite ? compositeValue+1 : 0); // Actualizamos el texto

        button.onClick.AddListener(StartRebiding); // Añadimos evento al click del botón
    }

    public void StartRebiding()
    {
        if (isBindingRunning) return;
        isBindingRunning = true;

        componenteTexto.text = textoEspera; // Muestra texto Awaiting...
        accionEntradaJugador.actions[actionIndex].Disable(); // Desactiva la acción (en caso de ser necesario)

        rebindingOperation = accionEntradaJugador.actions[actionIndex].PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape") // Arreglar bug letra e
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation => RebindCompleted())
            .OnCancel(operation => RebindCanceled());

        if (isComposite) rebindingOperation.WithTargetBinding(accionEntradaJugador.actions[actionIndex].bindings.IndexOf(x => x.isPartOfComposite && x.name.ToLower() == COMPOSITEKEYS[compositeValue]));

        rebindingOperation.Start();
    }

    public void RebindCompleted()
    {
        rebindingOperation.Dispose(); // Termina el rebinding
        isBindingRunning = false;
        accionEntradaJugador.actions[actionIndex].Enable(); // Activa la acción
        componenteTexto.text = textoOriginal = accionEntradaJugador.actions[actionIndex].GetBindingDisplayString(isComposite ? compositeValue+1 : 0); // Actualiza el texto
        globalSettings.GuardarBinding(playerIndex, actionIndex, isComposite ? compositeValue : 21, rebindingOperation.action.bindings[isComposite ? compositeValue + 1 : 0].ToString().Split(":")[^1], rebindingOperation.action.name);
    }

    public void RebindCanceled()
    {
        rebindingOperation.Dispose(); // Termina el rebinding
        isBindingRunning = false;
        accionEntradaJugador.actions[actionIndex].Enable(); // Activa la acción
        componenteTexto.text = textoOriginal; // Devuelve el texto original
    }
}
