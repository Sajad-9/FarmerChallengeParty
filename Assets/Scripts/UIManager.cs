using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Text PlayerOneScore;
    [SerializeField] Text PlayerTwoScore;

    [SerializeField] Image SoundButton;
    [SerializeField] Image MusicButton;
    [SerializeField] Sprite[] ButtonSoundSprite; // 0 sould be Enabled Sound and one sould be Disabled Sound
    [SerializeField] Sprite[] ButtonMusicSprite; // 0 sould be Enabled Music and one sould be Disabled Music
    [SerializeField] Button[] ChickenGame;

    [SerializeField] bool MenuUI = false;


    private int SelectedGame = 1;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateSoundAndMusic();
        if (MenuUI) UpdateSelectedGame();
    }
    public void UpdatePlayerScore(bool iPlayer, int score)
    {
        if (iPlayer)
            PlayerTwoScore.text = score.ToString();
        else PlayerOneScore.text = score.ToString();
    }
    public void UpdateSoundAndMusic()
    {
        MusicButton.sprite = GameManager.Instance.Music ? ButtonMusicSprite[0] : ButtonMusicSprite[1];
        SoundButton.sprite = GameManager.Instance.Sound ? ButtonSoundSprite[0] : ButtonSoundSprite[1];
    }
    public void Pause(bool iPause) =>
        GameManager.Instance.PauseGame(iPause);
    public void LoadMenu() =>
        GameManager.Instance.loadMenu();
    public void SelectGame(int iGame) =>
        SelectedGame = iGame;
    public void Play() =>
        GameManager.Instance.LoadGame((GameMode)(SelectedGame));
    public void UpdateSelectedGame() =>
        ChickenGame[SelectedGame]?.Select();
    public void ToggleSound() =>
        GameManager.Instance.ToggleSound();
    public void ToggleMusic() =>
        GameManager.Instance.ToggleMusic();

}
