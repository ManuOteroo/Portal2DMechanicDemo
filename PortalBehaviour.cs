using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PortalBehaviour : MonoBehaviour
{
    [SerializeField] float minExitVelocity = 5f;
    [SerializeField] string pairPortalTag;
    [SerializeField] float teleportCooldown = 1f;

    [SerializeField] PortalBehaviour targetPortal;

    private bool isTeleporting = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check Cooldown
        if (isTeleporting) return;
        //Find exit portal
        GameObject pairPortal = GameObject.FindGameObjectWithTag(pairPortalTag);
        if (pairPortal != null)
        {
            
            PortalBehaviour exitPortal = pairPortal.GetComponent<PortalBehaviour>();
            if (exitPortal != null)
            {
                
                //Teleport
                Teleport(other.gameObject, pairPortal.transform);
                
            }
        }
        
    }
    private void Teleport(GameObject obj, Transform exitTransform)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            //Calculate actual magnitude
            float magnitude = rb.velocity.magnitude;

            //If velocity is too low, we use minExitVelocity
            if (magnitude < minExitVelocity)
            {
                magnitude = minExitVelocity;
            }

            //Move object to exit with margin to avoid getting stuck
            obj.transform.position = exitTransform.position + exitTransform.right * 0.7f;
            //Apply final velocity towards portal normal 
            rb.velocity = exitTransform.right * magnitude;
        }

        Debug.Log("TP with velocity: " + rb.velocity.magnitude);
    }
    
}