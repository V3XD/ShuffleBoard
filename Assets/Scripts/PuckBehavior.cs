using UnityEngine;

public class PuckBehavior : MonoBehaviour
{
    protected Rigidbody2D rb2D;
    protected GameObject Restart;
    protected GameObject PuckBarrier;
    protected GameObject Cursor;
    protected Parameters parameters;
    protected TouchCursor touchCursor;
    protected PuckBarrier puckBarrier;
    private Renderer restartRenderer;

    protected virtual void OnStart()
    {
    }

    protected virtual void OnFixedUpdate()
    {
    }

    protected virtual void OnUpdate()
    {
    }

    protected Vector2 GetCollisionPoint()
    {
        Vector2 point = new Vector2(PuckBarrier.transform.position.x, 0);
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(0,
                                                                      Screen.height,
                                                                      Camera.main.nearClipPlane));
        point.y = Random.Range(-position.y, position.y);
        return point;
    }

    private void Start()
    {
        parameters = Parameters.Instance;
        rb2D = GetComponent<Rigidbody2D>();
        Cursor = GameObject.Find("Cursor");
        PuckBarrier = GameObject.Find("puckBarrier");
        Restart = GameObject.Find("Restart");
        touchCursor = Cursor.GetComponent<TouchCursor>();
        restartRenderer = Restart.GetComponent<Renderer>();
        puckBarrier = PuckBarrier.GetComponent<PuckBarrier>();
        OnStart();
    }

    private void FixedUpdate()
    {
        if (PuckBarrier.layer == 0)
        {
            Vector2 velocity = rb2D.velocity;
            if (velocity.magnitude > 0) // Has been shot
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity.normalized);
                //Debug.DrawRay(transform.position, velocity.normalized, Color.green);
                if (hit.collider != null)
                {
                    if (parameters.GetDelay() < 0)
                    {
                        float time = hit.distance / velocity.magnitude;
                        if (time <= Mathf.Abs(parameters.GetDelay()))
                        {
                            PuckBarrier.GetComponent<AudioSource>().Play();
                            PuckBarrier.layer = 2; // Ignore Raycast
                        }
                    }
                }
                else
                {
                    puckBarrier.EnableButton(Restart);
                    PuckBarrier.layer = 2; // Ignore Raycast
                }
            }
        }

        OnFixedUpdate();
    }

    private void Update()
    {
        if (restartRenderer.enabled)
        {
            Ray ray = new Ray(Cursor.transform.position, Vector3.forward);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }

        OnUpdate();
    }

    private void OnBecameInvisible()
    {
        rb2D.velocity = new Vector2();
    }
}