using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private int actualOxygen, maxOxygen, minOxygen;
    [SerializeField, Range(0,60), Tooltip("Time in seconds to decrease Oxygen To Character")] private int timeToDecreaseOxygen;
    private float elapsedTime;
    [SerializeField, Range(1,10)] private int decreaseRatio;

    public void ChangeDecreaseRatio(int newRatio) {     

        decreaseRatio = Mathf.Clamp(newRatio,1,10);
    
    }
    
    public void Init() {
        maxOxygen = 100;
        minOxygen = 0;
        actualOxygen = 100;
        ChangeDecreaseRatio(1);
        GameManager.Instance.SetDefaultValuesToOxygenBar(maxOxygen, minOxygen, actualOxygen);   

    }
    private void FixedUpdate()
    {

        CalculateTimeToDecreaseOxygen();     

    }
    private void CalculateTimeToDecreaseOxygen() {
        elapsedTime += Time.deltaTime;
        if ((int)elapsedTime % 60 == timeToDecreaseOxygen) {
            actualOxygen -= decreaseRatio;
            elapsedTime = 0;
            GameManager.Instance.UpdateOxygenBar(actualOxygen);
        }         

      
    }
}
