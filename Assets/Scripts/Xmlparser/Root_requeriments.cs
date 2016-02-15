using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_requeriments
{
    [XmlArray("requeriments")]
    [XmlArrayItem("requeriment")]
    public List<requeriments> requer = new List<requeriments>();

    public static Root_requeriments Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_requeriments));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_requeriments;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_requeriments LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_requeriments));
        return serializer.Deserialize(new StringReader(text)) as Root_requeriments;

    }
}

public class requeriments
{
    [XmlAttribute("id")]
    public int requerimentsId;

    [XmlElement("element")]
    public List<requerElements> requerElem = new List<requerElements>();
}

public class requerElements
{
    [XmlAttribute("id")]
    public int requerElementsId;

    [XmlAttribute("count")]
    public int requerElementsCount;
}
