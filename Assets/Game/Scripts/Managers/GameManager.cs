using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities.Editor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton 인스턴스   
    public static GameManager Instance;


    private void Awake()
    {
        Instance = this;
    }
}
