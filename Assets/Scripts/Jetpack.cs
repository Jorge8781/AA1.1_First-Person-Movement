using UnityEngine;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour
{
    public Rigidbody rb;

    public float maxFuel = 100f;
    public float jetpackForce = 8f;
    public float rechargeDelay = 0.5f;
    public float rechargeDuration = 0.5f;

    public Image fuelImage;

    private float fuel;

    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private bool recharging = false;

    void Start()
    {
        fuel = maxFuel;
    }

    void Update()
    {
        CheckGroundedState();
        UpdateUI();
    }

    void FixedUpdate()
    {
        HandleJetpack();
    }

    void CheckGroundedState()
    {
        if (isGrounded && !wasGroundedLastFrame)
        {
            OnGrounded();
        }

        wasGroundedLastFrame = isGrounded;
    }

    void HandleJetpack()
    {
        if (isGrounded)
            return;

        if (Input.GetKey(KeyCode.Space) && fuel > 0)
        {
            rb.AddForce(
                Vector3.up * jetpackForce,
                ForceMode.Acceleration
            );

            fuel -= (maxFuel / 1f) * Time.fixedDeltaTime;

            if (fuel < 0)
                fuel = 0;
        }
    }

    void OnGrounded()
    {
        if (recharging)
            return;

        if (fuel <= 0)
        {
            StartCoroutine(RechargeWithDelay());
        }
        else if (fuel < maxFuel)
        {
            StartCoroutine(RechargeInstantly());
        }
    }

    System.Collections.IEnumerator RechargeWithDelay()
    {
        recharging = true;

        yield return new WaitForSeconds(rechargeDelay);

        yield return StartCoroutine(RechargeRoutine());

        recharging = false;
    }

    System.Collections.IEnumerator RechargeInstantly()
    {
        recharging = true;

        yield return StartCoroutine(RechargeRoutine());

        recharging = false;
    }

    System.Collections.IEnumerator RechargeRoutine()
    {
        float duration = rechargeDuration;
        float startFuel = fuel;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            fuel = Mathf.Lerp(
                startFuel,
                maxFuel,
                elapsed / duration
            );

            yield return null;
        }

        fuel = maxFuel;
    }

    void UpdateUI()
    {
        if (fuelImage != null)
        {
            fuelImage.fillAmount = fuel / maxFuel;
        }
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}
