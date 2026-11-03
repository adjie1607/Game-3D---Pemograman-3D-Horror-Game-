using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damage = 25;
    public float range = 100f;
    public float fireRate = 0.2f;

    [Header("Ammo & Reload")]
    public int maxAmmo = 12;
    private int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    [Header("References")]
    public Camera fpsCam;
    public Transform firePoint;
    public LayerMask whatIsHittable;

    [Header("Visual Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 50f;
    public GameObject muzzleFlash;
    public GameObject impactEffect;

    [Header("Audio & Integrations")]
    public ProceduralRecoil recoilScript;
    public Text ammoText;
    public AudioSource shootAudio;
    public AudioSource reloadAudio;

    private float nextTimeToFire = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void OnEnable()
    {
        isReloading = false;
        UpdateAmmoUI();
    }

    void Update()
    {
        // 1. Jika sedang reload, LANGSUNG STOP. Jangan cek input lain.
        if (isReloading)
            return;

        // 2. Cek Kondisi Reload (Peluru habis atau tekan R)
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo))
        {
            StartCoroutine(Reload());
            return; // STOP di sini agar tidak lanjut ke logika nembak di bawah
        }

        // 3. Cek Input Nembak
        // FIX DISINI: Kita tambahkan syarat "&& currentAmmo > 0" 
        // untuk memastikan peluru benar-benar ada sebelum menembak.
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Double check safety (Opsional tapi aman)
        if (isReloading || currentAmmo <= 0) return;

        currentAmmo--;
        UpdateAmmoUI();

        if (shootAudio != null) shootAudio.Play();
        if (recoilScript != null) recoilScript.RecoilFire();

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);

        // --- RAYCAST LOGIC ---
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, whatIsHittable))
        {
            // Debugging
            Debug.Log("Hit object: " + hit.transform.name);
            targetPoint = hit.point;

            EnemyHealth enemy = hit.transform.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            else
            {
                // Debugging tambahan biar tau kalau script HP gak ketemu
                Debug.LogWarning("Objek kena tembak, tapi script EnemyHealth tidak ditemukan di object ini atau parent-nya!");
            }

            if (impactEffect != null)
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }

        else
        {
            targetPoint = fpsCam.transform.position + fpsCam.transform.forward * range;
        }

        // --- VISUAL BULLET ---
        if (bulletPrefab != null)
        {
            GameObject visualBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            visualBullet.transform.LookAt(targetPoint);

            Rigidbody rb = visualBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                Vector3 moveDir = (targetPoint - firePoint.position).normalized;
                rb.linearVelocity = moveDir * bulletSpeed;
            }
            Destroy(visualBullet, 2f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true; // Kunci status reload segera!
        Debug.Log("Reloading...");

        if (ammoText != null) ammoText.text = "Reloading...";
        if (reloadAudio != null) reloadAudio.Play();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false; // Buka kunci
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null && !isReloading)
        {
            ammoText.text = currentAmmo + " / " + maxAmmo;
        }
    }
}