using System.Threading.Tasks;
using UnityEngine;

public class Boss32 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float origMoveSpeed = 3f;
    [SerializeField] int bossHealth = 20;
    [SerializeField] float laserAttackSpeed = 5f;
    [SerializeField] int lasers = 3;
    [SerializeField] GameObject upgrade;
    [SerializeField] GameObject laserDisplayVertical;
    [SerializeField] GameObject laserDisplayHorizontal;

    bool invincible = false;
    bool laserActive = false;
    Rigidbody2D rb;
    Vector2 direction;
    Vector2 screenBounds;
    Vector2 spawnPosVertical;
    Vector2 spawnPosHorizontal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        LaserAttack();
        moveSpeed = origMoveSpeed;
    }

    // Update is called once per frame
    async Task Update()
    {
        if (player != null)
        {
            direction = (player.position - transform.position).normalized;
        }
        if (laserActive)
        {
            moveSpeed = 0;
            await Task.Delay(2000);
            laserActive = false;
            moveSpeed = origMoveSpeed;

        }
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    async Task LaserAttack()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        laserActive = true;
        for (int artilleryLoops = lasers; artilleryLoops != 0; artilleryLoops--)
        {
            spawnPosVertical = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), 0);
            Instantiate(laserDisplayVertical, spawnPosVertical, transform.rotation);

            spawnPosHorizontal = new Vector2(0, Random.Range(-screenBounds.y, screenBounds.y));
            Instantiate(laserDisplayHorizontal, spawnPosHorizontal, transform.rotation);
            await Task.Delay(2);

        }
        Invoke("LaserAttack", laserAttackSpeed);
    }

    private async Task OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !invincible)
        {
            bossHealth --;
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
