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
    [Header("Camera Settings")]
    [SerializeField] private float cameraPlayerYOffset = -10.0f; //Camera offset on Y axis when following player
    [SerializeField] private float cameraLerpSpeed = 0.01f;
    [Header("SlingShot Mode")]
    [SerializeField] private bool slingShotMode = false; //If true, jump direction is opposite to mouse/touch position
    [SerializeField] private LayerMask slingshotRaycastLayerMask; //Layer mask for raycast in slingshot mode (layers we want the raycast to hit)
    private float currentChargeTime = 0.0f; //How long current jump has been charged
    private Rigidbody2D rb;
    private bool cameraFollow = false;

    private Vector2 mousePosition;
    private Vector2 playerScreenPosition;

    private LineRenderer lineRenderer; //Shows where the player is aiming. Gets disabled, enabled and updated positions for every

    void Start()
    {
        chargeJumpAction = InputSystem.actions.FindAction("Jump"); //Get action for jump (left mouse)
        chargeJumpAction.Enable(); 
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();

        //If on IOS/Android, force touch input testing
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            touchInputTesting = true;
        }
    }

    void Update()
    {
        //Classic Mode (jump towards mouse/touch)
        if (!slingShotMode)
        {
            ClassicJump();
        }
        else
        {
            SlingShotJump();
        }
    }

    private void SlingShotJump()
    {
        if (touchInputTesting)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                mousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
                //Debug.Log("Touchscreen detected, touched pos: " + mousePosition);
            }
        }
        else
        {
            mousePosition = Mouse.current.position.ReadValue(); //Screen space position of mouse
            //Debug.Log("Mouse detected, mouse pos: " + mousePosition);
        }
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

            //Line renderer logic
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position); //Start at player position
            //Getting direction for end point
            Vector2 directionToMouse = mousePosition - playerScreenPosition; 
            Vector2 aimDirection = -directionToMouse.normalized; //Direction from player to mouse

            //Check with raycast if there is anything in the way 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, GetNormalizedCharge() * 5.0f, slingshotRaycastLayerMask);
            if (hit.collider != null) //If raycast hits something that isnt player
            {
                lineRenderer.SetPosition(1, hit.point); //End at hit point if something is hit
                return;
            }
            else
            {
                float lineLength = GetNormalizedCharge() * 5.0f; //5.0 is arboitary for visualisation
                Vector3 lineEndPosition = transform.position + (Vector3)(aimDirection * lineLength);  
                lineRenderer.SetPosition(1, lineEndPosition); //End at jump direction scaled by charge amount
            }
        }
        else if (chargeJumpAction.WasReleasedThisFrame()) //Jump Released
        {
            lineRenderer.enabled = false; //Disable line renderer on jump   
            
            //Using trigonometry to calculate jump X direction based on where mouse is
            //We have Opposite (mouse Y - player Y) and adjacent (mouse X - player X)
            //Get the vector pointing from the player to the mouse and normalise for direction 
            Vector2 directionToMouse = mousePosition - playerScreenPosition;
            Vector2 jumpDirection = -directionToMouse.normalized; //Opposite direction for slingshot effect

            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, currentChargeTime / maxChargeTime);
            rb.linearVelocity = jumpDirection * jumpForce;

            StartCoroutine(JumpForceStop(currentChargeTime));
            currentChargeTime = 0.0f;
        }
        if (cameraFollow)
        {
            CameraFollowLogic();
        }
    }

    private void ClassicJump()
    {
        if (touchInputTesting)
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                mousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
                //Debug.Log("Touchscreen detected, touched pos: " + mousePosition);
            }
        }
        else
        {
            mousePosition = Mouse.current.position.ReadValue(); //Screen space position of mouse
                                                                //Debug.Log("Mouse detected, mouse pos: " + mousePosition);
        }
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
            float angleDegrees = Mathf.Abs(angleRadians * Mathf.Rad2Deg); //Convert to degrees if needed
            //Debug.Log("Angle: " + angleDegrees + " radians: " + angleRadians);

            Vector2 jumpDirection = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));

            //Debug.Log("Released");
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, currentChargeTime / maxChargeTime); //Calculate jump force based on charge time
            rb.linearVelocity = jumpDirection * jumpForce; //Apply jump force 
            StartCoroutine(JumpForceStop(currentChargeTime));
            currentChargeTime = 0.0f; //Reset charge time
        }
        if (cameraFollow)
        {
            CameraFollowLogic();
        }
    }

    /* Camera smoothly travels up after the jump*/

    private void CameraFollowLogic()
    {
        //Camera follows player *after* the jump
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + cameraPlayerYOffset, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraLerpSpeed);
        if (Mathf.Abs(mainCamera.transform.position.y - targetPosition.y) < 0.1f)
        {
            cameraFollow = false; //Stop following when close enough
            finishJump.Invoke(); //Invoke finish jump event
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
