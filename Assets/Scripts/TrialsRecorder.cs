using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class Recorder {
    private string outputRoot;
    private string outputFilename;
    private BlockLogSchema blockLog;
    public void LogTrial(TrialLogSchema trialLog) {
        Debug.Log(trialLog);
        blockLog.TrialLogs.Add(trialLog);
    }
    public void StoreLogs() {
        if (blockLog.TrialLogs.Count == 0) return;
        string path = $"{outputRoot}/{outputFilename}.xml";
        XMLHandler<BlockLogSchema>.Save(path, blockLog);
        blockLog = new() {TrialLogs=new()};
    }
    public Recorder(string outputRoot, string outputFilename) {
        this.outputRoot = outputRoot;
        this.outputFilename = outputFilename;
        blockLog = new() {TrialLogs=new()};
    }
}