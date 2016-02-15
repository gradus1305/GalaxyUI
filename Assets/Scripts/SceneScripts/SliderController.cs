using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SliderController : MonoBehaviour 
{
    public int count;
    public static int minCount;
    public static int maxCount;
    public Slider SliderForDocDef;
    public Text valueShow;

    public GameObject slider;
    public InputField fieldInSlider;
    int countInSlider;
    bool fieldActive;
    public Text GalaxyCount;
    public Text SystemCount;

    void Start()
    {
        EventTrigger evTr = fieldInSlider.GetComponent<EventTrigger>();
        EventTrigger.Entry trEntry = new EventTrigger.Entry();
        trEntry.eventID = EventTriggerType.Select;
        trEntry.callback.AddListener((eventData) => { OnSliderInput(); });
        EventTrigger.Entry tr = new EventTrigger.Entry();
        tr.eventID = EventTriggerType.Deselect;
        tr.callback.AddListener((eventData) => { SliderActive(); });
        evTr.triggers.Add(trEntry);
        evTr.triggers.Add(tr);

    }   

    public void ConfirmBtn()
    {
        slider.SetActive(false);
        
        if (ParseAll.textInGalaxy == "Galactic")
        {
            GalaxyCount.text = count.ToString();
        }
        if (ParseAll.textInGalaxy == "System")
        {
            SystemCount.text = count.ToString();
        }
        if (ParseAll.panelName == "MyDoc" || ParseAll.panelName == "MyDefence")
        {
         GameObject.Find(ParseAll.nameBlock).transform.FindChild("DownBar").FindChild("InputField").FindChild("Text").GetComponent<Text>().text = count.ToString();
        }
       
    }

	void OnEnable()
    {
        SliderForDocDef.minValue = minCount;
        SliderForDocDef.maxValue = maxCount;
        fieldActive = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!fieldActive)
        {
            count = (int)SliderForDocDef.value;
            valueShow.text = count.ToString();
            ShowCount();
        }
        else
        {
            countInSlider = Convert.ToInt32(gameObject.transform.FindChild("ValueField").FindChild("ValueText").GetComponent<Text>().text);
            if (countInSlider < SliderForDocDef.maxValue)
            {
                SliderForDocDef.value = countInSlider;
                count = countInSlider;
            }
        }
	}

    public void ShowCount()
    {
        fieldInSlider.text = count.ToString();       
    }
    public void OnSliderInput()
    {
        fieldActive = true;
    }
    public void SliderActive()
    {
        fieldActive = false;
    }
}
