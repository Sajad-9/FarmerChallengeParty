using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private const string MUSIC_BOOL = "Music";
    private const string SOUND_BOOL = "Sound";
    private const string GROUND_TAG = "Ground";
    private const string PLAYER_ONE_TAG = "PlayerOne";
    private const string PLAYER_TWO_TAG = "PlayerTwo";

    public static GameManager Instance { get; private set; }

    [Tooltip("index 0 is player one and index 1 is player 2")]
    public GameObject[] PlayerGameObjects;
    public Material[] BlockMaterials;
    public GameMode Mode = GameMode.None;


    [Header("Chicken Game Setting")]
    [SerializeField] GameObject ChickenPrefab;
    [SerializeField] int WinScore = 10;
    [SerializeField] int PlayerOneScore = 0;
    [SerializeField] int PlayerTwoScore = 0;
    [SerializeField] bool Test = false;

    [Header("Farmer Game Setting")]
    [SerializeField] public GameObject WheatPrefab;

    [Header("Vegetable Game Setting")]
    [SerializeField] public GameObject BalloonObject;


    [Header("Audio Values")]
    [SerializeField] public bool Music = true;
    [SerializeField] public bool Sound = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
    }
    private void Start()
    {
        switch (Mode)
        {
            case GameMode.None:
                AudioManager.Instance.PlayMusic("Menu");
                break;
            case GameMode.ChickenGame:
                AudioManager.Instance.PlayMusic("Chicken Game");
                ChickenGameUpdate();
                break;
            case GameMode.FarmingGame:
                AudioManager.Instance.PlayMusic("Farming Game");
                FarmingGameUpdate();
                break;
            case GameMode.CannonGame:
                AudioManager.Instance.PlayMusic("Cannon Game");
                CanonGameUpdate();
                break;
            case GameMode.FishingGame:
                AudioManager.Instance.PlayMusic("Fishing Game");
                FishingGameUpdate();
                break;
            default:
                break;
        }

        if (!PlayerPrefs.HasKey(SOUND_BOOL))
            PlayerPrefs.SetInt(SOUND_BOOL, 1);
        if (!PlayerPrefs.HasKey(MUSIC_BOOL))
            PlayerPrefs.SetInt(MUSIC_BOOL, 1);

        Music = PlayerPrefs.GetInt(MUSIC_BOOL, 1) == 1;
        Sound = PlayerPrefs.GetInt(SOUND_BOOL, 1) == 1;
    }


    private void Update()
    {
        if (Test)
        {
            PlayerGameObjects[0].GetComponent<PlayerController>().Death(1f);
            Test = false;
        }

    }

    //-----------------------------------------------------Game Mode Update------------------------------------------------------
    private void ChickenGameUpdate()
    {

    }
    private void FarmingGameUpdate()
    {

    }
    private void CanonGameUpdate()
    {

    }
    private void FishingGameUpdate()
    {

    }

    //-----------------------------------------------------Game Functions------------------------------------------------------
    public void AddScore(Players iPlayer)
    {
        switch (iPlayer)
        {
            case global::Players.One:
                PlayerOneScore++;
                UIManager.Instance.UpdatePlayerScore(false, PlayerOneScore);
                break;
            case global::Players.Two:
                PlayerTwoScore++;
                UIManager.Instance.UpdatePlayerScore(true, PlayerTwoScore);
                break;
            default:
                break;
        }
        CheckWin();

    }
    public void PlayerTochedBlock(Players iPlayer, Block iBlock)
    {
        if (iBlock.Type == BlockType.Tnt)
        {
            PlayerGameObjects[(int)iPlayer].GetComponent<PlayerController>().Death(3f);
            RaycastHit[] Chickens = Physics.SphereCastAll(new Ray(iBlock.transform.position, Vector3.up), 1f);
            foreach (var chicken in Chickens)
            {
                Chicken ChickenScript = chicken.transform.gameObject.GetComponent<Chicken>();
                if (ChickenScript != null)
                    ChickenScript.Death();
            }
            //Kill Chicken on the top of tnt
            iBlock.BlockTochedEffect(BlockType.Box);
            iBlock.BlockSetup(BlockType.Box);
        }
        else if (iBlock.Type == BlockType.Box)
        {
            int r = Random.Range((int)BlockType.Box, (int)BlockType.Tnt + 1); // choosing between randome box or chicken box or tnt

            RaycastHit[] Chickens = Physics.SphereCastAll(new Ray(iBlock.transform.position, Vector3.up), 1f);
            foreach (var chicken in Chickens)
            {
                Chicken ChickenScript = chicken.transform.gameObject.GetComponent<Chicken>();
                if (ChickenScript != null)
                    ChickenScript.Jump();
            }

            //move Chicken on top
            iBlock.BlockTochedEffect((BlockType)r);
            iBlock.BlockSetup((BlockType)r);
        }
        else if (iBlock.Type == BlockType.ChickenBox)
        {
            Vector3 ChickenSpawnPoint = iBlock.transform.position + new Vector3(0, 1f, 0);
            Instantiate(ChickenPrefab, ChickenSpawnPoint, Quaternion.identity);
            iBlock.BlockTochedEffect(BlockType.Box);
            iBlock.BlockSetup(BlockType.Box);
        }
    }
    public void PlayerGetChickenToPen(PlayerController iPlayer, GameObject iPen)
    {
        if (!iPlayer.HasChicken) return;
        iPlayer.HasChicken = false;
        iPlayer.ChickenObject.MoveToPen(iPen);
        iPlayer.ChickenObject = null;
        AddScore(iPlayer.gameObject.CompareTag(PLAYER_ONE_TAG) ? Players.One : Players.Two);
    }
    //-----------------------------------------------------Logical Functions------------------------------------------------------
    public void LoadGame(GameMode iMode)
    {
        Mode = iMode;
        SceneManager.Instance.LoadGameScene(iMode);
    }
    public void Won(Players iPlayer)
    {
        CameraEffects.Instance.PlayerWin(iPlayer);
        PlayerGameObjects[(int)iPlayer].GetComponent<PlayerController>().Won();
        Invoke(nameof(loadMenu), 7f);
    }
    public void CheckWin()
    {
        if (PlayerOneScore >= WinScore)
            Won(Players.One);
        if (PlayerTwoScore >= WinScore)
            Won(Players.Two);
    }
    public void PauseGame(bool iPause = true)
    {
        Time.timeScale = iPause ? 0 : 1;
    }
    public void loadMenu()
    {
        SceneManager.Instance.LoadGameScene(0);
    }
    public void ToggleSound()
    {
        Sound = Sound ? false : true;
        PlayerPrefs.SetInt(SOUND_BOOL, Sound ? 1 : 0);
    }
    public void ToggleMusic()
    {
        Music = Music ? false : true;
        PlayerPrefs.SetInt(MUSIC_BOOL, Music ? 1 : 0);
    }
}

public enum GameMode
{
    None,
    ChickenGame,
    FarmingGame,
    CannonGame,
    FishingGame
}
public enum Players
{
    One,
    Two
}