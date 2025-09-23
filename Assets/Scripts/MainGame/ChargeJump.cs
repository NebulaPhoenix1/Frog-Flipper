using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ChargeJump : MonoBehaviour
{
    public UnityEvent finishJump;

    [SerializeField] private bool touchInputTesting;
    [SerializeField] private Camera mainCamera;
    private InputAction chargeJumpAction; //New input action for jump (InputSystem/Player/Jump)
    [SerializeField] private float maxChargeTime = 2.0f; 
    [SerializeField] private float minJumpForce = 5.0f;
    [SerializeField] private float maxJumpForce = 20.0f;
    [SerializeField] private float jumpStopDelay = 0.1f; //Delay before jump force stops being applied
    [SerializeField] private float cameraPlayerYOffset = 4.0f; //Camera offset on Y axis when following player
    [SerializeField] private float cameraLerpSpeed = 0.01f;
    private float currentChargeTime = 0.0f; //How long current jump has been charged
    private Rigidbody2D rb;
    private bool cameraFollow = false;

    private Vector2 mousePosition;
    private Vector2 playerScreenPosition;


    void Start()
    {
        chargeJumpAction = InputSystem.actions.FindAction("Jump"); //Get action for jump (left mouse)
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(touchInputTesting)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                mousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
                Debug.Log("Touchscreen detected, touched pos: " + mousePosition);
            }
        }
        else
        {
            mousePosition = Mouse.current.position.ReadValue(); //Screen space position of mouse
            Debug.Log("Mouse detected, mouse pos: " + mousePosition);
        }
        /*if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            mousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Debug.Log("Touchscreen detected, touched pos: " + mousePosition);
        }
        else
        {
            mousePosition = Mouse.current.position.ReadValue(); //Screen space position of mouse
            Debug.Log("Mouse detected, mouse pos: " + mousePosition);

        }*/
        playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position); //Screen space position of player
        //Debug.Log("Mouse: " + mousePosition + " Player: " + playerScreenPosition);

        if (chargeJumpAction.IsPressed() && !cameraFollow) //Charging Jump while camera not following
        {
            //Debug.Log("Charging");
            if (currentChargeTime < maxChargeTime)
            {
                currentChargeTime += Time.deltaTime; //Increase charge time
            }
            else
            {
                currentChargeTime = maxChargeTime; //Clamp to max charge time
            }
        }
        else if (chargeJumpAction.WasReleasedThisFrame()) //Jump Released
        {
            //Using trigonometry to calculate jump X direction based on where mouse is
            //We have Opposite (mouse Y - player Y) and adjacent (mouse X - player X)
            if (mousePosition.y < playerScreenPosition.y) //Constrain to only jump upwards
            {
                float temp = mousePosition.y;
                mousePosition.y = playerScreenPosition.y;
                playerScreenPosition.y = temp;
            }
            float opposite = mousePosition.y - playerScreenPosition.y;
            float adjacent = mousePosition.x - playerScreenPosition.x;
            float angleRadians = Mathf.Abs(Mathf.Atan2(opposite, adjacent)); //Angle in radians
            float angleDegrees =  Mathf.Abs(angleRadians * Mathf.Rad2Deg); //Convert to degrees if needed
            //Debug.Log("Angle: " + angleDegrees + " radians: " + angleRadians);

            Vector2 jumpDirection =new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));

            //Debug.Log("Released");
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, currentChargeTime / maxChargeTime); //Calculate jump force based on charge time
            rb.linearVelocity = jumpDirection * jumpForce; //Apply jump force 
            StartCoroutine(JumpForceStop(currentChargeTime));
            currentChargeTime = 0.0f; //Reset charge time
        }
        if(cameraFollow)
        {
            //Camera follows player *after* the jump
            Vector3 targetPosition = new Vector3(mainCamera.transform.position.x, transform.position.y + cameraPlayerYOffset, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraLerpSpeed);
            if(Mathf.Abs(mainCamera.transform.position.y - targetPosition.y) < 0.1f)
            {
                cameraFollow = false; //Stop following when close enough
                finishJump.Invoke(); //Invoke finish jump event
            }
        }
    }

    private IEnumerator JumpForceStop(float chargeTime)
    {
        yield return new WaitForSeconds(jumpStopDelay * chargeTime);
        rb.linearVelocity = Vector2.zero; //Stop upward movement after delay
        if (mainCamera)
        {
            cameraFollow = true;
        }
    }

    public float GetNormalizedCharge()
    {
        return (currentChargeTime / maxChargeTime);
    }

}
