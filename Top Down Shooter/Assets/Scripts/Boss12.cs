using System.Threading.Tasks;
using UnityEngine;

public class Boss12 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float dashSpeed = 10.0f;
    [SerializeField] float dashCooldown = 5.0f;
    [SerializeField] float origMoveSpeed = 3.0f;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] int bossHealth = 12;

    Transform dashPos;
    bool dashing = false;
    bool invincible = false;
    Rigidbody2D rb;
    Vector2 direction;
    Vector2 dashDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        moveSpeed = origMoveSpeed;
        DashAttack();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            direction = (player.position - transform.position).normalized;
        }
    }
    void FixedUpdate()
    {
        if (!dashing)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        if (dashing)
        {
            rb.MovePosition(rb.position + dashDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    async Task DashAttack()
    {
        dashing = true;
        moveSpeed = 0;
        await Task.Delay(1000);
        dashPos = player = GameObject.FindWithTag("Player").transform;
        dashDirection = (dashPos.position - transform.position).normalized;
        moveSpeed = origMoveSpeed * dashSpeed; 
        await Task.Delay(500);
        moveSpeed = origMoveSpeed;
        dashing = false;
        Invoke("DashAttack", dashCooldown);

    }
    private async Task OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !invincible)
        {
            bossHealth -= 1;
            Debug.Log(bossHealth);
            invincible = true;
            if (bossHealth <= 0)
            {
                Destroy(gameObject);
            }
            await Task.Delay(300);
            invincible = false;

        }
    }
}
