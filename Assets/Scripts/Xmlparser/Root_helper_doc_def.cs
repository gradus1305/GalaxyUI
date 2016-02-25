using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_helper_doc_def
{
    [XmlArray("help")]
    [XmlArrayItem("WeaponItem")]
    public List<helpDocs> helpDocs = new List<helpDocs>();

    public static Root_helper_doc_def Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_doc_def));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_helper_doc_def;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_helper_doc_def LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_doc_def));
        return serializer.Deserialize(new StringReader(text)) as Root_helper_doc_def;

    }
}

public class helpDocs
{
    [XmlAttribute("id")]
    public int helpDocsId;

    [XmlElement("title")]
    public string helpDocsTitle;

    [XmlElement("text")]
    public string helpDocsDesc;
}
