using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpscareManager : MonoBehaviour
{
    public GameObject jumpscareImage;
    public AudioSource jumpscareAudio;
    public float scareDuration = 3f;

    private void Start()
    {
        if (jumpscareImage != null)
            jumpscareImage.SetActive(false);
    }

    public void TriggerJumpscare()
    {
        StartCoroutine(ShowJumpscare());
    }

    private IEnumerator ShowJumpscare()
    {
        if (jumpscareImage != null)
            jumpscareImage.SetActive(true);

        if (jumpscareAudio != null)
            jumpscareAudio.Play();

        yield return new WaitForSeconds(scareDuration);

        if (jumpscareImage != null)
            jumpscareImage.SetActive(false);
    }
}
