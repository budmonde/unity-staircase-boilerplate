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
    public static SimpleTrialsGenerator CreateDummyGenerator() {
        List<TrialConfig> trialList = new() {
            TrialConfig.CreateDummyTrialConfig(1),
            TrialConfig.CreateDummyTrialConfig(2),
            TrialConfig.CreateDummyTrialConfig(3),
            TrialConfig.CreateDummyTrialConfig(4)
        };
        int currentTrialIdx = 0;
        return new SimpleTrialsGenerator{trialsList=trialList, currentTrialIdx=currentTrialIdx};
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
    public MultipleTrialSequenceGenerator() {
        sequences = new() {
            new OneSidedStaircaseTrialSequenceGenerator(0.0f, 1.0f, 0.5f, 3, TrialConfig.CreateFromFeatureInput),
            new OneSidedStaircaseTrialSequenceGenerator(1.0f, 2.0f, 0.5f, 3, TrialConfig.CreateFromFeatureInput),
            new OneSidedStaircaseTrialSequenceGenerator(2.0f, 3.0f, 0.5f, 3, TrialConfig.CreateFromFeatureInput),
        };
    }
}