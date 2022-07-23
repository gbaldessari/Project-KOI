using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    public float tiempoDeSpawn = 5;
    private float temporizador;

    public int limiteMenor = -9;
    public int limiteMayor = 3;

    public GameObject prefab0;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;
    public GameObject prefab7;
    public GameObject prefab8;
    public GameObject prefab9;
    private GameObject prefabSeleccionado;

    public int probabilidad0 = 200;
    public int probabilidad1 = 200;
    public int probabilidad2 = 200;
    public int probabilidad3 = 200;
    public int probabilidad4 = 200;
    public int probabilidad5 = 200;
    public int probabilidad6 = 200;
    public int probabilidad7 = 200;
    public int probabilidad8 = 200;
    public int probabilidad9 = 1;

    private float PosZ = 0;

    void Start()
    {
        temporizador = tiempoDeSpawn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        temporizador -= Time.deltaTime;
        if (temporizador <= 0)
        {
            int numeroAleatorio = Random.Range(1, probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3 + probabilidad4 + probabilidad5 + probabilidad6 + probabilidad7 + probabilidad8 + probabilidad9 + 1);

            if (numeroAleatorio < probabilidad0)
            {
                prefabSeleccionado = prefab0;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1)
            {
                prefabSeleccionado = prefab1;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2)
            {
                prefabSeleccionado = prefab2;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3)
            {
                prefabSeleccionado = prefab3;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3 + probabilidad4)
            {
                prefabSeleccionado = prefab4;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3 + probabilidad4 + probabilidad5)
            {
                prefabSeleccionado = prefab5;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3 + probabilidad4 + probabilidad5 + probabilidad6)
            {
                prefabSeleccionado = prefab6;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3 + probabilidad4 + probabilidad5 + probabilidad6 + probabilidad7)
            {
                prefabSeleccionado = prefab7;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3 + probabilidad4 + probabilidad5 + probabilidad6 + probabilidad7 + probabilidad8)
            {
                prefabSeleccionado = prefab8;
            }
            else if (numeroAleatorio < probabilidad0 + probabilidad1 + probabilidad2 + probabilidad3 + probabilidad4 + probabilidad5 + probabilidad6 + probabilidad7 + probabilidad8 + probabilidad9)
            {
                prefabSeleccionado = prefab9;
            }

            float posicionX = Random.Range(limiteMenor, limiteMayor);
            Vector3 posicion = new(posicionX, transform.position.y, transform.position.z+PosZ);

            int flipNum = Random.Range(0, 2);
            bool flipBool;

            if (flipNum == 1) flipBool = true;
            else flipBool = false;
            GameObject prefabIntanciado = Instantiate(prefabSeleccionado, posicion, Quaternion.identity);
            prefabIntanciado.GetComponent<SpriteRenderer>().flipX = flipBool;
            temporizador = tiempoDeSpawn;

            PosZ += 0.00001f;
        }
    }
}