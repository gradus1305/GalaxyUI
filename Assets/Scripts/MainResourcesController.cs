using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainResourcesController : MonoBehaviour
{
    //XML
    public static TextAsset xmlDoc;

    //Global Values
    public static int userId;
    public static string userPass;
    public static string userToken;
    public static int userMessages;
    public static int gameSpeed;
    public static double resource_multiplier;
    public static double production_level;
    public static List<SystemList> systemList = new List<SystemList>();
    public static List<ResourcesDictionary> resDictionary = new List<ResourcesDictionary>();
    public static int currentSystemID;
    public static string currentSystemName;
    public static int currentGalaxy;
    public static int currentSystemX;
    public static int currentSystemY;
    public static long timeNow;
    public static double planetsCountInGalaxy;
    public static int max_galaxy;
    public static int max_x;
    public static int max_y;
    public static int main_galaxy;
    public static int main_x;
    public static int main_y;
    public static int mainSystemID;

    //Time of start building on doc
    public static long weapone_update;
    
    //Planet Values
    public static double main_titan_value;
    public static double main_silicone_value;
    public static double main_antimatter_value;
    public static int energy_used;
    public static int energy_max;
    public static int main_credit_value;
    public static double titan_perhour;
    public static double silicon_perhour;
    public static double antimatter_perhour;
    public static int titan_max;
    public static int silicon_max;
    public static int antimatter_max;

    //Buildings Levels
    public static Dictionary<string, int> buildingsLevel = new Dictionary<string, int>();
    public static int titan_mine;
    public static int silicon_mine;
    public static int antimatter_sintetizer;
    public static int solar_plant;
    public static int fusion_plant;
    public static int robot_factory;
    public static int nano_factory;
    public static int hangar;
    public static int titan_store;
    public static int silicon_store;
    public static int antimatter_store;
    public static int laboratory;
    public static int terraformer;
    public static int nano;
    public static int sprungtor;
    public static int capitol;// can be 1

    //Technology Levels
    public static Dictionary<string, int> technologyLevel = new Dictionary<string, int>();
    public static int spy_tech;
    public static int computer_tech;
    public static int military_tech;
    public static int defence_tech;
    public static int shield_tech;
    public static int energy_tech;
    public static int hyperspace_tech;
    public static int combustion_tech;
    public static int impulse_motor_tech;
    public static int hyperspace_motor_tech;
    public static int laser_tech;
    public static int ionic_tech;
    public static int buster_tech;
   // public static int intergalactic_tech;
    public static int expedition_tech;
    public static int colonisation_tech;
    public static int graviton_tech;

    //Vars for statistic
    public static int user_id;
    public static int ally_id;
    public static int stat_type;
    public static int stat_code;
    public static int tech_rank;
    public static int tech_old_rank;
    public static int tech_points;
    public static int tech_count;
    public static int build_rank;
    public static int build_old_rank;
    public static int build_points;
    public static int build_count;
    public static int defs_rank;
    public static int defs_old_rank;
    public static int defs_points;
    public static int defs_count;
    public static int fleet_rank;
    public static int fleet_old_rank;
    public static int fleet_points;
    public static int fleet_count;
    public static int total_rank;
    public static int total_old_rank;
    public static int total_points;
    public static int total_count;
    public static int stat_date;

        //Text values in TopBlock
    [Header("Texts in TopBlock")]
    public Text titan_text;
    public Text silicone_text;
    public Text antimatter_text;
    public Text energy_text;
    public Text credit_text;
    public Text userMessages_text;
    public Text currentPlanetName;
    public GameObject messagesRound;
    

    //SomeParrentGo
    [Header("ParentGoFoFind")]
    public GameObject parentGOBuildings;

    //Research and Building Lists
    public static List<BuildingsInProgress> buildingInProgress = new List<BuildingsInProgress>();
    public static bool buildingListIsFull;

    public static List<ResearchInProgress> researchInProgress = new List<ResearchInProgress>();
    public static bool researchListIsFull;
    // DocDef list
    public static List<DocDefInProgress> docDefInProgress = new List<DocDefInProgress>();

    void Start()
    {
        xmlDoc = (TextAsset)Resources.Load("config");
    }

    //Ticking Of Resources (for 1 or 10 seconds)
    public void UpdateResources()
    {
        main_titan_value += (titan_perhour / 3600) * resource_multiplier; //* (production_level / 100);
        titan_text.text = CheckToZeroResources(main_titan_value);       

        main_silicone_value += (silicon_perhour / 3600) * resource_multiplier;// *production_level / 100;
        silicone_text.text = CheckToZeroResources(main_silicone_value);

        main_antimatter_value += (antimatter_perhour / 3600) * resource_multiplier;// *production_level / 100;
        antimatter_text.text = CheckToZeroResources(main_antimatter_value);
    }

    public void repeatTick()
    {
        InvokeRepeating("UpdateResources", 1f, 1f);
    }

    string CheckToZeroResources(double count)
    {
        string countStringBig;
        if (count >= 10000 && count < 1000000)
        {
            countStringBig = (count / 1000).ToString("F1") + "K";
        }
        else if (count >= 1000000)
        {
            countStringBig = (count / 1000000).ToString("F1") + "KK";
        }
        else
        {
            countStringBig = count.ToString("F2");
        }
        return countStringBig;
    }

    // elements on their place    
    public void ValOnTheirPlace()
    {
        titan_text.text = (main_titan_value < titan_max) ? 
            CheckToZeroResources(main_titan_value) : CheckToZeroResources(titan_max);

        silicone_text.text = (main_silicone_value < silicon_max) ?
            CheckToZeroResources(main_silicone_value) : CheckToZeroResources(silicon_max);

        antimatter_text.text = (main_antimatter_value < antimatter_max) ?
            CheckToZeroResources(main_antimatter_value) : CheckToZeroResources(antimatter_max);
      
        energy_text.text = CheckToZeroResources(energy_max + energy_used);
        credit_text.text = CheckToZeroResources(main_credit_value);
        if (userMessages == 0)
        {
            messagesRound.SetActive(false);
        }
        else
        {
            messagesRound.SetActive(true);
        }
        userMessages_text.text = userMessages.ToString();
       
        currentPlanetName.text = currentSystemName + " ["+currentGalaxy+":" + currentSystemX + ":" + currentSystemY+"]";
    }

    public static void CreadeDictionaryBuildingsLevel()
    {
        if (buildingsLevel.Count == 0)
        {
            buildingsLevel.Add("titan_mine", titan_mine);
            buildingsLevel.Add("silicon_mine", silicon_mine);
            buildingsLevel.Add("antimatter_sintetizer", antimatter_sintetizer);
            buildingsLevel.Add("solar_plant", solar_plant);
            buildingsLevel.Add("fusion_plant", fusion_plant);
            buildingsLevel.Add("robot_factory", robot_factory);
            buildingsLevel.Add("nano_factory", nano_factory);
            buildingsLevel.Add("hangar", hangar);
            buildingsLevel.Add("titan_store", titan_store);
            buildingsLevel.Add("silicon_store", silicon_store);
            buildingsLevel.Add("antimatter_store", antimatter_store);
            buildingsLevel.Add("laboratory", laboratory);
            buildingsLevel.Add("terraformer", terraformer);
            buildingsLevel.Add("nano", nano);
            buildingsLevel.Add("sprungtor", sprungtor);
            buildingsLevel.Add("capitol", capitol);
        }
        else
        {
            buildingsLevel["titan_mine"] = titan_mine;
            buildingsLevel["silicon_mine"] = silicon_mine;
            buildingsLevel["antimatter_sintetizer"] = antimatter_sintetizer;
            buildingsLevel["solar_plant"] = solar_plant;
            buildingsLevel["fusion_plant"] = fusion_plant;
            buildingsLevel["robot_factory"] = robot_factory;
            buildingsLevel["nano_factory"] = nano_factory;
            buildingsLevel["hangar"] = hangar;
            buildingsLevel["titan_store"] = titan_store;
            buildingsLevel["silicon_store"] = silicon_store;
            buildingsLevel["antimatter_store"] = antimatter_store;
            buildingsLevel["laboratory"] = laboratory;
            buildingsLevel["terraformer"] = terraformer;
            buildingsLevel["nano"] = nano;
            buildingsLevel["sprungtor"] = sprungtor;
            buildingsLevel["capitol"] = capitol;
        }
    }

    public static void CreadeDictionaryResearchLevel()
    {
        if (technologyLevel.Count == 0)
        {
            technologyLevel.Add("spy_tech", spy_tech);
            technologyLevel.Add("computer_tech", computer_tech);
            technologyLevel.Add("military_tech", military_tech);
            technologyLevel.Add("defence_tech", defence_tech);
            technologyLevel.Add("shield_tech", shield_tech);
            technologyLevel.Add("energy_tech", energy_tech);
            technologyLevel.Add("hyperspace_tech", hyperspace_tech);
            technologyLevel.Add("combustion_tech", combustion_tech);
            technologyLevel.Add("impulse_motor_tech", impulse_motor_tech);
            technologyLevel.Add("hyperspace_motor_tech", hyperspace_motor_tech);
            technologyLevel.Add("laser_tech", laser_tech);
            technologyLevel.Add("ionic_tech", ionic_tech);
            technologyLevel.Add("buster_tech", buster_tech);
            //technologyLevel.Add("intergalactic_tech", intergalactic_tech);
            technologyLevel.Add("expedition_tech", expedition_tech);
            technologyLevel.Add("colonisation_tech", colonisation_tech);
            technologyLevel.Add("graviton_tech", graviton_tech);
        }
        else
        {
            technologyLevel["spy_tech"] = spy_tech;
            technologyLevel["computer_tech"] = computer_tech;
            technologyLevel["military_tech"] = military_tech;
            technologyLevel["defence_tech"] = defence_tech;
            technologyLevel["shield_tech"] = shield_tech;
            technologyLevel["energy_tech"] = energy_tech;
            technologyLevel["hyperspace_tech"] = hyperspace_tech;
            technologyLevel["combustion_tech"] = combustion_tech;
            technologyLevel["impulse_motor_tech"] = impulse_motor_tech;
            technologyLevel["hyperspace_motor_tech"] = hyperspace_motor_tech;
            technologyLevel["laser_tech"] =laser_tech;
            technologyLevel["ionic_tech"] =ionic_tech;
            technologyLevel["buster_tech"] =buster_tech;
          //  technologyLevel["intergalactic_tech"] =intergalactic_tech;
            technologyLevel["expedition_tech"] =expedition_tech;
            technologyLevel["colonisation_tech"] =colonisation_tech;
            technologyLevel["graviton_tech"] =graviton_tech;
        }
    }

    Root_resources rRes;
    public void CreateDictionaryResources(string xmlDoc)
    {
        rRes = Root_resources.LoadFromText(xmlDoc);
        foreach (var mm in rRes.resElem)
        {
            resDictionary.Add(new ResourcesDictionary(
                mm.resourceElementID,
                mm.resourceElement_technical_name,
                mm.resourceElement_good_name));
        }
    }

    public static void UpdateBuilding(int _id, GameObject _parrentBuild)
    {
        foreach (var mm in _parrentBuild.GetComponentsInChildren<BlockController>())
        {
            if (_id == mm.blockId)
            {
                mm.SetBlockLevel(_id,ParseAll.WhatBlockParse.Builings);
                mm.SetTitle();
                mm.SetPrices();
                mm.SetTimeToBuild();
            }
        }
    }

    public static void UpdateResearch(int _id, GameObject _parrentBuild)
    {
        foreach (var mm in _parrentBuild.GetComponentsInChildren<BlockController>())
        {
            if (_id == mm.blockId)
            {
                mm.SetBlockLevel(_id,ParseAll.WhatBlockParse.Research);
                mm.SetTitle();
                mm.SetPrices();
                mm.SetTimeToBuild();
            }
        }
    }
}

public class SystemList
{
    public int sys_id;
    public int galaxy;
    public int x;
    public int y;
    public string sys_name;

    public SystemList(int _sys_id, int _galaxy, int _x, int _y, string _sys_name)
    {
        sys_id = _sys_id;
        galaxy = _galaxy;
        x = _x;
        y = _y;
        sys_name = _sys_name;
    }
}

public class ResourcesDictionary
{
    public int resId;
    public string resTechName;
    public string resGoodName;

    public ResourcesDictionary(int _resId, string _resTechName, string _resGoodName)
    {
        resId = _resId;
        resTechName = _resTechName;
        resGoodName = _resGoodName;
    }
}

public class BuildingsInProgress
{
    public int sysId;
    public int buildId;
    public long timeToEnd;
    public long startTimeBuild;

    public BuildingsInProgress(int _sysId, int _buildId, long _timeToEnd, long _startTimeBuild)
    {
        sysId = _sysId;
        buildId = _buildId;
        timeToEnd = _timeToEnd;
        startTimeBuild = _startTimeBuild;
    }
}

public class ResearchInProgress
{
    public int userid;
    public int techid;
    public long timeToEnd;
    public long startTime;
    public ResearchInProgress(int _userId, int _techId, long _timeToEnd, long _startTime)
    {
        userid = _userId;
        techid = _techId;
        timeToEnd = _timeToEnd;
        startTime = _startTime;
    }
}

public class DocDefInProgress
{
    public int systemId;
    public int elementId;
    public int count_elements;

    public DocDefInProgress( int _systemId, int _elementId, int _count_elements)
    {     
        systemId = _systemId;
        elementId = _elementId;
        count_elements = _count_elements;

    }

}


