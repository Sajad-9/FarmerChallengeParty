using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Vegetable : MonoBehaviour
{
    private const string PLAYER_TWO_TAG = "PlayerTwo";
    private const string PLAYER_ONE_TAG = "PlayerOne";

    public VegtableStat stat = VegtableStat.None;

    public GameObject Holder;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float scaleMultiplier = .8f;

    private Vector3 orginalScale;
    private void Start()
    {
        orginalScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (stat)
        {
            case VegtableStat.None:
                if (other.gameObject.CompareTag(PLAYER_ONE_TAG) || other.gameObject.CompareTag(PLAYER_TWO_TAG))
                {
                    PlayerController player = other.GetComponent<PlayerController>();
                    if (player.HasVegetable || player.DisableMove) return;

                    AudioManager.Instance.PlaySFX("Vegtable");
                    player.HasVegetable = true;
                    player.VegetableObject = this;
                    GameManager.Instance.BalloonObject.GetComponent<Balloon>().vegetableSpawned--;
                    stat = VegtableStat.Catched;
                    sprite.color = Color.black;
                    transform.localScale *= scaleMultiplier;
                    Holder = player.VegetableHolder;
                }
                break;
            case VegtableStat.Bullet:
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
                break;
            case VegtableStat.Catched:
                break;
            default:
                break;
        }

    }
    private void Update()
    {
        if (stat == VegtableStat.Catched && Holder)
        {
            transform.position = Holder.transform.position;
        }
        if (stat == VegtableStat.Bullet)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    public void Shoot()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        stat = VegtableStat.Bullet;
        sprite.color = Color.white;
        transform.localScale = orginalScale;
        Holder = null;
    }
}
public enum VegtableStat
{
    None,
    Bullet,
    Catched
}
