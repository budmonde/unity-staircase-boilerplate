using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaircaseSequence : MonoBehaviour {
    public float startValue;
    public float stepValue;

    public float currentValue;

    private void OnEnable() {
        currentValue = startValue;
    }

    public void OnCorrectResponse() {

    }
    public void OnIncorrectResponse() {
        
    }
}
