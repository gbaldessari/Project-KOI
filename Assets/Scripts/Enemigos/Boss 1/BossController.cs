using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Vector2 direccion;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    private float siguienteDisparo = 0;
    private float anguloActual = 0;
    private List<Transform> objetivos = new();
    private float vidas = 550;
    private bool activo = false;
    private bool encuentroIniciado = false;
    private BossContainer bossContainer;

    private bool hit = false;
    public GameObject hitSprite;
    private float temporizadorHit;
    private readonly float tiempoHit = 0.05f;

    // Start is called before the first frame update
    private void Start()
    {
        bossContainer = GetComponentInParent<BossContainer>();
    }

    // Update is called once per frame
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

        float direccionAngulo = Vector2.SignedAngle(new Vector2(0,-10).normalized, direccion.normalized);

        if ((bossContainer.GetEstado() == 1) && !bossContainer.GetHit())
        {
            encuentroIniciado = true;
            FlechaGuiada(direccionAngulo, 4, 1);
        }
        else if((bossContainer.GetEstado() == 2) && !bossContainer.GetHit())
        {
            AtaqueCirculo(20, 4, 0.3f);
        }
        else if ((bossContainer.GetEstado() == 3) && !bossContainer.GetHit())
        {
            AtaqueRadial(20, 0.08f);
        }
        else if((bossContainer.GetEstado() == 4) && !bossContainer.GetHit())
        {
            FlechaGuiada(direccionAngulo, 5, 0.7f);
        }
        else if((bossContainer.GetEstado() == 5) && !bossContainer.GetHit())
        {
            AtaqueCirculo(30, 5, 0.4f);
        }
        else if((bossContainer.GetEstado() == 6) && !bossContainer.GetHit())
        {
            AtaqueRadial(30, 0.1f);
        }

        if (hit && !bossContainer.GetHit())
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
    private void AtaqueCirculo(int numeroDeProyectiles, float velocidadDelProyectil, float tiempo)
    {
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            DisparoCirculo(numeroDeProyectiles,velocidadDelProyectil);
        }
    }
    private void AtaqueRadial(float angulo,float tiempo)
    {
        float anguloPorDisparo = 360 / angulo;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;
            DisparoRadial(anguloActual);
            DisparoRadial(anguloActual + 90);
            DisparoRadial(anguloActual + 180);
            DisparoRadial(anguloActual + 270);
            anguloActual += anguloPorDisparo;
        }
    }
    private void FlechaGuiada(float direccionAngulo, float velocidad, float tiempo)
    {
        float direccion = (direccionAngulo + 180) * -1;
        if (Time.time > siguienteDisparo)
        {
            siguienteDisparo = Time.time + tiempo;

            DisparoGuiado(direccion + 16, 0.1f, velocidad);
            DisparoGuiado(direccion + 8, 0.1f, velocidad);
            DisparoGuiado(direccion, 0.1f, velocidad);
            DisparoGuiado(direccion - 8, 0.1f, velocidad);
            DisparoGuiado(direccion - 16, 0.1f, velocidad);

            DisparoGuiado(direccion + 12, 0.4f, velocidad);
            DisparoGuiado(direccion + 4, 0.4f, velocidad);
            DisparoGuiado(direccion - 4, 0.4f, velocidad);
            DisparoGuiado(direccion - 12, 0.4f, velocidad);

            DisparoGuiado(direccion + 8, 0.7f, velocidad);
            DisparoGuiado(direccion, 0.7f, velocidad);
            DisparoGuiado(direccion - 8, 0.7f, velocidad);

            DisparoGuiado(direccion + 4, 1f, velocidad);
            DisparoGuiado(direccion - 4, 1f, velocidad);

            DisparoGuiado(direccion, 1.3f, velocidad);
        }
    }
    private void DisparoGuiado(float direccionAngulo, float radioDeSpawn, float velocidadDelProyectil)
    {
        float transformUpAngulo = Mathf.Atan2(transform.up.x, transform.up.y);

        Vector2 posicionInicial = new(Mathf.Sin(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo), Mathf.Cos(((direccionAngulo * Mathf.PI) / 180) + transformUpAngulo));
        Vector2 posicionInicialRelativa = (Vector2)transform.position + posicionInicial * radioDeSpawn;
        float rotacionZ = (360 - direccionAngulo) - (direccionAngulo * 2 * Mathf.PI + transformUpAngulo) * Mathf.Rad2Deg;
        Vector2 direccionMovimiento = (posicionInicialRelativa - (Vector2)transform.position).normalized * velocidadDelProyectil;
        Instantiate(prefab3, posicionInicialRelativa, Quaternion.Euler(0, 0, rotacionZ)).GetComponent<Rigidbody2D>().velocity = direccionMovimiento;
    }
    private void DisparoRadial(float anguloActual)
    {
        float radioDeSpawn = 0.5f;
        float velocidadDelProyectil = 4;
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