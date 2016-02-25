using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;
using System;

public class DocOnEnabled : MonoBehaviour
{
    public GameObject loadingPanel;

    public GameObject docPanel;
    public GameObject defPanel;
    public GameObject docDefErrorPanel;

    public GameObject headerDoc;
    public GameObject headerDef;

    public GameObject buildingInProgressParent;
    public GameObject prefabProgress;

    public ParseAll.WhatBlockParse whatBlock;

    public BlockController[] allDoc;
    public BlockController[] allDef;

    public GameObject ScrManager;
    ScreenManager scrnM;

    public void CheckPanel(int _whatBlock)
    {
        if (_whatBlock == (int)ParseAll.WhatBlockParse.Weapon)
        {
            whatBlock = ParseAll.WhatBlockParse.Weapon;

            headerDoc.SetActive(true);
            headerDef.SetActive(false);

            if (MainResourcesController.hangar != 0)
            {
                docPanel.SetActive(true);
                defPanel.SetActive(false);
                docDefErrorPanel.SetActive(false);
            }
            else
            {
                docDefErrorPanel.SetActive(true);

                docPanel.SetActive(false);
                defPanel.SetActive(false);
            }
        }
        else if (_whatBlock == (int)ParseAll.WhatBlockParse.Defence)
        {
            whatBlock = ParseAll.WhatBlockParse.Defence;

            headerDoc.SetActive(false);
            headerDef.SetActive(true);

            if (MainResourcesController.hangar != 0)
            {
                docPanel.SetActive(false);
                defPanel.SetActive(true);
                docDefErrorPanel.SetActive(false);
            }
            else
            {
                docDefErrorPanel.SetActive(true);

                docPanel.SetActive(false);
                defPanel.SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        scrnM = ScrManager.GetComponent<ScreenManager>();
        StartThis();
    }

    public void StartThis()
    {
        loadingPanel.SetActive(true);

        CheckPanel((int)whatBlock);
       /* if (whatBlock == ParseAll.WhatBlockParse.Weapon)
            RefreshController.R_Doc(docPanel);
        else if (whatBlock == ParseAll.WhatBlockParse.Defence)
            RefreshController.R_Defc(defPanel);*/

        if (buildingInProgressParent.GetComponentInChildren<QueneController>())
        {
            Destroy(buildingInProgressParent.GetComponentInChildren<QueneController>().gameObject);
        }

        if (whatBlock == ParseAll.WhatBlockParse.Weapon)
            ClearProgress(docPanel);
        else
            ClearProgress(defPanel);

        StartCoroutine(GetJson());
    }

    QueneController[] allQuene;
    void ClearProgress(GameObject _parent)
    {
        allQuene = _parent.GetComponentsInChildren<QueneController>();

        foreach (QueneController child in allQuene)
        {
            Destroy(child.gameObject);
        }
    }

    private JSONNode result;
    private GameObject instGO;
    private DocDefInProgress docDefInProgress;
    private BlockController thisBlc;

    public Text queneName;
    public Image queneAvatar;

    public GameObject docQuene;
    public GameObject defQuene;

    IEnumerator GetJson()
    {
        loadingPanel.SetActive(true);

        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=user&method=getuser&token=" + MainResourcesController.userToken);
        yield return www;

        if (www.error == null)
        {
            result = JSON.Parse(www.text);

            MainResourcesController.timeNow = result["now"].AsLong;
            //MainResourcesController.weapone_update = result["system"]["weapon_update"].AsLong;

            if(MainResourcesController.docDefInProgress.Count != 0)
            {
                buildingInProgressParent.SetActive(true);

                for (int i = 0; i < MainResourcesController.docDefInProgress.Count; i++)
                {
                    docDefInProgress = new DocDefInProgress(MainResourcesController.docDefInProgress[i].systemId,
                        MainResourcesController.docDefInProgress[i].elementId,
                        MainResourcesController.docDefInProgress[i].count_elements);

                    FindBLC(docDefInProgress.elementId);

                    queneName.text = thisBlc.title + "(" + docDefInProgress.count_elements + ")";
                    queneAvatar.sprite = thisBlc.ava;
                    instGO = Instantiate(prefabProgress);

                    QueneController qC = instGO.GetComponent<QueneController>();
                    qC.queneId = docDefInProgress.elementId;
                    qC.timeToEndTxt = instGO.transform.FindChild("TimeValue_text").GetComponent<Text>();
                    qC.timeToEndBuild = thisBlc.timeToBuild * docDefInProgress.count_elements - (MainResourcesController.timeNow - MainResourcesController.weapone_update);
                    qC.timeEndDoc = thisBlc.timeToBuild * docDefInProgress.count_elements;
                    qC.progress = instGO.transform.FindChild("Bg_image").GetComponent<Image>();

                    if (i == 0)
                    {
                        instGO.transform.SetParent(buildingInProgressParent.transform);
                        qC.TickDocDef();
                    }
                    else
                    {
                        qC.timeToEndTxt.text = SetTime(qC.timeEndDoc);

                        if (whatBlock == ParseAll.WhatBlockParse.Weapon)
                            instGO.transform.SetParent(docQuene.transform);
                        else
                            instGO.transform.SetParent(defQuene.transform);
                    }

                    instGO.transform.localScale = new Vector3(1, 1, 1);

                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
        else
        {
            Debug.Log("No Internet Connections");
        }

        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
        scrnM.ShowCurrentPanel();
    }

    void FindBLC(int _id)
    {
        if (_id >= 202 && _id <= 216)
        {
            foreach (var mm in ParseAll.allDoc)
            {
                if (docDefInProgress.elementId == mm.blockId)
                {
                    thisBlc = mm;
                }
            }
        }
        else
        {
            foreach (var mm in ParseAll.allDoc)
            {
                if (docDefInProgress.elementId == mm.blockId)
                {
                    thisBlc = mm;
                }
            }
        }
    }

    String SetTime(float _timeToEnd)
    {
        TimeSpan time = TimeSpan.FromSeconds(_timeToEnd);
        string str;

        int days;
        days = time.Days;
        if (days != 0)
        {
            str = string.Format("{0:D2} day(s) {1:D2}:{2:D2}:{3:D2}",
                time.Days,
                time.Hours,
                time.Minutes,
                time.Seconds);
        }
        else
        {
            str = string.Format("{0:D2}:{1:D2}:{2:D2}",
                time.Hours,
                time.Minutes,
                time.Seconds);
        }

        return str;
    }
}



