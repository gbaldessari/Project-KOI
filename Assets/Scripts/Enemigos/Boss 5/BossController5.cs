using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController5 : MonoBehaviour
{
    Vector2 direccion;
    public GameObject prefab1;//
    public GameObject prefab2;//
    public GameObject prefab3;//
    public GameObject prefab4;//
    public GameObject prefab5;//
    public GameObject prefab6;//
    public GameObject prefab7;//
    public GameObject prefab8;
    private float siguienteDisparo = 0;
    private float siguienteDisparo2 = 0;
    private float anguloActual = 0;
    private List<Transform> objetivos = new();
    private float vidas = 1150;
    private bool activo = false;
    private bool encuentroIniciado = false;
    private BossContainer5 bossContainer;
    float sumaDireccion = 45f;

    private bool hit = false;
    public GameObject hitSprite;
    private float temporizadorHit;
    private readonly float tiempoHit = 0.05f;

    private void Start()
    {
        bossContainer = GetComponentInParent<BossContainer5>();
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
            AtaqueCirculo(0.5f, 20, 3);
            FlechaUnica(direccionAngulo, 1.5f, 4);
        }
        else if (bossContainer.GetEstado() == 3 && !bossContainer.GetHit())
        {
            AtaqueMandala(0.5f, 30, 5, 5);
        }
        else if (bossContainer.GetEstado() == 4 && !bossContainer.GetHit())
        {
            AtaqueFlechas(0.7f, 20, 7);
        }
        else if (bossContainer.GetEstado() == 5 && !bossContainer.GetHit())
        {
            SuperFlechaGuiada(direccionAngulo, 1.5f, 8);
        }
        else if (bossContainer.GetEstado() == 6 && !bossContainer.GetHit())
        {
            AtaqueFlor(4, 36, 0.3f);
        }
        else if (bossContainer.GetEstado() == 7 && !bossContainer.GetHit())
        {
            AtaqueCuarto(0.25f, 10, false);
        }
        else if (bossContainer.GetEstado() == 8 && !bossContainer.GetHit())
        {
            FlechaGuiada(direccionAngulo, 1.5f, 5);
        }
        else if (bossContainer.GetEstado() == 9 && !bossContainer.GetHit())
        {
            AtaqueCirculo(0.6f, 30, 4);
            FlechaUnica(direccionAngulo, 2f, 5);
        }
        else if (bossContainer.GetEstado() == 10 && !bossContainer.GetHit())
        {
            AtaqueMandala(0.4f, 36, 4, 6);
        }
        else if (bossContainer.GetEstado() == 11 && !bossContainer.GetHit())
        {
            AtaqueFlechas(1f, 10, 10);
        }
        else if (bossContainer.GetEstado() == 12 && !bossContainer.GetHit())
        {
            SuperFlechaGuiada(direccionAngulo, 1.75f, 9);
        }
        else if (bossContainer.GetEstado() == 13 && !bossContainer.GetHit())
        {
            AtaqueFlor(5, 36, 0.2f);
        }
        else if (bossContainer.GetEstado() == 14 && !bossContainer.GetHit())
        {
            AtaqueCuarto(0.75f, 15, true);
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

    private void SuperFlechaGuiada(float direccionAngulo, float tiempo, float velocidadDelProyectil)
    {
        float direccion = (direccionAngulo + 180) * -1;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoGuiado(direccion + 10, 0.1f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion + 6, 0.1f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion + 2, 0.1f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 2, 0.1f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 6, 0.1f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 10, 0.1f, velocidadDelProyectil, prefab8);

            DisparoGuiado(direccion + 8, 0.3f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion + 4, 0.3f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion, 0.3f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 4, 0.3f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 8, 0.3f, velocidadDelProyectil, prefab8);

            DisparoGuiado(direccion + 6, 0.5f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion + 2, 0.5f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 2, 0.5f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 6, 0.5f, velocidadDelProyectil, prefab8);

            DisparoGuiado(direccion + 4, 0.7f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion, 0.7f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 4, 0.7f, velocidadDelProyectil, prefab8);

            DisparoGuiado(direccion + 2, 0.9f, velocidadDelProyectil, prefab8);
            DisparoGuiado(direccion - 2, 0.9f, velocidadDelProyectil, prefab8);

            DisparoGuiado(direccion, 1.1f, velocidadDelProyectil, prefab8);
        }
    }
    private void AtaqueCuarto(float tiempo, float numeroDeProyectiles, bool doble)
    {
        float sumaAngulo = 90 / numeroDeProyectiles;

        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            for (int i = 0; i <= numeroDeProyectiles; i++) DisparoGuiado(45 + sumaAngulo * i + sumaDireccion, 0.3f, 4, prefab5);

            if (doble) for (int i = 0; i <= numeroDeProyectiles; i++) DisparoGuiado(sumaAngulo * i + sumaDireccion + 225, 0.3f, 4, prefab5);

            sumaDireccion += 90;
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
    private void AtaqueFlor(float velocidadDelProyectil, float angulo, float tiempo)
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


            DisparoRadial(anguloActual + 45, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual + 45, velocidadDelProyectil);

            DisparoRadial(anguloActual + 225, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual + 225, velocidadDelProyectil);

            DisparoRadial(anguloActual + 135, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual + 135, velocidadDelProyectil);

            DisparoRadial(anguloActual + 315, velocidadDelProyectil);
            if (anguloActual % 45 != 0) DisparoRadial(-anguloActual + 315, velocidadDelProyectil);

            anguloActual += anguloPorDisparo;
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

            DisparoGuiado(direccion + 4, 0.1f, velocidadDelProyectil, prefab7);
            DisparoGuiado(direccion - 4, 0.1f, velocidadDelProyectil, prefab7);

            DisparoGuiado(direccion, 0.4f, velocidadDelProyectil, prefab7);
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
    private void DisparoMandala(int numeroDeProyectiles, float velocidadDelProyectil, int puntas)
    {
        float radioDeSpawn = 0.3f;
        float anguloPorDisparo = 360 / numeroDeProyectiles;
        float anguloActual = 0;
        float velocidad;

        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int i = 0; i < numeroDeProyectiles; i++)
        {
            velocidad = 2 * (float)Math.Abs(Math.Sin(puntas * anguloActual * (Math.PI / 360)));
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