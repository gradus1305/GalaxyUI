using UnityEngine;
using System.Collections;

public class DefOnEnabled : MonoBehaviour
{
    public GameObject defError;
    public GameObject defGood;

    void OnEnable()
    {
        if (MainResourcesController.hangar != 0)
        {
           StartCoroutine(GetJson());
           defGood.SetActive(true);
           defError.SetActive(false);
        }
        else
        {
            defGood.SetActive(false);
            defError.SetActive(true);
        }
        
    }

    public void Refresh()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (MainResourcesController.hangar != 0)
            {
                defError.SetActive(false);
                defGood.SetActive(true);
                
            }
            else
            {
                defError.SetActive(true);
                defGood.SetActive(false);
                
            }
        }
    }
    IEnumerator GetJson()
    {
        yield return null;
    }
}
