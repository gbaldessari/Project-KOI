using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform objetivo;
    public float posXMinima, posXMaxima;
    private void Start()
    {
        objetivo = FindObjectsOfType<PlayerController>()[0].transform;
    }

    //Aqui determinamos que pasara en cada frame
    void FixedUpdate()
    {
        /*Simplemente transformamos la posicion de la camara a la del jugador, haciendo que se mueva con este,
        pero asigandole un limite a esta, no pudiendo superar una posicion minima y maxima*/
        transform.position = new Vector3(Mathf.Clamp(objetivo.position.x, posXMinima, posXMaxima), transform.position.y, transform.position.z);
    }
}
