using UnityEngine;

public class Flick : PuckBehavior
{
    private Vector2 forceVector = new Vector2();
    private Vector2 prevPosition = new Vector2();

    protected override void OnFixedUpdate()
    {
        Ray ray = new Ray(Cursor.transform.position, Vector3.forward);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            if (touchCursor.isDown())
            {
                forceVector = hit.point;
                prevPosition = hit.point;
            }
            else if (touchCursor.isUp())
            {
                forceVector = (hit.point - forceVector).normalized;
                if (forceVector.magnitude > 0)
                {
                    rb2D.AddForce(forceVector * parameters.GetForce());
                    gameObject.layer = 2; // Ignore Raycast
                }
            }
            else
            {
                rb2D.MovePosition(hit.point - prevPosition + rb2D.position);
                prevPosition = hit.point;
            }
        }
    }
}