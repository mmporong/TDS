using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100; 

    private bool isHit = false;
    private float hitCooldown = 0.5f;
    public GameObject damageTextPrefab; 

    public void TakeDamage(int damage)
    {
        if (!isHit)
        {
            isHit = true;
            health -= damage;
            Debug.Log($"���� ü��: {health}");

            CreateDamageText(damage);

            if (health <= 0)
            {
                Die();
            }

            StartCoroutine(ResetHit());
        }
        
    }

    // ���
    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(hitCooldown);
        isHit = false;
    }

    private void CreateDamageText(int damage)
    {
        GameObject damageTextObj = Instantiate(damageTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        DamageText damageText = damageTextObj.GetComponent<DamageText>();

       
    }

}