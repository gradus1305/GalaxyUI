using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RatingOnEnabled : MonoBehaviour 
{
    [Header("Statistic ?btn")]
    public Text headerName;
    public Text textField;
    public GameObject popUp;
    Root_helper_headers rhh;

    [Header("Text fields")]
    public Text buildRang, buildRangPoints;
    public Text fleetRang, fleetRangPoints;
    public Text defRang, defRangPoints;
    public Text techRang, techRangPoints;
    public Text totalRang, totalRangPoints;
    public void ShowStatistic()
    {
        popUp.SetActive(true);
        rhh = Root_helper_headers.LoadFromText(MainResourcesController.xmlDoc.text);
        headerName.text = rhh.helpHeaders.statsPage.helpHeadersTitle;
        textField.text = rhh.helpHeaders.statsPage.helpHeadersDesc;
    }
    void Start()
    {
        ShowStatistic(MainResourcesController.build_old_rank, MainResourcesController.build_rank, MainResourcesController.build_points, buildRang, buildRangPoints);
        ShowStatistic(MainResourcesController.fleet_old_rank, MainResourcesController.fleet_rank, MainResourcesController.fleet_points, fleetRang, fleetRangPoints);
        ShowStatistic(MainResourcesController.defs_old_rank, MainResourcesController.defs_rank, MainResourcesController.defs_points, defRang, defRangPoints);
        ShowStatistic(MainResourcesController.tech_old_rank, MainResourcesController.tech_rank, MainResourcesController.tech_points, techRang, techRangPoints);
        ShowStatistic(MainResourcesController.total_old_rank, MainResourcesController.total_rank, MainResourcesController.total_points, totalRang, totalRangPoints);
    }
   
  
    public void ShowStatistic(int _oldRang, int _newRang, int _points, Text _rangField, Text _pointsField)
    {
        int diferent = _newRang - _oldRang;
        if (diferent > 0)
        {
            _rangField.text = _newRang.ToString() + "(<color=green>" +"+"+diferent + "</color> )";
        }   
        else if(diferent < 0)
        {
        _rangField.text = _newRang.ToString() + "(-<color=red>"+"-"+diferent+"</color> )";   
        }
        else
        {
            _rangField.text = _newRang.ToString() + "(0)";
        }
            
        _pointsField.text = _points.ToString();
    }
}
