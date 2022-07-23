using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletLimit"))
        {
            Destroy(gameObject);
        }
    }
}
