using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class XmlClass
{
   
    Root_build_conf canBuildID;

    Root_helper_buildings helpBuild;
    Root_helper_research helpResearch;
    Root_helper_tech helpTech;
    Root_helper_doc_def helpDoc;
    Root_helper_def helpDef;

    string xmlDoc;

    string title, desc, shortdesc;
    public string titleHead;
    public string descHead;

    int countLevel = 0;

    void ParseTexts(int _id, int _whatParse)
    {
        switch (_whatParse)
        {
            case 0:
                helpBuild = Root_helper_buildings.LoadFromText(xmlDoc);
                foreach (var mm in helpBuild.helpBuildings)
                {
                    if (_id == mm.helpBuildingsId)
                    {
                        title = mm.helpBuildingsTitle;
                        desc = mm.helpBuildingsDesc;
                    }
                }
                break;
            case 1:
                helpResearch = Root_helper_research.LoadFromText(xmlDoc);
                foreach (var mm in helpResearch.helpResearch)
                {
                    if (_id == mm.helpResearchId)
                    {
                        title = mm.helpResearchTitle;
                        desc = mm.helpResearchDesc;
                    }
                }
                break;
            case 2:
                helpDoc = Root_helper_doc_def.LoadFromText(xmlDoc);
                foreach (var mm in helpDoc.helpDocs)
                {
                    if (_id == mm.helpDocsId)
                    {
                        title = mm.helpDocsTitle;
                        desc = mm.helpDocsDesc;
                    }
                }
                break;
            case 3:
                helpDef = Root_helper_def.LoadFromText(xmlDoc);
                foreach (var mm in helpDef.helpDef)
                {
                    if (_id == mm.helpDefId)
                    {
                        title = mm.helpDefTitle;
                        desc = mm.helpDefDesc;
                    }
                }
                break;
            case 4:
                helpTech = Root_helper_tech.LoadFromText(xmlDoc);
                foreach (var mm in helpTech.TechTreeItem)
                {
                    if (_id == mm.TechTreeItemId)
                    {
                        title = mm.TechTreeItemTitle;
                        desc = mm.TechTreeItemDesc;
                    }
                }
                break;
           
        }
    }

    Root_pricelist priceList;
    Dictionary<string, float> priceD = new Dictionary<string, float>();
    void ParsePrices(int _id)
    {
        priceList = Root_pricelist.LoadFromText(xmlDoc);
        foreach (var pl in priceList.elemP)
        {
            if (_id == pl.elementPriceID)
            {
                priceD.Add("titan", pl.paramP.paramMetal);
                priceD.Add("silicon", pl.paramP.paramCrystal);
                priceD.Add("antimatter", pl.paramP.paramDeuterium);
                priceD.Add("energy", pl.paramP.paramEnergy);
                priceD.Add("factor", pl.paramP.paramFactor);
                break;
            }
        }
    }

    Sprite GetAvatar(int _id)
    {
        Sprite spr;

        if (_id >= 1 && _id <= 45)
        {
            spr = Resources.Load<Sprite>("buildingsAvatars/" + _id.ToString());
        }
        else if (_id >= 106 && _id <= 199)
        {
            spr = Resources.Load<Sprite>("researchAvatars/" + _id.ToString());
        }
        else if (_id >= 202 && _id <= 216)
        {
            spr = Resources.Load<Sprite>("fleetAvatars/" + _id.ToString());
        }
        else
        {
            spr = Resources.Load<Sprite>("defAvatars/" + _id.ToString());
        }
         
        return spr;
    }

    //Root_requeriments requer;
    //void ParseRequeriments(int _id)
    //{
    //    requer = Root_requeriments.LoadFromText(xmlDoc);
    //    requerList = new List<RequerList>();
    //    foreach (var mm in requer.requer)
    //    {
    //        if (mm.requerimentsId == _id)
    //        {
    //            requerStrFunc(mm.requerElem);
    //        }
    //    }
    //}

    //List<RequerList> requerList;
    //void requerStrFunc(List<requerElements> reL)
    //{
    //    foreach (var mm in reL)
    //    {
    //        foreach (var nn in MainResourcesController.resDictionary)
    //        {
    //            if (mm.requerElementsId == nn.resId)
    //            {
    //                requerList.Add(new RequerList(
    //                    mm.requerElementsId,
    //                    mm.requerElementsCount,
    //                    nn.resTechName,
    //                    nn.resGoodName));
    //            }
    //        }
    //    }
    //}

    Text Rname, Rinfo, FullDescr, metal, crystal, deuterium, researchRequir, timeToBuild;
    Image metalImg, crystalImg, deuteriumImg;
    Transform resourcesBlock;
    Sprite avatar;
    BlockController blockContr;
    RequerList requerController;
    Button upGrade;

    public GameObject analyseGO(int _id,int _whatParse, GameObject _prefab, Image _avatar, Transform _inform, Transform _downBar, string _xmlDoc)
    {
        xmlDoc = _xmlDoc;

        Rname = _inform.FindChild("ResearchName_text").GetComponent<Text>();
        Rinfo = _inform.FindChild("ResearchInfo_text").GetComponent<Text>();

        if (_whatParse != 4)
        {
            resourcesBlock = _inform.FindChild("Resources").GetComponent<Transform>();

            metal = resourcesBlock.FindChild("Metal_image").FindChild("MetalValue_text").GetComponent<Text>();
            metalImg = resourcesBlock.FindChild("Metal_image").GetComponent<Image>();
            crystal = resourcesBlock.FindChild("Crystal_image").FindChild("CrystalValue_text").GetComponent<Text>();
            crystalImg = resourcesBlock.FindChild("Crystal_image").GetComponent<Image>();
            deuterium = resourcesBlock.FindChild("Deuterium_image").FindChild("Deuterium_text").GetComponent<Text>();
            deuteriumImg = resourcesBlock.FindChild("Deuterium_image").GetComponent<Image>();

            timeToBuild = _downBar.FindChild("TimerValue_text").GetComponent<Text>();
        }

        researchRequir = _inform.FindChild("ResearchRequirements_text").GetComponent<Text>();
        
        upGrade = _downBar.FindChild("UpgradeButton").GetComponent<Button>();

        ParseTexts(_id,_whatParse);
        
        blockContr = _prefab.GetComponent<BlockController>();
        blockContr.blockId = _id;
        blockContr.title = title;
        blockContr.description = desc;
        blockContr.ava = GetAvatar(_id);

        blockContr.requairmentsText = researchRequir;

        blockContr.rName = Rname;
        blockContr.rInfo = Rinfo;
        blockContr.avatar = _avatar;
        blockContr.upGradeBtn = upGrade;

        if (_whatParse != 4)
        {
            ParsePrices(_id);
            blockContr.titan = priceD["titan"];
            blockContr.silicon = priceD["silicon"];
            blockContr.antimatter = priceD["antimatter"];
            blockContr.energy = priceD["energy"];
            blockContr.factor = priceD["factor"];
            priceD.Clear();

            blockContr.titanText = metal;
            blockContr.titanImg = metalImg;
            blockContr.siliconText = crystal;
            blockContr.siliconImg = crystalImg;
            blockContr.antimatterText = deuterium;
            blockContr.antimatterImg = deuteriumImg;
            blockContr.timeToBuildText = timeToBuild;
        }
        
        return _prefab;
    }
}