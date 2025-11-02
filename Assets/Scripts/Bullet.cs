using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string PLAYER_TWO_TAG = "PlayerTwo";
    private const string PLAYER_ONE_TAG = "PlayerOne";

    [SerializeField] private float speed = 15f;
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER_ONE_TAG))
        {
            GameManager.Instance.PlayerGameObjects[(int)Players.One].GetComponent<PlayerController>().Death(2f);
            GameManager.Instance.AddScore(Players.Two);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag(PLAYER_TWO_TAG))
        {
            GameManager.Instance.PlayerGameObjects[(int)Players.Two].GetComponent<PlayerController>().Death(2f);
            GameManager.Instance.AddScore(Players.One);
            Destroy(gameObject);
        }
        else { Destroy(gameObject); }
    }
}
