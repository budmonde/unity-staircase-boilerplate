using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface TrialsGenerator {
    public bool AllTrialsCompleted { get; }
    public TrialConfig CurrentTrial { get; }
    public string CurrentTrialId { get; }
    public void NextTrial(TrialResponse response);
}
public interface TrialSequenceGenerator : TrialsGenerator {}
public class SimpleTrialsGenerator : TrialsGenerator {
    private List<TrialConfig> trialsList;
    private int currentTrialIdx;
    public bool AllTrialsCompleted => currentTrialIdx >= trialsList.Count;
    public TrialConfig CurrentTrial => trialsList[currentTrialIdx];
    public string CurrentTrialId => $"{currentTrialIdx:0000}";
    public void NextTrial(TrialResponse response) {
        if (AllTrialsCompleted) return;
        ++currentTrialIdx;
    }
    public SimpleTrialsGenerator(List<TrialConfig> trialsList, int currentTrialIdx) {
        this.trialsList = trialsList;
        this.currentTrialIdx = currentTrialIdx;
    }
}
public class MultipleTrialSequenceGenerator : TrialsGenerator {
    private List<TrialSequenceGenerator> sequences;
    private int currentSequenceIdx;
    private TrialSequenceGenerator currentSequence => sequences[currentSequenceIdx];

    public bool AllTrialsCompleted => sequences.All(staircase => staircase.AllTrialsCompleted);
    public TrialConfig CurrentTrial => currentSequence.CurrentTrial;
    public string CurrentTrialId => $"s{currentSequenceIdx:0000}t{currentSequence.CurrentTrialId:0000}";
    public void NextTrial(TrialResponse response) {
        if (AllTrialsCompleted) return;
        currentSequence.NextTrial(response);
        while (!AllTrialsCompleted) {
            currentSequenceIdx = (++currentSequenceIdx) % sequences.Count;
            if (!currentSequence.AllTrialsCompleted)
                break;
        }
    }
    public MultipleTrialSequenceGenerator(List<TrialSequenceGenerator> sequences) {
        this.sequences = sequences;
    }
}
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