using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BlockController : MonoBehaviour 
{
    //Global values
    public int blockId;
    public int blockLevel;
    public int maxLenght = 80;

    //Colors
    public Color32 red = new Color32(233, 58, 78, 255); // E91C4E
    public Color32 green = new Color32(112, 185, 32, 255); // 70B920

    //Strings values (title,desc)
    public string title;
    public string description;
    public string shortDesc;
    public Sprite ava;

    //Resources values (first level)
    public float titan;
    public float silicon;
    public float antimatter;
    public float energy;
    public float factor;

    //Resource val (current level)
    public float titan_current;
    public float silicon_current;
    public float antimatter_current;
    public float energy_current;

    //Time values
    public float timeToBuild;

    //Variables to set some place
    public Image avatar;
    public Text rName;
    public Text rInfo;
    public Text titanText;
    public Text siliconText;
    public Text antimatterText;
    public Image titanImg;
    public Image siliconImg;
    public Image antimatterImg; 
    public Text requairmentsText;
    public Text timeToBuildText;
    public Button upGradeBtn;
    public GameObject downBar;

    string xmlDoc;
    string reqText;
    //Requarments values
    public List<RequerList> rList = new List<RequerList>();
    
    //Function to Set valuet to some places START
    public void SetBlockName()
    {        
        gameObject.name = gameObject.name.Replace("(Clone)", "_" + blockId);
    }
       
    public void SetAvatar()
    {
        avatar.sprite = ava;
    }

    public void SetTitle()
    {
        rName.text = title + " (" + blockLevel + ")";
    }
    public void SetTitleForDoc()
    {
        rName.text = title;
    }

    public void SetDesc()
    {
        rInfo.text = GeneratedShortDesc(description, maxLenght);
    }

    public void SetPrices()
    {
        titan_current = titan;
        titan_current = GetCurrentLvlPrice(titan);
        titanText.text = CheckToZeroResources(titan_current, titanImg.gameObject);

        silicon_current = silicon;
        silicon_current = GetCurrentLvlPrice(silicon);
        siliconText.text = CheckToZeroResources(silicon_current, siliconImg.gameObject);

        antimatter_current = antimatter;
        antimatter_current = GetCurrentLvlPrice(antimatter);
        antimatterText.text = CheckToZeroResources(antimatter_current, antimatterImg.gameObject);

        CheckResourcesToBuild();
    }

    public void SetTimeToBuild()
    {
        GetCurrentTime();

        TimeSpan time = TimeSpan.FromSeconds(timeToBuild);

        int days;
        days = time.Days;
        if (days != 0)
        {
            timeToBuildText.text = string.Format("{0:D2} day(s) {1:D2}:{2:D2}:{3:D2}",
                time.Days,
                time.Hours,
                time.Minutes,
                time.Seconds);
        }
        else
        {
            timeToBuildText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                time.Hours,
                time.Minutes,
                time.Seconds);
        }
    }
    public void ClearRequairments()
    {
        rList.Clear();
        requairmentsText.text = "";
        reqText = "";
    }

    public void SetRequairments(string _xmlDoc, ParseAll.WhatBlockParse _wbp)
    {
        GetRequeriments(blockId, _xmlDoc);
      
        foreach (var mm in rList)
        {
            foreach (var nn in MainResourcesController.buildingsLevel)
            {
                if (nn.Key == mm.strTech)
                {
                    if (nn.Value >= mm.count_need)
                    {
                        reqText += "<color=#70B920>" + mm.strGood + " (" + mm.count_need + "),</color> ";
                        requairmentsText.text = reqText;
                    }
                    else
                    {
                        reqText += "<color=#E91C4E>" + mm.strGood + " (" + mm.count_need + "),</color> ";
                        requairmentsText.text = reqText;
                    }
                }
            }  
                     
            foreach (var pp in MainResourcesController.technologyLevel)
            {
                if (pp.Key == mm.strTech)
                {
                    if (pp.Value >= mm.count_need)
                    {
                        reqText += "<color=#70B920>" + mm.strGood + " (" + mm.count_need + "),</color> ";
                        requairmentsText.text = reqText;
                    }
                    else
                    {
                        reqText += "<color=#E91C4E>" + mm.strGood + " (" + mm.count_need + "),</color> ";
                        requairmentsText.text = reqText;
                    }
                }
            }

            if (_wbp == ParseAll.WhatBlockParse.Technology)
            {
                gameObject.SetActive(true);
            }
            else
            {
                if (reqText.Contains("E91C4E"))
                    gameObject.SetActive(false);
                else
                    gameObject.SetActive(true);
            }
        }
    }

    public void SetCapitolium()
    {
        if(MainResourcesController.mainSystemID != MainResourcesController.currentSystemID)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetBlockLevel(int _id, ParseAll.WhatBlockParse _wbp)
    {
        if (_wbp == ParseAll.WhatBlockParse.Builings || _wbp == ParseAll.WhatBlockParse.Technology)
        {
            foreach (var mm in MainResourcesController.buildingsLevel)
            {
                foreach (var nn in MainResourcesController.resDictionary)
                {
                    if (nn.resId == _id)
                        if (nn.resTechName == mm.Key)
                        {
                            blockLevel = mm.Value;
                        }
                }
            }
        }

        if (_wbp == ParseAll.WhatBlockParse.Research || _wbp == ParseAll.WhatBlockParse.Technology)
        {
            foreach (var mm in MainResourcesController.technologyLevel)
            {
                foreach (var nn in MainResourcesController.resDictionary)
                {
                    if (nn.resId == _id)
                        if (nn.resTechName == mm.Key)
                        {
                            blockLevel = mm.Value;
                        }
                }
            }
        }
    }
    //Function to Set valuet to some places END

    //Other functions START
    void CheckResourcesToBuild()
    {
        upGradeBtn.interactable = true;

        if (titan_current > MainResourcesController.main_titan_value)
            titanText.color = red;
        else
            titanText.color = green;

        if (silicon_current > MainResourcesController.main_silicone_value)
            siliconText.color = red;
        else
            siliconText.color = green;

        if (antimatter_current > MainResourcesController.main_antimatter_value)
            antimatterText.color = red;
        else
            antimatterText.color = green;
    }

    float GetCurrentLvlPrice(float currrent)
    {
        float result = 0;

        result = Mathf.Floor(currrent * Mathf.Pow(factor, blockLevel));
   
        return result;
    }

    string GeneratedShortDesc(string str, int maxLenght)
    {
        if (str.Length > maxLenght)
        {
            int end = str.Substring(0, maxLenght).LastIndexOf(' ');
            str = str.Substring(0, end);
            str += "...";
            return str;
        }
        else
        {
            return str;
        }
    }

    string CheckToZeroResources(float count, GameObject activate)
    {
        if (count == 0)
        {
            activate.SetActive(false);
            return null;
        }
        else
        {
            activate.SetActive(true);
            string countStringBig;

            if (count >= 10000 && count < 1000000)
            {
                if ((count % 1000) != 0)
                    countStringBig = (count / 1000).ToString("F1") + "K";
                else
                    countStringBig = (count / 1000).ToString("F0") + "K";
            }
            else if (count >= 1000000)
            {
                if ((count % 1000000) != 0)
                    countStringBig = (count / 1000000).ToString("F1") + "KK";
                else
                    countStringBig = (count / 1000000).ToString("F0") + "KK";
            }
            else
            {
                countStringBig = count.ToString();
            }
            return countStringBig;
        }
    }

    void GetCurrentTime()
    {
        int mn = 0;
        float funtcion_1, function_2, function_3;

        if (blockId >= 1 && blockId <= 45)
        {
            funtcion_1 = (titan_current + silicon_current) / MainResourcesController.gameSpeed;
            function_2 = 1.0f / (MainResourcesController.robot_factory + 1.0f);
            function_3 = Mathf.Pow(0.5f, MainResourcesController.nano_factory);

            timeToBuild = funtcion_1 * function_2 * function_3;
            timeToBuild = Mathf.Floor((timeToBuild * 60.0f * 60.0f) * (1.0f - (mn * 0.25f)));
        }
        else if (blockId >= 106 && blockId <= 199)
        {
            funtcion_1 = (titan_current + silicon_current) / MainResourcesController.gameSpeed;
            function_2 = 1.0f / (MainResourcesController.laboratory + 1.0f);
            function_3 = Mathf.Pow(0.5f, MainResourcesController.nano + 1);

            timeToBuild = funtcion_1 * function_2 * function_3;
            timeToBuild = Mathf.Floor((timeToBuild * 60.0f * 60.0f) * (1.0f - (mn * 0.25f)));
        }
        else if (blockId >= 202 && blockId <= 408)
        {
            funtcion_1 = (titan_current + silicon_current) / MainResourcesController.gameSpeed;
            function_2 = 1.0f / (MainResourcesController.hangar + 1.0f);
            function_3 = Mathf.Pow(0.5f, MainResourcesController.nano_factory);

            timeToBuild = funtcion_1 * function_2 * function_3;
            timeToBuild = Mathf.Floor((timeToBuild * 60.0f * 60.0f) * (1.0f - (mn * 0.25f)));
        }
    }

    Root_requeriments requer;
    public void GetRequeriments(int _id, string _xmlDoc) //ParseRequeriments - old name
    {
        requer = Root_requeriments.LoadFromText(_xmlDoc);
        foreach (var mm in requer.requer)
        {
            if (mm.requerimentsId == _id)
            {
                requerStrFunc(mm.requerElem);
            }
        }
    }

    void requerStrFunc(List<requerElements> reL)
    {
        foreach (var mm in reL)
        {
            foreach (var nn in MainResourcesController.resDictionary)
            {
                if (mm.requerElementsId == nn.resId)
                {
                    rList.Add(new RequerList(
                        mm.requerElementsId,
                        mm.requerElementsCount,
                        nn.resTechName,
                        nn.resGoodName));
                }
            }
        }
    }
    //Other functions END
}

public class RequerList
{
    public int id;
    public int count_need;
    public string strTech;
    public string strGood;

    public RequerList(int _id, int _count_need, string _strTech, string _strGood)
    {
        id = _id;
        count_need = _count_need;
        strTech = _strTech;
        strGood = _strGood;
    }

}

