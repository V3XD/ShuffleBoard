using UnityEngine;

public class TouchBarrier : MonoBehaviour
{
    public GameObject RestartIdle;

    private void Start()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3((2 * Screen.width) / 3f,
                                                     Screen.height * 0.5f,
                                                     Camera.main.nearClipPlane));
        position.z = 0;
        transform.position = position;
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.layer = 2; // Ignore Raycast
        if (other.GetComponent<Rigidbody2D>().velocity.magnitude == 0)
        {
            RestartIdle.GetComponent<Renderer>().enabled = true;
            RestartIdle.layer = 0; // Default
        }
    }
}