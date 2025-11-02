using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    [SerializeField] private Scene[] scene;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(this);
    }
    public void LoadGameScene(GameMode iMode)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)iMode);
    }
    public void LoadGameScene(int iMode)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(iMode);
    }
}