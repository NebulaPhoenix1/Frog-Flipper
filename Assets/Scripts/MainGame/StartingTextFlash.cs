using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class StartingTextFlash : MonoBehaviour
{
    private TMP_Text hintText;
    private bool flash = true;
    private bool increaseAlpha = false;

    private InputAction chargeJumpAction; //New input action for jump (InputSystem/Player/Jump)
    [SerializeField] float fadeRate = 0.003f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hintText = GetComponent<TMP_Text>();
        chargeJumpAction = InputSystem.actions.FindAction("Jump"); //Get action for jump (left mouse)
    }

    // Update is called once per frame
    void Update()
    {
        if(chargeJumpAction.IsPressed()) { flash = false; hintText.enabled = false; } //Stop flashing when jump is pressed

        if(flash)
        {
            if(increaseAlpha)
            {
                hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, hintText.color.a + fadeRate);
                if(hintText.color.a >= 1f) { increaseAlpha = false; }
            }
            else
            {
                hintText.color = new Color(hintText.color.r, hintText.color.g, hintText.color.b, hintText.color.a - fadeRate);
                if(hintText.color.a <= 0f) { increaseAlpha = true; }
            }
        }
    }
}
