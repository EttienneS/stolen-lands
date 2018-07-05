using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(1, 20)]
    public int Speed = 5;

    [Range(10, 50)]
    public int ZoomMin = 10;

    [Range(50, 500)]
    public int ZoomMax = 500;

    [Range(50, 300)]
    public int ZoomStep = 100;

    private Camera Camera;

    public void OnEnable()
    {
        Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        var xMove = Input.GetAxis("Horizontal") * Speed;
        var yMove = Input.GetAxis("Vertical") * Speed;

        var z = (-1 * Mathf.Clamp(-transform.position.z - Input.GetAxis("Mouse ScrollWheel") * ZoomStep, ZoomMin, ZoomMax));

        transform.position = new Vector3(transform.position.x + xMove,
                                        transform.position.y + yMove, z);
    }
}