using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastExample : MonoBehaviour
{ 

    // Update is called once per frame
    void Update()
    {
       if (Input.GetButtonDown("Fire1"))
       {
           RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
               Debug.Log("Hit object: " + hit.collider.name);
            
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                // Jika tag-nya "Enemy", hancurkan obyek tersebut
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin, hit.point);
        }
    }

}
