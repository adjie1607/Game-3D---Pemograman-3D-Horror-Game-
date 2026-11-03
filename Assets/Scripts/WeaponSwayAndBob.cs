using UnityEngine;

public class WeaponSwayAndBob : MonoBehaviour
{
    [Header("Sway Settings (Gerakan Mouse)")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmooth = 4.0f;

    [Header("Rotation Sway Settings")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float rotationSmooth = 6f;

    [Header("Bobbing Settings (Gerakan Jalan)")]
    public float bobbingSpeed = 10f;
    public float bobbingAmount = 0.05f;
    public ControllerColliderHit controller; // Optional: referensi ke controller jika perlu logic speed

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Input Movement Player (Bisa diambil dari script PlayerMovement kamu)
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        InputHandler();
        SwayMovement();
        SwayRotation();
        BobbingMovement();
    }

    void InputHandler()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void SwayMovement()
    {
        float moveX = -Input.GetAxis("Mouse X") * swayAmount;
        float moveY = -Input.GetAxis("Mouse Y") * swayAmount;

        moveX = Mathf.Clamp(moveX, -maxSwayAmount, maxSwayAmount);
        moveY = Mathf.Clamp(moveY, -maxSwayAmount, maxSwayAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * swaySmooth);
    }

    void SwayRotation()
    {
        float tiltY = -Input.GetAxis("Mouse X") * rotationAmount;
        float tiltX = -Input.GetAxis("Mouse Y") * rotationAmount;

        tiltY = Mathf.Clamp(tiltY, -maxRotationAmount, maxRotationAmount);
        tiltX = Mathf.Clamp(tiltX, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(tiltX, tiltY, 0));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * rotationSmooth);
    }

    void BobbingMovement()
    {
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            // Player sedang bergerak
            float timer = Time.time * bobbingSpeed;
            float bobX = Mathf.Cos(timer) * bobbingAmount; // Gerakan kiri-kanan
            float bobY = Mathf.Sin(timer * 2) * bobbingAmount; // Gerakan naik-turun (2x lebih cepat)

            Vector3 bobPosition = new Vector3(bobX, bobY, 0);

            // Tambahkan ke posisi saat ini (supaya stack dengan sway)
            transform.localPosition += bobPosition * Time.deltaTime;
        }
        else
        {
            // Idle: Kembalikan pelan-pelan ke posisi sway saja
            // (Logika reset ditangani oleh Lerp di SwayMovement yang selalu mengarah ke initialPosition)
        }
    }
}