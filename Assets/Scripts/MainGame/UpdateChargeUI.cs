using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class UpdateChargeUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Image fillImage;
    private ChargeJump chargeJump;
    void Start()
    {
        fillImage = GetComponent<Image>();
        chargeJump = FindFirstObjectByType<ChargeJump>();
        Camera camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeJump.GetNormalizedCharge() >= 0f)
        {
            transform.position = Mouse.current.position.ReadValue();
            fillImage.enabled = true;
            fillImage.fillAmount = chargeJump.GetNormalizedCharge();
        }
        else { fillImage.enabled = false; }
    }
}
