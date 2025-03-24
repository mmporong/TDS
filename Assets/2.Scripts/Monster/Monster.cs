using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 60f;
    public float jumpForwardForce = 5f;
    public float jumpCooldown = 2f;
    public float lerpSpeed = 2.2f;

    private float floorY = -3.22f;

    [SerializeField]
    private bool isAttack = false;
    [SerializeField]
    private bool canJump = true;
    [SerializeField]
    private bool touchingTower = false;
    [SerializeField]
    private bool isPushed = false;

    private Rigidbody2D rb;
    private Coroutine jumpCoroutine;

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (touchingTower && transform.position.y < -3f)
        {
            // 밀려남
            isPushed = true;
            KnockBack();
        }
        else if (touchingTower && transform.position.y >= -3f)
        {
            // 떨어짐
            transform.position += Vector3.down * 1.25f * Time.deltaTime;

        }
        else if (!touchingTower && !isPushed)
        {
            // 이동
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        if (!touchingTower && isAttack && !isPushed)
        {
            // 올라탐
            StackMonsters();
        }
    }

    private void StackMonsters()
    {
        if (touchingTower || !canJump) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.6f);

        // 몬스터가 있으면
        if (hit.collider != null && hit.collider.CompareTag("Monster"))
        {
            // 몬스터 위로 점프
            Vector3 targetPos = hit.collider.transform.position + new Vector3(hit.collider.bounds.size.x, hit.collider.bounds.size.y, 0);

            if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
            jumpCoroutine = StartCoroutine(JumpOverMonster(targetPos));
        }
    }

    private IEnumerator JumpOverMonster(Vector3 targetPosition)
    {
        canJump = false;
    
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-jumpForwardForce, jumpForce), ForceMode2D.Impulse);

        float elapsedTime = 0f;
        while (elapsedTime < jumpCooldown)
        {
            if (touchingTower)
            {
                rb.velocity = Vector2.zero;
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canJump = true;
    }

    private void KnockBack()
    {
        Collider2D[] nearbyMonsters = Physics2D.OverlapBoxAll(transform.position, new Vector2(1f, 4f), 0f, LayerMask.GetMask("Monster"));
        foreach (var monster in nearbyMonsters)
        {
            if (monster.CompareTag("Monster"))
            {
                Monster otherMonster = monster.GetComponent<Monster>();
                if (otherMonster.touchingTower && otherMonster.transform.position.y > transform.position.y)
                {
                    Vector3 targetPosition = new Vector3(transform.position.x + monster.bounds.size.x * 1.1f, floorY, transform.position.z);
                    StartCoroutine(KnockBackLerp(targetPosition));
                    StartCoroutine(ResetJump());

                }
            }
        }
    }

    private IEnumerator KnockBackLerp(Vector3 targetPosition)
    {
        float timeElapsed = 0f;

        Vector3 initialPosition = new Vector3(transform.position.x, floorY, transform.position.z);

        while (timeElapsed < 2f)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed / 2f);
            timeElapsed += Time.deltaTime * lerpSpeed;
            yield return null;
        }

    }

    private IEnumerator ResetJump()
    {
        animator.SetBool("IsAttacking", false);

        yield return new WaitForSeconds(3f);
        canJump = true;
        isPushed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Hero"))
        {
            // 타워와 접촉
            isAttack = true;
            animator.SetBool("IsAttacking", true);
            touchingTower = true;
            StopJump();
        }
        else if (collision.gameObject.CompareTag("Monster"))
        {
            isAttack = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            touchingTower = true;
            StopJump();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            touchingTower = false;
        }
    }

    private void StopJump()
    {
        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
            rb.velocity = Vector2.zero;
            jumpCoroutine = null;
        }
    }

    public void OnAttack()
    {
    }
}

