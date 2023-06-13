using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public struct TrialConfig {
    public float feature;
    public static StringBuilder SerializeLabels() {
        return TypeHelpers.SerializeFieldLabels<TrialConfig>();
    }
    public StringBuilder SerializeFields() {
        return TypeHelpers.SerializeFields<TrialConfig>(this);
    }
    public override string ToString() {
        return $"{SerializeLabels()} = {SerializeFields()}";
    }
    public static TrialConfig CreateFromFeatureInput(float feature) {
        return new TrialConfig {feature=feature};
    }
    public static TrialConfig CreateDummyTrialConfig(int seed) {
        return new TrialConfig {feature=(float) seed};
    }
}
public enum TrialResponse {
    CORRECT,
    INCORRECT,
    INVALID,
}
public class ExperimentManager : MonoBehaviour {
    private TrialsGenerator trialsGenerator;
    private TrialsRecorder trialsRecorder;
    public bool AllTrialsCompleted => trialsGenerator.AllTrialsCompleted;
    public TrialConfig CurrentTrial => trialsGenerator.CurrentTrial;
    public string CurrentTrialId => trialsGenerator.CurrentTrialId;
    public string ExperimentName;
    public string SubjectInitials;
    public string BlockNumber;
    public string OutputRootPath;
    public void LogTrial(TrialResponse response) => trialsRecorder.LogTrial(CurrentTrialId, CurrentTrial, response);
    public void NextTrial(TrialResponse response) => trialsGenerator.NextTrial(response);
    public void StoreLogs() => trialsRecorder.StoreLogs();
    private void OnEnable() {
        trialsGenerator = new MultipleTrialSequenceGenerator();
        trialsRecorder = new TrialsRecorder(
            OutputRootPath,
            $"{ExperimentName}_{SubjectInitials}_{BlockNumber}_{System.DateTime.Now.ToString("yyyyMMdd")}_{System.DateTime.Now.GetHashCode():x}"
        );
    }
}