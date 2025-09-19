using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveInput;
    [SerializeField] float moveSpeed = 35;
    [SerializeField] float rotationSpeed = 700f;
    [SerializeField] float bulletSpeed = 7;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject gun;
    [SerializeField] float dashSpeed = 8;
    [SerializeField] double stamina = 10;
    [SerializeField] int staminaCooldown = 1000;

    float targetAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            stamina = 0;
            await Task.Delay(staminaCooldown);
            stamina = 1;
            
        }
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
    async Task Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        if (moveInput != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        }
    }

    void FixedUpdate()
    {
        float rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle - 90, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rotation);
    }
}
