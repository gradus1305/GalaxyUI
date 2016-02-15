using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.EventSystems;

public class LoginController : MonoBehaviour
{
    public Toggle regulations;
    public Button LoginBtn, RegisterBtn;
    string login_str, pass_str;

    public GameObject Error,LoadingPanel;
    public InputField login_field, pass_field;

    public GameObject LeftMenuPanel, LoginPanel, TopBlock, BuildingsPanel;

    public static TextAsset xmlDoc;

    public Transform dropColony;

    bool isFirstLogin = true;

    void Awake()
    {
        Application.runInBackground = true;
    }

    void Start()
    {
        xmlDoc = (TextAsset)Resources.Load("config");
       
        if(!regulations.isOn)
        {
            LoginBtn.interactable = false;
        }
        else
        {
            LoginBtn.interactable = true;
        }

        login_field.keyboardType = TouchScreenKeyboardType.EmailAddress;
        pass_field.keyboardType = TouchScreenKeyboardType.Default;

        login_field.transform.FindChild("Text").GetComponent<Text>().color = green;
        login_field.GetComponent<Image>().sprite = defaultLogin_sprite;
        
        pass_field.transform.FindChild("Text").GetComponent<Text>().color = green;
        pass_field.GetComponent<Image>().sprite = defaultLogin_sprite;
        errorText_go.SetActive(false);

        if (PlayerPrefs.HasKey("userLogin") && PlayerPrefs.HasKey("userPass"))
        {
            login_str = PlayerPrefs.GetString("userLogin");
            pass_str = PlayerPrefs.GetString("userPass");

            isFirstLogin = true;

            StartCoroutine(GetJSON());
        }
    }  
    public void RegulationsChange()
    {
        if (regulations.isOn)
        {
            LoginBtn.interactable = true;
        }
        else
        {
            LoginBtn.interactable = false;
        }
    }

    //Login Panel START
    public void Login()
    {
        login_str = login_field.text;
        pass_str = pass_field.text;

        if(login_str != "" && pass_str != "")
        {
            StartCoroutine(GetJSON());
        }
        else
        {
            login_field.transform.FindChild("Text").GetComponent<Text>().color = red;
            login_field.GetComponent<Image>().sprite = errorLogin_sprite;
            pass_field.transform.FindChild("Text").GetComponent<Text>().color = red;
            pass_field.GetComponent<Image>().sprite = errorLogin_sprite;
            errorText_go.SetActive(true);
            errorText_go.GetComponent<Text>().text = "Ошибка:\nВведите логин и пароль или зарегестрируйтесь.";
        }
    }

    public GameObject registerPanel;
    public void Register()
    {
        registerPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

    private bool isInputActive = false;
    private InputField currentInpf;
    public void InputFieldSelectedLogin(InputField _inpf)
    {
        _inpf.transform.FindChild("Text").GetComponent<Text>().color = green;
        _inpf.GetComponent<Image>().sprite = defaultLogin_sprite;
        errorText_go.SetActive(false);

        _inpf.placeholder.gameObject.SetActive(false);

        // Go to LateUpdate
        isInputActive = true;
        currentInpf = _inpf;
    }

    public void InputFieldDeselectedLogin(InputField _inpf)
    {
        _inpf.placeholder.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if(isInputActive)
        {
            currentInpf.caretPosition = currentInpf.text.Length;
            isInputActive = false;
        }
    }

    //Login Panel END

    // Get JSON START
    JSONNode result;
    JSONNode current_user;
    JSONNode current_planet;
    JSONNode game_config;
    JSONNode system_list;
    JSONNode statistic;

    JSONNode buildingsInProgress;
    JSONNode researchInProgress;
    JSONNode docDefInProgress;
    public GameObject xmlParser;
    MainResourcesController mrc;
    public Sprite errorLogin_sprite;
    public Sprite defaultLogin_sprite;
    public GameObject errorText_go;
    Color32 red = new Color32(233,59,78,255);
    Color32 green = new Color32(112, 185, 32, 255);

   public IEnumerator GetJSON()
    {
        //Add login and pass string
        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=user&method=login&email="+login_str+"&password="+pass_str);
        yield return www;

        if(www.error == null)
        {
            var error = JSON.Parse(www.text);

            if (error["error"] == null)
            {
                LoginPanel.SetActive(false);
                if (isFirstLogin)
                {
                    PlayerPrefs.SetString("userLogin", login_str);
                    PlayerPrefs.SetString("userPass", pass_str);
                    PlayerPrefs.Save();
                }

                if (PlayerPrefs.HasKey("userLogin") && PlayerPrefs.HasKey("userPass"))
                    LoadingPanel.SetActive(true);

                MainResourcesController.userId = error["user_id"].AsInt;
                MainResourcesController.userToken = error["token"].Value;

                WWW www_getuser = new WWW("http://vg2.v-galaktike.ru/api/?class=user&method=getuser&token="+MainResourcesController.userToken);
                yield return www_getuser;

                result = JSON.Parse(www_getuser.text);

                current_user = result["user"];
                current_planet = result["system"];
                game_config = result["game_config"];
                system_list = result["system_list"];
                statistic = result["stats"];

                buildingsInProgress = current_planet["building"];
                researchInProgress = current_user["tech"];
                docDefInProgress = current_planet["weapon"];


                GetValForJson();
                mrc = TopBlock.GetComponent<MainResourcesController>();
                mrc.ValOnTheirPlace();
                mrc.repeatTick();
                MainResourcesController.timeNow = result["now"].AsLong;
                MainResourcesController.weapone_update = current_planet["weapon_update"].AsLong;

                xmlParser.SetActive(true);
            }
            else
            {
                // Bad Login Or Pass
                login_field.transform.FindChild("Text").GetComponent<Text>().color = red;
                login_field.GetComponent<Image>().sprite = errorLogin_sprite;
                pass_field.transform.FindChild("Text").GetComponent<Text>().color = red;
                pass_field.GetComponent<Image>().sprite = errorLogin_sprite;
                errorText_go.SetActive(true);
                errorText_go.GetComponent<Text>().text = "Ошибка:\nЕмайл или пароль введены не верно.";
            }  
        }
        else
        {
            Debug.Log(www.error);
            Error.SetActive(true);
        }
    }

    public void GetValForJson()
    {
        MainResourcesController.userId = current_user["user_id"].AsInt;
        MainResourcesController.userMessages = current_user["new_message"].AsInt;
        MainResourcesController.gameSpeed = game_config["game_speed"].AsInt;
        MainResourcesController.resource_multiplier = game_config["resource_multiplier"].AsDouble;
        MainResourcesController.production_level = current_planet["production_level"].AsDouble;
        MainResourcesController.currentSystemID = current_user["main_system_id"].AsInt;
        MainResourcesController.currentSystemName = current_planet["system_name"].Value;
        MainResourcesController.currentGalaxy = current_planet["galaxy"].AsInt;
        MainResourcesController.currentSystemX = current_planet["x"].AsInt;
        MainResourcesController.currentSystemY = current_planet["y"].AsInt;
        MainResourcesController.timeNow = result["now"].AsLong;
        MainResourcesController.planetsCountInGalaxy = game_config["planet_count"].AsDouble;
        MainResourcesController.max_galaxy = result["max_galaxy"].AsInt;
        MainResourcesController.max_x = result["max_x"].AsInt;
        MainResourcesController.max_y = result["max_y"].AsInt;
        MainResourcesController.main_galaxy = current_planet["galaxy"].AsInt;
        MainResourcesController.main_x = current_planet["x"].AsInt;
        MainResourcesController.main_y = current_planet["y"].AsInt;
        MainResourcesController.mainSystemID = current_user["main_system_id"].AsInt;

        MainResourcesController.main_titan_value = current_planet["titan"].AsDouble;
        MainResourcesController.main_silicone_value = current_planet["silicon"].AsDouble;
        MainResourcesController.main_antimatter_value = current_planet["antimatter"].AsDouble;
        MainResourcesController.energy_used = current_planet["energy_used"].AsInt;
        MainResourcesController.energy_max = current_planet["energy_max"].AsInt;
        MainResourcesController.main_credit_value = current_user["credit"].AsInt;
        MainResourcesController.titan_perhour = current_planet["titan_perhour"].AsDouble;
        MainResourcesController.silicon_perhour = current_planet["silicon_perhour"].AsDouble;
        MainResourcesController.antimatter_perhour = current_planet["antimatter_perhour"].AsDouble;
        MainResourcesController.titan_max = current_planet["titan_max"].AsInt;
        MainResourcesController.silicon_max = current_planet["silicon_max"].AsInt;
        MainResourcesController.antimatter_max = current_planet["antimatter_max"].AsInt;

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
        MainResourcesController.nano = current_planet["nano"].AsInt;

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
        //MainResourcesController.intergalactic_tech = current_user["intergalactic_tech"].AsInt;
        MainResourcesController.expedition_tech = current_user["expedition_tech"].AsInt;
        MainResourcesController.colonisation_tech = current_user["colonisation_tech"].AsInt;
        MainResourcesController.graviton_tech = current_user["graviton_tech"].AsInt;

        for (int i = 0; i < system_list.Count; i++)
        {
            MainResourcesController.systemList.Add(new SystemList(system_list[i]["system_id"].AsInt,
                system_list[i]["galaxy"].AsInt, system_list[i]["x"].AsInt, system_list[i]["y"].AsInt,
                system_list[i]["system_name"].Value));
        }

        if (buildingsInProgress.Count != 0)
        {
            
            MainResourcesController.buildingInProgress.Add(new BuildingsInProgress(
                buildingsInProgress["system_id"].AsInt,
                buildingsInProgress["building_id"].AsInt,
                buildingsInProgress["building_end"].AsLong,
                buildingsInProgress["building_start"].AsLong));
        }
        else
        {
            MainResourcesController.buildingInProgress.Clear();
        }

        if (researchInProgress.Count != 0)
        {
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
        //Statistic
        MainResourcesController.user_id = statistic["user_id"].AsInt;
        MainResourcesController.ally_id = statistic["ally_id"].AsInt;
        MainResourcesController.stat_type = statistic["stat_type"].AsInt;
        MainResourcesController.stat_code = statistic["stat_code"].AsInt;
        MainResourcesController.tech_rank = statistic["tech_rank"].AsInt;
        MainResourcesController.tech_old_rank = statistic["tech_old_rank"].AsInt;
        MainResourcesController.tech_points = statistic["tech_points"].AsInt;
        MainResourcesController.tech_count = statistic["tech_count"].AsInt;
        MainResourcesController.build_rank = statistic["build_rank"].AsInt;
        MainResourcesController.build_old_rank = statistic["build_old_rank"].AsInt;
        MainResourcesController.build_points = statistic["build_points"].AsInt;
        MainResourcesController.build_count = statistic["build_count"].AsInt;
        MainResourcesController.defs_rank = statistic["defs_rank"].AsInt;
        MainResourcesController.defs_old_rank = statistic["defs_old_rank"].AsInt;
        MainResourcesController.defs_points = statistic["defs_points"].AsInt;
        MainResourcesController.defs_count = statistic["defs_count"].AsInt;
        MainResourcesController.fleet_rank = statistic["fleet_rank"].AsInt;
        MainResourcesController.fleet_old_rank = statistic["fleet_old_rank"].AsInt;
        MainResourcesController.fleet_points = statistic["fleet_points"].AsInt;
        MainResourcesController.fleet_count = statistic["fleet_count"].AsInt;
        MainResourcesController.total_rank = statistic["total_rank"].AsInt;
        MainResourcesController.total_old_rank = statistic["total_old_rank"].AsInt;
        MainResourcesController.total_points = statistic["total_points"].AsInt;
        MainResourcesController.total_count = statistic["total_count"].AsInt;
        MainResourcesController.stat_date = statistic["stat_date"].AsInt;
    }
    // Get JSON END
    public void ExitBtn()
    {
        if (PlayerPrefs.HasKey("userLogin") && PlayerPrefs.HasKey("userPass"))
        {
            PlayerPrefs.DeleteKey("userLogin");
            PlayerPrefs.DeleteKey("userPass");
        }
            DestroyColonyPrefabs();
            MainResourcesController.systemList.Clear();
            MainResourcesController.researchInProgress.Clear();
            MainResourcesController.buildingInProgress.Clear();
            MainResourcesController.docDefInProgress.Clear();
            MainResourcesController.buildingsLevel.Clear();
            MainResourcesController.technologyLevel.Clear();
            
            Application.LoadLevel(0);
    }

    public void DestroyColonyPrefabs()
    {
        foreach (Transform child in dropColony)
        {
            Destroy(child.gameObject);
        }        
    }
}
