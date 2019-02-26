﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSparksUserID : MonoBehaviour
{
    public string myUserID;

    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<GameSparksUserID>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
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
