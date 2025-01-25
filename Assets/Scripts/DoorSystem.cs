using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public bool playerHasObject;
    public bool payerInteractedDoorWithObject;

    private string doorNumber;
    private string objectNumber;

    [SerializeField] private Transform door;
    [SerializeField] private AudioSource sound;


    public bool CheckObject() { 
    return doorNumber.Equals(objectNumber);
    }
   
    public bool CanOpenDoor() {
        return playerHasObject && payerInteractedDoorWithObject;
    }
    public void OpenDoor() {
        if(CanOpenDoor())door.Rotate(0f,90f,0f);
        else sound.Play();
    }
}
