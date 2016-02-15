using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceAddController : MonoBehaviour 
{
    public Text titan_current_text, titan_max_text;
    public Text titan_hour;
    public Text silicon_current_text, silicon_max_text;
    public Text silicon_hour;
    public Text antimater_current_text, antimater_max_text;

    Root_helper_headers rhh;
    [Header("Resourses info")]
    public Text nameTitle;
    public Text textField;
    public GameObject popUp;
    public void ButtonQ()
    {
        popUp.SetActive(true);
        rhh = Root_helper_headers.LoadFromText(MainResourcesController.xmlDoc.text);
        nameTitle.text = rhh.helpHeaders.resoucePage.helpHeadersTitle;
        textField.text = rhh.helpHeaders.resoucePage.helpHeadersDesc;
    }
    public void TransportQ()
    {
        popUp.SetActive(true);
        rhh = Root_helper_headers.LoadFromText(MainResourcesController.xmlDoc.text);
        nameTitle.text = rhh.helpHeaders.transportScore.helpHeadersTitle;
        textField.text = rhh.helpHeaders.transportScore.helpHeadersDesc;
    }


    public void ValOnPlace()
    {
        titan_current_text.text = MainResourcesController.main_titan_value.ToString("F3");
        titan_max_text.text = MainResourcesController.titan_max.ToString("F3");

        silicon_current_text.text = MainResourcesController.main_silicone_value.ToString("F3");
        silicon_max_text.text = MainResourcesController.silicon_max.ToString("F3");

        antimater_current_text.text = MainResourcesController.main_antimatter_value.ToString("F3");
        antimater_max_text.text = MainResourcesController.antimatter_max.ToString("F3");

        titan_hour.text = MainResourcesController.titan_perhour.ToString("F2") + "/час";
        silicon_hour.text = MainResourcesController.silicon_perhour.ToString("F2") + "/час";
    }

    void Start()
    {
        ValOnPlace();
        InvokeRepeating("TickRes", 1f, 1f);
    }

    public void TickRes()
    {
        titan_current_text.text = MainResourcesController.main_titan_value.ToString("F3");
        silicon_current_text.text = MainResourcesController.main_silicone_value.ToString("F3");
        antimater_current_text.text = MainResourcesController.main_antimatter_value.ToString("F3");
    }
}
