using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("root")]
public class Root_pricelist
{
    [XmlArray("pricelist")]
    [XmlArrayItem("element")]
    public List<elementPrice> elemP = new List<elementPrice>();

    public static Root_pricelist Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Root_pricelist));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Root_pricelist;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static Root_pricelist LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(Root_pricelist));
        return serializer.Deserialize(new StringReader(text)) as Root_pricelist;

    }
}

public class elementPrice
{
    [XmlAttribute("id")]
    public int elementPriceID;

    [XmlElement("param")]
    public paramsPrice paramP = new paramsPrice();
}

public class paramsPrice
{
    [XmlAttribute("metal")]
    public int paramMetal;

    [XmlAttribute("crystal")]
    public int paramCrystal;

    [XmlAttribute("deuterium")]
    public int paramDeuterium;

    [XmlAttribute("energy")]
    public int paramEnergy;

    [XmlAttribute("factor")]
    public float paramFactor;
}
