using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController2 : MonoBehaviour
{
    Vector2 direccion;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    private float siguienteDisparo = 0;
    private float anguloActual = 0;
    private List<Transform> objetivos = new();
    private float vidas = 700;
    private bool activo = false;
    private bool encuentroIniciado = false;
    private BossContainer2 bossContainer;
    float sumaDireccion = 45f;

    private bool hit = false;
    public GameObject hitSprite;
    private float temporizadorHit;
    private readonly float tiempoHit = 0.05f;

    private void Start()
    {
        bossContainer = GetComponentInParent<BossContainer2>();
    }
    void FixedUpdate()
    {
        foreach (PlayerController jugador in FindObjectsOfType<PlayerController>().ToList())
        {
            Transform transform = jugador.GetComponent<Transform>();
            objetivos.AddRange(new List<Transform> { transform });
        }

        if (!activo) return;

        if (anguloActual == 360 || anguloActual == -360 || anguloActual == 720 || anguloActual == -720)
        {
            anguloActual = 0;
        }

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

        if (bossContainer.GetEstado() == 1 && !bossContainer.GetHit())
        {
            encuentroIniciado = true;
            FlechaGuiada(direccionAngulo, 0.5f, 4);
        }
        else if (bossContainer.GetEstado() == 2 && !bossContainer.GetHit())
        {
            AtaqueCirculo(0.4f, 30, 4);
        }
        else if (bossContainer.GetEstado() == 3 && !bossContainer.GetHit())
        {
            AtaqueFlor(4, 36, 0.15f);
        }
        else if (bossContainer.GetEstado() == 4 && !bossContainer.GetHit())
        {
            AtaqueFlechas(0.75f, 45);
        }
        else if (bossContainer.GetEstado() == 5 && !bossContainer.GetHit())
        {
            FlechaGuiada(direccionAngulo, 0.45f, 5);
        }
        else if (bossContainer.GetEstado() == 6 && !bossContainer.GetHit())
        {
            AtaqueCirculo(0.5f, 40, 5);
        }
        else if (bossContainer.GetEstado() == 7 && !bossContainer.GetHit())
        {
            AtaqueFlor(5, 36, 0.1f);
        }
        else if (bossContainer.GetEstado() == 8 && !bossContainer.GetHit())
        {
            AtaqueFlechas(0.6f, 30);
        }

        if (hit)
        {
            temporizadorHit -= Time.deltaTime;
            hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            if (temporizadorHit <= 0)
            {
                hit = false;
                temporizadorHit = tiempoHit;
            }
        }
        else
        {
            hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
        if (bossContainer.GetHit())
        {
            hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
    private void AtaqueCirculo(float tiempo, int numeroDeProyectiles, float velocidadDelProyectil)
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            DisparoCirculo(numeroDeProyectiles, velocidadDelProyectil);
        }
    }
    private void AtaqueFlor(float velocidadDelProyectil,float angulo,float tiempo)
    {
        float anguloPorDisparo = 360 / angulo;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoRadial(anguloActual, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual, velocidadDelProyectil);
            
            DisparoRadial(anguloActual + 180, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual + 180, velocidadDelProyectil);
            
            DisparoRadial(anguloActual + 90, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual + 90, velocidadDelProyectil);

            DisparoRadial(anguloActual + 270, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual + 270, velocidadDelProyectil);
            
            anguloActual += anguloPorDisparo;
        }
    }
    private void AtaqueFlechas(float tiempo,float angulo)
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoGuiado(180 + 12 + sumaDireccion, 0.3f, 4, prefab4);
            DisparoGuiado(180 + 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(180 + 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(180 + sumaDireccion, 0.9f, 4, prefab4);
            DisparoGuiado(180 - 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(180 - 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(180 - 12 + sumaDireccion, 0.3f, 4, prefab4);

            DisparoGuiado(0 + 12 + sumaDireccion, 0.3f, 4, prefab4);
            DisparoGuiado(0 + 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(0 + 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(0 + sumaDireccion, 0.9f, 4, prefab4);
            DisparoGuiado(0 - 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(0 - 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(0 - 12 + sumaDireccion, 0.3f, 4, prefab4);

            DisparoGuiado(90 + 12 + sumaDireccion, 0.3f, 4, prefab4);
            DisparoGuiado(90 + 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(90 + 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(90 + sumaDireccion, 0.9f, 4, prefab4);
            DisparoGuiado(90 - 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(90 - 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(90 - 12 + sumaDireccion, 0.3f, 4, prefab4);

            DisparoGuiado(-90 + 12 + sumaDireccion, 0.3f, 4, prefab4);
            DisparoGuiado(-90 + 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(-90 + 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(-90 + sumaDireccion, 0.9f, 4, prefab4);
            DisparoGuiado(-90 - 4 + sumaDireccion, 0.7f, 4, prefab4);
            DisparoGuiado(-90 - 8 + sumaDireccion, 0.5f, 4, prefab4);
            DisparoGuiado(-90 - 12 + sumaDireccion, 0.3f, 4, prefab4);

            sumaDireccion += angulo;
        }
    }
    private void FlechaGuiada(float direccionAngulo, float tiempo, float velocidadDelProyectil)
    {
        float direccion = (direccionAngulo + 180) * -1;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoGuiado(direccion + 12, 0.4f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion + 4, 0.4f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 4, 0.4f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 12, 0.4f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion + 8, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 8, 0.7f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion + 4, 1f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 4, 1f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion, 1.3f, velocidadDelProyectil, prefab3);
        }
    }
    private void DisparoGuiado(float direccionAngulo, float radioDeSpawn, float velocidadDelProyectil, GameObject prefab)
    {
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        Vector2 posicionInicial = new(Mathf.Sin(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - direccionAngulo) - (direccionAngulo * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(prefab, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void DisparoRadial(float anguloActual, float velocidadDelProyectil)
    {
        float radioDeSpawn = 0.5f;
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        Vector2 posicionInicial = new(Mathf.Sin(((anguloActual * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((anguloActual * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - anguloActual) - (anguloActual * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(prefab2, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void DisparoCirculo(int numeroDeProyectiles, float velocidadDelProyectil)
    {
        float radioDeSpawn = 0.3f;
        float anguloPorDisparo = 360 / numeroDeProyectiles;
        float anguloActual = 0;

        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int i = 0; i < numeroDeProyectiles; i++)
        {
            Vector2 posicionInicial = new(Mathf.Sin(((anguloActual * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((anguloActual * Mathf.PI) / 180) + transformUpAngulo));
            Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
            float rotacionZ = (360 - anguloActual) - (anguloActual * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
            Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
            Instantiate(prefab1, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
            anguloActual += anguloPorDisparo;
        }
    }
    public float GetVidas()
    {
        return vidas;
    }

    public void SetVidas(float vidasIngresadas)
    {
        vidas = vidasIngresadas;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Activador")) activo = true;
        else if (collision.CompareTag("Desactivador")) activo = false;

        //Si colisiona con un objeto con la etiqueta Player o BasedPlayerProjectile, la vida del proyectil se reduce en 2
        if (collision.CompareTag("Player") && encuentroIniciado)
        {
            vidas -= 2;
            hit = true;
        }

        if (collision.CompareTag("BasedPlayerProjectile") && encuentroIniciado)
        {
            vidas -= 0.5f;
            hit = true;
        }

        //Si colisiona con un objeto con la etiqueta PlayerProjectile, la vida del proyectil se reduce en 1
        if (collision.CompareTag("PlayerProjectile") && encuentroIniciado)
        {
            vidas -= 1;
            hit = true;
        }
    }
}