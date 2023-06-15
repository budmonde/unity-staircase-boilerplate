using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public abstract class BlockConfigSchema {
    public abstract BlockRunner CreateGenerator();
}
[Serializable, XmlType("StaticBlockConfig")]
public class StaticBlockConfigSchema : BlockConfigSchema {
    [XmlElement("TrialConfig")]
    public List<TrialConfigSchema> TrialConfigs;
    public override string ToString() => XMLHandler<StaticBlockConfigSchema>.ToString(this);
    public override BlockRunner CreateGenerator() {
        return new StaticBlockRunner(this);
    }
}
[Serializable, XmlType("FeatureTwoStaircaseBlockConfig")]
public class FeatureTwoStaircaseValueConfigSchema : BlockConfigSchema {
    [XmlElement("TrialConfig")]
    public TrialConfigSchema TemplateConfig;
    [XmlAttribute]
    public int NumTrials;
    [XmlAttribute]
    public float StartValue;
    [XmlAttribute]
    public float TargetValue;
    [XmlAttribute]
    public float StepValue;
    public TrialConfigSchema CreateConfig(float currentValue) {
        var outputConfig = TemplateConfig;
        outputConfig.FeatureTwo = currentValue;
        return outputConfig;
    }
    public override string ToString() => XMLHandler<FeatureTwoStaircaseValueConfigSchema>.ToString(this);
    public override BlockRunner CreateGenerator() {
        return new FeatureTwoOneSidedStaircaseBlockConfigGenerator(this);
    }
}
[Serializable, XmlType("InterleavedMultipleBlockConfig")]
public class MultipleBlockConfigSchema : BlockConfigSchema {
    [XmlElement("StaticBlockConfig", typeof(StaticBlockConfigSchema))]
    [XmlElement("FeatureTwoStaircaseBlockConfig", typeof(FeatureTwoStaircaseValueConfigSchema))]
    public List<BlockConfigSchema> BlockConfigs;
    public override string ToString() => XMLHandler<MultipleBlockConfigSchema>.ToString(this);
    public override BlockRunner CreateGenerator() {
        return new InterleavedMultipleBlockRunner(this);
    }
}
[Serializable, XmlType("BlockLog")]
public class BlockLogSchema {
    [XmlElement("TrialLog")]
    public List<TrialLogSchema> TrialLogs;
    public override string ToString() => XMLHandler<BlockLogSchema>.ToString(this);
}