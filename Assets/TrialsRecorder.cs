using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class TrialsRecorder {
    private string outputRoot;
    private string outputFilename;
    List<(string trialId, TrialConfig trialConfig, TrialResponse trialResponse)> recordedTrials;

    public void LogTrial(string trialId, TrialConfig trialConfig, TrialResponse trialResponse) {
        recordedTrials.Add(new(trialId, trialConfig, trialResponse));
        Debug.Log($"{trialId}: {trialConfig} => {trialResponse}");
    }
    private void CleanupDir(string path) {
        foreach (FileInfo f in new DirectoryInfo(path).GetFiles()) {
            f.Delete();
        }
    }
    public void StoreLogs() {
        if (recordedTrials.Count == 0) return;

        string tempRoot = $"{Application.dataPath}/_TempData";
        string tempPath = $"{tempRoot}/{outputFilename}.csv";
        string zipPath = $"{outputRoot}/{outputFilename}.zip";
        recordedTrials.Sort((a, b) => a.trialId.CompareTo(b.trialId));

        if (!Directory.Exists(tempRoot))
            Directory.CreateDirectory(tempRoot);
        CleanupDir(tempRoot);
        using (StreamWriter outStream = System.IO.File.CreateText(tempPath)) {
            outStream.Write("trial_id");
            outStream.Write(",");
            outStream.Write(recordedTrials[0].trialConfig.SerializeLabels());
            outStream.Write(",");
            outStream.Write("response");
            outStream.Write("\n");
            foreach (var item in recordedTrials) {
                outStream.Write(item.trialId);
                outStream.Write(",");
                outStream.Write(item.trialConfig.SerializeFields());
                outStream.Write(",");
                outStream.Write(item.trialResponse.ToString());
                outStream.Write("\n");
            }
        }
        if (!File.Exists(zipPath))
            ZipFile.CreateFromDirectory(tempRoot, zipPath);
        CleanupDir(tempRoot);
        Directory.Delete(tempRoot);

        recordedTrials = new();
    }
    public TrialsRecorder(string outputRoot, string outputFilename) {
        this.outputRoot = outputRoot;
        this.outputFilename = outputFilename;
        recordedTrials = new();
    }
}