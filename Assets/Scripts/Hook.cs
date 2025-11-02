using Unity.VisualScripting;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] FishingMiniGame game;

    private const string FISH_TAG = "Fish";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == FISH_TAG)
            game.Fishing = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == FISH_TAG)
            game.Fishing = false;
    }
}
