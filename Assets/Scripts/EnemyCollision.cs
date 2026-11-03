using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private bool hasHitPlayer = false;
    public int collisionDamage = 10; // Damage tabrakan badan

    private void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer) return;

        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;

            // Damage player (pastikan kamu punya script PlayerHealth)
            // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            // if (playerHealth != null)
            // {
            //    playerHealth.TakeDamage(collisionDamage);
            // }

            Debug.Log("Musuh menabrak player!");

            // HAPUS LOGIKA NextArea() DARI SINI
            // Kita tidak mau pindah area cuma gara-gara ditabrak musuh.

            // Hancurkan diri sendiri setelah nabrak (opsional, kayak kamikaze)
            // Atau biarkan hidup buat nyerang lagi (hapus baris Destroy di bawah kalau mau musuh tetap hidup)
            Destroy(gameObject, 0.1f);
        }
    }
}