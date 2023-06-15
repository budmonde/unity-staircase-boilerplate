using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour {
    private BlockRunner blockRunner;
    private Recorder recorder;
    private string ZipFilename => $"{ExperimentName}_{SubjectInitials}_{BlockNumber}_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.GetHashCode():x}";
    public bool AllTrialsCompleted => blockRunner.AllTrialsCompleted;
    public TrialConfigSchema CurrentTrial => blockRunner.CurrentTrial;
    public string CurrentTrialId => blockRunner.CurrentTrialId;
    public string ExperimentName;
    public string SubjectInitials;
    public string BlockNumber;
    public string ExperimentSettingsPath;
    public string OutputRootPath;
    public void LogTrial(TrialResponseSchema trialResponse) {
        recorder.LogTrial(new() {
            Id = CurrentTrialId,
            Config = CurrentTrial,
            Response = trialResponse
        });
    } 
    public void NextTrial(TrialResponseSchema trialResponse) => blockRunner.NextTrial(trialResponse);
    public void StoreLogs() => recorder.StoreLogs();
    private void OnEnable() {
        MultipleBlockConfigSchema blockConfig;
        XMLHandler<MultipleBlockConfigSchema>.Load(ExperimentSettingsPath, out blockConfig);
        Debug.Log(blockConfig);
        blockRunner = blockConfig.CreateGenerator();
        recorder = new Recorder(OutputRootPath, ZipFilename);
    }
}