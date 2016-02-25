using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System;

public class PlanetMenuItemController : MonoBehaviour
{
    JSONNode result;
    JSONNode current_user;
    JSONNode current_planet;

    public Text mainName;
    public Text timeNow;
    public Text planetsCount;
    public Text diametr;
    public Text usedSectors;
    public Text temp;
    public Text coordinates;
    public Text clushTitan;
    public Text clushSilicone;
    public Text timeToJump;

    public InputField renamePlanetInput;
    string oldNamePlanet;
    string newNamePlanet; 

    public GameObject parrentColony;
    public GameObject colonyDetailPrefab;
    public GameObject renamePanel;

    public double timeNowFloat;
    public Image planetAva;


    Root_helper_headers rhh;
    [Header("Planet info")]
    public Text nameTitle;
    public Text textField;
    public GameObject popUp;
    
    public void ButtonQ()
    {
        popUp.SetActive(true);
        rhh = Root_helper_headers.LoadFromText(MainResourcesController.xmlDoc.text);
        nameTitle.text = rhh.helpHeaders.hhh.helpHeadersTitle;
        textField.text = rhh.helpHeaders.hhh.helpHeadersDesc;
    }

    void OnEnable()
    {        
        DestroyPlanet();      
    }

    public void DestroyPlanet()
    {
        if (parrentColony.transform.FindChild("ColonyDetail(Clone)"))
        {
            foreach (var mm in parrentColony.transform.GetComponentsInChildren<ColonyDetail>())
            {
                Destroy(mm.gameObject);
            }
        }

        StartCoroutine(GetJson());
    }

    public void ChangePlanet()
    {
        if (parrentColony.transform.FindChild("ColonyDetail(Clone)"))
        {
            foreach (var mm in parrentColony.transform.GetComponentsInChildren<ColonyDetail>())
            {
                Destroy(mm.gameObject);
            }
        }

        StartCoroutine(GetJson());
    }

    IEnumerator GetJson()
    {
        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=planet&method=getplanet&token="
           + MainResourcesController.userToken
           + "&system_id="
           + MainResourcesController.currentSystemID);
        yield return www;

        if (www.error == null)
        {
            result = JSON.Parse(www.text);

            MainResourcesController.timeNow = result["now"].AsLong;

            current_user = result["user"];
            current_planet = result["system"];

            mainName.text = current_planet["system_name"].Value;

            timeNowFloat = result["now"].AsFloat;

            planetsCount.text = MainResourcesController.planetsCountInGalaxy.ToString();
            diametr.text = string.Format("{0:F1}", ((Mathf.Pow(current_planet["field_max"].AsFloat, 14f / 1.5f) * 75f) / 1000));
            usedSectors.text = current_planet["field_current"].Value + " из " + current_planet["field_max"].Value + " занято";
            temp.text = current_planet["temp"].Value + " °С / " + (current_planet["temp"].AsInt + 40) + " °С";
            coordinates.text = "["+current_planet["galaxy"].Value +":" + current_planet["x"].Value + ":" + current_planet["y"].Value + "]";
            clushTitan.text = current_planet["crush_titan"].Value;
            clushSilicone.text = current_planet["crush_silicon"].Value;
            timeToJump.text = current_planet["last_jump_time"].Value;
            planetAva.sprite = GetAvatar(current_planet["image_id"].AsInt);

            foreach (var nn in MainResourcesController.systemList)
            {
                if (MainResourcesController.currentSystemID != nn.sys_id)
                {
                    Text name = colonyDetailPrefab.transform.FindChild("ColonyInfo").FindChild("Info").FindChild("Name_text").GetComponent<Text>();
                    Text cordinatesInPlanet = colonyDetailPrefab.transform.FindChild("ColonyInfo").FindChild("Info").FindChild("Value_text").GetComponent<Text>();

                    name.text = nn.sys_name;
                    cordinatesInPlanet.text = "[" + nn.x + ":" + nn.y + "]";

                    GameObject go = Instantiate(colonyDetailPrefab) as GameObject;
                    go.transform.SetParent(parrentColony.transform);
                    go.transform.localScale = new Vector3(1, 1, 1);
                }
            }

            //InvokeRepeating("RepeatTick", 1f, 1.1f);
        }
        else
        {
            Debug.Log("Problem from internet coonections");
        }
    }

    float time = 1.1f;
    void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            RepeatTick();
            time = 1.1f;
        }
    }

    void RepeatTick()
    {
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dt = dt.AddSeconds(timeNowFloat).ToLocalTime();

        timeNow.text = string.Format("{0:D2}-{1:D2}-{2:D2}, {3:D2}:{4:D2}:{5:D2}",
            dt.Day,
            dt.Month,
            dt.Year,
            dt.Hour,
            dt.Minute,
            dt.Second);

        timeNowFloat++;
    }

    Sprite GetAvatar(int _id)
    {
        Sprite spr;
        spr = Resources.Load<Sprite>("planetAvatars/" + _id.ToString());
        return spr;
    }

    public void ShowRenamePanel()
    {
        renamePanel.SetActive(true);
    }

    public void CloseRenamePanel()
    {
        renamePanel.SetActive(false);
    }

    private bool isInputActive = false;
    private InputField currentInpf;
    public void InputSelectRenemaPlanet(InputField _inpf)
    {
        _inpf.transform.FindChild("Text").GetComponent<Text>().color = green;
        _inpf.GetComponent<Image>().sprite = defaultLogin_sprite;
        errorText_go.SetActive(false);

        _inpf.placeholder.gameObject.SetActive(false);

        // Go to LateUpdate
        isInputActive = true;
        currentInpf = _inpf;
    }

    public void InputDeselectRenemaPlanet(InputField _inpf)
    {
        _inpf.placeholder.gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if (isInputActive)
        {
            currentInpf.caretPosition = currentInpf.text.Length;
            isInputActive = false;
        }
    }

    public Text topBlockPlanetName;
    Color32 red = new Color32(233, 59, 78, 255);
    Color32 green = new Color32(112, 185, 32, 255);
    public Sprite errorLogin_sprite;
    public Sprite defaultLogin_sprite;
    public GameObject errorText_go;
    bool error = false;

    public void SetNewName()
    {
        newNamePlanet = renamePlanetInput.text;
        oldNamePlanet = MainResourcesController.currentSystemName;

        foreach (var mm in newNamePlanet)
        {
            if(mm == ' ')
            {
                error = true;
                break;    
            }
            else
            {
                error = false;
            }
        }

        if(!error)
        {
            renamePlanetInput.text = "";
            renamePlanetInput.DeactivateInputField();

            if (newNamePlanet != "")
            {
                StartCoroutine(SetNewNamePlanet());
            }
            else
            {
                errorText_go.SetActive(true);
                errorText_go.GetComponent<Text>().text = "Ошибка:\nИмя не может быть пустым.";
                renamePlanetInput.transform.FindChild("Text").GetComponent<Text>().color = red;
                renamePlanetInput.GetComponent<Image>().sprite = errorLogin_sprite;
            }
        }
        else
        {
            errorText_go.SetActive(true);
            errorText_go.GetComponent<Text>().text = "Ошибка:\nИмя не может содержать пробел.";
            renamePlanetInput.transform.FindChild("Text").GetComponent<Text>().color = red;
            renamePlanetInput.GetComponent<Image>().sprite = errorLogin_sprite;
        }
    }

    public GameObject loadingPanel;
    IEnumerator SetNewNamePlanet ()
    {
        loadingPanel.SetActive(true);
        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=planet&method=renameplanet&token="
        + MainResourcesController.userToken
        + "&system_id="
        + MainResourcesController.currentSystemID + "&planet_name=" + newNamePlanet);
        yield return www;

        if (www.error == null)
        {
            CloseRenamePanel();
            MainResourcesController.currentSystemName = newNamePlanet;
            foreach(var mm in MainResourcesController.systemList)
            {
                if (mm.sys_name == oldNamePlanet)
                {
                    mm.sys_name = newNamePlanet;
                }
            }

            mainName.text = newNamePlanet;
            topBlockPlanetName.text = newNamePlanet + " [" + 
                MainResourcesController.currentGalaxy + ":" + 
                MainResourcesController.currentSystemX + ":" + 
                MainResourcesController.currentSystemY + "]";

            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        loadingPanel.SetActive(false);
    }
}
