using System.Collections;
using UnityEngine;

public class Auto : PuckBehavior
{
    private GameObject ready;
    private Renderer readyRenderer;

    protected override void OnStart()
    {
        if(parameters.trialNumber == 0)
        {
            ready = GameObject.Find("Ready");
            readyRenderer = ready.GetComponent<Renderer>();
            puckBarrier.EnableButton(ready);
        }
        else
        {
            StartCoroutine(Shoot());
        }

        gameObject.layer = 2; // Ignore Raycast
    }

    protected override void OnUpdate()
    {
        if (readyRenderer.enabled)
        {
            Ray ray = new Ray(Cursor.transform.position, Vector3.forward);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null)
            {
                StartCoroutine(Shoot());
                puckBarrier.DisableButton(ready);
            }
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(parameters.GetAutoShootDelay());
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 forceVector = (GetCollisionPoint() - currentPosition).normalized;
        rb2D.AddForce(forceVector * parameters.GetForce());
    }
}