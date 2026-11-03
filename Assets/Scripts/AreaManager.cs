using UnityEngine;
using System.Collections;

public class AreaManager : MonoBehaviour
{
    public static AreaManager Instance;

    [Header("Urutan Area")]
    public AreaController[] areas;

    [Header("UI Menang")]
    public GameObject winPanel;
    public AudioSource winAudio;

    private int currentAreaIndex = 0;
    private bool isTransitioning = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);

        // Reset semua area
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i] != null) areas[i].ForceDisableArea();
        }

        ActivateArea(0);
    }

    public void ActivateArea(int index)
    {
        currentAreaIndex = index; // Update index saat ini

        // Cek Safety: Kalau index di luar batas array, anggap Menang
        if (index >= areas.Length)
        {
            TriggerWin();
            return;
        }

        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i] != null)
            {
                if (i == index)
                    areas[i].ForceEnableArea();
                else
                    areas[i].ForceDisableArea();
            }
        }
    }

    public void NextArea()
    {
        if (isTransitioning) return;

        int next = currentAreaIndex + 1;
        Debug.Log($"Logic Check: Area Selesai ({currentAreaIndex}). Coba lanjut ke ({next}). Total Area: {areas.Length}");

        // KONDISI MENANG:
        // 1. Jika next index melebihi jumlah area (misal 3 >= 3)
        // 2. ATAU Jika slot area selanjutnya Kosong/Null (misal size 4 tapi area D kosong)
        if (next >= areas.Length || areas[next] == null)
        {
            Debug.Log("🏆 KONDISI MENANG TERPENUHI!");
            TriggerWin();
            return;
        }

        StartCoroutine(DoNextArea(next));
    }

    void TriggerWin()
    {
        Debug.Log("✨ MENAMPILKAN WIN PANEL ✨");

        // 1. Munculkan UI
        if (winPanel != null)
        {
            winPanel.SetActive(true);

            // Pastikan panel ada di paling depan (takut ketutup UI lain)
            winPanel.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogError("❌ ERROR: Kamu belum memasukkan object 'WinPanel' ke script AreaManager di Inspector!");
        }

        // 2. Audio
        if (winAudio != null) winAudio.Play();

        // 3. Unlock Mouse (PENTING)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 4. Stop Waktu
        Time.timeScale = 0f;
    }

    private IEnumerator DoNextArea(int nextIndex)
    {
        isTransitioning = true;
        yield return new WaitForSeconds(1f);
        ActivateArea(nextIndex);
        yield return new WaitForSeconds(0.5f);
        isTransitioning = false;
    }
}