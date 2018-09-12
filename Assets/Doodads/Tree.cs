using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private MeshRenderer _leaves;
    private MeshRenderer _trunk;

    private MeshRenderer Leaves
    {
        get
        {
            if (_leaves == null)
            {
                _leaves = transform.Find("Leaves").GetComponent<MeshRenderer>();
            }

            return _leaves;
        }
    }

    private MeshRenderer Trunk
    {
        get
        {
            if (_trunk == null)
            {
                _trunk = transform.Find("Trunk").GetComponent<MeshRenderer>();
            }

            return _trunk;
        }
    }

    void Start()
    {
        Leaves.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.8f), Random.Range(0.8f, 1.2f));
        Leaves.transform.localPosition += new Vector3(0, (Leaves.transform.localScale.y / 2) - 0.5f, 0);

        transform.eulerAngles = new Vector3(-90, 0, 0);// Random.Range(5, 85)
    }

}
