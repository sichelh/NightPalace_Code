using UnityEngine;

public class CameraMouseOffset : MonoBehaviour
{
    public Transform cameraTarget; // 빈 GameObject
    public float offsetStrength = 1f;
    public float smoothing = 5f;

    private Vector3 initialPosition;

    void Start()
    {
        if (cameraTarget != null)
            initialPosition = cameraTarget.position;
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        Vector2 offsetFromCenter = (mousePosition - screenCenter) / screenCenter;

        Vector3 targetOffset = new Vector3(offsetFromCenter.x, offsetFromCenter.y, 0) * offsetStrength;
        Vector3 desiredPosition = initialPosition + targetOffset;

        if (cameraTarget != null)
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, desiredPosition, Time.deltaTime * smoothing);
    }
}
