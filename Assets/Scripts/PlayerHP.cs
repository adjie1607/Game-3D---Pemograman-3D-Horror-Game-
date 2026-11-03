using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("Low HP Vignette")]
    public Image vignetteImage;
    public float vignetteFadeSpeed = 4f;
    public float lowHPThreshold = 70;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public TextMeshProUGUI healthText;
    public Slider healthBar;

    [Header("Blink Settings")]
    public Image healthBarFill;
    public float blinkSpeed = 0.5f;

    [Header("Pulse Screen Effect")]
    public Image pulseScreen;
    public float pulseSpeed = 2f;
    public float lowHPThresholdPulse = 40f;
    private bool isPulsing = false;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Fade to Black")]
    public Image fadePanel;               // FULLSCREEN BLACK PANEL
    public float fadeSpeed = 2f;          // Makin besar makin cepat fade

    [Header("Audio")]
    public AudioSource hurtBreathAudio;
    public AudioSource screamAudio;

    private bool isBreathingPlayed = false;
    private bool isBlinking = false;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        UpdateHealthUI();

        // Pastikan fade panel transparan diawal
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;
        }
    }


    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + currentHealth.ToString();

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth < 70 && !isBlinking)
            StartCoroutine(BlinkHealthBar());

        UpdateVignetteEffect();
        CheckLowHPAudio();

        if (currentHealth < lowHPThresholdPulse && !isPulsing)
            StartCoroutine(PulseScreenEffect());
    }


    void UpdateVignetteEffect()
    {
        if (vignetteImage == null) return;

        float targetAlpha = currentHealth < lowHPThreshold ? 0.45f : 0f;

        Color c = vignetteImage.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * vignetteFadeSpeed);
        vignetteImage.color = c;
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        if (Camerashake.instance != null)
            Camerashake.instance.Shake(0.15f, 0.25f);

        if (currentHealth <= 0)
            Die();
    }


    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Died!");

        // Stop all audio
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudio)
        {
            a.Stop();
        }

        StopAllCoroutines();

        // Matiin efek low HP
        if (pulseScreen != null)
        {
            Color c = pulseScreen.color;
            c.a = 0f;
            pulseScreen.color = c;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        // Mulai fade to black
        StartCoroutine(FadeToBlack());
    }


    IEnumerator FadeToBlack()
    {
        if (fadePanel != null)
        {
            float t = 0;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime * fadeSpeed;

                Color c = fadePanel.color;
                c.a = Mathf.Lerp(0f, 1f, t);
                fadePanel.color = c;

                yield return null;
            }
        }

        // Setelah fade selesai → tampilkan Game Over panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
    }



    // ================== Low HP EFFECTS ===================== //

    IEnumerator BlinkHealthBar()
    {
        isBlinking = true;

        while (currentHealth < 70 && !isDead)
        {
            if (healthBarFill != null)
            {
                healthBarFill.color = new Color(
                    healthBarFill.color.r,
                    healthBarFill.color.g,
                    healthBarFill.color.b,
                    0.2f
                );
            }

            yield return new WaitForSeconds(blinkSpeed);

            if (healthBarFill != null)
            {
                healthBarFill.color = new Color(
                    healthBarFill.color.r,
                    healthBarFill.color.g,
                    healthBarFill.color.b,
                    1f
                );
            }

            yield return new WaitForSeconds(blinkSpeed);
        }

        isBlinking = false;
    }


    void CheckLowHPAudio()
    {
        if (isDead) return;

        if (currentHealth < 50)
        {
            if (!isBreathingPlayed)
            {
                isBreathingPlayed = true;

                if (hurtBreathAudio != null && !hurtBreathAudio.isPlaying)
                    hurtBreathAudio.Play();
            }
        }
        else
        {
            if (hurtBreathAudio != null && hurtBreathAudio.isPlaying)
                hurtBreathAudio.Stop();

            isBreathingPlayed = false;
        }
    }


    IEnumerator PulseScreenEffect()
    {
        isPulsing = true;

        while (currentHealth < lowHPThresholdPulse && !isDead)
        {
            yield return StartCoroutine(FadePulse(0.5f));
            yield return StartCoroutine(FadePulse(0f));
        }

        if (pulseScreen != null)
        {
            Color c = pulseScreen.color;
            c.a = 0f;
            pulseScreen.color = c;
        }

        isPulsing = false;
    }


    IEnumerator FadePulse(float targetAlpha)
    {
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * pulseSpeed;

            if (pulseScreen != null)
            {
                Color c = pulseScreen.color;
                c.a = Mathf.Lerp(c.a, targetAlpha, t);
                pulseScreen.color = c;
            }

            yield return null;
        }
    }

    // ========== BUTTON RESTART ========== //

    public void RestartGame()
    {
        Time.timeScale = 1f; // unfreeze game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
