using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable, XmlType("TrialConfig")]
public class TrialConfigSchema {
    [XmlAttribute]
    public string FeatureOne;
    [XmlAttribute]
    public float FeatureTwo;
    [XmlAttribute]
    public int FeatureThree;
    public override string ToString() => XMLHandler<TrialConfigSchema>.ToString(this);
}
[Serializable, XmlType("TrialResponse")]
public enum TrialResponseSchema {
    CORRECT,
    INCORRECT,
    INVALID,
}
[Serializable, XmlType("TrialLog")]
public class TrialLogSchema {
    [XmlAttribute]
    public string Id;
    [XmlElement]
    public TrialConfigSchema Config;
    [XmlElement]
    public TrialResponseSchema Response;
    public override string ToString() => XMLHandler<TrialLogSchema>.ToString(this);
}