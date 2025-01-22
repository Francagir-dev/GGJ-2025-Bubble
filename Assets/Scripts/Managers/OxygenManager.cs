using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private int actualOxygen, maxOxygen, minOxygen;
    [SerializeField, Range(0,60), Tooltip("Time in seconds to decrease Oxygen To Character")]  private int timeToDecreaseOxygen;
    private float elapsedTime;
    [Range(0,100)] public int decreaseRatio = 1;
        
    public void Init() {
        maxOxygen = 100;
        minOxygen = 0;
        actualOxygen = 100;
        GameManager.Instance.SetDefaultValuesToOxygenBar(maxOxygen, minOxygen, actualOxygen);
    }
    private void FixedUpdate()
    {

        CalculateTime();
      
       
    }
    private void CalculateTime() {
        elapsedTime += Time.deltaTime;
        if ((int)elapsedTime % 60 == timeToDecreaseOxygen) {
            actualOxygen -= 5;
            elapsedTime = 0;
            GameManager.Instance.UpdateOxygenBar(actualOxygen);
        }
            

      
    }
}
