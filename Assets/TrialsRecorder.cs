using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class TrialsRecorder {
    private string outputRoot;
    private string outputFilename;
    private Block trialBlock;
    public void LogTrial(Trial trial) {
        Debug.Log(trial);
        trialBlock.Trials.Add(trial);
    }
    public void StoreLogs() {
        if (trialBlock.Trials.Count == 0) return;
        string path = $"{outputRoot}/{outputFilename}.xml";
        XMLHandler<Block>.Save(path, trialBlock);
        trialBlock = new() {Trials=new()};
    }
    public TrialsRecorder(string outputRoot, string outputFilename) {
        this.outputRoot = outputRoot;
        this.outputFilename = outputFilename;
        trialBlock = new() {Trials=new()};
    }
}