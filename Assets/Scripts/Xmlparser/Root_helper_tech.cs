using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_helper_tech
{

    [XmlArray("help")]
    [XmlArrayItem("TechTreeItem")]
    public List<TechTreeItem> TechTreeItem = new List<TechTreeItem>();

    public static Root_helper_tech Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_tech));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_helper_tech;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_helper_tech LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_tech));
        return serializer.Deserialize(new StringReader(text)) as Root_helper_tech;

    }
}

public class TechTreeItem
{
    [XmlAttribute("id")]
    public int TechTreeItemId;

    [XmlElement("title")]
    public string TechTreeItemTitle;

    [XmlElement("text")]
    public string TechTreeItemDesc;
}


