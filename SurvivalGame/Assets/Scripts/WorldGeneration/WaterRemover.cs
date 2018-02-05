using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRemover : MonoBehaviour {

    public List<WaterOpti> waterTiles;
    bool destroyChunk = true;

	void Start ()
    {
        StartCoroutine(LateStart(1f));
	}

    bool DestroyCheck()
    {
        for(int i = 0; i < waterTiles.Count; ++i)
        {
            if(waterTiles[i].removeWater == false)
            {
                destroyChunk = false;
                return destroyChunk;
            }
        }

        return destroyChunk;
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        bool destroy = DestroyCheck();

        if(destroy == true)
        {
            Destroy(gameObject);
        }
    }

	void Update ()
    {
		
	}
}
