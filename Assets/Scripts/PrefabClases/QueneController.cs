using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QueneController : MonoBehaviour
{
    public int queneId;
    public float timeToEndBuild;
    public Image progress;
    public Text timeToEndTxt;
    public GameObject parrentBuilding;
    public GameObject parrentDoc;
    public GameObject parrentDef;
    public float timeEndDoc;
   
    public void TickBuild()
    {
        InvokeRepeating("StartTickBuildings", 0f, 1f);
    }
    public void TickResearch()
    {
        InvokeRepeating("StartTickResearch", 0f, 1f);
    }
    public void TickDocDef() 
    {
        InvokeRepeating("StartTickDoc", 0f, 1f);
    }

    public void StartTickBuildings()
    {
        if (timeToEndBuild != 0)
        {
            timeToEndBuild--;
            progress.fillAmount = 1f - timeToEndBuild / (MainResourcesController.buildingInProgress[0].timeToEnd - MainResourcesController.buildingInProgress[0].startTimeBuild);
            TimeSpan time = TimeSpan.FromSeconds(timeToEndBuild);

            int days;
            days = time.Days;
            if (days != 0)
            {
                timeToEndTxt.text = string.Format("{0:D2} day(s) {1:D2}:{2:D2}:{3:D2}",
                    time.Days,
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
            else
            {
                timeToEndTxt.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
        }
        else
        {
            OnEndBuild();
        }
    }

    public void StartTickResearch()
    {
        if (timeToEndBuild != 0)
        {
            timeToEndBuild--;
            progress.fillAmount = 1f - timeToEndBuild / (MainResourcesController.researchInProgress[0].timeToEnd - MainResourcesController.researchInProgress[0].startTime);
            //Debug.Log(progress.fillAmount);
            TimeSpan time = TimeSpan.FromSeconds(timeToEndBuild);

            int days;
            days = time.Days;
            if (days != 0)
            {
                timeToEndTxt.text = string.Format("{0:D2} day(s) {1:D2}:{2:D2}:{3:D2}",
                    time.Days,
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
            else
            {
                timeToEndTxt.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
        }
        else
        {
            OnEndResearch();
        }
    }

    public void StartTickDoc()
    {
        if (timeToEndBuild > 0)
        {
            timeToEndBuild--;         
            progress.fillAmount = 1f - timeToEndBuild / timeEndDoc; 
            TimeSpan time = TimeSpan.FromSeconds(timeToEndBuild);

            int days;
            days = time.Days;
            if (days != 0)
            {
                timeToEndTxt.text = string.Format("{0:D2} day(s) {1:D2}:{2:D2}:{3:D2}",
                    time.Days,
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
            else
            {
                timeToEndTxt.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
        }
        else
        {
            OnEndDoc();
        }
     
    }
    
    public void ShowTimeInDocQuene()
    {
        if (timeToEndBuild > 0)
        {
            TimeSpan time = TimeSpan.FromSeconds(timeToEndBuild);

            int days;
            days = time.Days;
            if (days != 0)
            {
                timeToEndTxt.text = string.Format("{0:D2} day(s) {1:D2}:{2:D2}:{3:D2}",
                    time.Days,
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
            else
            {
                timeToEndTxt.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }
        }
    }

    public void OnEndBuild()
    {
        MainResourcesController.buildingListIsFull = false;
        MainResourcesController.buildingInProgress.Clear();
        foreach(var mm in MainResourcesController.resDictionary)
        {
            if (mm.resId == queneId)
            {
                MainResourcesController.buildingsLevel[mm.resTechName] += 1;
            }
        }


        RefreshController.R_Buildings(parrentBuilding);

        CancelInvoke("StartTickBuildings");

        Destroy(this.gameObject);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void OnEndResearch()
    {
        MainResourcesController.researchListIsFull = false;
        MainResourcesController.researchInProgress.Clear();
        foreach (var mm in MainResourcesController.resDictionary)
        {
            if (mm.resId == queneId)
            {
                MainResourcesController.technologyLevel[mm.resTechName] += 1;
            }
        }

        MainResourcesController.UpdateResearch(queneId, parrentBuilding);

        CancelInvoke("StartTickResearch");

        Destroy(this.gameObject);
        gameObject.transform.parent.gameObject.SetActive(false);

    }

    DocOnEnabled doe;
    public void OnEndDoc()
    {
        parrentDef = doe.defPanel;
        parrentDoc = doe.docPanel;

        RefreshController.R_Defc(parrentDef);
        RefreshController.R_Doc(parrentDoc);

        CancelInvoke("StartTickDoc");

        Destroy(this.gameObject);

        if (MainResourcesController.docDefInProgress.Count > 1)
        {
            if (GameObject.Find("DocDef").gameObject.activeInHierarchy)
            {
                doe = GameObject.Find("DocDef").GetComponent<DocOnEnabled>();
                doe.StartThis();
            }
        }               
    }
}
