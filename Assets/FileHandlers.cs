using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public static class XMLHandler<T> where T : class {
    public static string ToString(T sourceObject) {
        try {
            var serializer = new XmlSerializer (typeof(T));
            var emptyNamespaces = new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty});
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings)) {
                serializer.Serialize(writer, sourceObject, emptyNamespaces);
                return stream.ToString();
            }
        }  catch (Exception e) {
            Debug.Log($"Exception saving XML file {e}");
            return null;
        }
    }
    public static void Save(string path, T sourceObject) {
        try {
            var serializer = new XmlSerializer (typeof(T));
            var emptyNamespaces = new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty});
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            using (var stream = new FileStream(path, FileMode.Create))
            using (var writer = XmlWriter.Create(stream, settings)) {
                serializer.Serialize(writer, sourceObject, emptyNamespaces);
            }
        }  catch (Exception e) {
            Debug.Log($"Exception saving XML file {e}");
        }
    }
    public static void Load(string path, out T targetObject) {
        try {
            var serializer = new XmlSerializer (typeof(T));
            using (var stream = new FileStream(path, FileMode.Open)) {
                targetObject = serializer.Deserialize(stream) as T;
            }
        } catch (Exception e) {
            Debug.LogError($"Exception loading XML file: {e}");
            targetObject = null;
        }
    }
}