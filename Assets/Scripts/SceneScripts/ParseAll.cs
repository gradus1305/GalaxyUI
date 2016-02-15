using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SimpleJSON;
using System;

public class ParseAll : MonoBehaviour
{

    //Link to xml file on server
    string linkToXml = "https://api.v-galaktike.ru/config/config.xml";
    string pathToXml;
    TextAsset xmlDoc;

    //Variables
    Root_build_conf canBuild;
    Root_helper_tech helpTextList;
    Root_requeriments requer;

    //Global
    public GameObject TopBlock;
    public GameObject loading;
    public GameObject popUpBuildResearch;
    public GameObject popUpTech;
    public GameObject planetBlock;

    //Buildings
    [Header("Buildings")]
    public GameObject prefabBuildings;
    public Image avatarBuildings;
    public Transform parentGOBuildings;
    public Transform infoBuildings;
    public Transform downbarBuildings;
    public Button upGradeBtnBuildings;

    //Research
    [Header("Research")]
    public GameObject prefabResearch;
    public Image avatarResearch;
    public Transform parentGOResearch;
    public Transform infoResearch;
    public Transform downbarResearch;
    public Button upGradeBtnResearch;

    //Doc(Weapon)
    [Header("Doc(Weapon)")]
    public GameObject prefabDoc;
    public Image avatarDoc;
    public Transform parentGODoc;
    public Transform infoDoc;
    public Transform downbarDoc;
    public Button upGradeBtnDoc; 
    private int countFromSlider;
    public Button inputFieldBtn;
    

    //Defence
    [Header("Def(Defence)")]
    public GameObject prefabDef;
    public Image avatarDef;
    public Transform parentGODef;
    public Transform infoDef;
    public Transform downbarDef;
    public Button upGradeBtnDef;

    //Technology
    [Header("Technology")]
    public GameObject prefabTech;
    public Image avatarTech;
    public Transform parentGOTech;
    public Transform infoTech;
    public Transform downbarTech;
    public Button upGradeBtnTech;
    bool CheckDoc;
    public GameObject contentDoc;

    [Header("LeftMenu Buttons")]
    public Button[] allLeftBtn;
    
    public enum WhatBlockParse
    {
        Builings,
        Research,
        Weapon,
        Defence,
        Technology
    };

    GameObject goInst;
    IEnumerator Start()
    {
        loading.SetActive(true);

        foreach(var mm in allLeftBtn)
        {
            mm.interactable = true;
        }

        pathToXml = Application.dataPath + "/config.xml";
        xmlDoc = (TextAsset)Resources.Load("config");
        MainResourcesController mrc = TopBlock.GetComponent<MainResourcesController>();
        MainResourcesController.CreadeDictionaryBuildingsLevel();
        MainResourcesController.CreadeDictionaryResearchLevel();
        mrc.CreateDictionaryResources(xmlDoc.text);
        canBuild = Root_build_conf.LoadFromText(xmlDoc.text);
        helpTextList = Root_helper_tech.LoadFromText(xmlDoc.text);       
        XmlClass buidings = new XmlClass();

        foreach (var mm in canBuild.build_conf.allow_planet.canBuildList)
        {
            StartCoroutine(BuildingsInst(goInst, buidings, mm.canBuildId));
            StartCoroutine(TechInst(goInst, buidings, mm.canBuildId));
            yield return null;
        }

        foreach (var mm in canBuild.build_conf.allow_planet.canResearchList)
        {
            StartCoroutine(ResearchInst(goInst, buidings, mm.canResearchId));
            StartCoroutine(TechInst(goInst, buidings, mm.canResearchId));
            yield return null;
        }

        foreach (var mm in canBuild.build_conf.allow_planet.canFleetList)
        {
            StartCoroutine(DocInst(goInst, buidings, mm.canFleetId));
            StartCoroutine(TechInst(goInst, buidings, mm.canFleetId));
            yield return null;
        }

        foreach (var mm in canBuild.build_conf.allow_planet.canDefenseList)
        {
            StartCoroutine(DefInst(goInst, buidings, mm.canDefenseId));
            StartCoroutine(TechInst(goInst, buidings, mm.canDefenseId));
            yield return null;
        }

        ScreenManager scr = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();       
        scr.currentPanel = planetBlock;

        planetBlock.SetActive(true);
        loading.SetActive(false);
    }

    IEnumerator BuildingsInst(GameObject goInst, XmlClass buidings, int _id)
    {
        goInst = buidings.analyseGO(_id, (int)WhatBlockParse.Builings, prefabBuildings, avatarBuildings, infoBuildings, downbarBuildings, xmlDoc.text);
        goInst = Instantiate(goInst);
        goInst.transform.SetParent(parentGOBuildings);
        goInst.transform.localScale = new Vector3(1, 1, 1);

        BlockController blc = goInst.GetComponent<BlockController>();
        blc.SetBlockName();
        blc.SetAvatar();
        blc.SetBlockLevel(_id, WhatBlockParse.Builings);
        blc.SetTitle();
        blc.SetDesc();
        blc.SetPrices();
        blc.SetTimeToBuild();       
        blc.SetRequairments(xmlDoc.text, WhatBlockParse.Builings);

        if (blc.blockId == 45)
            blc.SetCapitolium();

            upGradeBtnBuildings = goInst.transform.FindChild("DownBar").FindChild("UpgradeButton").GetComponent<Button>();
        upGradeBtnBuildings.onClick.AddListener(() => { UpGradeBuildResearchTech((int)WhatBlockParse.Builings); });
        yield return null;
    }

    IEnumerator ResearchInst(GameObject goInst, XmlClass buidings, int _id)
    {
        goInst = buidings.analyseGO(_id, (int)WhatBlockParse.Research, prefabResearch, avatarResearch, infoResearch, downbarResearch, xmlDoc.text);
        goInst = Instantiate(goInst);
        goInst.transform.SetParent(parentGOResearch);
        goInst.transform.localScale = new Vector3(1, 1, 1);

        BlockController blc = goInst.GetComponent<BlockController>();
        blc.SetBlockName();
        blc.SetAvatar();
        blc.SetBlockLevel(_id, WhatBlockParse.Research);
        blc.SetTitle();
        blc.SetDesc();
        blc.SetPrices();
        blc.SetTimeToBuild();
        blc.SetRequairments(xmlDoc.text, WhatBlockParse.Research);

        upGradeBtnResearch = goInst.transform.FindChild("DownBar").FindChild("UpgradeButton").GetComponent<Button>();
        upGradeBtnResearch.onClick.AddListener(() => { UpGradeBuildResearchTech((int)WhatBlockParse.Research); });
        yield return null;
    }

    public static List<BlockController> allDoc = new List<BlockController>();
    IEnumerator DocInst(GameObject goInst, XmlClass buidings, int _id)
    {
        goInst = buidings.analyseGO(_id, (int)WhatBlockParse.Weapon, prefabDoc, avatarDoc, infoDoc, downbarDoc, xmlDoc.text);
        goInst = Instantiate(goInst);
        goInst.transform.SetParent(parentGODoc);
        goInst.transform.localScale = new Vector3(1, 1, 1);

        BlockController blc = goInst.GetComponent<BlockController>();
        allDoc.Add(blc);
        blc.SetBlockName();
        blc.SetAvatar();
        blc.SetBlockLevel(_id, WhatBlockParse.Weapon);
        blc.SetTitleForDoc();
        blc.SetDesc();
        blc.SetPrices();
        blc.SetTimeToBuild();
        blc.SetRequairments(xmlDoc.text, WhatBlockParse.Weapon);

        upGradeBtnDoc = goInst.transform.FindChild("DownBar").FindChild("UpgradeButton").GetComponent<Button>();
        upGradeBtnDoc.onClick.AddListener(() => { UpGradeBuildResearchTech((int)WhatBlockParse.Weapon); });

        inputFieldBtn = goInst.transform.FindChild("DownBar").FindChild("InputField").GetComponent<Button>();
        inputFieldBtn.onClick.AddListener(() => { OpenSlider(); });

        yield return null;
    }

    public static List<BlockController> allDef = new List<BlockController>();
    IEnumerator DefInst(GameObject goInst, XmlClass buidings, int _id)
    {
        goInst = buidings.analyseGO(_id, (int)WhatBlockParse.Defence, prefabDef, avatarDef, infoDef, downbarDef, xmlDoc.text);
        goInst = Instantiate(goInst);
        goInst.transform.SetParent(parentGODef);
        goInst.transform.localScale = new Vector3(1, 1, 1);

        BlockController blc = goInst.GetComponent<BlockController>();
        allDef.Add(blc);
        blc.SetBlockName();
        blc.SetAvatar();
        blc.SetBlockLevel(_id, WhatBlockParse.Defence);
        blc.SetTitleForDoc();
        blc.SetDesc();
        blc.SetPrices();
        blc.SetTimeToBuild();
        blc.SetRequairments(xmlDoc.text, WhatBlockParse.Defence);

        upGradeBtnDoc = goInst.transform.FindChild("DownBar").FindChild("UpgradeButton").GetComponent<Button>();
        upGradeBtnDoc.onClick.AddListener(() => { UpGradeBuildResearchTech((int)WhatBlockParse.Defence); });

        inputFieldBtn = goInst.transform.FindChild("DownBar").FindChild("InputField").GetComponent<Button>();
        inputFieldBtn.onClick.AddListener(() => { OpenSlider(); });

        yield return null;
    }

    IEnumerator TechInst(GameObject goInst, XmlClass buidings, int _id)
    {
        goInst = buidings.analyseGO(_id, (int)WhatBlockParse.Technology, prefabTech, avatarTech, infoTech, downbarTech, xmlDoc.text);
        goInst = Instantiate(goInst);
        goInst.transform.SetParent(parentGOTech);
        goInst.transform.localScale = new Vector3(1, 1, 1);

        BlockController blc = goInst.GetComponent<BlockController>();
        blc.SetBlockName();
        blc.SetAvatar();
        blc.SetBlockLevel(_id, WhatBlockParse.Technology);
        blc.SetTitle();
        blc.SetDesc();
        blc.ClearRequairments();
        blc.SetRequairments(xmlDoc.text, WhatBlockParse.Technology);

        upGradeBtnTech = goInst.transform.FindChild("DownBar").FindChild("UpgradeButton").GetComponent<Button>();
        upGradeBtnTech.onClick.AddListener(() => { UpGradeBuildResearchTech((int)WhatBlockParse.Technology); });
        yield return null;
    }

    //Buildings/Research Upgrade Values
    [Header("Buildings Research PopUp")]
    public Image avatarPopUp;
    public Text popUpName;
    public Text popUpDesc;
    public Text timeToBuildPopUp;
    public Text titan_textPopUp, silicone_textPopUp, antimatter_textPopUp;
    public Image titan_imgPopUp, silicone_imgPopUp, antimatter_imgPopUp;
    public Button upGradeBRPopUp_confirm;

    //Tech Upgrade Values
    [Header("Tech PopUp")]
    public Text popUpName_tech;
    public Text popUpDesc_tech;

    public void BuildInfo()
    {

    }

    public void UpGradeBuildResearchTech(int _whatBlock)
    {
        GameObject thisBlock = EventSystem.current.currentSelectedGameObject.transform.parent.parent.gameObject;
        BlockController blc = thisBlock.GetComponent<BlockController>();
       
        if (_whatBlock == (int)WhatBlockParse.Technology)
        {
            popUpName_tech.text = blc.rName.text;
            popUpDesc_tech.text = blc.description;
            popUpTech.SetActive(true);
        }
        else
        {
            avatarPopUp.sprite = blc.ava;
            popUpName.text = blc.rName.text;
            popUpDesc.text = blc.description;

            timeToBuildPopUp.text = blc.timeToBuildText.text;
            if (blc.titan_current != 0)
            {
                titan_textPopUp.text = blc.titanText.text;
            }
            else
            {
                titan_imgPopUp.gameObject.SetActive(false);
            }

            if (blc.silicon_current != 0)
            {
                silicone_textPopUp.text = blc.siliconText.text;
            }
            else
            {
                silicone_imgPopUp.gameObject.SetActive(false);
            }

            if (blc.antimatter_current != 0)
            {
                antimatter_textPopUp.text = blc.antimatterText.text;
            }
            else
            {
                antimatter_imgPopUp.gameObject.SetActive(false);
            }

            upGradeBRPopUp_confirm.onClick.AddListener(() => { UpGradeBuildResearch_confirm(blc, _whatBlock); });

            popUpBuildResearch.SetActive(true);
        }
    }

    public void CancelBuildResearchTechPoUp(GameObject _whatBlock)
    {
        _whatBlock.SetActive(false);      
    }

    //Quene list for Buildings Values and Functions
    [Header("Building FiFo List Values")]
    public GameObject quenePrefab;
    public Transform parrentQuene;
    public Transform parrentQueneResearch;
    public Image queneAvatar;
    public Text queneName;
    public Text queneTime;
    private GameObject goInstQuene;
    private GameObject goDoc;
    public GameObject errorWindow;

    [Header("Research FiFo List Values")]
    public GameObject quenePrefab_r;
    public Transform parrentQuene_r;
    public Transform parrentQueneResearch_r;
    public Image queneAvatar_r;
    public Text queneName_r;
    public Text queneTime_r;
    private GameObject goInstQuene_r;

    [Header("DocDef FiFo List Values")]
    public GameObject quenePrefab_d;
    public Transform parrentQuene_d;
    public Transform parrentQueneResearch_d;
    public Image queneAvatar_d;
    public Text queneName_d;
    public Text queneTime_d;
    private GameObject goInstQuene_d;
    public GameObject docDefQuene;

    [Header("Open slider")]
    public GameObject slider;
    public static string nameBlock;
    public static string textInGalaxy;
    public static string panelName;
    string nameGO;

    public void OpenSlider()
    {
        slider.SetActive(true);

        GiveValuesToSlider();
        nameBlock = EventSystem.current.currentSelectedGameObject.transform.parent.parent.gameObject.name;
        textInGalaxy = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.name;
        panelName = EventSystem.current.currentSelectedGameObject.transform.parent.parent.parent.gameObject.name;
    }

    public void GiveValuesToSlider()
    {
        nameGO = EventSystem.current.currentSelectedGameObject.transform.name;

        if (GameObject.Find(nameGO).GetComponent<MinMaxController>())
        {
            slider.transform.FindChild("Slider").GetComponent<Slider>().minValue = GameObject.Find(nameGO).GetComponent<MinMaxController>().min;
            slider.transform.FindChild("Slider").GetComponent<Slider>().maxValue = GameObject.Find(nameGO).GetComponent<MinMaxController>().max;
        }

        slider.transform.FindChild("Slider").GetComponent<Slider>().value = float.Parse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
    }

    public void UpGradeBuildResearch_confirm(BlockController _blc, int _whatBlock)
    {
        if (!MainResourcesController.buildingListIsFull)
        {
            popUpBuildResearch.SetActive(false);
            if ((_blc.titan_current <= MainResourcesController.main_titan_value) && (_blc.silicon_current <= MainResourcesController.main_silicone_value) && (_blc.antimatter_current <= MainResourcesController.main_antimatter_value))
            {
                if (_whatBlock == (int)WhatBlockParse.Builings)
                {
                    string json_query = "http://vg2.v-galaktike.ru/api/?class=planet&method=addbuilding&token=" +
                        MainResourcesController.userToken + "&system_id=" +
                        MainResourcesController.currentSystemID + "&element_id=" +
                        _blc.blockId + "&mode=add";

                    queneName.text = _blc.title;
                    queneAvatar.sprite = _blc.ava;

                    StartCoroutine(AddBuildings(json_query, _blc));
                }
            }
            else
            {
                Debug.Log("Need more resources");
                NoEnoughtMoney_Panel.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Some buildings in progress please wait end build or use credit");
        }

        if (!MainResourcesController.researchListIsFull)
        {
            popUpBuildResearch.SetActive(false);
            if ((_blc.titan_current <= MainResourcesController.main_titan_value) && (_blc.silicon_current <= MainResourcesController.main_silicone_value) && (_blc.antimatter_current <= MainResourcesController.main_antimatter_value))
            {
                if (_whatBlock == (int)WhatBlockParse.Research)
                {
                    string json_query = "http://vg2.v-galaktike.ru/api/?class=planet&method=addresearch&token=" +
                        MainResourcesController.userToken + "&system_id=" +
                        MainResourcesController.currentSystemID + "&tech_id=" +
                        _blc.blockId + "&mode=add";

                    queneName.text = _blc.title;
                    queneAvatar.sprite = _blc.ava;

                    StartCoroutine(AddResearch(json_query, _blc));
                }
            }
            else
            {
                Debug.Log("Need more resources");
                NoEnoughtMoney_Panel.SetActive(true);
            }
        }


        if (_whatBlock == (int)WhatBlockParse.Weapon || _whatBlock == (int)WhatBlockParse.Defence)
        {
            popUpBuildResearch.SetActive(false);
            if ((_blc.titan_current <= MainResourcesController.main_titan_value) && (_blc.silicon_current <= MainResourcesController.main_silicone_value) && (_blc.antimatter_current <= MainResourcesController.main_antimatter_value))
            {
                if (_blc.blockId == 407 && !PlayerPrefs.HasKey("blockid407"))
                {
                    PlayerPrefs.SetInt("blockid407", 1);
                    countFromSlider = Int32.Parse((_blc.downBar.transform.FindChild("InputField").FindChild("Text").GetComponent<Text>().text));
                    string json_query = "http://vg2.v-galaktike.ru/api/?class=planet&method=addweapon&token=" + MainResourcesController.userToken +
                        "&system_id=" + MainResourcesController.currentSystemID +
                        "&element_id=" + _blc.blockId +
                        "&element_count=" + countFromSlider;

                    StartCoroutine(AddWeapon(json_query, _blc));
                }
                else if (_blc.blockId == 408 && !PlayerPrefs.HasKey("blockid408"))
                {
                    PlayerPrefs.SetInt("blockid408", 1);
                    countFromSlider = Int32.Parse((_blc.downBar.transform.FindChild("InputField").FindChild("Text").GetComponent<Text>().text));
                    string json_query = "http://vg2.v-galaktike.ru/api/?class=planet&method=addweapon&token=" + MainResourcesController.userToken +
                        "&system_id=" + MainResourcesController.currentSystemID +
                        "&element_id=" + _blc.blockId +
                        "&element_count=" + countFromSlider;

                    StartCoroutine(AddWeapon(json_query, _blc));
                }
                else if (_blc.blockId == 409 && !PlayerPrefs.HasKey("blockid409"))
                {
                    PlayerPrefs.SetInt("blockid409", 1);
                    countFromSlider = Int32.Parse((_blc.downBar.transform.FindChild("InputField").FindChild("Text").GetComponent<Text>().text));
                    string json_query = "http://vg2.v-galaktike.ru/api/?class=planet&method=addweapon&token=" + MainResourcesController.userToken +
                        "&system_id=" + MainResourcesController.currentSystemID +
                        "&element_id=" + _blc.blockId +
                        "&element_count=" + countFromSlider;

                    StartCoroutine(AddWeapon(json_query, _blc));
                }
                else
                {
                    countFromSlider = Int32.Parse((_blc.downBar.transform.FindChild("InputField").FindChild("Text").GetComponent<Text>().text));
                    string json_query = "http://vg2.v-galaktike.ru/api/?class=planet&method=addweapon&token=" + MainResourcesController.userToken +
                        "&system_id=" + MainResourcesController.currentSystemID +
                        "&element_id=" + _blc.blockId +
                        "&element_count=" + countFromSlider;

                    StartCoroutine(AddWeapon(json_query, _blc));
                }
            }
            else
            {
                Debug.Log("Need more resources");
                NoEnoughtMoney_Panel.SetActive(true);
            }
        }

        upGradeBRPopUp_confirm.onClick.RemoveAllListeners();
    }


    IEnumerator AddBuildings(string json_query, BlockController _blc)
    {
        WWW www = new WWW(json_query);
        yield return www;

        if (www.error == null)
        {
            var result = JSON.Parse(www.text);
            JSONNode system = result["system"];
            JSONNode buildingsInProgress = result["system"]["building"];

            MainResourcesController.buildingListIsFull = true;

            if (result["error"].Value != "")
            {
                Debug.Log("Json Building Error id: " + result["error"].Value);
                yield break;
            }
            else
            {
                parrentQuene.gameObject.SetActive(true);

                MainResourcesController.main_titan_value = system["titan"].AsDouble;
                MainResourcesController.main_silicone_value = system["silicon"].AsDouble;
                MainResourcesController.main_antimatter_value = system["antimatter"].AsDouble;
                MainResourcesController.energy_used = system["energy_used"].AsInt;
                MainResourcesController.timeNow = result["now"].AsLong;

                MainResourcesController mrc = TopBlock.GetComponent<MainResourcesController>();
                mrc.ValOnTheirPlace();

                if (parrentQuene.transform.FindChild("ProgressElement(Clone)"))
                {
                    Destroy(parrentQuene.transform.FindChild("ProgressElement(Clone)").gameObject);
                }

                    MainResourcesController.buildingInProgress.Add(new BuildingsInProgress(
                    buildingsInProgress["system_id"].AsInt,
                    buildingsInProgress["building_id"].AsInt,
                    buildingsInProgress["building_end"].AsLong,
                    buildingsInProgress["building_start"].AsLong));

                goInstQuene = Instantiate(quenePrefab);
                goInstQuene.transform.SetParent(parrentQuene);
                goInstQuene.transform.localScale = new Vector3(1, 1, 1);

                QueneController qC = goInstQuene.GetComponent<QueneController>();
                qC.parrentBuilding = parentGOBuildings.gameObject;
                qC.timeToEndBuild = buildingsInProgress["building_end"].AsLong - MainResourcesController.timeNow; // _blc.timetoend
                qC.queneId = _blc.blockId;
                qC.timeToEndTxt = goInstQuene.transform.FindChild("TimeValue_text").GetComponent<Text>();
                qC.progress = goInstQuene.transform.FindChild("Bg_image").GetComponent<Image>();
                qC.progress.fillAmount = 1f - _blc.timeToBuild / (MainResourcesController.buildingInProgress[0].timeToEnd - MainResourcesController.buildingInProgress[0].startTimeBuild);
                qC.TickBuild();
                yield break;
            }
        }
        else
        {
            Debug.Log(www.error);
            errorWindow.SetActive(true);
            yield break;
        }
    }

    IEnumerator AddResearch(string json_query, BlockController _blc)
    {
        WWW www = new WWW(json_query);
        yield return www;

        if (www.error == null)
        {
            var result = JSON.Parse(www.text);
            JSONNode system = result["system"];
            JSONNode current_user = result["user"];
            JSONNode researchInProgress = current_user["tech"];

            MainResourcesController.researchListIsFull = true;

            if (result["error"].Value != "")
            {
                Debug.Log("Json Building Error id: " + result["error"].Value);
                yield break;
            }
            else
            {
                parrentQuene_r.gameObject.SetActive(true);

                MainResourcesController.main_titan_value = system["titan"].AsDouble;
                MainResourcesController.main_silicone_value = system["silicon"].AsDouble;
                MainResourcesController.main_antimatter_value = system["antimatter"].AsDouble;
                MainResourcesController.energy_used = system["energy_used"].AsInt;
                MainResourcesController.timeNow = result["now"].AsLong;

                MainResourcesController mrc = TopBlock.GetComponent<MainResourcesController>();
                mrc.ValOnTheirPlace();

                if (parrentQuene_r.transform.FindChild("ProgressElement(Clone)"))
                {
                    Destroy(parrentQuene_r.transform.FindChild("ProgressElement(Clone)").gameObject);
                }
                
                MainResourcesController.researchInProgress.Add(new ResearchInProgress(
                    researchInProgress["user_id"].AsInt,
                    researchInProgress["tech_id"].AsInt,
                    researchInProgress["tech_end"].AsLong,
                    researchInProgress["tech_start"].AsLong));

                goInstQuene_r = Instantiate(quenePrefab_r);
                goInstQuene_r.transform.SetParent(parrentQuene_r);
                goInstQuene_r.transform.localScale = new Vector3(1, 1, 1);

                QueneController qC = goInstQuene_r.GetComponent<QueneController>();
                qC.parrentBuilding = parentGOResearch.gameObject;
                qC.queneId = _blc.blockId;
                qC.timeToEndBuild = researchInProgress["tech_end"].AsLong - MainResourcesController.timeNow;  //_blc.timetoend
                qC.timeToEndTxt = goInstQuene_r.transform.FindChild("TimeValue_text").GetComponent<Text>();                                                   
                qC.progress = goInstQuene_r.transform.FindChild("Bg_image").GetComponent<Image>();
                qC.progress.fillAmount = 1f - _blc.timeToBuild / (MainResourcesController.researchInProgress[0].timeToEnd - MainResourcesController.researchInProgress[0].startTime);
                qC.TickResearch();
                yield break;
            }
        }
        else
        {
            Debug.Log(www.error);
            errorWindow.SetActive(true);
            yield break;
        }
    }

    IEnumerator AddWeapon(string json_query, BlockController _blc)
    {
        WWW www = new WWW(json_query);
        yield return www;

        if (www.error == null)
        {
            var result = JSON.Parse(www.text);
            JSONNode system = result["system"];
            JSONNode docDefInProgress = result["system"]["weapon"];

            if (result["error"].Value != "")
            {
                Debug.Log("Json Doc Error id: " + result["error"].Value);
                yield break;
            }
            else
            {
                parrentQuene_d.gameObject.SetActive(true);

                MainResourcesController.main_titan_value = system["titan"].AsDouble;
                MainResourcesController.main_silicone_value = system["silicon"].AsDouble;
                MainResourcesController.main_antimatter_value = system["antimatter"].AsDouble;
                MainResourcesController.energy_used = system["energy_used"].AsInt;
                MainResourcesController.timeNow = result["now"].AsLong;
                MainResourcesController.weapone_update = system["weapon_update"].AsLong;

                MainResourcesController mrc = TopBlock.GetComponent<MainResourcesController>();
                mrc.ValOnTheirPlace();

                MainResourcesController.docDefInProgress.Clear();
                for (int i = 0; i < docDefInProgress.Count; i++)
                {
                    MainResourcesController.docDefInProgress.Add(new DocDefInProgress(
                        docDefInProgress[i]["system_id"].AsInt,
                        docDefInProgress[i]["element_id"].AsInt,
                        docDefInProgress[i]["count"].AsInt));
                }

                GameObject.Find("DocDef").GetComponent<DocOnEnabled>().StartThis();

                yield return null;
            }
        }
        else
        {
            errorWindow.SetActive(true);
            yield break;
        }
    }


    public GameObject NoEnoughtMoney_Panel;
    public void NoEnoughtMoney_Close_Btn()
    {
        NoEnoughtMoney_Panel.SetActive(false);
    } 
}

   

