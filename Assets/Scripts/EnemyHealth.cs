using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Audio")]
    public AudioSource lowHPAudio;
    public AudioSource deathAudio;

    [HideInInspector] public AreaController assignedArea;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"⚔️ {gameObject.name} KENA HIT! Damage: {damageAmount}. Sisa HP: {currentHealth}");

        // Kalau HP habis
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // LAPOR KE AREA
        if (assignedArea != null)
        {
            assignedArea.ReportEnemyDeath();
        }
        else
        {
            // Coba cari manual kalau assignedArea lepas (Fallback)
            assignedArea = GetComponentInParent<AreaController>();
            if (assignedArea != null) assignedArea.ReportEnemyDeath();
            else Debug.LogError($"❌ {gameObject.name} Mati tapi bingung lapor ke Area mana! (Pastikan Musuh adalah Child dari Area)");
        }

        StartCoroutine(PlayDeathAndDestroy());
    }

    IEnumerator PlayDeathAndDestroy()
    {
        // Matikan visual instan biar player tau udah mati
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (var r in rends) r.enabled = false;

        if (deathAudio != null)
        {
            deathAudio.Play();
            yield return new WaitForSeconds(deathAudio.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        Destroy(gameObject);
    }
}