using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyProjectileSpawner: MonoBehaviour
{
    Vector2 direccion; // Direccion hacia un objetivo
    public GameObject proyectil;
    private float siguienteDisparo = 0;
    private bool activo = false;
    private List<Transform> objetivos = new(); // Lista de objetivos
    public bool basado = false;
    public bool volteado = false;
    public bool disparoFijo = false;

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

        if (!basado)
        {
            DisparoUnico(direccionAngulo);
        }
        else
        {
            if (!disparoFijo)
            {
                AtaqueBarrido(direccionAngulo);
            }
            else
            {
                AtaqueBarrido2();
            }
        }
    }
    private void AtaqueBarrido2()
    {
        float direccion = 180;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + 1f;

            DisparoGuiado(direccion + 16, 0.1f, 4);
            DisparoGuiado(direccion + 12, 0.1f, 4);
            DisparoGuiado(direccion + 8, 0.1f, 4);
            DisparoGuiado(direccion + 4, 0.1f, 4);
            DisparoGuiado(direccion, 0.1f, 4);
            DisparoGuiado(direccion - 4, 0.1f, 4);
            DisparoGuiado(direccion - 8, 0.1f, 4);
            DisparoGuiado(direccion - 12, 0.1f, 4);
            DisparoGuiado(direccion - 16, 0.1f, 4);
        }
    }
    private void DisparoUnico(float direccionAngulo)
    {
        float Direccion;
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
            siguienteDisparo = Time.time + 1.25f;
            DisparoGuiado(Direccion, 0.5f,4);
        }
    }
    private void AtaqueBarrido(float direccionAngulo)
    {
        float Direccion;
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
            siguienteDisparo = Time.time + 1.25f;

            DisparoGuiado(Direccion + 12, 0.1f,4);
            DisparoGuiado(Direccion, 0.1f,4);
            DisparoGuiado(Direccion - 12, 0.1f,4);

            DisparoGuiado(Direccion + 12, 0.6f,4);
            DisparoGuiado(Direccion, 0.6f,4);
            DisparoGuiado(Direccion - 12, 0.6f,4);
        }
    }
    private void DisparoGuiado(float direccionAngulo, float radioDeSpawn,float velocidadDelProyectil)
    {
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        Vector2 posicionInicial = new(Mathf.Sin(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - direccionAngulo) - (direccionAngulo * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(proyectil,new Vector3(posicionInicialRelativa.x, posicionInicialRelativa.y, proyectil.transform.position.z), Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Activador")) activo = true;
        else if (collision.CompareTag("Desactivador")) activo = false;
    }
}