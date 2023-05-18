using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Create a static instance of the Main Camera 
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
