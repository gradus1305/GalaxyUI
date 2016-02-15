using UnityEngine;
using UnityEngine.UI;

public class FleetOnEnable : MonoBehaviour {

    Root_helper_headers rhh;
    [Header("Building info")]
    public Text nameTitle;
    public Text textField;
    public GameObject popUp;

    public void ButtonQ()
    {
        popUp.SetActive(true);
        rhh = Root_helper_headers.LoadFromText(MainResourcesController.xmlDoc.text);
        nameTitle.text = rhh.helpHeaders.flletSend.helpHeadersTitle;
        textField.text = rhh.helpHeaders.flletSend.helpHeadersDesc;
    }
}
