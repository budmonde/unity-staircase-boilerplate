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

public class DummyTrialsGenerator : TrialsGenerator {
    private List<TrialConfig> trialsList;
    private int currentTrialIdx;
    public bool AllTrialsCompleted => currentTrialIdx >= trialsList.Count;
    public TrialConfig CurrentTrial => trialsList[currentTrialIdx];
    public string CurrentTrialId => $"{currentTrialIdx:0000}";
    public void NextTrial(TrialResponse response) {
        if (AllTrialsCompleted) return;
        ++currentTrialIdx;
    }
    public DummyTrialsGenerator() {
        trialsList = new() {
            new TrialConfig {
                feature = 1f,
            },
            new TrialConfig {
                feature = 2f,
            },
            new TrialConfig {
                feature = 3f,
            },
            new TrialConfig {
                feature = 4f,
            }
        };
        currentTrialIdx = 0;
    }
}

public class MultipleAdaptiveTrialSequenceGenerator : TrialsGenerator {
    private List<AdaptiveTrialSequence> sequences;
    private int currentSequenceIdx;
    private AdaptiveTrialSequence currentSequence => sequences[currentSequenceIdx];

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
    public MultipleAdaptiveTrialSequenceGenerator() {
        sequences = new() {
            new OneSidedStaircaseSequence(0.0f, 1.0f, 0.5f, 3),
            new OneSidedStaircaseSequence(1.0f, 2.0f, 0.5f, 3),
            new OneSidedStaircaseSequence(2.0f, 3.0f, 0.5f, 3),
        };
    }
}