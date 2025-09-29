using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] bool bombActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bombActive)
        {
            Instantiate(bomb, transform.position, transform.rotation);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
}
