using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PuckButton;
    public GameObject PuckAuto;
    public GameObject PuckFlick;
    protected Parameters parameters;

    private void Awake()
    {
        parameters = Parameters.Instance;
    }

    private void Start()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 70,
                                                                      Screen.height * 0.5f,
                                                                      Camera.main.nearClipPlane));
        position.z = -1;
        transform.position = position;
        if (parameters.condition == 1)
        {
            Instantiate(PuckAuto, position, Quaternion.identity);
        }
        else if (parameters.condition == 2)
        {
            Instantiate(PuckButton, position, Quaternion.identity);
        }
        else if (parameters.condition == 3)
        {
            Instantiate(PuckFlick, position, Quaternion.identity);
        }
    }
}