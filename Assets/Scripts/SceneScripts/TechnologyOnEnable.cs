using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TechnologyOnEnable : MonoBehaviour
{
    public GameObject technologyBlocks;
    public GameObject ScrManager;
    ScreenManager scrnM;
    VerticalLayoutGroup vlG;
    ContentSizeFitter csF;

    public GameObject loadingPanel;

    void OnEnable()
    {
        loadingPanel.SetActive(true);
        scrnM = ScrManager.GetComponent<ScreenManager>();
        StartCoroutine(GetRefresh());
    }

    IEnumerator GetRefresh()
    {
        foreach (var mm in technologyBlocks.transform.GetComponentsInChildren<BlockController>())
        {
            StartCoroutine(RefreshBlock(mm));
        }

        yield return new WaitForSeconds(1f);
        scrnM.ShowCurrentPanel();
        loadingPanel.SetActive(false);
        vlG = technologyBlocks.transform.GetComponent<VerticalLayoutGroup>();
        csF = technologyBlocks.transform.GetComponent<ContentSizeFitter>();
        vlG.enabled = false;
        csF.enabled = false;
    }

    IEnumerator RefreshBlock(BlockController _blc)
    {
        _blc.SetBlockLevel(_blc.blockId, ParseAll.WhatBlockParse.Technology);
        _blc.SetTitle();
        _blc.ClearRequairments();
        _blc.SetRequairments(LoginController.xmlDoc.text, ParseAll.WhatBlockParse.Technology);

        yield return null;
    }
}
