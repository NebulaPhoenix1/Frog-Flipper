using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ChargeJump : MonoBehaviour
{
    public UnityEvent finishJump;


    [SerializeField] private Camera mainCamera;
    private InputAction chargeJumpAction; //New input action for jump (InputSystem/Player/Jump)
    [SerializeField] private float maxChargeTime = 2.0f; 
    [SerializeField] private float minJumpForce = 5.0f;
    [SerializeField] private float maxJumpForce = 20.0f;
    [SerializeField] private float jumpStopDelay = 0.1f; //Delay before jump force stops being applied
    [SerializeField] private float cameraPlayerYOffset = 4.0f; //Camera offset on Y axis when following player
    private float cameraLerpSpeed = 0.01f;
    private float currentChargeTime = 0.0f; //How long current jump has been charged
    private Rigidbody2D rb;
    private bool cameraFollow = false;

    


    void Start()
    {
        chargeJumpAction = InputSystem.actions.FindAction("Jump"); //Get action for jump (left mouse)
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (chargeJumpAction.IsPressed() && !cameraFollow) //Charging Jump while camera not following
        {
            //Debug.Log("Charging");
            if(currentChargeTime < maxChargeTime)
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
            //Debug.Log("Released");
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, currentChargeTime / maxChargeTime); //Calculate jump force based on charge time
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //Apply jump force 
            StartCoroutine(JumpForceStop());
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

    private IEnumerator JumpForceStop()
    {
        yield return new WaitForSeconds(jumpStopDelay * currentChargeTime);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); //Stop upward movement after delay
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
