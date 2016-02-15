using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_build_conf
{
    [XmlElement("build_conf")]
    public AllowPlanetAll build_conf;

    public static Root_build_conf Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_build_conf));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_build_conf;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_build_conf LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_build_conf));
        return serializer.Deserialize(new StringReader(text)) as Root_build_conf;

    }
}


public class AllowPlanetAll
{
    [XmlElement("allow_planet")]
    public AllowPlanet allow_planet;
}

public class AllowPlanet
{
    [XmlElement("bulding")]
    public List<bulding> canBuildList = new List<bulding>();

    [XmlElement("research")]
    public List<research> canResearchList = new List<research>();

    [XmlElement("fleet")]
    public List<fleet> canFleetList = new List<fleet>();

    [XmlElement("defense")]
    public List<defense> canDefenseList = new List<defense>();
}

public class bulding
{
    [XmlText]
    public int canBuildId;
}

public class research
{
    [XmlText]
    public int canResearchId;
}

public class fleet
{
    [XmlText]
    public int canFleetId;
}

public class defense
{
    [XmlText]
    public int canDefenseId;
}
