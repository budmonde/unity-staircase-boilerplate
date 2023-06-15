using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable, XmlType]
public class Trial {
    [XmlAttribute]
    public string Id;
    [XmlElement]
    public Config Config;
    [XmlElement]
    public Response Response;
    public override string ToString() => XMLHandler<Trial>.ToString(this);
}
[Serializable, XmlType]
public class Config {
    [XmlAttribute]
    public string FeatureOne;
    [XmlAttribute]
    public float FeatureTwo;
    [XmlAttribute]
    public int FeatureThree;
    public override string ToString() => XMLHandler<Config>.ToString(this);
}
[Serializable, XmlType]
public enum Response {
    CORRECT,
    INCORRECT,
    INVALID,
}
[Serializable, XmlType]
public class Block {
    [XmlElement("Trial")]
    public List<Trial> Trials;
    public override string ToString() => XMLHandler<Block>.ToString(this);
}
[Serializable, XmlType]
public class BlockConfig {
    [XmlElement("Config")]
    public List<Config> Configs;
    public override string ToString() => XMLHandler<BlockConfig>.ToString(this);
}