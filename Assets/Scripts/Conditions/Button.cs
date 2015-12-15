using UnityEngine;

public class Button : PuckBehavior
{
    protected override void OnFixedUpdate()
    {
        if (touchCursor.isUp())
        {
            Ray ray = new Ray(Cursor.transform.position, Vector3.forward);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null)
            {
                Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
                Vector2 forceVector = (GetCollisionPoint() - currentPosition).normalized;
                rb2D.AddForce(forceVector * parameters.GetForce());
                gameObject.layer = 2; // Ignore Raycast
            }
        }
    }
}