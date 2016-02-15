using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("root")]
public class Root_helper_headers 
{
    [XmlElement("help")]
    public Planet_Overview helpHeaders;

    public static Root_helper_headers Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_headers));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_helper_headers;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_helper_headers LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_helper_headers));
        return serializer.Deserialize(new StringReader(text)) as Root_helper_headers;

    }
}

public class Planet_Overview
{
    [XmlElement("Planet_Overview")]
    public Helper hhh;

    [XmlElement("FleetSend")]
    public Helper flletSend;

    [XmlElement("BuildingPage")]
    public Helper buildPage;

    [XmlElement("Stats_Page")]
    public Helper statsPage;

    [XmlElement("Resourse_Page")]
    public Helper resoucePage;

    [XmlElement("Transport_Score")]
    public Helper transportScore;
}

public class Helper
{
    [XmlElement("title")]
    public string helpHeadersTitle;

    [XmlElement("text")]
    public string helpHeadersDesc;
}

