using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface BlockRunner {
    public bool AllTrialsCompleted { get; }
    public TrialConfigSchema CurrentTrial { get; }
    public string CurrentTrialId { get; }
    public void NextTrial(TrialResponseSchema response);
}
public class InterleavedMultipleBlockRunner : BlockRunner {
    private List<BlockRunner> blockRunners;
    private int currentBlockIdx;
    private BlockRunner currentBlock => blockRunners[currentBlockIdx];
    public bool AllTrialsCompleted => blockRunners.All(block => block.AllTrialsCompleted);
    public TrialConfigSchema CurrentTrial => currentBlock.CurrentTrial;
    public string CurrentTrialId => $"s{currentBlockIdx:0000}t{currentBlock.CurrentTrialId:0000}";
    public void NextTrial(TrialResponseSchema response) {
        if (AllTrialsCompleted) return;
        currentBlock.NextTrial(response);
        while (!AllTrialsCompleted) {
            currentBlockIdx = (++currentBlockIdx) % blockRunners.Count;
            if (!currentBlock.AllTrialsCompleted)
                break;
        }
    }
    public InterleavedMultipleBlockRunner(MultipleBlockConfigSchema multipleBlockConfig) {
        blockRunners = new();
        foreach (var blockConfig in multipleBlockConfig.BlockConfigs) {
            blockRunners.Add(blockConfig.CreateGenerator());
        }
        currentBlockIdx = 0;
    }
}