using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public bool[] doorsOpened = new bool[4];
    public bool canOpenExitDoor { get; private set; }

    private bool hasDead;

    public void Init()
    {
        if (!hasDead) {
            ResetAll();
        }
            
        
    }
    public void ResetAll() {
        hasDead = false;
        for(int i = 0; i < doorsOpened.Length; i++) doorsOpened[i] = false;
        canOpenExitDoor = false;
    }
}
