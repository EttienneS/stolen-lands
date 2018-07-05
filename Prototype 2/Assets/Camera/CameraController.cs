using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(1, 20)]
    public int Speed = 5;

    [Range(100, 500)]
    public int Z = 300;

    [Range(30, 100)]
    public int ZoomMin = 35;

    [Range(30, 150)]
    public int ZoomMax = 150;

    [Range(10, 100)]
    public int ZoomStep = 50;

    private Camera Camera;

    public void OnEnable()
    {
        Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        var xMove = Input.GetAxis("Horizontal") * Speed;
        var yMove = Input.GetAxis("Vertical") * Speed;

        Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * ZoomStep, ZoomMin, ZoomMax);

        transform.position = new Vector3(transform.position.x + xMove, transform.position.y + yMove, -Z);
    }
}