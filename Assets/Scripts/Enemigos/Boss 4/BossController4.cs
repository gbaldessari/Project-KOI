using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController4 : MonoBehaviour
{
    Vector2 direccion;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;
    public GameObject prefab7;
    private float siguienteDisparo = 0;
    private float siguienteDisparo2 = 0;
    private float anguloActual = 0;
    private List<Transform> objetivos = new();
    private float vidas = 975;
    private bool activo = false;
    private bool encuentroIniciado = false;
    private BossContainer4 bossContainer;
    float sumaDireccion = 45f;

    private bool hit = false;
    public GameObject hitSprite;
    private float temporizadorHit;
    private readonly float tiempoHit = 0.05f;

    private void Start()
    {
        bossContainer = GetComponentInParent<BossContainer4>();
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
            FlechaGuiada(direccionAngulo, 1, 4);
        }
        else if (bossContainer.GetEstado() == 2 && !bossContainer.GetHit())
        {
            AtaqueMandala(0.5f, 30, 5, 5);
        }
        else if (bossContainer.GetEstado() == 3 && !bossContainer.GetHit())
        {
            AtaqueRadial(4, 0.2f, 5, prefab2, 20);
        }
        else if (bossContainer.GetEstado() == 4 && !bossContainer.GetHit())
        {
            AtaqueFlechas(0.7f, 20, 7);
        }
        else if (bossContainer.GetEstado() == 5 && !bossContainer.GetHit())
        {
            AtaqueRadial(3, 0.2f, 8, prefab5, 10);
        }
        else if (bossContainer.GetEstado() == 6 && !bossContainer.GetHit())
        {
            AtaqueCirculo(0.5f, 20, 3);
            FlechaUnica(direccionAngulo, 1.5f, 4);
        }
        else if (bossContainer.GetEstado() == 7 && !bossContainer.GetHit())
        {
            FlechaGuiada(direccionAngulo, 0.9f, 5);
        }
        else if (bossContainer.GetEstado() == 8 && !bossContainer.GetHit())
        {
            AtaqueMandala(0.4f, 36, 4, 6);
        }
        else if (bossContainer.GetEstado() == 9 && !bossContainer.GetHit())
        {
            AtaqueRadial(5, 0.2f, 6, prefab2, 25);
        }
        else if (bossContainer.GetEstado() == 10 && !bossContainer.GetHit())
        {
            AtaqueFlechas(1f, 10, 10);
        }
        else if (bossContainer.GetEstado() == 11 && !bossContainer.GetHit())
        {
            AtaqueRadial(3, 0.1f, 10, prefab5, 10);
        }
        else if (bossContainer.GetEstado() == 12 && !bossContainer.GetHit())
        {
            AtaqueCirculo(0.6f, 30, 4);
            FlechaUnica(direccionAngulo, 2f, 5);
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
    private void AtaqueMandala(float tiempo, int numeroDeProyectiles, float velocidadDelProyectil, int puntas)
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            DisparoMandala(numeroDeProyectiles, velocidadDelProyectil, puntas);
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
    private void AtaqueRadial(float velocidadDelProyectil, float tiempo, int numeroDeRayos,GameObject prefab, float sumaAngulo)
    {
        float anguloPorDisparo = 360 / numeroDeRayos;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            for (int i = 0; i <= numeroDeRayos; i++)
            {
                DisparoRadial(anguloActual + anguloPorDisparo * i, velocidadDelProyectil, prefab);
            }
            anguloActual += sumaAngulo;
        }
    }
    private void AtaqueFlechas(float tiempo, float angulo, int numeroDeFlechas)
    {
        float anguloPorFlecha = 360 / numeroDeFlechas;

        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            for (int i = 0; i < numeroDeFlechas; i++)
            {
                DisparoGuiado(anguloPorFlecha * i + 8 + sumaDireccion, 0.5f, 4, prefab4);
                DisparoGuiado(anguloPorFlecha * i + 4 + sumaDireccion, 0.7f, 4, prefab4);
                DisparoGuiado(anguloPorFlecha * i + sumaDireccion, 0.9f, 4, prefab4);
                DisparoGuiado(anguloPorFlecha * i - 4 + sumaDireccion, 0.7f, 4, prefab4);
                DisparoGuiado(anguloPorFlecha * i - 8 + sumaDireccion, 0.5f, 4, prefab4);
            }
            sumaDireccion += angulo;
        }
    }
    private void FlechaUnica(float direccionAngulo, float tiempo, float velocidadDelProyectil)
    {
        float direccion = (direccionAngulo + 180) * -1;
        if (Time.time > siguienteDisparo2)
        {
            siguienteDisparo2 = Time.time + tiempo;

            DisparoGuiado(direccion + 4, 1f, velocidadDelProyectil, prefab7);
            DisparoGuiado(direccion - 4, 1f, velocidadDelProyectil, prefab7);

            DisparoGuiado(direccion, 1.3f, velocidadDelProyectil, prefab7);
        }
    }
    private void FlechaGuiada(float direccionAngulo, float tiempo, float velocidadDelProyectil)
    {
        float direccion = (direccionAngulo + 180) * -1;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoGuiado(direccion + 6, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 6, 0.7f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion + 3, 1f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 3, 1f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion, 1.3f, velocidadDelProyectil, prefab3);


            DisparoGuiado(direccion + 51, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion + 45, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion + 39, 0.7f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion + 48, 1f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion + 42, 1f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion + 45, 1.3f, velocidadDelProyectil, prefab3);


            DisparoGuiado(direccion - 39, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 45, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 51, 0.7f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion - 42, 1f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 48, 1f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion - 45, 1.3f, velocidadDelProyectil, prefab3);


            DisparoGuiado(direccion + 28.5f, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion + 22.5f, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion + 16.5f, 0.7f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion + 25.5f, 1f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion + 19.5f, 1f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion + 22.5f, 1.3f, velocidadDelProyectil, prefab3);


            DisparoGuiado(direccion - 16.5f, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 22.5f, 0.7f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 28.5f, 0.7f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion - 19.5f, 1f, velocidadDelProyectil, prefab3);
            DisparoGuiado(direccion - 25.5f, 1f, velocidadDelProyectil, prefab3);

            DisparoGuiado(direccion - 22.5f, 1.3f, velocidadDelProyectil, prefab3);
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
            Instantiate(prefab6, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
            anguloActual += anguloPorDisparo;
        }
    }
    private void DisparoRadial(float anguloActual, float velocidadDelProyectil,GameObject prefab)
    {
        float radioDeSpawn = 0.5f;
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        Vector2 posicionInicial = new(Mathf.Sin(((anguloActual * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((anguloActual * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - anguloActual) - (anguloActual * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(prefab, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void DisparoMandala(int numeroDeProyectiles, float velocidadDelProyectil, int puntas)
    {
        float radioDeSpawn = 0.3f;
        float anguloPorDisparo = 360 / numeroDeProyectiles;
        float anguloActual = 0;
        float velocidad;

        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int i = 0; i < numeroDeProyectiles; i++)
        {
            velocidad = 2*(float)Math.Abs(Math.Sin(puntas * anguloActual * (Math.PI / 360)));
            Vector2 posicionInicial = new(Mathf.Sin(((anguloActual * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((anguloActual * Mathf.PI) / 180) + transformUpAngulo));
            Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
            float rotacionZ = (360 - anguloActual) - (anguloActual * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
            Vector2 direccionMovimiento = (velocidad + velocidadDelProyectil) * (posicionInicialRelativa - (Vector2)transform.position).normalized;
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