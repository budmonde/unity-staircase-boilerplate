using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


//public interface TrialConfig {}
public struct TrialConfig {
    public float feature;
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
        trialsGenerator = new MultipleAdaptiveTrialSequenceGenerator();
        trialsRecorder = new TrialsRecorder(
            OutputRootPath,
            $"{ExperimentName}_{SubjectInitials}_{BlockNumber}_{System.DateTime.Now.ToString("yyyyMMdd")}_{System.DateTime.Now.GetHashCode():x}"
        );
    }
}