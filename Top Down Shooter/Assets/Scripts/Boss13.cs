using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss13 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject bullet;  
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int bossHealth = 12;
    [SerializeField] float bulletSpeed = 4f;
    [SerializeField] GameObject upgrade;
    [SerializeField] float attackSpeed = 1f;

    bool invincible = false;
    Rigidbody2D rb;
    Vector2 direction;
    float rotate = 0f;
    bool spinning = false;
    int spinAmount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        SpinAttack();
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
        if (!spinning)
        {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotate);



    }

    async Task SpinAttack()
    {
        spinning = true;
        spinAmount = 0;
        while (spinning)
        {
            rotate += 25;
            if (rotate >= 360)
            {
                spinAmount++;
                rotate = 0;
                if (spinAmount == 3)
                {
                spinning = false;
                }

            }
            await Task.Delay(100);
            Rigidbody2D enemyBullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            enemyBullet.AddForce(transform.up * bulletSpeed / 350, ForceMode2D.Impulse);
        }
        Invoke("SpinAttack", attackSpeed);
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
                Instantiate(upgrade, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            await Task.Delay(300);
            invincible = false;

        }
    }
}
