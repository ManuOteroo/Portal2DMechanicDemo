using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectile : MonoBehaviour
{

    [SerializeField] GameObject portalPrefab;
    [SerializeField] string portalTag;

    [SerializeField] float minDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Extract contact info
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 hitPoint = contact.point;
        Vector2 normal = contact.normal;

        //Find portals in scene
        string[] portalTags = { "PortalA", "PortalB" };

        foreach (string tagPortal in portalTags)
        {
            GameObject existingPortal = GameObject.FindGameObjectWithTag(tagPortal);

            if (existingPortal != null)
            {
                float distance = Vector2.Distance(hitPoint, existingPortal.transform.position);

                if (distance < minDistance)
                {
                    Debug.Log("Blocked Impact");
                    Destroy(gameObject);
                    return;
                }

            }
        }

        //Calculate rotation based on normal, Atan2 gives angle, Rad2Deg converts to degrees
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        //Destroy old portal
        GameObject oldPortal = GameObject.FindGameObjectWithTag(portalTag);
        if (oldPortal != null)
        {
            Destroy(oldPortal);
        }

        
        //Instantiate new portal
        GameObject newPortal = Instantiate(portalPrefab, hitPoint + (normal * 0.02f), rotation);

        if (collision.gameObject.CompareTag("Platforms"))
        {
            newPortal.transform.SetParent(collision.transform);
        }

        //Destroy projectile
        Destroy(gameObject);

        

        

    }
}
