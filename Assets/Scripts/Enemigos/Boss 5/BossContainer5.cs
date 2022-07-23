using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossContainer5 : MonoBehaviour
{
    private float cronometro = 0;
    private bool inicio = false;
    public float tiempoInicial = 4;
    private float vidas;
    private int contadorEstado = 0;
    public BossController5 boss;
    private Scripter scripter;
    private float cronometroSalida = 0;
    public GameObject fuego;
    private bool terminarEncuentro = false;
    private bool hit = false;
    public GameObject hitSprite;
    private float temporizadorHit;
    private readonly float tiempoHit = 2;
    private bool IsHit = false;
    public GameObject explosion;
    public GameObject humo;
    public GameObject contenedorExplosiones;
    public GameObject extraLife;
    public GameObject basedCore;
    private bool final = false;

    void Start()
    {
        vidas = boss.GetVidas();
        scripter = GameObject.FindObjectOfType<Scripter>();
        temporizadorHit = tiempoHit;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inicio) return;

        cronometro += Time.deltaTime;

        if (scripter.GetStatusBossBar()) scripter.BossBarUpdate(Mathf.Clamp01(boss.GetVidas() / 1150));

        if (contadorEstado == 0)
        {
            gameObject.GetComponent<Animator>().SetBool("Entrada", true);
            gameObject.GetComponent<Animator>().SetBool("Reloj", false);
            gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
            gameObject.GetComponent<Animator>().SetBool("W", false);
            gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
            gameObject.GetComponent<Animator>().SetBool("LAL", false);
            gameObject.GetComponent<Animator>().SetBool("Medio", false);
            gameObject.GetComponent<Animator>().SetBool("Ola", false);
            gameObject.GetComponent<Animator>().SetBool("Salida", false);
        }

        if (cronometro >= tiempoInicial && contadorEstado == 0)
        {
            if (!scripter.GetStatusBossBar()) scripter.EnableBossBar(true);
            gameObject.GetComponent<Animator>().SetBool("Entrada", false);
            gameObject.GetComponent<Animator>().SetBool("Reloj", true);
            gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
            gameObject.GetComponent<Animator>().SetBool("W", false);
            gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
            gameObject.GetComponent<Animator>().SetBool("LAL", false);
            gameObject.GetComponent<Animator>().SetBool("Medio", false);
            gameObject.GetComponent<Animator>().SetBool("Ola", false);
            gameObject.GetComponent<Animator>().SetBool("Salida", false);
            contadorEstado++;
            hit = true;
        }

        if (boss.GetVidas() <= vidas - 100 && contadorEstado == 1)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", true);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 100;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }

        if (boss.GetVidas() <= vidas - 100 && contadorEstado == 2)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", true);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 100;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }

        if (boss.GetVidas() <= vidas - 100 && contadorEstado == 3)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", true);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 100;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 100 && contadorEstado == 4)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", true);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 100;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 100 && contadorEstado == 5)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", true);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 100;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 100 && contadorEstado == 6)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", true);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 100;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 100 && contadorEstado == 7)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                humo.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                    Instantiate(basedCore, new Vector3(transform.position.x, transform.position.y, basedCore.transform.position.z), Quaternion.identity);
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", true);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 100;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 75 && contadorEstado == 8)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", true);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 75;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 75 && contadorEstado == 9)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", true);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 75;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 75 && contadorEstado == 10)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", true);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 75;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 75 && contadorEstado == 11)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", true);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 75;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 75 && contadorEstado == 12)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", true);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 75;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 75 && contadorEstado == 13)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", true);
                gameObject.GetComponent<Animator>().SetBool("Salida", false);
                vidas -= 75;
                boss.SetVidas(vidas);
                scripter.RaiseScore(1500);
                contadorEstado++;
                hit = true;
            }
        }
        if (boss.GetVidas() <= vidas - 75 && contadorEstado == 14)
        {
            if (hit)
            {
                IsHit = true;
                temporizadorHit -= Time.deltaTime;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                explosion.SetActive(true);
                fuego.SetActive(true);
                contenedorExplosiones.SetActive(true);
                if (temporizadorHit <= 0)
                {
                    explosion.SetActive(false);
                    hit = false;
                    temporizadorHit = tiempoHit;
                    Instantiate(extraLife, new Vector3(transform.position.x, transform.position.y, extraLife.transform.position.z), Quaternion.identity);
                    if (scripter.GetStatusBossBar()) scripter.EnableBossBar(false);
                }
            }
            else
            {
                IsHit = false;
                hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

                gameObject.GetComponent<Animator>().SetBool("Entrada", false);
                gameObject.GetComponent<Animator>().SetBool("Reloj", false);
                gameObject.GetComponent<Animator>().SetBool("MedioCirculo", false);
                gameObject.GetComponent<Animator>().SetBool("W", false);
                gameObject.GetComponent<Animator>().SetBool("TrianguloInv", false);
                gameObject.GetComponent<Animator>().SetBool("LAL", false);
                gameObject.GetComponent<Animator>().SetBool("Medio", false);
                gameObject.GetComponent<Animator>().SetBool("Ola", false);
                gameObject.GetComponent<Animator>().SetBool("Salida", true);
                scripter.RaiseScore(10000);
                contadorEstado++;
                fuego.SetActive(true);
            }
        }
        if (terminarEncuentro)
        {
            cronometroSalida += Time.deltaTime;
            if (cronometroSalida >= 2)
            {
                final = true;
            }
        }
    }

    public void IniciarEncuentro()
    {
        inicio = true;
    }
    public int GetEstado()
    {
        return contadorEstado;
    }
    public bool GetHit()
    {
        return IsHit;
    }
    public bool Final()
    {
        return final;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Desactivador") && contadorEstado == 15)
        {
            terminarEncuentro = true;
        }
    }
}