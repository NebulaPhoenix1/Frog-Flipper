using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class UpdateChargeUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Image fillImage;
    private ChargeJump chargeJump;
    private Camera camera;
    private Transform playerTransform;
    private Vector2 inputPosition;
    private float minDisplayDistance;
    private float maxDisplayDistance;

    [SerializeField] private float minDisplayDistancePercent = 0.1f;
    [SerializeField] private float maxDisplayDistancePercent = 0.3f;

    /* Chages fill amount based on how charged the jump is
    Also, the UI element follows the player in a circle dependent on how the angle between player and mouse/finger is
    */

    void Start()
    {
        fillImage = GetComponent<Image>();
        chargeJump = FindFirstObjectByType<ChargeJump>();
        camera = Camera.main;
        playerTransform = chargeJump.transform;
        //Calculate min and max display distance based on screen size
        float shortestSide = Mathf.Min(Screen.width, Screen.height);
        minDisplayDistance = shortestSide * minDisplayDistancePercent;
        maxDisplayDistance = shortestSide * maxDisplayDistancePercent;
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeJump.GetNormalizedCharge() >= 0f)
        {
            //Check if touch input
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                //transform.position = Touchscreen.current.primaryTouch.position.ReadValue();
                inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            else if (Touchscreen.current != null) //If touchscreen and not pressing, hide charge UI
            {
                fillImage.enabled = false;
                return;
            }
            else
            {
                //transform.position = Mouse.current.position.ReadValue();
                inputPosition = Mouse.current.position.ReadValue();
            }
            Vector2 playerScreenPosition = camera.WorldToScreenPoint(playerTransform.position);
            Vector2 direction = inputPosition - playerScreenPosition;
            //Change position based on charge amount
            float currentDistance = Mathf.Lerp(minDisplayDistance, maxDisplayDistance, chargeJump.GetNormalizedCharge());
            Vector2 targetPosition = playerScreenPosition + (direction.normalized * currentDistance);
            transform.position = targetPosition;
            //transform.position = Mouse.current.position.ReadValue();

            fillImage.enabled = true;
            fillImage.fillAmount = chargeJump.GetNormalizedCharge();
        }
        else { fillImage.enabled = false; }
    }
}
