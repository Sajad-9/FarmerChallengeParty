using UnityEngine;
using Unity.Cinemachine;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance { get; private set; }


    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Camera Setup")]
    public CinemachineCamera cineCam;    // Cinemachine 3 camera
    public Transform cameraTarget;       // midpoint target object
    public Transform sceneCenter;        // center point for intro rotation
    public GameObject IntroCamera;
    public GameObject PlayerOneWinCamera;
    public GameObject PlayerTwoWinCamera;

    [SerializeField] private bool playIntro = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
    }
    private void Start()
    {
        if (!playIntro) return;
        cineCam.gameObject.SetActive(true);
        IntroCamera.SetActive(false);
    }
    void LateUpdate()
    {
        if (player1 == null || player2 == null || cineCam == null || cameraTarget == null)
            return;


        // MOVE TARGET TO MIDPOINT
        Vector3 midpoint = (player1.position + player2.position) / 2f;
        cameraTarget.position = midpoint;
    }
    public void PlayerWin(Players iPlayer)
    {
        switch (iPlayer)
        {
            case Players.One:
                PlayerOneWinCamera.SetActive(true);
                break;
            case Players.Two:
                PlayerTwoWinCamera.SetActive(true);
                break;
            default:
                break;
        }
        cineCam.gameObject.SetActive(false);
    }
}
