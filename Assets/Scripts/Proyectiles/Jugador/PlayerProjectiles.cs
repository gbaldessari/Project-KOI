using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    public float velocidad;
    public float tiempoDeVida;
    public float roceFinal = 10;
    public float tiempoDeDestruccion = 0.6f;
    public float capaExplosion;

    private void Start()
    {
        //Aqui le ponemos un temporizador al proyectil para que se destruya pasado un tiempo
        Destroy(gameObject, tiempoDeVida);
    }
    //Aqui determinamos que pasara en cada frame
    void FixedUpdate()
    {
        //Aqui aplicaremos una fuerza para que el proyectil se mueva a la velocidad deceada, ademas multiplicamos
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocidad);
    }

    //Aqui determinamos que pasara con el proyectil cuando colisione con algo en especifico
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si colisiona con un objeto con la etiqueta Enemy, el bool colicion sera verdadero
        if (collision.CompareTag("Enemy"))
        {
            gameObject.GetComponent<Animator>().SetBool("Die", true);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, capaExplosion);
            gameObject.GetComponent<Rigidbody2D>().drag = roceFinal;
            Destroy(gameObject, tiempoDeDestruccion);
        }

        if (collision.CompareTag("BulletLimit"))
        {
            Destroy(gameObject);
        }
    }
}