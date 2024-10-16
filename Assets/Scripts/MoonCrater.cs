using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoonCrater : MonoBehaviour
{
    [SerializeField] Sprite[] craters;
    [SerializeField] Sprite[] patches;
    // Start is called before the first frame update
    
    private int _craterIndex;
    
    public bool isPatched = false;
    void Start()
    {
        _craterIndex = Random.Range(0, craters.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = craters[_craterIndex];
    }
    public void Patch()
    {
        isPatched = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = patches[_craterIndex];
    }
    
    // Get closest crater to the right of a point
    public static (MoonCrater, float) GetClosestCraterRight(Vector3 point, List<MoonCrater> craters)
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
        return (closestCrater, closestDistance);
    }
}