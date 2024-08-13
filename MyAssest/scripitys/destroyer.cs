using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour
{
    public float timeDestroyer;
    void Start()
    {
        Destroy(gameObject,timeDestroyer);
    }
    void Update()
    {
        MoveTowardsDino();
    }
    void MoveTowardsDino()
    {
        transform.Translate(Vector3.left * 5 * Time.deltaTime);
    }
}
