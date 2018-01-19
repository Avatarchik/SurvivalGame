using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
    public int amount;

    private void Start()
    {
        amount = Random.Range(100, 300);
    }
}
