using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlatformSpecificTextUI : MonoBehaviour
{
    /* Check what platform we are playing on and set text component to correct instructions */
    [SerializeField] private string mouseAndKeyboardText;
    [SerializeField] private string touchInputText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            //Touch input
            GetComponent<TextMeshProUGUI>().text = touchInputText;
        }
        else
        {
            //Mouse and keyboard input
            GetComponent<TextMeshProUGUI>().text = mouseAndKeyboardText;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
