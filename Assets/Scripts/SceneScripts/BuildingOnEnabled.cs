using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class BuildingOnEnabled : MonoBehaviour
{
    public GameObject progressParrent;
    public GameObject blocksParrent;
    public BuildingsInProgress buildsInProgress;
    private BlockController thisBlc;
    private GameObject goInstQuene;
    public Text queneName;
    public Image queneAvatar;
    public GameObject quenePrefab;

    public ParseAll.WhatBlockParse wbp;
    public GameObject loadingPanel;
    public GameObject ScrManager;
    ScreenManager scrnM;
    public void StartThis()
    {
        loadingPanel.SetActive(true);
        
        if (progressParrent.GetComponentInChildren<QueneController>())
        {
            Destroy(progressParrent.GetComponentInChildren<QueneController>().gameObject);
        }

        StartCoroutine(GetJson());
    }

    void OnEnable()
    {
        scrnM = ScrManager.GetComponent<ScreenManager>();   
        StartThis();       
    }

    JSONNode result;
    IEnumerator GetJson()
    {
        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=user&method=getuser&token=" + MainResourcesController.userToken );
        yield return www;

        result = JSON.Parse(www.text);

        MainResourcesController.timeNow = result["now"].AsLong;

        if(MainResourcesController.buildingInProgress.Count != 0)
        {
            progressParrent.SetActive(true);

            buildsInProgress = new BuildingsInProgress(MainResourcesController.buildingInProgress[0].sysId,
                MainResourcesController.buildingInProgress[0].buildId,
                MainResourcesController.buildingInProgress[0].timeToEnd - MainResourcesController.timeNow,
                MainResourcesController.buildingInProgress[0].timeToEnd - MainResourcesController.buildingInProgress[0].startTimeBuild );

            thisBlc = blocksParrent.transform.FindChild("Building_prefab_" + buildsInProgress.buildId).GetComponent<BlockController>();

            queneName.text = thisBlc.title;
            queneAvatar.sprite = thisBlc.ava;
            goInstQuene = Instantiate(quenePrefab);
            goInstQuene.transform.SetParent(progressParrent.transform);
            goInstQuene.transform.localScale = new Vector3(1, 1, 1);

            QueneController qC = goInstQuene.GetComponent<QueneController>();
            qC.queneId = buildsInProgress.buildId;
            qC.timeToEndBuild = buildsInProgress.timeToEnd;
            qC.timeToEndTxt = goInstQuene.transform.FindChild("TimeValue_text").GetComponent<Text>();
            qC.parrentBuilding = progressParrent;
            qC.progress = goInstQuene.transform.FindChild("Bg_image").GetComponent<Image>();
            qC.progress.fillAmount = 1f - buildsInProgress.timeToEnd / buildsInProgress.startTimeBuild;
            qC.TickBuild();
        }
        else
        {
            progressParrent.SetActive(false);
        }

        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
        scrnM.ShowCurrentPanel();       
    }

    Root_helper_headers rhh;
    [Header("Building info")]
    public Text nameTitle;
    public Text textField;
    public GameObject popUp;

    public void ButtonQ()
    {
        popUp.SetActive(true);
        rhh = Root_helper_headers.LoadFromText(MainResourcesController.xmlDoc.text);
        nameTitle.text = rhh.helpHeaders.buildPage.helpHeadersTitle;
        textField.text = rhh.helpHeaders.buildPage.helpHeadersDesc;
    }
}
