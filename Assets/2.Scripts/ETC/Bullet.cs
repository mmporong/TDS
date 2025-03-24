using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 70; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster")) 
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Floor")) 
        {
            Destroy(gameObject); 
        }
    }
    
}
