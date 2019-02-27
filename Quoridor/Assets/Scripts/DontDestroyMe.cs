using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType(this.GetType()).Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
