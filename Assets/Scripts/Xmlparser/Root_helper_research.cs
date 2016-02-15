using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_helper_research 
{
    [XmlArray("help")]
    [XmlArrayItem("ResearchItem")]
    public List<helpResearch> helpResearch = new List<helpResearch>();

    public static Root_helper_research Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_research));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_helper_research;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_helper_research LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_research));
        return serializer.Deserialize(new StringReader(text)) as Root_helper_research;

    }
}

public class helpResearch
{
    [XmlAttribute("id")]
    public int helpResearchId;

    [XmlElement("title")]
    public string helpResearchTitle;

    [XmlElement("text")]
    public string helpResearchDesc;
}
