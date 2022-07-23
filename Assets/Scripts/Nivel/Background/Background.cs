using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float posicionMinimaOMaxima;
    public GameObject backgroundSiguiente;

    void Start(){}

    //Aqui determinamos que pasara en cada frame
    void FixedUpdate()
    {
        /*Aqui condicionamos que si la imagen de fondo al moverse sobrepasa una posicion ingresada,
        esta se mueve a la posicion de la siguiente imagen de fondo menos la posicion ingresada*/
        if (transform.position.y <= posicionMinimaOMaxima)
        {
            transform.position = backgroundSiguiente.transform.position + new Vector3(0, -posicionMinimaOMaxima, 0);
        }

        //Aqui se suma a la posicion del fondo un vector que hace que se mueva a la velocidad deceada
        transform.position += new Vector3(0, -2f*Time.deltaTime, 0);
    }
}
