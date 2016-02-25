using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_helper_buildings
{
    [XmlArray("help")]
    [XmlArrayItem("BuildingItem")]
    public List<helpBuildings> helpBuildings = new List<helpBuildings>();

    public static Root_helper_buildings Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_buildings));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_helper_buildings;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_helper_buildings LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_buildings));
        return serializer.Deserialize(new StringReader(text)) as Root_helper_buildings;

    }
}

public class helpBuildings
{
    [XmlAttribute("id")]
    public int helpBuildingsId;

    [XmlElement("title")]
    public string helpBuildingsTitle;

    [XmlElement("text")]
    public string helpBuildingsDesc;
}

