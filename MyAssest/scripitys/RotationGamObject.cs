using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationGamObject : MonoBehaviour
{
    public GameObject objc;
    public float velocitSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        objc.transform.Rotate(0, 0, velocitSpeed * Time.deltaTime);
    }
}
