using System;
using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    private const string GROUND_TAG = "Ground";
    private const string PLAYER_ONE_TAG = "PlayerOne";
    private const string PLAYER_TWO_TAG = "PlayerTwo";


    public BlockType Type;
    public Block[] AttachedBlocks;

    [SerializeField] private Transform wheatholder;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float DestroyDelay = 2f;
    [SerializeField] private bool PlaceAble = false;


    private Material[] materials;
    private Wheat wheat;
    private Vector3 firstPosition;
    private int jumpDirection = 1;
    private bool tochAble = false;
    private bool jump = false;
    private bool planted = false;
    private bool ableDestry = true;

    void Start()
    {
        firstPosition = transform.position;
        materials = GameManager.Instance.BlockMaterials;
        BlockSetup(Type);
        if (PlaceAble) { StartCoroutine(Grow()); Invoke(nameof(ClearBlock), DestroyDelay); }
    }
    void Update()
    {
        if (jump)
            transform.Translate(jumpDirection * Vector3.up * Time.deltaTime * jumpForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!tochAble) return;


        if (collision.gameObject.CompareTag(PLAYER_ONE_TAG))
        {
            GameManager.Instance.PlayerTochedBlock(Players.One, this);
            switch (Type)
            {
                case BlockType.Box:
                    break;
                case BlockType.ChickenBox:
                    break;
                case BlockType.Tnt:
                    AudioManager.Instance.PlaySFX("Block.Tnt");
                    break;
                default:
                    break;
            }
        }
        if (collision.gameObject.CompareTag(PLAYER_TWO_TAG))
        {
            GameManager.Instance.PlayerTochedBlock(Players.Two, this);
        }

    }

    public void BlockSetup(BlockType iType)
    {
        Type = iType;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && materials.Length > (int)iType)
            meshRenderer.material = materials[(int)iType];

        tochAble = false;

        switch (iType)
        {
            case BlockType.Air:
                gameObject.GetComponent<Collider>().isTrigger = true;
                break;
            case BlockType.Dirt:
                gameObject.GetComponent<Collider>().isTrigger = false;
                gameObject.tag = GROUND_TAG;
                break;
            case BlockType.Grass:
                gameObject.GetComponent<Collider>().isTrigger = false;
                gameObject.tag = GROUND_TAG;
                break;
            case BlockType.Stone:
                if (GameManager.Instance.Mode != GameMode.ChickenGame) gameObject.tag = GROUND_TAG;
                break;
            case BlockType.Water:
                break;
            case BlockType.Box:
                gameObject.GetComponent<Collider>().isTrigger = false;
                if (GameManager.Instance.Mode == GameMode.ChickenGame) Invoke(nameof(SetTochable), .1f);
                if (GameManager.Instance.Mode == GameMode.CannonGame) gameObject.tag = GROUND_TAG;
                break;
            case BlockType.ChickenBox:
                Invoke(nameof(SetTochable), .1f);
                break;
            case BlockType.Tnt:
                Invoke(nameof(SetTochable), 1f);
                break;
            case BlockType.Ladder:
                gameObject.GetComponent<Collider>().isTrigger = true;
                gameObject.tag = GROUND_TAG;
                break;
            case BlockType.Platform:
                gameObject.GetComponent<Collider>().isTrigger = true;
                gameObject.tag = GROUND_TAG;
                break;
            default:
                break;
        }
    }

    public void BlockTochedEffect(BlockType iType)
    {
        AudioManager.Instance.PlaySFX("Block.Toched");
        jump = true;
        Invoke(nameof(StopJumping), jumpDuration);
        Invoke(nameof(ChangeJumpDirection), jumpDuration / 2f);
        Invoke(nameof(ChangeJumpDirection), jumpDuration);
    }
    public void PlantSeed(PlayerController iPlayer)
    {
        if (planted)
        {
            AudioManager.Instance.PlaySFX("Block.PlantRejected");
            if (wheat == null) return;
            if (wheat.FullGrown) Harvest(iPlayer);
            return;
        }
        if (iPlayer.SeedCount == 0) { iPlayer.PlayDontHaveSeedAnim(); return; }

        AudioManager.Instance.PlaySFX("Block.Palanted");
        if (PlaceAble) ableDestry = false;
        iPlayer.SeedCount--;
        BlockSetup(BlockType.Dirt);
        foreach (var item in AttachedBlocks)
            item.BlockSetup(BlockType.Dirt);
        wheat = Instantiate(GameManager.Instance.WheatPrefab, wheatholder).GetComponent<Wheat>();

        planted = true;
    }
    public void Harvest(PlayerController iPlayer)
    {
        AudioManager.Instance.PlaySFX("Harvest");
        iPlayer.SeedCount++;
        GameManager.Instance.AddScore(iPlayer.gameObject.CompareTag(PLAYER_ONE_TAG) ? Players.One : Players.Two);
        wheat.Harvest();
        wheat = null;
        Invoke(nameof(TurnGrass), 0.5f);
    }

    private void TurnGrass()
    {
        planted = false;
        BlockSetup(BlockType.Grass);
        foreach (var item in AttachedBlocks)
            item.BlockSetup(BlockType.Grass);
        if (PlaceAble) { ableDestry = true; Invoke(nameof(ClearBlock), DestroyDelay); }
    }
    private void StopJumping() { jump = false; transform.position = firstPosition; }
    private void ChangeJumpDirection() => jumpDirection *= -1;
    private void SetTochable() => tochAble = true;
    private void ClearBlock() { if (ableDestry) Destroy(gameObject); }

    private IEnumerator Grow()
    {
        float time = 0f;
        while (time < .2f)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one / 2f, time / .2f);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one / 2f; // make sure it ends exactly
    }
}

public enum BlockType
{
    Air,
    Dirt,
    Grass,
    Stone,
    Water,
    Box,
    ChickenBox,
    Tnt,
    Gold,
    Ice,
    BlueIce,
    Snow,
    Ladder,
    Platform,

}