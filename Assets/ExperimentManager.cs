using System;
using UnityEngine;

public enum TrialResponse {
    CORRECT,
    INCORRECT,
    INVALID,
}
public class ExperimentManager : MonoBehaviour {
    private TrialsGenerator trialsGenerator;
    private TrialsRecorder trialsRecorder;
    private string ZipFilename => $"{ExperimentName}_{SubjectInitials}_{BlockNumber}_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.GetHashCode():x}";
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
        trialsGenerator = GeneratorFactory.CreateDummyMultipleTrialSequenceGenerator();
        trialsRecorder = new TrialsRecorder(OutputRootPath, ZipFilename);
    }
}
public class GeneratorFactory {
    public static SimpleTrialsGenerator CreateDummySimpleTrialsGenerator() {
        return new SimpleTrialsGenerator(
            new() {
                SimpleTrialConfig.CreateDummyTrialConfig(1),
                SimpleTrialConfig.CreateDummyTrialConfig(2),
                SimpleTrialConfig.CreateDummyTrialConfig(3),
                SimpleTrialConfig.CreateDummyTrialConfig(4)
            },
            0
        );
    }
    public static TrialsGenerator CreateDummyMultipleTrialSequenceGenerator() {
        return new MultipleTrialSequenceGenerator(
            new() {
                new OneSidedStaircaseTrialSequenceGenerator(0.0f, 1.0f, 0.5f, 3, SimpleTrialConfig.CreateFromFeatureInput),
                new OneSidedStaircaseTrialSequenceGenerator(1.0f, 2.0f, 0.5f, 3, SimpleTrialConfig.CreateFromFeatureInput),
                new OneSidedStaircaseTrialSequenceGenerator(2.0f, 3.0f, 0.5f, 3, SimpleTrialConfig.CreateFromFeatureInput),
            }
        );
    }
}