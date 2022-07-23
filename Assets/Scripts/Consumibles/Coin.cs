using UnityEngine;

public class Coin : MonoBehaviour
{
    private Scripter scripter; // Scripter
    private SoundObject soundObject; // Sound Object
    private readonly float tiempoDeDestruccion = 0.3f; // Tiempo de autodestrucción cuando un jugador toca una moneda
    private readonly float roceFinal = 1000f; // Roce final de una moneda al tocar un jugador


    /// <summary>
    /// Aqui determinamos que pasara al iniciarse el script
    /// </summary>
    void Awake()
    {
        scripter = FindObjectOfType<Scripter>(); // Obtenemos el scripter
        soundObject = FindObjectOfType<SoundObject>(); // Obtenemos el Sound Object
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
    }


    /// <summary>
    /// Aqui determinamos que pasara con la moneda cuando colisione con algo en especifico
    /// </summary>
    /// <param name="collision">Objeto de colisión del elemento chocado</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.CompareTag("ExtraLife"))
            {
                collision.GetComponentInParent<PlayerController>().OnLifeCoinCollected(); 
                soundObject.PlayEffectBasedMode(); // Reproduce la música de una vida extra obtenida
                scripter.RaiseScore(1500);
            }
            else if (gameObject.CompareTag("BasedMode"))
            {
                collision.GetComponentInParent<PlayerController>().OnBasedCoinCollected(); 
                soundObject.PlayEffectBasedMode(); // Reproduce la música de una moneda basada obtenida
                scripter.RaiseScore(1000);
            }
            else if (gameObject.CompareTag("Coin"))
            {
                collision.GetComponentInParent<PlayerController>().OnCoinCollected();
                soundObject.PlayEffectCoinCollected(); // Reproduce la música de una moneda obtenida
                scripter.RaiseScore(200);
            }

            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -4f); // Mueve la moneda a una capa superior (para evitar solapamiento de animación)
            gameObject.GetComponent<Rigidbody2D>().drag = roceFinal; // Para la moneda a través de roce
            gameObject.GetComponent<Animator>().SetBool("Die", true); // Dispara la animación de moneda obtenida
            Destroy(gameObject, tiempoDeDestruccion); // Destruye la moneda
        }
        
        else if (collision.CompareTag("BulletLimit"))
        {
            Destroy(gameObject); // Destruye la moneda si se salió del mapa
        }
    }
}
