using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticBlockRunner : BlockRunner {
    private StaticBlockConfigSchema blockConfig;
    private int currentTrialIdx;
    public bool AllTrialsCompleted => currentTrialIdx >= blockConfig.TrialConfigs.Count;
    public TrialConfigSchema CurrentTrial => blockConfig.TrialConfigs[currentTrialIdx];
    public string CurrentTrialId => $"{currentTrialIdx:0000}";
    public void NextTrial(TrialResponseSchema response) {
        if (AllTrialsCompleted) return;
        ++currentTrialIdx;
    }
    public StaticBlockRunner(StaticBlockConfigSchema blockConfig) {
        this.blockConfig = blockConfig;
        currentTrialIdx = 0;
    }
}
public interface DynamicBlockRunner : BlockRunner {}
public class FeatureTwoOneSidedStaircaseBlockConfigGenerator : DynamicBlockRunner {
    private FeatureTwoStaircaseValueConfigSchema valueConfig;
    private float currentValue;
    private int trialIdx;
    private int approachSign => Math.Sign(valueConfig.TargetValue - valueConfig.StartValue);
    public bool AllTrialsCompleted => trialIdx >= valueConfig.NumTrials;
    public TrialConfigSchema CurrentTrial => valueConfig.CreateConfig(currentValue);
    public string CurrentTrialId => $"{trialIdx:0000}";
    private float clampValue(float value) {
        return Math.Clamp(
            value,
            (approachSign == 1) ? valueConfig.StartValue : valueConfig.TargetValue,
            (approachSign == 1) ? valueConfig.TargetValue : valueConfig.StartValue
        );
    }
    public void NextTrial(TrialResponseSchema response) {
        if (AllTrialsCompleted) return;
        switch(response) {
            case TrialResponseSchema.CORRECT:
                currentValue = clampValue(currentValue + approachSign * valueConfig.StepValue);
                ++trialIdx;
                break;
            case TrialResponseSchema.INCORRECT:
                currentValue = clampValue(currentValue - approachSign * valueConfig.StepValue);
                ++trialIdx;
                break;
            case TrialResponseSchema.INVALID:
                break;
        }
    }
    public FeatureTwoOneSidedStaircaseBlockConfigGenerator(FeatureTwoStaircaseValueConfigSchema valueConfig) {
        this.valueConfig = valueConfig;
        currentValue = valueConfig.StartValue;
        trialIdx = 0;
    }
}