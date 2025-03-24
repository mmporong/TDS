using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] private float fadeDuration = 0.5f; 

    private TextMeshProUGUI damageText;
    private float lifetime;

    private void Start()
    {
        damageText = GetComponent<TextMeshProUGUI>(); 
        lifetime = fadeDuration;
    }

    private void Update()
    {
        // �ؽ�Ʈ�� ���� �̵�
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // ���� ����
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject); 
        }
    }

 
}
