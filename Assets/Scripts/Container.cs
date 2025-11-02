using UnityEngine;

public class Container : MonoBehaviour
{
    private const string PLAYER_ONE_TAG = "PlayerOne";
    private const string PLAYER_TWO_TAG = "PlayerTwo";

    [SerializeField] private GameObject ChickenHolder;
    [SerializeField] private ContainerSide container;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_ONE_TAG) && container == ContainerSide.left)
        {
            GameManager.Instance.PlayerGetChickenToPen(collision.gameObject.GetComponent<PlayerController>(), ChickenHolder);
        }
        else if (collision.gameObject.CompareTag(PLAYER_TWO_TAG) && container == ContainerSide.right)
        {
            GameManager.Instance.PlayerGetChickenToPen(collision.gameObject.GetComponent<PlayerController>(), ChickenHolder);
        }
    }
}

public enum ContainerSide
{
    left,
    right
}