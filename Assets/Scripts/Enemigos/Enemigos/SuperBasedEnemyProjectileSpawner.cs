using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuperBasedEnemyProjectileSpawner : MonoBehaviour
{
    Vector2 direccion; // Direccion hacia un objetivo
    public GameObject proyectil;
    private float siguienteDisparo = 0;
    private float anguloActual = 0;
    private bool activo = false;
    private List<Transform> objetivos = new(); // Lista de objetivos
    public bool B1 = false;
    public bool B2 = false;
    public bool B3 = false;
    public bool B4 = false;
    public bool B5 = false;
    public bool volteado = false;

    private void Start()
    {
        foreach (PlayerController jugador in FindObjectsOfType<PlayerController>().ToList())
        {
            Transform transform = jugador.GetComponent<Transform>();
            objetivos.AddRange(new List<Transform> { transform });
        }
    }
    void FixedUpdate()
    {
        if (!activo) return;

        try
        {
            direccion = objetivos[UnityEngine.Random.Range(0, objetivos.Count - 1)].position - transform.position;
        }
        catch (MissingReferenceException)
        {
            objetivos = new();
            foreach (PlayerController jugador in FindObjectsOfType<PlayerController>().ToList())
            {
                Transform transform = jugador.GetComponent<Transform>();
                objetivos.AddRange(new List<Transform> { transform });
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            // Los jugadores se acabaron
            return;
        }

        float direccionAngulo = Vector2.SignedAngle(new Vector2(0, -10).normalized, direccion.normalized);

        if (B1)
        {
            AtaqueBarrido(direccionAngulo);
        }
        else if (B2)
        {
            FlechaGuiada(direccionAngulo);
        }
        else if (B3)
        {
            AtaqueCirculo();
        }
        else if (B4)
        {
            GranBarrido(direccionAngulo);
        }
        else if (B5)
        {
            AtaqueRadial();
        }
    }

    private void AtaqueBarrido(float direccionAngulo)
    {
        float Direccion;
        float velocidad = 3;
        if (!volteado)
        {
            Direccion = (direccionAngulo + 180) * -1;

            if (Time.time > siguienteDisparo)
            {
                siguienteDisparo = Time.time + 1.75f;

                DisparoGuiado(Direccion + 12, 0.1f, velocidad);
                DisparoGuiado(Direccion, 0.1f, velocidad);
                DisparoGuiado(Direccion - 12, 0.1f, velocidad);

                DisparoGuiado(Direccion + 12, 0.6f, velocidad);
                DisparoGuiado(Direccion, 0.6f, velocidad);
                DisparoGuiado(Direccion - 12, 0.6f, velocidad);
            }
        }
        else
        {
            Direccion = (direccionAngulo) * -1;

            if (Time.time > siguienteDisparo)
            {
                siguienteDisparo = Time.time + 2;

                DisparoGuiado(Direccion + 12, 0.1f, velocidad);
                DisparoGuiado(Direccion, 0.1f, velocidad);
                DisparoGuiado(Direccion - 12, 0.1f, velocidad);
            }
        }
    }

    private void AtaqueRadial()
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + 0.02f;
            DisparoRadial(anguloActual);
            anguloActual += 10f;
        }
    }
    private void GranBarrido(float direccionAngulo)
    {
        float Direccion;
        float velocidad = 3;
        if (!volteado)
        {
            Direccion = (direccionAngulo + 180) * -1;
        }
        else
        {
            Direccion = (direccionAngulo) * -1;
        }
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + 2f;

            DisparoGuiado(Direccion + 24, 0.1f, velocidad);
            DisparoGuiado(Direccion + 12, 0.3f, velocidad);
            DisparoGuiado(Direccion, 0.5f, velocidad);
            DisparoGuiado(Direccion - 12, 0.3f, velocidad);
            DisparoGuiado(Direccion - 24, 0.1f, velocidad);

            DisparoGuiado(Direccion + 28, 0.6f, velocidad);
            DisparoGuiado(Direccion + 14, 0.8f, velocidad);
            DisparoGuiado(Direccion, 1f, velocidad);
            DisparoGuiado(Direccion - 14, 0.8f, velocidad);
            DisparoGuiado(Direccion - 28, 0.6f, velocidad);
        }
    }

    private void FlechaGuiada(float direccionAngulo)
    {
        float Direccion;
        float velocidad = 3;
        if (!volteado)
        {
            Direccion = (direccionAngulo + 180) * -1;
        }
        else
        {
            Direccion = (direccionAngulo) * -1;
        }
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + 1.75f;

            DisparoGuiado(Direccion + 8, 0.1f, velocidad);
            DisparoGuiado(Direccion + 4, 0.3f, velocidad);
            DisparoGuiado(Direccion, 0.5f, velocidad);
            DisparoGuiado(Direccion - 4, 0.3f, velocidad);
            DisparoGuiado(Direccion - 8, 0.1f, velocidad);

        }
    }
    private void AtaqueCirculo()
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + 0.5f;
            DisparoCirculo();
        }
    }
    private void DisparoGuiado(float direccionAngulo, float radioDeSpawn, float velocidadDelProyectil)
    {
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        Vector2 posicionInicial = new(Mathf.Sin(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - direccionAngulo) - (direccionAngulo * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(proyectil, new Vector3(posicionInicialRelativa.x, posicionInicialRelativa.y, proyectil.transform.position.z), Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void DisparoCirculo()
    {
        int numeroDeProyectiles = 15;
        float radioDeSpawn = 0.3f;
        float velocidadDelProyectil = 3;
        float anguloPorDisparo = 360 / numeroDeProyectiles;
        float anguloActual = 0;

        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int i = 0; i < numeroDeProyectiles; i++)
        {
            Vector2 posicionInicial = new(Mathf.Sin(((anguloActual * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((anguloActual * Mathf.PI) / 180) + transformUpAngulo));
            Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
            float rotacionZ = (360 - anguloActual) - (anguloActual * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
            Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
            Instantiate(proyectil, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
            anguloActual += anguloPorDisparo;
        }
    }

    private void DisparoRadial(float anguloActual)
    {
        float radioDeSpawn = 0.5f;
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);
        float velocidadDelProyectil = 3;

        Vector2 posicionInicial = new(Mathf.Sin(((anguloActual * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((anguloActual * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - anguloActual) - (anguloActual * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(proyectil, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Activador")) activo = true;
        else if (collision.CompareTag("Desactivador")) activo = false;
    }
}