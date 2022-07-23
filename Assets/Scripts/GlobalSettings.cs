using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalSettings : MonoBehaviour
{
    public static bool accionesEntradaInitialized = false;
    public static List<InputActionMap> accionEntradaJugadores;

    public static int creationSlot; // Número de slot al crear una partida nueva
    public static int deletingSlot; // Número de slot al eliminar una partida
    public static int activeSlot; // Número de slot al jugar

    public readonly IReadOnlyList<string> CONTROLLSKEYS = new List<string>() {
        "CONTROLS-1-0-0-Movement",
        "CONTROLS-1-0-1-Movement",
        "CONTROLS-1-0-2-Movement",
        "CONTROLS-1-0-3-Movement",
        "CONTROLS-1-1-21-Fire",
        "CONTROLS-1-3-21-Slowdown",
        "CONTROLS-1-2-21-BasedMode",
        "CONTROLS-2-0-0-Movement",
        "CONTROLS-2-0-1-Movement",
        "CONTROLS-2-0-2-Movement",
        "CONTROLS-2-0-3-Movement",
        "CONTROLS-2-1-21-Fire",
        "CONTROLS-2-3-21-Slowdown",
        "CONTROLS-2-2-21-BasedMode"
    };

    public readonly IReadOnlyList<string> MOVEMENTKEYS = new List<string>() {
        "Up",
        "Down",
        "Left",
        "Right"
    };

    // Start is called before the first frame update
    void Awake()
    {
        if (accionesEntradaInitialized) return;

        accionEntradaJugadores = new();
        accionEntradaJugadores.Add(new InputActions().asset.actionMaps[1]);
        accionEntradaJugadores.Add(new InputActions().asset.actionMaps[2]);

        CargarBinding();
    }

    public InputActionMap ObtenerMapaJugador(int index)
    {
        if (index != 1 && index != 2) return accionEntradaJugadores[0];
        return accionEntradaJugadores[index - 1];
    }

    public void CargarBinding()
    { 
        foreach (string key in CONTROLLSKEYS)
        {
            if (!PlayerPrefs.HasKey(key)) continue;

            string value = PlayerPrefs.GetString(key);
            string[] partes = key.Split("-");
            int playerIndex = int.Parse(partes[1]);
            int actionIndex = int.Parse(partes[2]);
            int compositeValue = int.Parse(partes[3]);
            string name = partes[4];

            if (compositeValue == 21) accionEntradaJugadores[playerIndex - 1].actions[actionIndex].ChangeBinding(0).WithPath(value);
            else accionEntradaJugadores[playerIndex - 1].actions[actionIndex].ChangeCompositeBinding(name).NextPartBinding(MOVEMENTKEYS[compositeValue]).WithPath(value);
        }
    }

    public void GuardarBinding(int playerIndex, int actionIndex, int compositeValue, string newBinding, string name)
    {
        PlayerPrefs.SetString("CONTROLS-" + playerIndex + "-" + actionIndex + "-" + compositeValue + "-" + name, newBinding);
    }

    public static void SetCreationSlot(int slotFileNumberCreation)
    {
        GlobalSettings.creationSlot = slotFileNumberCreation;
    }

    public static void UnsetCreationSlot()
    {
        GlobalSettings.creationSlot = 0;
    }

    public static void SetDeletingSlot(int slotFileNumberToDelete)
    {
        GlobalSettings.deletingSlot = slotFileNumberToDelete;
    }

    public static void UnsetDeletingSlot()
    {
        GlobalSettings.deletingSlot = 0;
    }

    public static void SetActiveSlot(int slotFile)
    {
        GlobalSettings.activeSlot = slotFile;
    }

    public static void UnsetActiveSlot()
    {
        GlobalSettings.activeSlot = 0;
    }
}
