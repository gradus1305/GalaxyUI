using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_resources 
{

    [XmlArray("resources")]
    [XmlArrayItem("resource")]
    public List<resourceElement> resElem = new List<resourceElement>();

    public static Root_resources Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_resources));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_resources;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_resources LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_resources));
        return serializer.Deserialize(new StringReader(text)) as Root_resources;

    }
}

public class resourceElement
{
    [XmlAttribute("id")]
    public int resourceElementID;

    [XmlElement("technical_name")]
    public string resourceElement_technical_name;

    [XmlElement("good_name")]
    public string resourceElement_good_name;
}
