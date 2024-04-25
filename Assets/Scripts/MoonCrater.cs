using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoonCrater : MonoBehaviour
{
    [SerializeField] Sprite[] craters;
    [SerializeField] Sprite[] patches;
    // Start is called before the first frame update
    
    private int craterIndex;
    void Start()
    {
        craterIndex = Random.Range(0, craters.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = craters[craterIndex];
    }
    public void Patch()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = patches[craterIndex];
    }
    
    // Get closest crater to the right of a point
    public static MoonCrater GetClosestCraterRight(Vector3 point, List<MoonCrater> craters)
    {
        MoonCrater closestCrater = null;
        float closestDistance = float.MaxValue;
        foreach (MoonCrater crater in craters)
        {
            if (crater.transform.position.x > point.x)
            {
                float distance = Vector3.Distance(point, crater.transform.position);
                if (distance < closestDistance)
                {
                    closestCrater = crater;
                    closestDistance = distance;
                }
            }
        }
        return closestCrater;
    }
}