using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShootTriggerable : MonoBehaviour
{   

    [HideInInspector] public int gunDamage = 1;
    [HideInInspector] public float weaponRange = 50f;
    [HideInInspector] public float hitForce = 100f;
    public Transform gunEnd;
    [HideInInspector] public LineRenderer laserLine;


    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds (.07f);
    
    public void Initialize ()
    {
        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInParent<Camera>();
    }

    public void Fire ()
    {
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(.5f,.5f,0));
        Debug.DrawRay(rayOrigin,fpsCam.transform.forward*weaponRange,Color.green);
        RaycastHit hit;
        StartCoroutine(ShotEffect());
        laserLine.SetPosition(0,gunEnd.position);

        if (Physics.Raycast(rayOrigin,fpsCam.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1,hit.point);
            ShootableBox health = hit.collider.GetComponent<ShootableBox>();

            if (health != null)
            {
                health.Damage (gunDamage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            laserLine.SetPosition(1, fpsCam.transform.forward*weaponRange);
        }

    }


    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
