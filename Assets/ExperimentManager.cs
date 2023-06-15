using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentManager : MonoBehaviour {
    private TrialsGenerator trialsGenerator;
    private TrialsRecorder trialsRecorder;
    private string ZipFilename => $"{ExperimentName}_{SubjectInitials}_{BlockNumber}_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.GetHashCode():x}";
    public bool AllTrialsCompleted => trialsGenerator.AllTrialsCompleted;
    public Config CurrentTrial => trialsGenerator.CurrentTrial;
    public string CurrentTrialId => trialsGenerator.CurrentTrialId;
    public string ExperimentName;
    public string SubjectInitials;
    public string BlockNumber;
    public string ExperimentSettingsPath;
    public string OutputRootPath;
    public void LogTrial(Response trialResponse) {
        trialsRecorder.LogTrial(new() {
            Id = CurrentTrialId,
            Config = CurrentTrial,
            Response = trialResponse
        });
    } 
    public void NextTrial(Response trialResponse) => trialsGenerator.NextTrial(trialResponse);
    public void StoreLogs() => trialsRecorder.StoreLogs();
    private void OnEnable() {
        trialsGenerator = GeneratorFactory.CreateTrialsGeneratorFromXML(ExperimentSettingsPath);
        trialsRecorder = new TrialsRecorder(OutputRootPath, ZipFilename);
    }
}
public class GeneratorFactory {
    // TODO: Test this feature again
    //public static TrialsGenerator CreateDummyMultipleTrialSequenceGenerator() {
    //    return new MultipleTrialSequenceGenerator(
    //        new() {
    //            new OneSidedStaircaseTrialSequenceGenerator(0.0f, 1.0f, 0.5f, 3, SimpleTrialConfig.CreateFromFeatureInput),
    //            new OneSidedStaircaseTrialSequenceGenerator(1.0f, 2.0f, 0.5f, 3, SimpleTrialConfig.CreateFromFeatureInput),
    //            new OneSidedStaircaseTrialSequenceGenerator(2.0f, 3.0f, 0.5f, 3, SimpleTrialConfig.CreateFromFeatureInput),
    //        }
    //    );
    //}
    public static TrialsGenerator CreateTrialsGeneratorFromXML(string csvPath) {
        BlockConfig blockConfig;
        XMLHandler<BlockConfig>.Load(csvPath, out blockConfig);
        foreach (Config trialConfig in blockConfig.Configs) {
            Debug.Log(trialConfig);
        }
        return new SimpleTrialsGenerator(blockConfig, 0);
    }
}