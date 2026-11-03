using UnityEngine;
using System.Collections.Generic;

public class AreaController : MonoBehaviour
{
    public Collider triggerZone;
    [Header("List ini akan terisi OTOMATIS, biarkan kosong")]
    public List<EnemyHealth> enemies = new List<EnemyHealth>();

    private int totalEnemiesAlive = 0;
    private bool areaIsActive = false;

    // Dipanggil saat EnemyHealth mendeteksi kematian
    public void ReportEnemyDeath()
    {
        if (!areaIsActive) return;

        totalEnemiesAlive--;
        Debug.Log($"🔻 Musuh Mati di {name}. Sisa: {totalEnemiesAlive}");

        if (totalEnemiesAlive <= 0)
        {
            Debug.Log($"✅ AREA {name} SELESAI! Memanggil NextArea...");
            areaIsActive = false;

            if (AreaManager.Instance != null)
            {
                AreaManager.Instance.NextArea();
            }
        }
    }

    public void ForceEnableArea()
    {
        areaIsActive = true;

        // --- FITUR BARU: CARI MUSUH OTOMATIS ---
        // Mencari semua script EnemyHealth yang jadi ANAK (Children) dari Area ini
        EnemyHealth[] foundEnemies = GetComponentsInChildren<EnemyHealth>(true);
        enemies.Clear();
        enemies.AddRange(foundEnemies);

        totalEnemiesAlive = enemies.Count;
        // ---------------------------------------

        if (triggerZone != null) triggerZone.enabled = true;

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
                enemy.assignedArea = this; // Paksa musuh kenal area-nya
            }
        }

        gameObject.SetActive(true);
        Debug.Log($"🟢 {name} START. Terdeteksi {totalEnemiesAlive} musuh (Auto-Detect).");

        if (totalEnemiesAlive == 0)
        {
            Debug.LogError($"⚠️ PERINGATAN: Area {name} tidak punya anak object Musuh! Pastikan musuh ditaruh DI DALAM object Area ini di Hierarchy.");
            // Otomatis skip kalau gak ada musuh biar game gak macet
            if (AreaManager.Instance != null) AreaManager.Instance.NextArea();
        }
    }

    public void ForceDisableArea()
    {
        areaIsActive = false;
        if (triggerZone != null) triggerZone.enabled = false;

        // Cari lagi incase belum ke-load
        if (enemies.Count == 0)
        {
            enemies.AddRange(GetComponentsInChildren<EnemyHealth>(true));
        }

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy != null) enemy.gameObject.SetActive(false);
        }
    }
}