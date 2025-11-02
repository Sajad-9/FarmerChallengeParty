using UnityEngine;

public class Wheat : MonoBehaviour
{

    public bool FullGrown = false;

    [SerializeField] private GameObject[] WheatSprite;
    [SerializeField] private float GrowCicleDelay = 1f;
    [SerializeField] private int GrowCicleCount = 3;
    [SerializeField] private bool AbleGrow = true;

    private int currentGrowCount = 0;
    private void Start()
    {
        if (!AbleGrow) return;

        InvokeRepeating(nameof(Grow), 0f, GrowCicleDelay);
    }

    public void Harvest()
    {
        Destroy(gameObject);
    }
    private void Grow()
    {
        if (currentGrowCount >= GrowCicleCount) { FullGrown = true; return; }
        foreach (var item in WheatSprite)
            item.SetActive(false);
        WheatSprite[currentGrowCount].SetActive(true);

        currentGrowCount++;
    }

}
