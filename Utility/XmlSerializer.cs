using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public static class EntityXmlSerializer<T>
{
    public static void XmlSerialize(string path, T entity)
    {
        XmlSerialize(path, entity, Encoding.UTF8);
    }

    public static void XmlSerialize(string path, T entity, Encoding code)
    {
        using (var sw = new StreamWriter(path, false, code))
        {
            var serialization = new XmlSerializer(entity.GetType());
            serialization.Serialize(sw, entity);
        }
    }

    public static T ReadFromFile(string xmlPath)
    {
        return ReadFromFile(xmlPath, Encoding.UTF8);
    }

    public static T ReadFromFile(string xmlPath, Encoding encoding)
    {
        if (!File.Exists(xmlPath))
        {
            return default(T);
        }
        using (StreamReader reader = new StreamReader(xmlPath, encoding))
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            var entity = (T)xs.Deserialize(reader);
            return entity;
        }
    }

    public static T ReadFromContent(string xmlContent)
    {
        if (string.IsNullOrEmpty(xmlContent))
        {
            return default(T);
        }
        var bytes = Encoding.UTF8.GetBytes(xmlContent);
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            var entity = (T)xs.Deserialize(stream);
            return entity;
        }
    }
}

public static class XmlUtility
{
    public static Tuple<XmlDocument, XmlNode> GetNodeFromDocument(string xmlFile, string xPath)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFile);
        //XPath https://docs.microsoft.com/zh-cn/previous-versions/ms256086(v=vs.120)?redirectedfrom=MSDN
        var root = xmlDoc.SelectSingleNode(xPath);
        return new Tuple<XmlDocument, XmlNode>(xmlDoc, root);
    }

    public static Tuple<XmlDocument, XmlNode> GetNodeFromContent(string xmlContent, string xPath)
    {
        if (string.IsNullOrEmpty(xmlContent))
        {
            return new Tuple<XmlDocument, XmlNode>(null, null);
        }
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlContent);
        //XPath https://docs.microsoft.com/zh-cn/previous-versions/ms256086(v=vs.120)?redirectedfrom=MSDN
        var root = xmlDoc.SelectSingleNode(xPath);
        return new Tuple<XmlDocument, XmlNode>(xmlDoc, root);
    }


    public static void AppendAttribute(XmlDocument xmlDoc, XmlNode node, string attributeName, string value)
    {
        var attr = xmlDoc.CreateAttribute(attributeName);
        attr.Value = value;
        node.Attributes.Append(attr);
    }
}
