using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class ResearchOnEnabled : MonoBehaviour
{

    public GameObject errorPanel;
    public GameObject goodPanel;

    public GameObject progressParrent;
    public GameObject blocksParrent;
    public ResearchInProgress researchInProgress;
    private BlockController thisBlc;
    private GameObject goInstQuene;
    public Text queneName;
    public Image queneAvatar;
    public GameObject quenePrefab;
    public ParseAll.WhatBlockParse wbp;
    public GameObject ScrManager;
    ScreenManager scrnM;
    public GameObject loadingPanel;

    public void StartThis()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (MainResourcesController.laboratory != 0)
            {
                loadingPanel.SetActive(true);
                goodPanel.SetActive(true);
                errorPanel.SetActive(false);

                if (progressParrent.GetComponentInChildren<QueneController>())
                {
                    Destroy(progressParrent.GetComponentInChildren<QueneController>().gameObject);
                }

                StartCoroutine(GetJson());
            }
            else
            {
                goodPanel.SetActive(false);
                errorPanel.SetActive(true);
            }
        }
    }

    void OnEnable()
    {
        scrnM = ScrManager.GetComponent<ScreenManager>();
        StartThis();
    }

    JSONNode result;
    IEnumerator GetJson()
    {
        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=user&method=getuser&token=" + MainResourcesController.userToken);
        yield return www;

        result = JSON.Parse(www.text);

        MainResourcesController.timeNow = result["now"].AsLong;

        if (MainResourcesController.researchInProgress.Count != 0)
        {
            progressParrent.SetActive(true);

            researchInProgress = new ResearchInProgress(MainResourcesController.researchInProgress[0].userid,
                MainResourcesController.researchInProgress[0].techid,
                MainResourcesController.researchInProgress[0].timeToEnd - MainResourcesController.timeNow,
                MainResourcesController.researchInProgress[0].timeToEnd - MainResourcesController.researchInProgress[0].startTime);

            thisBlc = blocksParrent.transform.FindChild("Research_prefab_" + researchInProgress.techid).GetComponent<BlockController>();

            queneName.text = thisBlc.title;
            queneAvatar.sprite = thisBlc.ava;
            goInstQuene = Instantiate(quenePrefab);
            goInstQuene.transform.SetParent(progressParrent.transform);
            goInstQuene.transform.localScale = new Vector3(1, 1, 1);

            QueneController qC = goInstQuene.GetComponent<QueneController>();
            qC.queneId = researchInProgress.techid;
            qC.timeToEndBuild = researchInProgress.timeToEnd;
            qC.timeToEndTxt = goInstQuene.transform.FindChild("TimeValue_text").GetComponent<Text>();
            qC.parrentBuilding = progressParrent;
            qC.progress = goInstQuene.transform.FindChild("Bg_image").GetComponent<Image>();
            qC.progress.fillAmount = 1f - researchInProgress.timeToEnd / researchInProgress.startTime;
            qC.TickResearch();
        }
        else
        {
            progressParrent.SetActive(false);
        }

        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
        scrnM.ShowCurrentPanel();
    }
}
