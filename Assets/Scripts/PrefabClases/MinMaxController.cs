using UnityEngine;
using System.Collections;

public class MinMaxController : MonoBehaviour
{
    public int min;
    public int max;
    private ScreenManager screenManager;
    BlockController blc;

    public enum WhatInput
    {
        Galactic,
        System,
        Planet,
        DocDef
    };

    public WhatInput whatInputName;

    void OnEnable()
    {
        switch (whatInputName)
        {
            case WhatInput.Galactic:
                min = 1;
                max = MainResourcesController.max_galaxy;
                break;
            case WhatInput.System:
                min = 1;
                max = MainResourcesController.max_y;
                break;
            case WhatInput.DocDef:
                min = 1;
                max = 99;
                break;
            default:
                min = 1;
                max = 1;
                break;
        }

        SliderController.minCount = min;
        SliderController.maxCount = max;
    }
}
