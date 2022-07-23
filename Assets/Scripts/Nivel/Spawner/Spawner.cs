using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    public float disparosPorSegundo;
    private float siguienteDisparo = 0;
    public bool aleatorio = true;
    public int probabilidadSegundoPrefab = 0;
    private int contadorBasado = 1;
    private int contador = 0;

    public int numeroDeEnemigos = 3;
    private bool activo = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activo == true)
        {
            if (contador == numeroDeEnemigos)
            {
                activo = false;
            }
            if (activo)
            {
                if (Time.time > siguienteDisparo)
                {
                    siguienteDisparo = Time.time + 1 / disparosPorSegundo;
                    Disparo();
                }
            }
        }
    }

    void Disparo()
    {
        if (aleatorio == true)
        {
            int numeroBasado = Random.Range(1, probabilidadSegundoPrefab + 1);
            if (numeroBasado < probabilidadSegundoPrefab)
            {
                Instantiate(prefab1, transform.position, Quaternion.identity);
                contador++;
            }
            else
            {
                Instantiate(prefab2, transform.position, Quaternion.identity);
                contador++;
            }
        }
        else
        {
            if ((contadorBasado % 2) != 0)
            {
                Instantiate(prefab1, transform.position, Quaternion.identity);
                contadorBasado++;
                contador++;
            }
            else if((contadorBasado % 2) == 0)
            {
                Instantiate(prefab2, transform.position, Quaternion.identity);
                contadorBasado++;
                contador++;
            }
        }
    }

    public void Inicio()
    {
        activo = true;
    }

    public bool Final()
    {
        return activo;
    }
}
