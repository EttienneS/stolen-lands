using UnityEngine;

public class Pulse : MonoBehaviour
{
    [Range(0.01f, 1f)] public float Max = 0.1f;
    [Range(0.001f, 0.01f)] public float Strenght = 0.005f;

    private int _multiplier = 1;

    private Vector3 _initialScale;

    void Start()
    {
        _initialScale = transform.localScale;
    }

    void Update()
    {
        var change = Strenght * _multiplier;
        transform.localScale += new Vector3(change, change, change);

        if (Mathf.Abs(transform.localScale.x - _initialScale.x) > Max)
        {
            _multiplier *= -1;
        }
    }
}
