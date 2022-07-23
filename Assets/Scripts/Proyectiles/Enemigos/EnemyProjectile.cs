using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private readonly float roceFinal = 10;
    private readonly float tiempoDeDestruccion = 0.5f;
    private readonly float capaExplosion = -4f;

    //private readonly float amplitud = 20f;
    //private readonly float frecuencia = 10f;

    /*
    private void FixedUpdate()
    {
        //transform.position = transform.position + new Vector3(0f, 0f, 0f);
        // Mathf.Sin(_time * _frequency) * _amplitude * _frequency;
        float x = -Mathf.Sin(Time.time * frecuencia) * amplitud * frecuencia * Time.deltaTime;
        //float y = Mathf.Cos(Time.time * frecuencia) * amplitud * frecuencia * Time.deltaTime;
        GetComponent<Rigidbody2D>().velocity = new Vector2(x, GetComponent<Rigidbody2D>().velocity.y);
        //.angularVelocity += 10f; lo hace girar sobre su propio eje
        //transform.position = transform.position + new Vector3(Mathf.Cos(Time.time) * Time.deltaTime, Mathf.Sin(Time.time) * Time.deltaTime, 0.0f);
    }
    */

    /// <summary>
    /// Aqui determinamos que pasara con el proyectil cuando colisione con algo en especifico
    /// </summary>
    /// <param name="collision">Data de la colisión</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Si colisiona con un objeto con la etiqueta Player, este se destruye en el tiempo deseado y
        recibe un roce determinado para que se detenga en el lugar o disminuya su velocidad*/
        if (collision.CompareTag("Player") || collision.CompareTag("BulletEraser"))
        {
            gameObject.GetComponent<Animator>().SetBool("Die", true);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, capaExplosion);
            gameObject.GetComponent<Rigidbody2D>().drag = roceFinal;
            Destroy(gameObject, tiempoDeDestruccion);
        }
        if(collision.CompareTag("BulletLimit"))
        {
            Destroy(gameObject);
        }
    }
}
