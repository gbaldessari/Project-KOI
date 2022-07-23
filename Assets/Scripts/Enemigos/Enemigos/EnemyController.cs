using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float vidas = 10;
    public float tiempoDeDestruccion = 0.05f;
    private readonly float fuerzaFinal = 5;
    private bool hit = false;
    public GameObject hitSprite;
    private float temporizadorHit;
    private readonly float tiempoHit = 0.05f;

    public GameObject moneda1;
    public GameObject moneda2;
    public GameObject moneda3;
    private GameObject monedaSeleccionada;

    public int probabilidadMoneda1 = 90;
    public int probabilidadMoneda2 = 9;
    public int probabilidadMoneda3 = 1;
    private float temporizadorMoneda;

    
    public int puntuacion = 50;
    private Scripter scripter;

    void Start()
    {
        //Guardamos el tiempo en el que se destruye el enemigo en otra variable para usarla como temporizador
        temporizadorMoneda = (tiempoDeDestruccion/2);

        temporizadorHit = tiempoHit;

        scripter = FindObjectOfType<Scripter>(); // Obtiene el Scripter;

        int randomCoin = Random.Range(1, probabilidadMoneda1 + probabilidadMoneda2 + probabilidadMoneda3 + 1);

        if (randomCoin <= probabilidadMoneda1)
        {
            monedaSeleccionada = moneda1;
        }
        else if(randomCoin > probabilidadMoneda1 && randomCoin <= probabilidadMoneda1 + probabilidadMoneda2)
        {
            monedaSeleccionada = moneda2;
        }
        else if (randomCoin > probabilidadMoneda1 + probabilidadMoneda2)
        {
            monedaSeleccionada = moneda3;
        }
    }

    //Aqui determinamos que pasara en cada frame
    void FixedUpdate()
    {
        if (vidas <= 0)
        {
            transform.SetParent(null, true);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10, 11), Random.Range(-10, 11)).normalized*fuerzaFinal);
            gameObject.GetComponent<Animator>().SetBool("Die", true);
            temporizadorMoneda -= Time.deltaTime;
            Destroy(gameObject, tiempoDeDestruccion);
            

            if (temporizadorMoneda <= 0)
            {
                temporizadorMoneda = 10000;
                scripter.RaiseScore(puntuacion);
                Instantiate(monedaSeleccionada, new Vector3(transform.position.x,transform.position.y, monedaSeleccionada.transform.position.z), Quaternion.identity);
            }
        }
        if (hit)
        {
            temporizadorHit -= Time.deltaTime;
            hitSprite.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
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
    }

    //Aqui determinamos que pasara con el enemigo cuando colisione con algo en especifico
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si colisiona con un objeto con la etiqueta Player o BasedPlayerProjectile, la vida del proyectil se reduce en 2
        if (collision.CompareTag("Player"))
        {
            vidas -= 2;
            hit = true;
        }

        if (collision.CompareTag("BasedPlayerProjectile"))
        {
            vidas -= 0.5f;
            hit = true;
        }

        //Si colisiona con un objeto con la etiqueta PlayerProjectile, la vida del proyectil se reduce en 1
        if (collision.CompareTag("PlayerProjectile"))
        {
            vidas -= 1;
            hit = true;
        }
    }
}