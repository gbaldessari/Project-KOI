using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float tiempoDeDestruccion = 0.6f;

    private void Start()
    {
        //Aqui le ponemos un temporizador al proyectil para que se destruya pasado un tiempo
        Destroy(gameObject, tiempoDeDestruccion);
    }
}