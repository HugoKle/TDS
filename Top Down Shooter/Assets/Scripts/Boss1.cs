using System.Threading.Tasks;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject bullet;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] int bossHealth = 20;
    [SerializeField] float bulletSpeed = 4f;
    [SerializeField] GameObject upgrade;

    bool invincible = false;
    Rigidbody2D rb;
    Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        await Task.Delay(20000);
        Shooting();
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
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
    
    void Shooting()
    {
        Rigidbody2D enemyBullet1 = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
        enemyBullet1.AddForce(transform.up * bulletSpeed/35, ForceMode2D.Impulse);

        Rigidbody2D enemyBullet2 = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
        enemyBullet2.AddForce(transform.right * bulletSpeed / 35, ForceMode2D.Impulse);

        Rigidbody2D enemyBullet3 = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
        enemyBullet3.AddForce((transform.up * bulletSpeed / 35) * -1, ForceMode2D.Impulse);

        Rigidbody2D enemyBullet4 = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
        enemyBullet4.AddForce((transform.right * bulletSpeed / 35) * -1, ForceMode2D.Impulse);
        Invoke("Shooting", 1.2f);
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

