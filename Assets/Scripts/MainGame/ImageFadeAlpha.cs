using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ImageFadeAlpha : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image image;
    [SerializeField] private float fadeSpeed = 0.01f; //Speed of fade
    [SerializeField] private int targetAlpha; //Target alpha value to fade to
    private float targetAlphaFloat; //Target alpha value as float   
    public UnityEvent onFadeComplete;

    private bool isFading = false;
    void Start()
    {
        image = GetComponent<Image>();
        targetAlphaFloat = (float)targetAlpha / 255.0f; //Convert target alpha to float 0-1
    }

    //Function is called with unity events to fade image alpha
    public void TriggerFadeAlpha()
    {
        Debug.Log("Triggering fade to alpha: " + targetAlpha);
        isFading = true;
    }

   
    void Update()
    {
        if (isFading)
        {
            // Check if the alpha has reached the target yet
            if (image.color.a < targetAlphaFloat)
            {
                Color color = image.color;
                // Use MoveTowards for a linear, frame-rate independent fade
                //(lerp isnt actually moving the value towards target, it moves it a percentage of the way there)
                color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
                image.color = color;
            }
            else
            {
                // The fade is complete
                isFading = false;
                Debug.Log("Fade complete");
                //SceneManager.LoadScene("Scenes/Main game");
                onFadeComplete.Invoke();
            }
        }
    }
}
