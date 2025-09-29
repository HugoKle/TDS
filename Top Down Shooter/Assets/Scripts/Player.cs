using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveInput;
    Vector2 screenBoundery;
    [SerializeField] float moveSpeed = 35f;
    [SerializeField] float rotationSpeed = 700f;
    [SerializeField] float bulletSpeed = 7f;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject explosiveBullet;
    [SerializeField] GameObject gun;
    [SerializeField] float dashSpeed = 8f;
    [SerializeField] double stamina = 10;
    [SerializeField] int staminaCooldown = 1;
    [SerializeField] int health = 5;
    [SerializeField] int invincibleTime = 2000;
    [SerializeField] int bullets = 1;
    [SerializeField] bool backwardsBullet = false;
    [SerializeField] bool explodingBullet = false;
    [SerializeField] bool dash = false;

    GameObject currentBullet;
    bool invincible = false;
    int maxDash = 3;
    float targetAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DashRecharge();
        rb = GetComponent<Rigidbody2D>();
        screenBoundery = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    async Task OnJump()
    {
        if (stamina > 0 && dash)
        {
            moveSpeed = moveSpeed * dashSpeed;
            await Task.Delay(50);
            moveSpeed = moveSpeed / dashSpeed;
            stamina -= 1;
            
        }
    }

    void DashRecharge()
    {
        if (stamina < maxDash)
        {
            stamina += 1;
            
        }
        Invoke("DashRecharge", staminaCooldown);
    }

    private object getComponent<T>()
    {
        throw new NotImplementedException();
    }

    async Task OnAttack()
    {

        for (int bulletLoop = bullets; bulletLoop != 0; bulletLoop--)
        {
            if (explodingBullet)
            {
                currentBullet = explosiveBullet;
            }
            if (!explodingBullet)
            {
                currentBullet = bullet;
            }
            Rigidbody2D playerBullet = Instantiate(currentBullet, gun.transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            playerBullet.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
            if (backwardsBullet)
            {
            Rigidbody2D backwardsPlayerBullet = Instantiate(currentBullet, gun.transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            backwardsPlayerBullet.AddForce(transform.up * -bulletSpeed, ForceMode2D.Impulse);
            }

            await Task.Delay(50);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        if (moveInput != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -screenBoundery.x, screenBoundery.x)
                                        ,Mathf.Clamp(transform.position.y, -screenBoundery.y, screenBoundery.y));
    }

    void FixedUpdate()
    {
        float rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle - 90, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rotation);
    }
    
    private async Task OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") && !invincible) ||
            (collision.gameObject.CompareTag("Boss") && !invincible) || 
            (collision.gameObject.CompareTag("Boss Projectile") && !invincible))
        {
            health -= 1;
            Debug.Log(health);
            invincible = true;
            Debug.Log(invincible);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
            await Task.Delay(invincibleTime * 1000);
            invincible = false;
            Debug.Log(invincible);
        }
    }
    private async Task OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") && !invincible) ||
            (collision.gameObject.CompareTag("Boss") && !invincible) ||
            (collision.gameObject.CompareTag("Boss Projectile") && !invincible))
        {
            health --;
            Debug.Log(health);
            invincible = true;
            Debug.Log(invincible);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
            await Task.Delay(invincibleTime);
            invincible = false;
            Debug.Log(invincible);
        }


        if (collision.gameObject.CompareTag("Heal") && health < 5)
        {
            health++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Speed"))
        {
            moveSpeed += 0.5f;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Backward bullet"))
        {
            backwardsBullet = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Exploding Bullet"))
        {
            explodingBullet = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Burst"))
        {
            bullets ++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Dash"))
        {
            if (dash)
            {
                maxDash ++;
            }
            dash = true;
            Destroy(collision.gameObject);
        }
    }
}
