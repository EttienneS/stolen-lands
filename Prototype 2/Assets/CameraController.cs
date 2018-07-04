using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int Speed = 5;

    void Update()
    {
        var xMove = Input.GetAxis("Horizontal") * Speed;
        var yMove = Input.GetAxis("Vertical") * Speed;

        transform.position = new Vector3(transform.position.x + xMove, transform.position.y + yMove, -10);
    }
}
