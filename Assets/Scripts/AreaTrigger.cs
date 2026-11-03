using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public GameObject enemy; // drag & drop setan wilayah ini ke sini

    private void Start()
    {
        if (enemy != null)
            enemy.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemy != null)
        {
            Debug.Log("Player masuk wilayah: " + gameObject.name);
            enemy.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemy != null)
        {
            Debug.Log("Player keluar wilayah: " + gameObject.name);
            enemy.SetActive(false);
        }
    }
}
