using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chicken : MonoBehaviour
{
    private const string PLAYER_ONE_TAG = "PlayerOne";
    private const string PLAYER_TWO_TAG = "PlayerTwo";

    public GameObject Holder;

    [SerializeField] Collider boxCollider;
    [SerializeField] ChickenAction action = ChickenAction.Stay;
    [SerializeField] float jumpForce = 4f;
    [SerializeField] float tickTime = 1.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float stop = 0.4f;
    [SerializeField] bool caught = false;

    private Rigidbody rb;
    private float timer = 0.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (caught) return;
        if(other.GetComponent<PlayerController>() == null) return;
        if (other.GetComponent<PlayerController>().HasChicken) return;
        
        if (other.gameObject.CompareTag(PLAYER_ONE_TAG) || other.gameObject.CompareTag(PLAYER_TWO_TAG))
        {
            other.GetComponent<PlayerController>().HasChicken = true;
            GameObject holder = other.GetComponent<PlayerController>().ChickenHolder;
            other.GetComponent<PlayerController>().ChickenObject = this;
            Holder = holder;
            boxCollider.isTrigger = true;
            caught = true;
        }
    }
    private void Update()
    {
        if (caught)
        {
            transform.position = Holder.transform.position;
            return;
        }

        timer += Time.deltaTime;
        if (timer > tickTime && rb.linearVelocity.y < 0.1f)
        {
            SwitchAction();
            timer = 0.0f;
        }
        switch (action)
        {
            case ChickenAction.MoveRight:
                Move(1);
                break;
            case ChickenAction.MoveLeft:
                Move(-1);
                break;
            case ChickenAction.Stay:
                break;
            default:
                break;
        }
    }

    public void MoveToPen(GameObject pen)
    {
        boxCollider.isTrigger = true;
        AudioManager.Instance.PlaySFX("Chicken.Pen");
        Holder = pen;
    }
    public void Death()
    {
        //instantiate the coocked chicken!
        AudioManager.Instance.PlaySFX("Chicken.Death");
        jumpForce *= 3;
        Jump();
        Invoke(nameof(Die), .5f);
    }
    public void Jump()
    {
        if (caught) return;

        float xForce = Random.Range(-jumpForce, jumpForce);
        float yForce = jumpForce;

        rb.AddForce(new Vector3(xForce, yForce), ForceMode.Impulse);
    }

    private void Die() => Destroy(gameObject);
    private void Move(int iDirection)
    {
        rb.linearVelocity = new Vector3(speed * iDirection, rb.linearVelocity.y, rb.linearVelocity.z);
        Invoke(nameof(Stop), stop);
    }
    private void Stop()
    {
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
        action = ChickenAction.Stay;
    }
    private void SwitchAction()
    {
        int a = Random.Range(0, 3);
        action = (ChickenAction)a;
    }
}
public enum ChickenAction
{
    MoveRight,
    MoveLeft,
    Stay
}
