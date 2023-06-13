using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AdaptiveTrialSequence {
    public bool AllTrialsCompleted { get; }
    public TrialConfig CurrentTrial { get; }
    public string CurrentTrialId { get; }
    public void NextTrial(TrialResponse response);
}

public interface StaircaseTrialSequence : AdaptiveTrialSequence {
}

public class OneSidedStaircaseSequence : StaircaseTrialSequence {
    private float currentValue;
    private int trialIdx;
    private int numTrials;
    private int approachSign => Math.Sign(TargetValue - StartValue);
    public float StartValue;
    public float TargetValue;
    public float StepValue;
    public bool AllTrialsCompleted => trialIdx >= numTrials;
    public TrialConfig CurrentTrial => new() {feature=currentValue};
    public string CurrentTrialId => $"{trialIdx:0000}";
    private float clampValue(float value) {
        return Math.Clamp(
            value,
            (approachSign == 1) ? StartValue : TargetValue,
            (approachSign == 1) ? TargetValue : StartValue
        );
    }
    public void NextTrial(TrialResponse response) {
        if (AllTrialsCompleted) return;
        switch(response) {
            case TrialResponse.CORRECT:
                currentValue = clampValue(currentValue + approachSign * StepValue);
                ++trialIdx;
                break;
            case TrialResponse.INCORRECT:
                currentValue = clampValue(currentValue - approachSign * StepValue);
                ++trialIdx;
                break;
            case TrialResponse.INVALID:
                break;
        }
    }
    public OneSidedStaircaseSequence(float startValue, float targetValue, float stepValue, int numTrials) {
        trialIdx = 0;
        StartValue = currentValue = startValue;
        TargetValue = targetValue;
        StepValue = stepValue;
        this.numTrials = numTrials;
    }

}