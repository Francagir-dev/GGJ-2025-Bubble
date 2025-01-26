using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
 public bool doorCanBeenOpened;
 [SerializeField]private AudioSource sound;
    public void PlaySound() {
       
       sound.Play();
    }
}
