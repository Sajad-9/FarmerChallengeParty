using UnityEngine;

public class CenterBox : MonoBehaviour
{
    private const string PLAYER_TWO_TAG = "PlayerTwo";
    private const string PLAYER_ONE_TAG = "PlayerOne";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_ONE_TAG) || collision.gameObject.CompareTag(PLAYER_TWO_TAG))
            collision.gameObject.GetComponent<PlayerController>().CanFish = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_ONE_TAG) || collision.gameObject.CompareTag(PLAYER_TWO_TAG))
            collision.gameObject.GetComponent<PlayerController>().CanFish = false;
    }
}
