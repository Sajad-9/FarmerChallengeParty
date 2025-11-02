using UnityEngine;

public class Cannon : MonoBehaviour
{
    private const string PLAYER_ONE_TAG = "PlayerOne";
    private const string PLAYER_TWO_TAG = "PlayerTwo";

    public bool AbleRotate = false;

    [SerializeField] private Transform cannonBase;
    [SerializeField] private Transform cannonShootPoint;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private CannonSide side;
    [SerializeField] private float angle = 30f;
    [SerializeField] private float speed = 2f;


    private float startZ;

    private void Start()
    {
        startZ = transform.localEulerAngles.z;
    }

    private void Update()
    {
        if (!AbleRotate) return;

        float zRotation = Mathf.Sin(Time.time * speed) * angle;
        cannonBase.localEulerAngles = new Vector3(
            cannonBase.localEulerAngles.x,
            cannonBase.localEulerAngles.y,
            (startZ + zRotation)
        );

    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;
        bool con = (side == CannonSide.Left && other.gameObject.CompareTag(PLAYER_ONE_TAG) || side == CannonSide.Right && other.gameObject.CompareTag(PLAYER_TWO_TAG)) && player.HasVegetable;
        if (con)
        {
            AbleRotate = true;
            player.CannonObject = this;
            player.UsingCannon = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (side == CannonSide.Left && other.gameObject.CompareTag(PLAYER_ONE_TAG) || side == CannonSide.Right && other.gameObject.CompareTag(PLAYER_TWO_TAG))
        {
            AbleRotate = false;
            other.GetComponent<PlayerController>().UsingCannon = false;
            other.GetComponent<PlayerController>().CannonObject = null;
        }
    }

    public void Shoot(Vegetable vegetable)
    {
        Destroy(vegetable);
        Instantiate(Bullet, cannonShootPoint.position, cannonShootPoint.rotation);
        AudioManager.Instance.PlaySFX("Cannon");
    }
}
public enum CannonSide
{
    Left,
    Right
};
