using System.Threading.Tasks;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        await Task.Delay(500);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
