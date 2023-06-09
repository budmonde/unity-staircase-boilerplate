using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct TrialConfig {
    public string foo;
    public int bar;
    public float baz;
}
public class ExperimentManager : MonoBehaviour {
    private List<TrialConfig> trialsList;

    public int currentTrialIdx;
    public int NumTrials => trialsList.Count;
    public TrialConfig CurrentTrial => trialsList[currentTrialIdx];
    public bool AllTrialsCompleted => currentTrialIdx >= NumTrials;

    public void NextTrial() => ++currentTrialIdx;

    private void OnEnable() {
        trialsList = GenerateDummyTrialList();
        currentTrialIdx = 0;
    }
    private List<TrialConfig> GenerateDummyTrialList() {
        List<TrialConfig> tl = new() {
            new TrialConfig {
                foo = "one",
                bar = 1,
                baz = 1f,
            },
            new TrialConfig {
                foo = "two",
                bar = 2,
                baz = 2f,
            },
            new TrialConfig {
                foo = "three",
                bar = 3,
                baz = 3f,
            },
            new TrialConfig {
                foo = "four",
                bar = 4,
                baz = 4f,
            }
        };
        return tl;
    }
}
