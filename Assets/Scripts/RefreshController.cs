using UnityEngine;
using System.Collections;
using SimpleJSON;

public class RefreshController : MonoBehaviour
{
    //Refresh Buildings Blocks START
    public static void R_Buildings(GameObject _buildingsBlocks)
    {
        foreach (var mm in _buildingsBlocks.GetComponentsInChildren<BlockController>(true))
        {
            mm.SetBlockLevel(mm.blockId, ParseAll.WhatBlockParse.Builings);
            mm.SetTitle();
            mm.SetPrices();
            mm.SetTimeToBuild();
            mm.ClearRequairments();
            mm.SetRequairments(LoginController.xmlDoc.text, ParseAll.WhatBlockParse.Builings);

            if (mm.blockId == 45)
                mm.SetCapitolium();
        }
    }

    public static void R_BuildingsInProgress(GameObject _parentInProgress)
    {
        if (MainResourcesController.buildingInProgress.Count != 0)
        {

        }
    }
    //Refresh Buildings Blocks END

    //Refresh Research Blocks START
    public static void R_Research(GameObject _researchBlocks)
    {
        foreach (var mm in _researchBlocks.transform.GetComponentsInChildren<BlockController>(true))
        {
            mm.SetBlockLevel(mm.blockId, ParseAll.WhatBlockParse.Research);
            mm.SetTitle();
            mm.SetPrices();
            mm.SetTimeToBuild();
            mm.ClearRequairments();
            mm.SetRequairments(LoginController.xmlDoc.text, ParseAll.WhatBlockParse.Research);
        }
    }

    public static void R_ResearchInProgress(GameObject _parentInProgress)
    {
        if (MainResourcesController.researchInProgress.Count != 0)
        {

        }
    }
    //Refresh Research Blocks END

    //Refresh Doc/Def Blocks START
    public static void R_Doc(GameObject _docBlocks)
    {
        foreach (var mm in _docBlocks.transform.GetComponentsInChildren<BlockController>(true))
        {
            mm.SetBlockLevel(mm.blockId, ParseAll.WhatBlockParse.Weapon);
            // mm.SetTitle();
            mm.SetPrices();
            mm.SetTimeToBuild();
            mm.ClearRequairments();
            mm.SetRequairments(LoginController.xmlDoc.text, ParseAll.WhatBlockParse.Weapon);
        }
    }

    public static void R_Defc(GameObject _defBlocks)
    {
        foreach (var mm in _defBlocks.transform.GetComponentsInChildren<BlockController>(true))
        {
            mm.SetBlockLevel(mm.blockId, ParseAll.WhatBlockParse.Defence);
            // mm.SetTitle();
            mm.SetPrices();
            mm.SetTimeToBuild();
            mm.ClearRequairments();
            mm.SetRequairments(LoginController.xmlDoc.text, ParseAll.WhatBlockParse.Defence);
        }
    }

    public static void R_DocDefInProgress(GameObject _parentInProgress)
    {

    }
    //Refresh Doc/Def Blocks END

    //Refresh Tech Blocks START
    public static void R_Technology(GameObject _techBlocks)
    {
        foreach (var mm in _techBlocks.transform.GetComponentsInChildren<BlockController>())
        {
            mm.SetBlockLevel(mm.blockId, ParseAll.WhatBlockParse.Technology);
            mm.SetTitle();
            mm.ClearRequairments();
            mm.SetRequairments(LoginController.xmlDoc.text, ParseAll.WhatBlockParse.Technology);
        }
    }
}