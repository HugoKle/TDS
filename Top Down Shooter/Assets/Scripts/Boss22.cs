using System.Threading.Tasks;
using UnityEngine;

public class Boss22 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject bombDisplay;
    [SerializeField] float attackSpeed;
    [SerializeField] float artilleryAttackSpeed;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int bossHealth = 25;
    [SerializeField] int artilleryStrikes = 25;

    [SerializeField] GameObject upgrade;

    bool invincible = false;
    bool artilleryActive = false;
    Rigidbody2D rb;
    Vector2 direction;
    Vector2 screenBounds;
    Vector2 spawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        BombAttack();
        await Task.Delay(1000);
        ArtilleryAttack();
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
        await Task.Delay(1000);
        while (artilleryActive)
        {
            await Task.Delay(500);
            artilleryActive = false;
        }
        Invoke("BombAttack", attackSpeed);
    }

    async Task ArtilleryAttack()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        artilleryActive = true;
        for (int artilleryLoops = artilleryStrikes; artilleryLoops != 0; artilleryLoops--)
        {
            spawnPos = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));
            Instantiate(bombDisplay, spawnPos, transform.rotation);
            await Task.Delay(2);
        }
        Invoke("ArtilleryAttack", artilleryAttackSpeed);
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
