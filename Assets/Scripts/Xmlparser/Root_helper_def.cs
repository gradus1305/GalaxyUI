using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_helper_def
{
    [XmlArray("help")]
    [XmlArrayItem("WeaponItem")]
    public List<helpDef> helpDef = new List<helpDef>();

    public static Root_helper_def Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_def));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_helper_def;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_helper_def LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_def));
        return serializer.Deserialize(new StringReader(text)) as Root_helper_def;

    }
}

public class helpDef
{
    [XmlAttribute("id")]
    public int helpDefId;

    [XmlElement("title")]
    public string helpDefTitle;

    [XmlElement("text")]
    public string helpDefDesc;
}