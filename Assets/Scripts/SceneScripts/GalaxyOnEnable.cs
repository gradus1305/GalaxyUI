using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class GalaxyOnEnable : MonoBehaviour
{
    public int galaxyID;
    public int systemID;
    public string jsonQuery = "http://vg2.v-galaktike.ru/api/?class=planet&method=getmap";
    public GameObject prefab;
    public Transform parentPrefab;
    public Text planetName;
    public Text playerName;
    public Image planetAvatar;

    public Text galaxyInput;
    public Text systemInput;

    private Button flotSendBtn;
    public GameObject fleetSendPanel;

    public GameObject loadingPanel;

    void Start()
    {
        galaxyID = MainResourcesController.main_galaxy;
        galaxyInput.text = galaxyID.ToString();
        systemID = MainResourcesController.main_x;
        systemInput.text = systemID.ToString();

        jsonQuery += "&token=" + MainResourcesController.userToken;
        jsonQuery += "&galaxy=" + galaxyID;
        jsonQuery += "&x=" + systemID;

        StartCoroutine(GetJson());
    }

    public void GoToButton()
    {
        loadingPanel.SetActive(true);
        ClearPlanetList();

        galaxyID = int.Parse(galaxyInput.text);
        systemID = int.Parse(systemInput.text);

        jsonQuery += "&token=" + MainResourcesController.userToken;
        jsonQuery += "&galaxy=" + galaxyID;
        jsonQuery += "&x=" + systemID;

        StartCoroutine(GetJson());
    }

    public void HomeButton()
    {
        loadingPanel.SetActive(true);
        ClearPlanetList();

        galaxyID = MainResourcesController.main_galaxy;
        galaxyInput.text = galaxyID.ToString();
        systemID = MainResourcesController.main_x;
        systemInput.text = systemID.ToString();

        jsonQuery += "&token=" + MainResourcesController.userToken;
        jsonQuery += "&galaxy=" + galaxyID;
        jsonQuery += "&x=" + systemID;

        StartCoroutine(GetJson());
    }

    JSONArray system;
    IEnumerator GetJson()
    {
        WWW www = new WWW(jsonQuery);
        yield return www;

        if (www.error == null)
        {
            var result = JSON.Parse(www.text);
            system = result["system"].AsArray;
            GameObject go;

            for(int i = 1; i < 15; i++)
            {
                SetPlanet(i);
                go = Instantiate(prefab);
                go.transform.SetParent(parentPrefab);
                go.transform.localScale = new Vector3(1, 1, 1);

                flotSendBtn = go.transform.FindChild("AttackButton").GetComponent<Button>();
                flotSendBtn.onClick.AddListener(() => { SendFleet(); });
            }

        }

        yield return new WaitForSeconds(1f);
        loadingPanel.SetActive(false);
    }

    ScreenManager sM;
    public void SendFleet()
    {
        sM = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
        this.gameObject.SetActive(false);
        fleetSendPanel.transform.position = this.gameObject.transform.position;
        fleetSendPanel.SetActive(true);
        sM.currentPanel = GameObject.Find("FleetSending");
    }

    void SetPlanet(int id)
    {
        PlanetController pC = prefab.GetComponent<PlanetController>();
        foreach (JSONNode mm in system)
        {
            if (mm["y"].AsInt == id)
            {
                planetName.text = mm["system_name"].ToString();
                playerName.text = mm["nickname"].ToString();
                planetAvatar.sprite = GetAvatar(mm["system_id"].AsInt);

                pC.galaxy = mm["galaxy"].AsInt;
                pC.x = mm["x"].AsInt;
                pC.y = mm["y"].AsInt;
                break;
            }
            else
            {
                planetName.text = "Пусто";
                playerName.text = "Пусто";
                planetAvatar.sprite = GetAvatar(mm["system_id"].AsInt);

                pC.galaxy = mm["galaxy"].AsInt;
                pC.x = mm["x"].AsInt;
                pC.y = mm["y"].AsInt;
            }
        }
    }

    void ClearPlanetList()
    {
        foreach (var mm in parentPrefab.transform.GetComponentsInChildren<RectTransform>())
        {
            if (mm.name == "PlayerGalactic(Clone)")
                Destroy(mm.gameObject);
        }
    }

    Sprite GetAvatar(int _id)
    {
        Sprite spr;
        spr = Resources.Load<Sprite>("planetAvatars/" + _id.ToString());
        return spr;
    }
}
