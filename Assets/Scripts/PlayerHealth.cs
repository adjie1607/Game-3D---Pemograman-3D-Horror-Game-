using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("UI References")]
    public TextMeshProUGUI healthText;
    public GameObject jumpscareImage;
    public float jumpscareDuration = 0.5f;

    [Header("Audio Settings - Jumpscare")]
    public AudioSource jumpscareAudio;
    public AudioClip jumpscareClip;
    public float jumpscareVolume = 1f;

    [Header("Audio Settings - BGM")]
    public AudioSource bgmAudio;
    public AudioClip bgmClip;
    public float bgmVolume = 0.4f;
    public bool loopBGM = true;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public float restartDelay = 3f;

    private bool isJumpscaring = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (jumpscareImage != null)
            jumpscareImage.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 🎶 Mulai BGM pas game start
        if (bgmAudio != null && bgmClip != null)
        {
            bgmAudio.clip = bgmClip;
            bgmAudio.volume = bgmVolume;
            bgmAudio.loop = loopBGM;
            bgmAudio.Play();
        }
    }

    public void TakeDamage()
    {
        if (isDead || isJumpscaring) return;

        currentHealth--;
        UpdateHealthUI();
        StartCoroutine(ShowJumpscare());

        if (currentHealth <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator ShowJumpscare()
    {
        isJumpscaring = true;

        if (jumpscareImage != null)
            jumpscareImage.SetActive(true);

        if (jumpscareAudio != null && jumpscareClip != null)
        {
            jumpscareAudio.PlayOneShot(jumpscareClip, jumpscareVolume);
            Debug.Log("🔊 Mainkan suara jumpscare!");
        }

        yield return new WaitForSeconds(jumpscareDuration);

        if (jumpscareImage != null)
            jumpscareImage.SetActive(false);

        isJumpscaring = false;
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + currentHealth.ToString();
    }

    IEnumerator GameOver()
    {
        isDead = true;
        Debug.Log("💀 GAME OVER");

        // Nonaktifin movement player
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        // 🛑 Stop BGM
        if (bgmAudio != null && bgmAudio.isPlaying)
            bgmAudio.Stop();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
