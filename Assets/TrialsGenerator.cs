using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface TrialsGenerator {
    public bool AllTrialsCompleted { get; }
    public Config CurrentTrial { get; }
    public string CurrentTrialId { get; }
    public void NextTrial(Response response);
}
public class SimpleTrialsGenerator : TrialsGenerator {
    private BlockConfig trialConfigBlock;
    private int currentTrialIdx;
    public bool AllTrialsCompleted => currentTrialIdx >= trialConfigBlock.Configs.Count;
    public Config CurrentTrial => trialConfigBlock.Configs[currentTrialIdx];
    public string CurrentTrialId => $"{currentTrialIdx:0000}";
    public void NextTrial(Response response) {
        if (AllTrialsCompleted) return;
        ++currentTrialIdx;
    }
    public SimpleTrialsGenerator(BlockConfig trialsConfigBlock, int currentTrialIdx) {
        this.trialConfigBlock = trialsConfigBlock;
        this.currentTrialIdx = currentTrialIdx;
    }
}
public interface TrialSequenceGenerator : TrialsGenerator {}
public class OneSidedStaircaseTrialSequenceGenerator : TrialSequenceGenerator {
    private Func<float, Config> trialConfigConstructor;
    private float currentValue;
    private int trialIdx;
    private int numTrials;
    private int approachSign => Math.Sign(TargetValue - StartValue);
    public float StartValue;
    public float TargetValue;
    public float StepValue;
    public bool AllTrialsCompleted => trialIdx >= numTrials;
    public Config CurrentTrial => trialConfigConstructor(currentValue);
    public string CurrentTrialId => $"{trialIdx:0000}";
    private float clampValue(float value) {
        return Math.Clamp(
            value,
            (approachSign == 1) ? StartValue : TargetValue,
            (approachSign == 1) ? TargetValue : StartValue
        );
    }
    public void NextTrial(Response response) {
        if (AllTrialsCompleted) return;
        switch(response) {
            case Response.CORRECT:
                currentValue = clampValue(currentValue + approachSign * StepValue);
                ++trialIdx;
                break;
            case Response.INCORRECT:
                currentValue = clampValue(currentValue - approachSign * StepValue);
                ++trialIdx;
                break;
            case Response.INVALID:
                break;
        }
    }
    public OneSidedStaircaseTrialSequenceGenerator(float startValue, float targetValue, float stepValue, int numTrials, Func<float, Config> trialConfigConstructor) {
        trialIdx = 0;
        StartValue = currentValue = startValue;
        TargetValue = targetValue;
        StepValue = stepValue;
        this.numTrials = numTrials;
        this.trialConfigConstructor = trialConfigConstructor;
    }
}
public class MultipleTrialSequenceGenerator : TrialsGenerator {
    private List<TrialSequenceGenerator> sequences;
    private int currentSequenceIdx;
    private TrialSequenceGenerator currentSequence => sequences[currentSequenceIdx];

    public bool AllTrialsCompleted => sequences.All(staircase => staircase.AllTrialsCompleted);
    public Config CurrentTrial => currentSequence.CurrentTrial;
    public string CurrentTrialId => $"s{currentSequenceIdx:0000}t{currentSequence.CurrentTrialId:0000}";
    public void NextTrial(Response response) {
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