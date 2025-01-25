using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMenu : MonoBehaviour
{

    public void Restart() { 
        GameManager.Instance.ResetPositions();
        GameManager.Instance.UpdateOxygenBar(100);
        GameManager.Instance.SetDefaultValuesToOxygenBar(100,0,100);
        
        GameManager.Instance.isInteract = false;
        GameManager.Instance.isSwimming = false;        
        GameManager.Instance.isDashing = false;
        GameManager.Instance.isResting = true;
        gameObject.SetActive(false);    
       
    }
    public void Quit() { 
        Application.Quit(0); 
    }
}
