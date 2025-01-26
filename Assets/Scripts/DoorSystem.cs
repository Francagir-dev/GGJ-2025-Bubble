using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{

 [SerializeField]private AudioSource sound;
 [SerializeField]private AudioClip openSound;
    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    public void ChangeSound()
    {

        sound.clip= openSound;
        PlaySound();
    }

    public void PlaySound() {
       
       sound.Play();
    }
}
