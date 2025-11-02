using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public bool DisableMove = false;


    public GameObject ChickenHolder;
    public Chicken ChickenObject;
    public int SeedCount = 3;
    public bool HasChicken = false;

    public GameObject PlaceAbleGrassPrefab;

    public GameObject VegetableHolder;
    public Cannon CannonObject;
    public Vegetable VegetableObject;
    public bool HasVegetable = false;
    public bool UsingCannon = false;

    public GameObject FishingGame;
    public float FishingCooldown = 1f;
    public bool CanFish = false;

    private readonly int AnimSpeedHash = Animator.StringToHash("Speed");
    private readonly int AnimJumpingHash = Animator.StringToHash("Jumping");
    private readonly int AnimDirectionHash = Animator.StringToHash("Direction");
    private static string GROUND_TAG = "Ground";

    [Header("Movement Settings")]
    [SerializeField] private GameObject root;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float deathJumpForce = 5f;
    [SerializeField] private float intractionDelay = .5f;
    [SerializeField] private int faceDirection = 1; // 1 is right and -1 is left

    private Rigidbody rb;
    private Animator animator;
    private Vector2 moveInput;
    private Vector3 StartingPosition;
    private bool isGrounded = true;
    private bool AbleIntract = true;
    private bool isFishing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        StartingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (DisableMove) return;
        HandleMovement();
        HandleJump();
        HandleAction();
    }

    private void Update()
    {
        UpdateDirectionalFlip();
    }
    public void Death(float iReSpawnTime)
    {
        //death animation
        AudioManager.Instance.PlaySFX("Player.Death");
        Vector3 f = StartingPosition - transform.position;
        rb.linearVelocity = f * deathJumpForce;
        DisableMove = true;
        Invoke(nameof(ReSpawn), iReSpawnTime);
    }
    public void Won()
    {
        DisableMove = true;
        InvokeRepeating(nameof(Jump), .5f, 2f);
    }
    public void PlayDontHaveSeedAnim()
    {
        print(this.name + "dont have seed");
    }

    public bool HookingFish() => moveInput.y < 0;

    private void ReSpawn()
    {
        transform.position = StartingPosition;
        //Reset the DeathAnimation
        //ReSpawn Animationo
        DisableMove = false;
    }

    // -------------------------------------- Movement Handlers -------------------------------------------------------
    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, 0) * moveSpeed;
        //if (Mathf.Abs(rb.linearVelocity.x) < 5f)
        //    rb.AddForce(move, ForceMode.Impulse);
        transform.Translate(move * Time.deltaTime);
        AudioManager.Instance.PlaySFX("Player.Move");

    }
    private void HandleJump()
    {
        if (!isGrounded) return;
        if (moveInput.y <= 0) return;
        Jump();
    }
    private void HandleAction()
    {
        if (moveInput.y >= 0) return;
        if (!AbleIntract) return;

        if (GameManager.Instance.Mode == GameMode.FarmingGame)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position - new Vector3(0, -.25f, 0), .1f, Vector3.down, out hit))
            {
                Block block = hit.transform.GetComponent<Block>();
                if (block != null && (block.Type == BlockType.Grass || block.Type == BlockType.Dirt))
                {
                    block.PlantSeed(this);
                }
                else if (block != null && block.Type == BlockType.Air)
                {
                    Instantiate(PlaceAbleGrassPrefab, hit.transform.position, hit.transform.rotation);
                }
            }
        }
        if (GameManager.Instance.Mode == GameMode.CannonGame)
        {
            if (UsingCannon && CannonObject != null)
            {
                CannonObject.Shoot(VegetableObject);
                HasVegetable = false;
                VegetableObject = null;
            }
        }
        if (GameManager.Instance.Mode == GameMode.FishingGame)
        {
            if (FishingGame.activeInHierarchy)
            {
                //FishingGame Input ... 
                // hook should go up when this bar is presed
                return;
            }

            if (isFishing)
            {
                CancelInvoke(nameof(Fish));
            }
            if (CanFish)
            {
                Invoke(nameof(Fish), FishingCooldown);
                isFishing = true;
            }
        }
        AudioManager.Instance.PlaySFX("Player.Intract");

        AbleIntract = false;
        Invoke(nameof(IntractionsReset), intractionDelay);
    }
    private void IntractionsReset() => AbleIntract = true;
    private void Fish()
    {
        isFishing = false;
        FishingGame.SetActive(true);
    }

    private void Jump()
    {
        AudioManager.Instance.PlaySFX("Player.Jump");
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, 0f);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // -------------------------------------- Animations ---------------------------------------------------------
    private void UpdateDirectionalFlip()
    {
        if (moveInput.x * faceDirection >= 0) return;
        faceDirection *= -1;
    }

    // -------------------------------------- Input System -------------------------------------------------------
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // -------------------------------------- Physics ------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG)) isGrounded = true;
        AudioManager.Instance.PlaySFX("Player.Land");
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG)) isGrounded = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.Mode == GameMode.CannonGame)
            if (other.gameObject.CompareTag(GROUND_TAG)) isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.Instance.Mode == GameMode.CannonGame)
            if (other.gameObject.CompareTag(GROUND_TAG)) isGrounded = false;
    }
}