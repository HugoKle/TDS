using System.Threading.Tasks;
using UnityEngine;

public class BombDisplay : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] int warningTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        await Task.Delay(warningTime * 1000);
        Instantiate(bomb, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
