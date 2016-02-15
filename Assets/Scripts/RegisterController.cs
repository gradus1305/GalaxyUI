using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System.Text.RegularExpressions;

public class RegisterController : MonoBehaviour 
{
    public GameObject registerPanel;
    public GameObject loginPanel;
    public GameObject loading;

    // Register planet START
    public void RegisterCancel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public InputField planet_name_input;
    public InputField nickname_input;
    public InputField planet_email_input;
    public InputField password_input;
    public InputField confim_password_input;

    string planet_name;
    string email;
    string password;
    string confirm_password;
    string nickname;

    public Sprite errorLogin_sprite;
    public Sprite defaultLogin_sprite;
    public GameObject errorText_go;
    Color32 red = new Color32(233, 59, 78, 255);
    Color32 green = new Color32(112, 185, 32, 255);

    public void RegisterConfirm()
    {
        planet_name = planet_name_input.text;
        nickname = nickname_input.text;
        email = planet_email_input.text;
        password = password_input.text;
        confirm_password = confim_password_input.text;

        if (planet_name != "" && email != "" && password != "" && confirm_password != "" && nickname != "")
        {
            if (password == confirm_password)
                StartCoroutine(RegisterUser());
            else
            {
                password_input.transform.FindChild("Text").GetComponent<Text>().color = red;
                password_input.GetComponent<Image>().sprite = errorLogin_sprite;
                confim_password_input.transform.FindChild("Text").GetComponent<Text>().color = red;
                confim_password_input.GetComponent<Image>().sprite = errorLogin_sprite;

                errorText_go.SetActive(true);
                errorText_go.GetComponent<Text>().text = "Ошибка:\nПароли не совпадают.";
            }
        }
        else
        {
            planet_name_input.transform.FindChild("Text").GetComponent<Text>().color = red;
            planet_name_input.GetComponent<Image>().sprite = errorLogin_sprite;
            nickname_input.transform.FindChild("Text").GetComponent<Text>().color = red;
            nickname_input.GetComponent<Image>().sprite = errorLogin_sprite;
            planet_email_input.transform.FindChild("Text").GetComponent<Text>().color = red;
            planet_email_input.GetComponent<Image>().sprite = errorLogin_sprite;
            password_input.transform.FindChild("Text").GetComponent<Text>().color = red;
            password_input.GetComponent<Image>().sprite = errorLogin_sprite;
            confim_password_input.transform.FindChild("Text").GetComponent<Text>().color = red;
            confim_password_input.GetComponent<Image>().sprite = errorLogin_sprite;

            errorText_go.SetActive(true);
            errorText_go.GetComponent<Text>().text = "Ошибка:\nЗаполните все поля.";
        }
    }

    private bool isInputActive = false;
    private InputField currentInpf;
    public void InputFieldSelectedRegister(InputField _inpf)
    {
        _inpf.transform.FindChild("Text").GetComponent<Text>().color = green;
        _inpf.GetComponent<Image>().sprite = defaultLogin_sprite;
        errorText_go.SetActive(false);

        _inpf.placeholder.gameObject.SetActive(false);

        // Go to LateUpdate
        isInputActive = true;
        currentInpf = _inpf;
    }

    public void InputFieldDeselectRegister(InputField _inpf)
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

    void Start()
    {
        planet_email_input.keyboardType = TouchScreenKeyboardType.EmailAddress;
    }

    /*void LockInput(int i, InputField input, bool Good)
    {
        
        if (input.text.Length >= i)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(arg0);
            if (match.Success)
            {
                email = arg0;
                Debug.Log(email);
            }
            else
                Debug.Log("Incorrect email adress");
        }
        else
        {
            Debug.Log("Emain field is empty");
        }
    }*/

    IEnumerator RegisterUser()
    {
        WWW www = new WWW("http://vg2.v-galaktike.ru/api/?class=user&method=registeruser&email="+email+"&password="+password+"&planet_name="+planet_name+"&nickname="+nickname);
        yield return www;

        if(www.error == null)
        {
            var result = JSON.Parse(www.text);

            if(result["error"].Value == "")
            {
                registerPanel.SetActive(false);
                loginPanel.SetActive(true);
                Debug.Log("good register"); // show login form
            }
            else
            {
                planet_email_input.transform.FindChild("Text").GetComponent<Text>().color = red;
                planet_email_input.GetComponent<Image>().sprite = errorLogin_sprite;

                errorText_go.SetActive(true);
                errorText_go.GetComponent<Text>().text = "Ошибка:\nТакой емайл адрес уже зарегистрирован.";
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    // Register planel END
}
