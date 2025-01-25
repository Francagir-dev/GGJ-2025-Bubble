using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private int actualOxygen, maxOxygen, minOxygen;
    [SerializeField, Range(0,60), Tooltip("Time in seconds to decrease Oxygen To Character")] private int timeToDecreaseOxygen;
    private float elapsedTime;
    public int oxygenRatio; //Decrease or increase Oxygen value


    
    public void Init() {
        maxOxygen = 100;
        minOxygen = 0;
        actualOxygen = 100;
        GameManager.Instance.ChangeDecreaseRatio(1);
        GameManager.Instance.SetDefaultValuesToOxygenBar(maxOxygen, minOxygen, actualOxygen);   
        GameManager.Instance.isSwimming = true;
    }
    private void FixedUpdate()
    {

       if(!GameManager.Instance.isResting) DecreaseOxygen(); 
       else IncreaseOxygen();

    }
    //Decrease Oxygen when swimming
    private void DecreaseOxygen() {
        elapsedTime += Time.deltaTime;
        if ((int)elapsedTime % 60 == timeToDecreaseOxygen) {
            actualOxygen -= oxygenRatio;
            actualOxygen = Mathf.Clamp(actualOxygen, minOxygen, maxOxygen);
            elapsedTime = 0;
            GameManager.Instance.UpdateOxygenBar(actualOxygen);
        }

        if (actualOxygen == 0) {
            Die();
        }
    }
    public void IncreaseOxygen()
    {
        elapsedTime += Time.deltaTime;
        if ((int)elapsedTime % 60 == timeToDecreaseOxygen)
        {
            actualOxygen += oxygenRatio;
            actualOxygen = Mathf.Clamp(actualOxygen, minOxygen, maxOxygen);
            elapsedTime = 0;
            GameManager.Instance.UpdateOxygenBar(actualOxygen);
        }

    }
    private void Die()
    {
        GameManager.Instance.hasDead = true;

    }
}
