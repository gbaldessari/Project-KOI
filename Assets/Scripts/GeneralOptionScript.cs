using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralOptionScript : MonoBehaviour
{
    void OnEnable()
    {
        GetComponentsInChildren<Button>()[0].Select(); // Selecciona el primer botón

    }
}
