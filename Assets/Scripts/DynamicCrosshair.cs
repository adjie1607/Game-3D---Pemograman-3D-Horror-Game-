using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    public RectTransform crosshairRect; // Masukkan object UI Crosshair ke sini

    [Header("Settings")]
    public float restSize = 10f;    // Ukuran saat diam (misal 10 atau 50 tergantung gambar)
    public float moveSize = 25f;    // Ukuran saat jalan
    public float shootSize = 40f;   // Ukuran saat nembak
    public float speed = 10f;       // Kecepatan perubahan ukuran

    private float currentSize;

    void Start()
    {
        currentSize = restSize;
    }

    void Update()
    {
        if (crosshairRect == null) return;

        // 1. Deteksi Gerakan (Jalan)
        bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        // 2. Deteksi Nembak
        bool isShooting = Input.GetButton("Fire1");

        // Tentukan Target Ukuran
        float targetSize = restSize;

        if (isShooting)
            targetSize = shootSize;
        else if (isMoving)
            targetSize = moveSize;

        // 3. Lerp (Perubahan halus)
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * speed);

        // 4. Terapkan ukuran ke UI
        crosshairRect.sizeDelta = new Vector2(currentSize, currentSize);
    }
}