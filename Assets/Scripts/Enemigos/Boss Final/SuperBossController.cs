using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuperBossController : MonoBehaviour
{
    Vector2 direccion;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;
    public GameObject prefab7;
    public GameObject prefab8;
    public GameObject prefab9;
    private float siguienteDisparo = 0;
    private float siguienteDisparo2 = 0;
    private float anguloActual = 0;
    private List<Transform> objetivos = new();
    private float vidas = 1700;
    private bool activo = false;
    private bool encuentroIniciado = false;
    private SuperBossContainer bossContainer;
    float sumaDireccion = 45f;
    float sumaDireccion2 = 45;
    float sumaDireccion3 = 0;
    float sumaAngulo = 0;
    readonly float tiempoCambio = 12;
    float temporizadorCambio = 12;
    float tiempoCambio2 = 3;
    float temporizadorCambio2 = 3;
    float movimiento = 1;

    private bool hit = false;
    public GameObject hitSprite;
    private float temporizadorHit;
    private readonly float tiempoHit = 0.05f;

    private void Start()
    {
        bossContainer = GetComponentInParent<SuperBossContainer>();
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
            FlechaGuiada(direccionAngulo, 1.25f, 5);
        }
        else if (bossContainer.GetEstado() == 2 && !bossContainer.GetHit())
        {
            AtaqueCuarto(0.25f, 45);
            AtaqueCubos(1, 30, 4);
        }
        else if (bossContainer.GetEstado() == 3 && !bossContainer.GetHit())
        {
            AtaqueCirculo(0.25f, 30, 4);
        }
        else if (bossContainer.GetEstado() == 4 && !bossContainer.GetHit())
        {
            AtaqueFlechas(0.7f, 15, 8);
        }
        else if (bossContainer.GetEstado() == 5 && !bossContainer.GetHit())
        {
            SuperFlechaGuiada(direccionAngulo, 1.5f, 10);
        }
        else if (bossContainer.GetEstado() == 6 && !bossContainer.GetHit())
        {
            AtaqueBarrido(3, 90, 0.1f);
        }
        else if (bossContainer.GetEstado() == 7 && !bossContainer.GetHit())
        {
            tiempoCambio2 = 3;
            AtaqueCruz(0.2f, 45, 4);
        }
        else if (bossContainer.GetEstado() == 8 && !bossContainer.GetHit())
        {
            AtaqueMandala(0.5f, 30, 5, 5);
        }
        else if (bossContainer.GetEstado() == 9 && !bossContainer.GetHit())
        {
            FlechaGuiada(direccionAngulo, 1f, 5);
        }
        else if (bossContainer.GetEstado() == 10 && !bossContainer.GetHit())
        {
            AtaqueCuarto(0.5f, 30);
            AtaqueCubos(1, 20, 5);
        }
        else if (bossContainer.GetEstado() == 11 && !bossContainer.GetHit())
        {
            AtaqueCirculo(0.3f, 36, 4);
        }
        else if (bossContainer.GetEstado() == 12 && !bossContainer.GetHit())
        {
            AtaqueFlechas(0.75f, 10, 9);
        }
        else if (bossContainer.GetEstado() == 13 && !bossContainer.GetHit())
        {
            SuperFlechaGuiada(direccionAngulo, 1.75f, 12);
        }
        else if (bossContainer.GetEstado() == 14 && !bossContainer.GetHit())
        {
            AtaqueBarrido(3, 90, 0.09f);
        }
        else if (bossContainer.GetEstado() == 15 && !bossContainer.GetHit())
        {
            tiempoCambio2 = 2;
            AtaqueCruz(0.2f, 45, 4);
        }
        else if (bossContainer.GetEstado() == 16 && !bossContainer.GetHit())
        {
            AtaqueMandala(0.4f, 36, 4, 6);
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

            DisparoGuiado(direccion + 10, 0.1f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion + 6, 0.1f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion + 2, 0.1f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 2, 0.1f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 6, 0.1f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 10, 0.1f, velocidadDelProyectil, prefab5);

            DisparoGuiado(direccion + 8, 0.3f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion + 4, 0.3f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion, 0.3f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 4, 0.3f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 8, 0.3f, velocidadDelProyectil, prefab5);

            DisparoGuiado(direccion + 6, 0.5f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion + 2, 0.5f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 2, 0.5f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 6, 0.5f, velocidadDelProyectil, prefab5);

            DisparoGuiado(direccion + 4, 0.7f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion, 0.7f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 4, 0.7f, velocidadDelProyectil, prefab5);

            DisparoGuiado(direccion + 2, 0.9f, velocidadDelProyectil, prefab5);
            DisparoGuiado(direccion - 2, 0.9f, velocidadDelProyectil, prefab5);

            DisparoGuiado(direccion, 1.1f, velocidadDelProyectil, prefab5);
        }
    }
    private void AtaqueCuarto(float tiempo, float angulo)
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoGuiado(180 + 12 + sumaDireccion, 0.3f, 4, prefab2);
            DisparoGuiado(180 + 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(180 + 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(180 + sumaDireccion, 0.9f, 4, prefab2);
            DisparoGuiado(180 - 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(180 - 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(180 - 12 + sumaDireccion, 0.3f, 4, prefab2);

            DisparoGuiado(0 + 12 + sumaDireccion, 0.3f, 4, prefab2);
            DisparoGuiado(0 + 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(0 + 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(0 + sumaDireccion, 0.9f, 4, prefab2);
            DisparoGuiado(0 - 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(0 - 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(0 - 12 + sumaDireccion, 0.3f, 4, prefab2);

            DisparoGuiado(90 + 12 + sumaDireccion, 0.3f, 4, prefab2);
            DisparoGuiado(90 + 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(90 + 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(90 + sumaDireccion, 0.9f, 4, prefab2);
            DisparoGuiado(90 - 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(90 - 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(90 - 12 + sumaDireccion, 0.3f, 4, prefab2);

            DisparoGuiado(-90 + 12 + sumaDireccion, 0.3f, 4, prefab2);
            DisparoGuiado(-90 + 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(-90 + 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(-90 + sumaDireccion, 0.9f, 4, prefab2);
            DisparoGuiado(-90 - 4 + sumaDireccion, 0.7f, 4, prefab2);
            DisparoGuiado(-90 - 8 + sumaDireccion, 0.5f, 4, prefab2);
            DisparoGuiado(-90 - 12 + sumaDireccion, 0.3f, 4, prefab2);

            sumaDireccion += angulo;
        }
    }
    private void AtaqueCubos(float tiempo, float angulo, int numeroDeFlechas)
    {
        float anguloPorFlecha = 360 / numeroDeFlechas;

        if (Time.time > siguienteDisparo2)
        {
            siguienteDisparo2 = Time.time + tiempo;
            for (int i = 0; i < numeroDeFlechas; i++)
            {
                DisparoGuiado(45 + anguloPorFlecha * i + sumaDireccion2, 0.9f, 4, prefab9);
            }
            sumaDireccion2 += angulo;
        }
    }
    private void AtaqueCirculo(float tiempo, int numeroDeProyectiles, float velocidadDelProyectil)
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            DisparoCirculo(numeroDeProyectiles, velocidadDelProyectil, sumaAngulo);
            sumaAngulo += 5;
        }
    }
    private void AtaqueBarrido(float velocidadDelProyectil, float angulo, float tiempo)
    {
        float anguloPorDisparo = 360 / angulo * movimiento;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoRadial(anguloActual + 5, velocidadDelProyectil);
            DisparoRadial(anguloActual - 5, velocidadDelProyectil);

            DisparoRadial(anguloActual + 185, velocidadDelProyectil);
            DisparoRadial(anguloActual + 175, velocidadDelProyectil);

            DisparoRadial(anguloActual + 95, velocidadDelProyectil);
            DisparoRadial(anguloActual + 85, velocidadDelProyectil);

            DisparoRadial(anguloActual + 275, velocidadDelProyectil);
            DisparoRadial(anguloActual + 265, velocidadDelProyectil);
            
            anguloActual += anguloPorDisparo;
        }
        temporizadorCambio -= Time.deltaTime;
        if (temporizadorCambio <= 0)
        {
            movimiento *= -1;
            temporizadorCambio = tiempoCambio;
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
    private void AtaqueCruz(float tiempo, float angulo, int numeroDeFlechas)
    {
        float anguloPorFlecha = 360 / numeroDeFlechas;

        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            for (int i = 0; i < numeroDeFlechas; i++)
            {
                DisparoGuiado(anguloPorFlecha * i + 20f + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i + 15f + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i + 10f + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i + 5f + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i - 5f + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i - 10f + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i - 15f + sumaDireccion3, 0.1f, 6, prefab7);
                DisparoGuiado(anguloPorFlecha * i - 20f + sumaDireccion3, 0.1f, 6, prefab7);
            }
        }
        temporizadorCambio2 -= Time.deltaTime;
        if (temporizadorCambio2 <= 0)
        {
            sumaDireccion3 += angulo;
            temporizadorCambio2 = tiempoCambio2;
        }
    }
    private void FlechaGuiada(float direccionAngulo, float tiempo, float velocidadDelProyectil)
    {
        float direccion = (direccionAngulo + 180) * -1;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoGuiado(direccion + 6, 0.7f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion, 0.7f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion - 6, 0.7f, velocidadDelProyectil, prefab1);

            DisparoGuiado(direccion + 3, 1f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion - 3, 1f, velocidadDelProyectil, prefab1);

            DisparoGuiado(direccion, 1.3f, velocidadDelProyectil, prefab1);


            DisparoGuiado(direccion + 28.5f, 0.7f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion + 22.5f, 0.7f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion + 16.5f, 0.7f, velocidadDelProyectil, prefab1);

            DisparoGuiado(direccion + 25.5f, 1f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion + 19.5f, 1f, velocidadDelProyectil, prefab1);

            DisparoGuiado(direccion + 22.5f, 1.3f, velocidadDelProyectil, prefab1);


            DisparoGuiado(direccion - 16.5f, 0.7f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion - 22.5f, 0.7f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion - 28.5f, 0.7f, velocidadDelProyectil, prefab1);

            DisparoGuiado(direccion - 19.5f, 1f, velocidadDelProyectil, prefab1);
            DisparoGuiado(direccion - 25.5f, 1f, velocidadDelProyectil, prefab1);

            DisparoGuiado(direccion - 22.5f, 1.3f, velocidadDelProyectil, prefab1);
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
    private void DisparoGuiado(float direccionAngulo, float radioDeSpawn, float velocidadDelProyectil, GameObject prefab)
    {
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        Vector2 posicionInicial = new(Mathf.Sin(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - direccionAngulo) - (direccionAngulo * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(prefab, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void DisparoCirculo(int numeroDeProyectiles, float velocidadDelProyectil,float sumaAngulo)
    {
        float radioDeSpawn = 0.3f;
        float anguloPorDisparo = 360 / numeroDeProyectiles;
        float anguloActual = sumaAngulo;

        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        for (int i = 0; i < numeroDeProyectiles; i++)
        {
            Vector2 posicionInicial = new(Mathf.Sin(((anguloActual * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((anguloActual * Mathf.PI) / 180) + transformUpAngulo));
            Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
            float rotacionZ = (360 - anguloActual) - (anguloActual * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
            Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
            Instantiate(prefab3, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
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
        Instantiate(prefab6, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
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
            Instantiate(prefab8, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
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