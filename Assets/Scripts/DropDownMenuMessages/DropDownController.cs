using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class DropDownController : MonoBehaviour
{
    [Header("Global")]
    public GameObject dropDownMenu;
    public GameObject loading;
    public Transform parentDropDown;
    public Text mainName;

    [Header("Prefab and childrens")]
    public GameObject prefabDropDown;
    public Text pName;
    public Text pId;
    public GameObject parentButtons;
    Button planetBtn;
    GameObject goInst;
    QueneController qC;
    public static TextAsset xmlDoc;

    void Start()
    {
        xmlDoc = (TextAsset)Resources.Load("config");
    }
    
    public void DwopDownBtn_open()
    {
        if (!dropDownMenu.activeInHierarchy)
        {
            foreach (var mm in MainResourcesController.systemList)
            {
                StartCoroutine(DropDownGetJsonData(mm.sys_id, mm.sys_name, mm.galaxy, mm.x, mm.y));
            }

            dropDownMenu.SetActive(true);
        }
        else
        {
            DwopDownBtn_close();
        }
    }
   
    IEnumerator DropDownGetJsonData(int _sys_id,string _sys_name,int _galaxy,int _x, int _y)
    {
        pName.text = _sys_name;
        pId.text = "["+ _galaxy +":" + _x + ":" + _y + "]";
        goInst = Instantiate(prefabDropDown);
        goInst.name = goInst.name.Replace("(Clone)", "_" + _sys_id);
        goInst.transform.SetParent(parentDropDown);
        goInst.transform.localScale = new Vector3(1, 1, 1);

        planetBtn = parentButtons.transform.FindChild("DropDownItem_" + _sys_id).GetComponent<Button>();
        planetBtn.onClick.AddListener(() => { DropDownItem_btn(_sys_id, _sys_name,_galaxy,_x,_y); });
        yield return null;
    }

    public void DwopDownBtn_close()
    {
        foreach (Transform child in parentDropDown)
        {
            GameObject.Destroy(child.gameObject);
        }
            
        dropDownMenu.SetActive(false);
    }

    public void DropDownItem_btn(int _id, string _name, int _galaxy, int _x, int _y)
    {
        MainResourcesController.currentSystemID = _id;
        MainResourcesController.currentSystemName = _name;
        MainResourcesController.currentGalaxy = _galaxy;
        MainResourcesController.currentSystemX = _x;
        MainResourcesController.currentSystemY = _y;
        mainName.text = _name + " ["+_galaxy+":" + _x + ":" + _y + "]";
   
        if (GameObject.Find("ProgressElement(Clone)"))
        {
            qC = GameObject.Find("ProgressElement(Clone)").GetComponent<QueneController>();         
                qC.CancelInvoke("StartTickBuildings");
                qC.CancelInvoke("StartTickResearch");
                qC.CancelInvoke("StartTickDoc");
                Destroy(GameObject.Find("ProgressElement(Clone)").gameObject);
        }
       
        StartCoroutine(DropDownItem());
    }

    JSONNode current_user;
    JSONNode current_planet;
    JSONNode buildingsInProgress;
    JSONNode researchInProgress;
    JSONNode docDefInProgress;
    public GameObject TopBlock;
    public GameObject partentBuildings;
    public GameObject partentResearch;
    public GameObject planetBlock;
    public GameObject buildingBlocks;
    public GameObject researchBlocks;
    public GameObject docBlocks;
    public GameObject defBlocks;
    public GameObject techBlocks;
    public ScreenManager scrM;

    IEnumerator DropDownItem()
    {
        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=planet&method=getplanet&token="
            +MainResourcesController.userToken
            +"&system_id="
            +MainResourcesController.currentSystemID);

        yield return www;

        if (www.error == null)
        {
            var result = JSON.Parse(www.text);

            MainResourcesController.timeNow = result["now"].AsLong;
           
            current_user = result["user"];
            current_planet = result["system"];
            buildingsInProgress = current_planet["building"];
            researchInProgress = current_user["tech"];
            docDefInProgress = current_planet["weapon"];

            MainResourcesController.main_titan_value = current_planet["titan"].AsDouble;
            MainResourcesController.main_silicone_value = current_planet["silicon"].AsDouble;
            MainResourcesController.main_antimatter_value = current_planet["antimatter"].AsDouble;
            MainResourcesController.energy_used = current_planet["energy_used"].AsInt;
            MainResourcesController.energy_max = current_planet["energy_max"].AsInt;
            MainResourcesController.weapone_update = current_planet["weapon_update"].AsLong;
            MainResourcesController mrc = TopBlock.GetComponent<MainResourcesController>();
            mrc.ValOnTheirPlace();

            if (buildingsInProgress.Count != 0)
            {
                MainResourcesController.buildingInProgress.Clear();
                MainResourcesController.buildingInProgress.Add(new BuildingsInProgress(
                    buildingsInProgress["system_id"].AsInt,
                    buildingsInProgress["building_id"].AsInt,
                    buildingsInProgress["building_end"].AsLong,
                    buildingsInProgress["building_start"].AsLong));
            }
            else
            {
                MainResourcesController.buildingListIsFull = false;
                MainResourcesController.buildingInProgress.Clear();
            }

            if (researchInProgress.Count != 0)
            {
                MainResourcesController.researchInProgress.Clear();

                MainResourcesController.researchInProgress.Add(new ResearchInProgress(
                    researchInProgress["user_id"].AsInt,
                    researchInProgress["tech_id"].AsInt,
                    researchInProgress["tech_end"].AsLong,
                    researchInProgress["tech_start"].AsLong));
            }
            else
            {
                MainResourcesController.researchInProgress.Clear();
            }

            if (docDefInProgress.Count != 0)
            {
              MainResourcesController.docDefInProgress.Clear();

              for (int i = 0; i < docDefInProgress.Count; i++)
              {
                  MainResourcesController.docDefInProgress.Add(new DocDefInProgress(
                     docDefInProgress[i]["system_id"].AsInt,
                     docDefInProgress[i]["element_id"].AsInt,
                     docDefInProgress[i]["count"].AsInt));
              }
            }
            else
            {
                MainResourcesController.docDefInProgress.Clear();
            }
               
            GetValForJson();

            MainResourcesController.CreadeDictionaryBuildingsLevel();
            MainResourcesController.CreadeDictionaryResearchLevel();

            loading.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            RefreshController.R_Buildings(buildingBlocks);
            RefreshController.R_Research(researchBlocks);
            yield return new WaitForSeconds(0.5f);
            RefreshController.R_Doc(docBlocks);
            RefreshController.R_Defc(defBlocks);
            yield return new WaitForSeconds(0.5f);
            RefreshController.R_Technology(techBlocks);
            loading.SetActive(false);

            DwopDownBtn_close();

            if (planetBlock.activeInHierarchy)
            {
                planetBlock.GetComponent<PlanetMenuItemController>().ChangePlanet();
            }
            
            if(scrM.currentPanel.name == "Building")
            {
                buildingBlocks.transform.GetComponent<BuildingOnEnabled>().StartThis();
            }
            else if (scrM.currentPanel.name == "Research")
            {
                researchBlocks.transform.GetComponent<ResearchOnEnabled>().StartThis();
            }
            
            else if (scrM.currentPanel.name == "DocDef")
            {
                docBlocks.transform.GetComponent<DocOnEnabled>().StartThis();
            }
            
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    public void GetValForJson()
    {
        MainResourcesController.energy_used = current_planet["energy_used"].AsInt;
        MainResourcesController.energy_max = current_planet["energy_max"].AsInt;

        MainResourcesController.titan_mine = current_planet["titan_mine"].AsInt;
        MainResourcesController.silicon_mine = current_planet["silicon_mine"].AsInt;
        MainResourcesController.antimatter_sintetizer = current_planet["antimatter_sintetizer"].AsInt;
        MainResourcesController.solar_plant = current_planet["solar_plant"].AsInt;
        MainResourcesController.fusion_plant = current_planet["fusion_plant"].AsInt;
        MainResourcesController.robot_factory = current_planet["robot_factory"].AsInt;
        MainResourcesController.nano_factory = current_planet["nano_factory"].AsInt;
        MainResourcesController.hangar = current_planet["hangar"].AsInt;
        MainResourcesController.titan_store = current_planet["titan_store"].AsInt;
        MainResourcesController.silicon_store = current_planet["silicon_store"].AsInt;
        MainResourcesController.antimatter_store = current_planet["antimatter_store"].AsInt;
        MainResourcesController.laboratory = current_planet["laboratory"].AsInt;
        MainResourcesController.terraformer = current_planet["terraformer"].AsInt;

        MainResourcesController.spy_tech = current_user["spy_tech"].AsInt;
        MainResourcesController.computer_tech = current_user["computer_tech"].AsInt;
        MainResourcesController.military_tech = current_user["military_tech"].AsInt;
        MainResourcesController.defence_tech = current_user["defence_tech"].AsInt;
        MainResourcesController.shield_tech = current_user["shield_tech"].AsInt;
        MainResourcesController.energy_tech = current_user["energy_tech"].AsInt;
        MainResourcesController.hyperspace_tech = current_user["hyperspace_tech"].AsInt;
        MainResourcesController.combustion_tech = current_user["combustion_tech"].AsInt;
        MainResourcesController.impulse_motor_tech = current_user["impulse_motor_tech"].AsInt;
        MainResourcesController.hyperspace_motor_tech = current_user["hyperspace_motor_tech"].AsInt;
        MainResourcesController.laser_tech = current_user["laser_tech"].AsInt;
        MainResourcesController.ionic_tech = current_user["ionic_tech"].AsInt;
        MainResourcesController.buster_tech = current_user["buster_tech"].AsInt;
       // MainResourcesController.intergalactic_tech = current_user["intergalactic_tech"].AsInt;
        MainResourcesController.expedition_tech = current_user["expedition_tech"].AsInt;
        MainResourcesController.colonisation_tech = current_user["colonisation_tech"].AsInt;
        MainResourcesController.graviton_tech = current_user["graviton_tech"].AsInt;
    }
}
