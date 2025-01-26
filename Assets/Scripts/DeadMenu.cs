using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMenu : MonoBehaviour
{

    public void Restart() { 
        GameManager.Instance.ResetPositions();       
        GameManager.Instance.SetDefaultValuesToOxygenBar(100,0,100);
        GameManager.Instance.UpdateOxygenBar(100);

        GameManager.Instance.isInteract = false;
        GameManager.Instance.isSwimming = false;        
        GameManager.Instance.isDashing = false;
        GameManager.Instance.isResting = true;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("Character").transform.position= GameManager.Instance.playerInit.position;
        GameObject.Find("Jeff The Shark").transform.position= GameManager.Instance.sharkInit.position;
    }
    public void Quit() { 
        Application.Quit(0); 
    }
}
