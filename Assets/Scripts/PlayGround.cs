using UnityEngine;



/// <summary>
/// this class made just for testing and generatoing stuff for developer *Sajad* and not to do anything in game 
/// </summary>
public class PlayGround : MonoBehaviour
{
    [SerializeField] GameObject Block;
    [SerializeField] bool doSomething = false;
    [SerializeField] float x = 100;
    [SerializeField] float y = 100;
    [SerializeField] float z = 100;
    [Tooltip("dont change me!!!!")][SerializeField] bool didSomething = false;

    private void OnDrawGizmos()
    {
        if (!doSomething) return;
        if (didSomething) return;

        for (float i = 0; i < z; i += .5f)
        {
            for (float j = 0; j < y; j += .5f)
            {
                for (float k = 0; k < x; k += .5f)
                {
                    Instantiate(Block, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
        didSomething = true;
    }
}
