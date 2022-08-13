using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponentsInChildren<Button>()[0].Select(); // Selecciona el primer botón
    }
}
