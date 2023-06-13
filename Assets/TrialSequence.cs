using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TrialSequenceGenerator : TrialsGenerator {}

public class OneSidedStaircaseTrialSequenceGenerator : TrialSequenceGenerator {
    private Func<float, TrialConfig> trialConfigConstructor;
    private float currentValue;
    private int trialIdx;
    private int numTrials;
    private int approachSign => Math.Sign(TargetValue - StartValue);
    public float StartValue;
    public float TargetValue;
    public float StepValue;
    public bool AllTrialsCompleted => trialIdx >= numTrials;
    public TrialConfig CurrentTrial => trialConfigConstructor(currentValue);
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
    public OneSidedStaircaseTrialSequenceGenerator(float startValue, float targetValue, float stepValue, int numTrials, Func<float, TrialConfig> trialConfigConstructor) {
        trialIdx = 0;
        StartValue = currentValue = startValue;
        TargetValue = targetValue;
        StepValue = stepValue;
        this.numTrials = numTrials;
        this.trialConfigConstructor = trialConfigConstructor;
    }

}