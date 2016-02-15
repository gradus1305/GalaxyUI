using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFps : MonoBehaviour 
{
    public Text label;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (label != null)
        {
            int r = Mathf.FloorToInt(1 / Time.deltaTime);
            label.text = r.ToString("N0");
        }
	}
}
