using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss13 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int bossHealth = 12;
    [SerializeField] GameObject upgrade;

    bool invincible = false;
    Rigidbody2D rb;
    Vector2 direction;
    float rotate = 0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
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
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotate);
        rotate += 25;
        if (rotate >= 360)
        {
            rotate = 0;
        }
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
