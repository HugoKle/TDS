using System.Threading.Tasks;
using UnityEngine;

public class Boss22 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform bombDisplayLocation;
    [SerializeField] GameObject bombDisplay;
    [SerializeField] GameObject bomb;
    [SerializeField] float attackSpeed;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int bossHealth = 25;

    bool invincible = false;
    Rigidbody2D rb;
    Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        BombAttack();
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

    async Task BombAttack()
    {
        Instantiate(bombDisplay, player.position, transform.rotation);
        bombDisplayLocation = GameObject.FindWithTag("BombDisplay").transform;
        await Task.Delay(1000);
        Instantiate(bomb, bombDisplayLocation.position, transform.rotation);
        Invoke("BombAttack", attackSpeed);
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
