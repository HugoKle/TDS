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
    [SerializeField] GameObject gun;
    [SerializeField] float dashSpeed = 8f;
    [SerializeField] double stamina = 10;
    [SerializeField] int staminaCooldown = 1;
    [SerializeField] int health = 5;
    [SerializeField] int invincibleTime = 2000;

    bool invincible = false;
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
        if (stamina > 0)
        {
            moveSpeed = moveSpeed * dashSpeed;
            await Task.Delay(50);
            moveSpeed = moveSpeed / dashSpeed;
            stamina -= 1;
            
        }
    }

    void DashRecharge()
    {
        if (stamina < 3)
        {
            stamina += 1;
            
        }
        Invoke("DashRecharge", staminaCooldown);
    }

    private object getComponent<T>()
    {
        throw new NotImplementedException();
    }

    void OnAttack()
    {
        Rigidbody2D playerBullet = Instantiate(bullet, gun.transform.position, transform.rotation).GetComponent<Rigidbody2D>();
        playerBullet.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
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

    private async Task OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") && !invincible) ||
            (collision.gameObject.CompareTag("Boss") && !invincible))
        {
            health -= 1;
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
    }
}
