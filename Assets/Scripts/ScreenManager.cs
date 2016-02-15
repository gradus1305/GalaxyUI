using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScreenManager : MonoBehaviour
{
    // All Panels And LeftMenu Buttons START
    [HideInInspector]
    public GameObject currentPanel;
    public GameObject leftMenuAnim,TopBlockMenu,LoadingPanel;
    public Canvas canvas;
    public static bool Error404;
    bool isPaused;
    int currentLevel;
    public GameObject slider;
    public GameObject leftMenu;
    public GameObject login;

    void Start()
    {
        if (PlayerPrefs.HasKey("userLogin") && PlayerPrefs.HasKey("userPass"))
        {
            EventSystem.current.firstSelectedGameObject = leftMenu;
        }
        else
        {
            EventSystem.current.firstSelectedGameObject = login;
        }
        currentPanel = EventSystem.current.firstSelectedGameObject;
        currentPanel.SetActive(true);
        currentLevel = Application.loadedLevel;
    }

#if !UNITY_EDITOR
    void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {

        }
        else
        {
            StartCoroutine(Exit());
        }
    }
#endif

    AsyncOperation ext = null;
    IEnumerator Exit()
    {
        MainResourcesController.systemList.Clear();
        MainResourcesController.researchInProgress.Clear();
        MainResourcesController.buildingInProgress.Clear();
        MainResourcesController.docDefInProgress.Clear();
        MainResourcesController.buildingsLevel.Clear();
        MainResourcesController.technologyLevel.Clear();

        ext = Application.LoadLevelAsync("Main");
        yield return ext;
    }

    public void ShowPanel(GameObject go)
    {
        if (currentPanel != null)
            HideCurrentPanel();

        currentPanel = go;
        currentPanel.SetActive(true);
        //{
        //    StartCoroutine(ShowCurPanel());
        //}

    }
    public void ShowCurrentPanel()                     
    {
        StartCoroutine(ShowCurPanel());
    }

    public void ShowUpPanel(GameObject go)/*on this panel no loading yet*/
    {
        if (currentPanel != null)
            HideCurrentPanel();

        currentPanel = go;
        currentPanel.SetActive(true);
        {
            StartCoroutine(ShowCurPanel());
        }
    }

    IEnumerator ShowCurPanel()
    {
        //currentPanel.SetActive(true);
        yield return new WaitForEndOfFrame();
        currentPanel.GetComponent<Animation>().Play("Panel_Show");
        leftMenuAnim.GetComponent<Animation>().Play("LeftMenu_Hide");
        TopBlockMenu.GetComponent<Animation>().Play("TopBlock_Show");
    }
    
    public void HideCurrentPanel()
    {
        StartCoroutine(HideCurPanel());
    }
    IEnumerator HideCurPanel()
    {
        currentPanel.SetActive(false);
        yield return null;
    }

    public void MainMenu_Btn()
    {
        if(leftMenuAnim.transform.localPosition.x == -540)
        {
            currentPanel.GetComponent<Animation>().Play("Panel_Show");
            leftMenuAnim.GetComponent<Animation>().Play("LeftMenu_Hide");
            TopBlockMenu.GetComponent<Animation>().Play("TopBlock_Show");
        }
        else
        {
            currentPanel.GetComponent<Animation>().Play("Panel_Hide");
            leftMenuAnim.GetComponent<Animation>().Play("LeftMenu_Show");
            TopBlockMenu.GetComponent<Animation>().Play("TopBlock_Hide");
            GameObject.Find("LoginRegisterController").GetComponent<LoginController>().DestroyColonyPrefabs();
            slider.SetActive(false);           
            MessagePanel.SetActive(false);
            DropDownPanels.SetActive(false);
            currentPanel.SetActive(true);
        }
    }
    // All Panels And LeftMenu Buttons END

    //Top Block Messages Buttons START

    public GameObject MessagePanel;

    public void Message_Btn(GameObject MessagePanel)
    {
        if (!MessagePanel.activeInHierarchy)
        {
            MessagePanel.SetActive(true);
            currentPanel.SetActive(false);
        }
        else
        {
            MessagePanel.SetActive(false);
            currentPanel.SetActive(true);
        }
    }

    public GameObject MessagPopUp;
    public Text MsgPopUp_date, MsgPopUp_title, MsgPopUp_message;

    public void SomeMessage_click(GameObject MainSomeMessageGO)
    {
        MessagPopUp.SetActive(true);
        MsgPopUp_date.text = MainSomeMessageGO.GetComponentsInChildren<Text>()[1].text;
        MsgPopUp_title.text = MainSomeMessageGO.GetComponentsInChildren<Text>()[0].text;
        MsgPopUp_message.text = MainSomeMessageGO.GetComponentsInChildren<Text>()[2].text; // message type but must message text (xml or json)
    }

    public void MessagPopUp_close()
    {
        MessagPopUp.SetActive(false);
        Error404 = true;
    }

    List<GameObject> messageSelected = new List<GameObject>();
    bool toogle;

    public void MessageToogleChange(Toggle tog)
    {
        if(tog.isOn)
        {
            toogle = true;
        }
        else
        {
            toogle = false;
        }
    }

    public void MessageToogleAddToList(GameObject ss)
    {
        if (toogle)
            messageSelected.Add(ss);
        else
            messageSelected.Remove(ss);
    }

    public void DeleteMessage()
    {
        foreach(GameObject go in messageSelected)
        {
            go.SetActive(false);
        }
    }

    GameObject[] del;
    public void DeleteAllMessage()
    {
        del = GameObject.FindGameObjectsWithTag("Message");
        foreach(GameObject go in del)
        {
            go.SetActive(false);
        }
        messageSelected.Clear();
    }
    //Top Block Messages Buttons END

    //Drop Down Menu (Planest and Colony) START
    public GameObject DropDownPanels;

    public void DropDown_btn(GameObject DropDownPanel)
    {
        if(!DropDownPanel.activeInHierarchy)
            DropDownPanel.SetActive(true);
        else
            DropDownPanel.SetActive(false);
    }

    public void DropDown_closeBtn(GameObject DropDownPanel)
    {
        DropDownPanel.SetActive(false);
    }

    //Drop Down Menu (Planest and Colony) END

    //Building Buttons START

    public void BuildingUpGrade_btn(GameObject buildingsPopUp)
    {
        buildingsPopUp.SetActive(true);
    }

    public void BuildingPopUp_close(GameObject buildingsPopUp)
    {
        buildingsPopUp.SetActive(false);
    }

    //Building Buttons END

    //Research Buttons START
    public void ResearchUpGrade_btn(GameObject ResearchPopUp)
    {
        ResearchPopUp.SetActive(true);
    }

    public void ResearchPopUp_close(GameObject ResearchPopUp)
    {
        ResearchPopUp.SetActive(false);
    }

    public void ResearchPopUp_UpGrade(GameObject ResearchPopUp)
    {
        ResearchPopUp.SetActive(false);
        //Show loading and send Json
        StartCoroutine(UpGradeResearch());
    }

    IEnumerator UpGradeResearch()
    {
        LoadingPanel.SetActive(true);
        //canvas.pixelPerfect = false;
        yield return new WaitForSeconds(2f);
        LoadingPanel.SetActive(false);
        //canvas.pixelPerfect = true;
    }
    //Research Buttons END

    //PlusAndMinus Buttons START
    int minValue = 0, maxValue = 100,countValue;

    public void MaxValues(int max)
    {
        maxValue = max;
    }

    public void PlusBtn(Text value)
    {
        countValue = ParseStr(value.text);

        if (countValue < maxValue)
        {
            countValue++;
            if(addPrecent)
                value.text = countValue.ToString() + "%";
            else
                value.text = countValue.ToString();
        }
    }

    public void MinusBtn(Text value)
    {
        countValue = ParseStr(value.text);

        if (countValue > minValue)
        {
            countValue--;
            if (addPrecent)
                value.text = countValue.ToString() + "%";
            else
                value.text = countValue.ToString();
        }
    }

    bool addPrecent;
    int ParseStr(string str)
    {
        int value;
        if (!int.TryParse(str, out value))
        {
            str = str.Remove(str.Length - 1);
            value = int.Parse(str);
            addPrecent = true;
        }
        else
        {
            addPrecent = false;
        }

        return value;
    }
    //PlusAndMinus Buttons END

    //Bank Buttons START

    public void BuySomething(GameObject errorWindow)
    {
        errorWindow.SetActive(true);
    }

    public void ErrorWindow_close(GameObject errorWindow)
    {
        errorWindow.SetActive(false);
    }

    //Bank Buttons END

    // Doc and Deffence Buttons START

    List<GameObject> queneToBuild = new List<GameObject>();

    string TimeToBuild(string[] str)
    {
        int hour, minutes, seconds;
        hour = int.Parse(str[0]);
        minutes = int.Parse(str[1]);
        seconds = int.Parse(str[2]);

        seconds = seconds * countValue;
        minutes = minutes * countValue;
        hour = hour * countValue;

        if(seconds / 60 >= 1)
        {
            minutes += seconds / 60;
            seconds = seconds % 60;
        }

        if(minutes / 60 >= 1)
        {
            hour += minutes / 60;
            minutes = minutes % 60;
        }

        return hour.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");

    }

    public void DocAndDef_Build_btn(Text time)
    {
        string str = TimeToBuild(time.text.Split(':'));
    }

    // Doc and Deffence Buttons END

    //Input valueschange

    public void ValueChange(GameObject inputField)
    {
        InputField inp = inputField.GetComponent<InputField>();
        Text value_txt = inputField.GetComponentInChildren<Text>();

        value_txt.text = inp.text;
    }

    //Input valueschange
}