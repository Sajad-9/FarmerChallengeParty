using UnityEngine;

public class Balloon : MonoBehaviour
{
    public int vegetableSpawned = 0;


    [SerializeField] GameObject[] vegetables;
    [SerializeField] Transform rightLimit;
    [SerializeField] Transform leftLimit;
    [SerializeField] float speed = 2f;
    [SerializeField] float spawnCooldown = 2f;
    [SerializeField] int vegetableSpawnLimit = 5;

    private int direction = 1;

    private void Start()
    {
        InvokeRepeating(nameof(DropVegetable), spawnCooldown, spawnCooldown);
    }
    private void Update()
    {
        if (transform.position.x >= rightLimit.position.x || transform.position.x <= leftLimit.position.x)
            direction *= -1;

        transform.Translate(direction * Vector3.left * speed * Time.deltaTime);

    }
    private void DropVegetable()
    {
        if (vegetableSpawned >= vegetableSpawnLimit) return;

        Instantiate(vegetables[Random.Range(0, vegetables.Length)], transform.position, Quaternion.identity);
        vegetableSpawned++;
    }
}
