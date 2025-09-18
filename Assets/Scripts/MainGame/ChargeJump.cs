using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeJump : MonoBehaviour
{
    private InputAction chargeJumpAction; //New input action for jump (InputSystem/Player/Jump)
    [SerializeField] private float maxChargeTime = 2.0f; 
    [SerializeField] private float minJumpForce = 5.0f;
    [SerializeField] private float maxJumpForce = 20.0f;
    [SerializeField] private float jumpStopDelay = 0.1f; //Delay before jump force stops being applied
    private float currentChargeTime = 0.0f; //How long current jump has been charged
    private Rigidbody2D rb;

    void Start()
    {
        chargeJumpAction = InputSystem.actions.FindAction("Jump"); //Get action for jump (left mouse)
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (chargeJumpAction.IsPressed()) //Charging Jump
        {
            Debug.Log("Charging");
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
            Debug.Log("Released");
            float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, currentChargeTime / maxChargeTime); //Calculate jump force based on charge time
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); //Apply jump force 
            StartCoroutine(JumpForceStop());
            currentChargeTime = 0.0f; //Reset charge time
        }
    }

    private IEnumerator JumpForceStop()
    {
        yield return new WaitForSeconds(jumpStopDelay * currentChargeTime);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); //Stop upward movement after delay
    }

}
