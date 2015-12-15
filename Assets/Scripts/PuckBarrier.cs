using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class PuckBarrier : MonoBehaviour
{
    public GameObject Visual;
    public GameObject Audio;
    public GameObject Restart;
    public Transform Cursor;
    private Parameters parameters;
    private int collisionY;
    private Vector2 collisionVelocity;
    private Vector2 velocity;
    private Renderer audioRenderer;

    public void EnableButton(GameObject button)
    {
        button.GetComponent<Renderer>().enabled = true;
        button.layer = 0; // Default
    }

    public void DisableButton(GameObject button)
    {
        button.GetComponent<Renderer>().enabled = false;
        button.layer = 2; // Ignore Raycast
    }

    private void Start()
    {
        parameters = Parameters.Instance;
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(parameters.GetBarrierPosition(),
                                                                      Screen.height * 0.5f,
                                                                      Camera.main.nearClipPlane));
        position.z = 0;
        transform.position = position;
        audioRenderer = Audio.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (audioRenderer.enabled)
        {
            Ray ray = new Ray(Cursor.position, Vector3.forward);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null)
            {
                int selection = 1;
                if (hit.collider.name == "Audio")
                {
                    selection = -1;
                }

                DisableButton(Audio);
                DisableButton(Visual);
                LogParameters(selection);
                parameters.trialNumber++;
                if (parameters.trialNumber < parameters.totalTrials)
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
                else
                {
                    Application.LoadLevel("MainMenu");
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        float audioDelay = parameters.GetDelay();
        if (audioDelay >= 0)
        {
            GetComponent<AudioSource>().PlayDelayed(audioDelay);
            StartCoroutine(waitForAudio(audioDelay));
        }
        else
        {
            EnableButton(Audio);
            EnableButton(Visual);
        }

        Vector2 hitPoint = coll.contacts[0].point;
        Vector3 hitPosition = Camera.main.WorldToScreenPoint(new Vector3(hitPoint.x, hitPoint.y, 0));
        collisionY = (int)hitPosition.y;
        velocity = coll.relativeVelocity;
        gameObject.layer = 2; // Ignore Raycast
    }

    private IEnumerator waitForAudio(float audioDelay)
    {
        yield return new WaitForSeconds(audioDelay);
        EnableButton(Audio);
        EnableButton(Visual);
    }

    private void LogParameters(int selection)
    {
        File.AppendAllText(parameters.outpath,
                           parameters.ID.ToString() + "," +
                           parameters.trialNumber.ToString() + "," +
                           parameters.condition.ToString() + "," +
                           parameters.GetDelay().ToString() + "," +
                           parameters.GetAutoShootDelay().ToString() + "," +
                           parameters.GetForce().ToString() + "," +
                           parameters.GetBarrierPosition().ToString() + "," +
                           velocity.x.ToString() + "," +
                           velocity.y.ToString() + "," +
                           collisionY.ToString() + "," +
                           selection.ToString() + Environment.NewLine);
    }
}