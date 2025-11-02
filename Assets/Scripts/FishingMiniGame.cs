using UnityEngine;
using UnityEngine.UIElements;

public class FishingMiniGame : MonoBehaviour
{
    public bool Fishing = false;

    [SerializeField] private PlayerController player;
    [SerializeField] private Players playerID = Players.One;
    [SerializeField] private Rigidbody2D hookRB;
    [SerializeField] private Transform topPivot;
    [SerializeField] private Transform bottomPivot;
    [SerializeField] private Transform ProgressBarContainer;
    [SerializeField] private Transform fish;
    [SerializeField] private float timerMultiplicator = 3f;
    [SerializeField] private float smoothMotion = 1f;
    [SerializeField] private float hookPower = 1f;
    [SerializeField] private float hookProgressValue = .1f;
    [SerializeField] private float hookDownProgressValue = .1f;



    private Transform fishStartingPoint;
    private Transform hookStartingPoint;
    private float fishPosition;
    private float fishDestination;
    private float fishTimer;
    private float fishSpeed;
    private float hookProgress = 0.1f;


    private void Start()
    {
        fishStartingPoint = fish;
        hookStartingPoint = hookRB.transform;
        ReStart();
    }
    private void Update()
    {
        Fish();
        Hook();
        ProgressCheck();
    }
    void Fish()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0)
        {
            fishTimer = Random.value * timerMultiplicator;
            fishDestination = Random.value;
        }
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }
    void Hook()
    {
        if (player.HookingFish())
        {
            hookRB.AddForceY(hookPower);
            AudioManager.Instance.PlaySFX("Hook");
        }
    }
    void ProgressCheck()
    {
        Vector3 scale = ProgressBarContainer.localScale;
        scale.y = hookProgress;
        ProgressBarContainer.localScale = scale;

        if (Fishing)
            hookProgress += hookProgressValue * Time.deltaTime;
        else
            hookProgress -= hookDownProgressValue * Time.deltaTime;

        hookProgress = Mathf.Clamp01(hookProgress);

        if (hookProgress >= (1 - float.Epsilon))
            Catch();
        else if (hookProgress <= float.Epsilon)
            Lost();

    }
    void Catch()
    {
        AudioManager.Instance.PlaySFX("Fish.Catched");
        ReStart();
        gameObject.SetActive(false);
        GameManager.Instance.AddScore(playerID);
    }
    void Lost()
    {
        AudioManager.Instance.PlaySFX("Fish.Lost");
        ReStart();
        gameObject.SetActive(false);
    }
    void ReStart()
    {
        hookRB.transform.position = hookStartingPoint.position;
        fish.position = fishStartingPoint.position;
        hookProgress = 0.1f;
        Vector3 scale = ProgressBarContainer.localScale;
        scale.y = hookProgress;
        ProgressBarContainer.localScale = scale;
    }
}
