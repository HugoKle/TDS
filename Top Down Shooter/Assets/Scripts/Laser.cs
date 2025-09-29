using System.Threading.Tasks;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] int aliveTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        await Task.Delay(aliveTime * 100);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
