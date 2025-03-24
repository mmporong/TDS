using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private float fireRate = 1.0f; 
    [SerializeField] private float bulletSpreadAngle = 10f; 
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int pelletCount = 6; 
    [SerializeField] private float detectionRange = 10f; 

    private float lastFireTime;

    private void Update()
    {
        Transform target = FindClosestMonster();
        if (target != null)
        {
            RotateTowards(target);
            Attack(target);
        }
    }

    private Transform FindClosestMonster()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, LayerMask.GetMask("Monster"));
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D col in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = col.transform;
            }
        }

        return closestTarget;
    }

    private void RotateTowards(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Attack(Transform target)
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            lastFireTime = Time.time;
            FireShotgun();
        }
    }

    private void FireShotgun()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            float spread = Random.Range(-bulletSpreadAngle, bulletSpreadAngle);
            Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + spread);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bullet.transform.right * bulletSpeed;
        }
    }

}
