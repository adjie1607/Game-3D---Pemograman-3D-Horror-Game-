using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    // Rotasi
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [Header("Recoil Settings")]
    public float recoilX = -2f; // Menendang ke atas (negatif)
    public float recoilY = 2f;  // Acak kiri/kanan
    public float recoilZ = 0.35f; // Roll sedikit

    [Header("Speed Settings")]
    public float snappiness = 6f; // Seberapa cepat hentakan terjadi
    public float returnSpeed = 2f; // Seberapa cepat kembali ke posisi semula

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));

        // Bonus: Panggil CameraShake yang sudah kamu punya!
        if (Camerashake.instance != null)
        {
            Camerashake.instance.Shake(0.1f, 0.2f);
        }
    }
}