using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControlerBasic : MonoBehaviour
{
    private float _update;
    
    // Use this for initialization
    void Start()
    {
        _update = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _update += Time.deltaTime;
        if (_update > 1.0f)
        {
            transform.position += new Vector3(1.0f, 0, 0);
            _update = 0.0f;
        }
    }
}
