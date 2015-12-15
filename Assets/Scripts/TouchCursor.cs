using UnityEngine;

public class TouchCursor : MonoBehaviour
{
    private static int zPostion = -3;
    private int ID = -1;
    private bool isTouchDown = false;
    private bool isTouchUp = false;
    private SurfaceManager surfaceManager;

    public bool isDown()
    {
        if (isTouchDown)
        {
            isTouchDown = false;
            return true;
        }
        return false;
    }

    public bool isUp()
    {
        if (isTouchUp)
        {
            isTouchUp = false;
            return true;
        }
        return false;
    }

    // Unity3d bottom left 0,0. Surface top left 0,0
    private Vector3 SurfaceToWorldPoint(Vector2 screenPoints)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(screenPoints.x,
                                                                      Screen.height - screenPoints.y,
                                                                      Camera.main.nearClipPlane));
        position.z = zPostion;
        return position;
    }

    private void Awake()
    {
        surfaceManager = SurfaceManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt) && Input.GetKeyUp(KeyCode.F4))
        {
            Application.LoadLevel("MainMenu");
        }

        if (surfaceManager.isConnected())
        {
            Touch touchDown = surfaceManager.getTouchDown();
            if (touchDown != null)
            {
                if (ID == -1)
                {
                    isTouchDown = true;
                    Vector3 position = SurfaceToWorldPoint(touchDown.getPosition());
                    ID = touchDown.getID();
                    transform.position = position;
                }
            }

            Touch touchUp = surfaceManager.getTouchUp();
            if (touchUp != null)
            {
                if (ID == touchUp.getID())
                {
                    isTouchUp = true;
                    ID = -1;
                }
            }

            if (ID != -1)
            {
                Vector2 worldPosition = surfaceManager.getTouchMove(ID);
                transform.position = SurfaceToWorldPoint(worldPosition);
            }
        }
    }
}