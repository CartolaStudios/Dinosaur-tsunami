using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject FX;

    public void Start()
    {
        Destroy(gameObject, 8);
    }
    void Update()
    {
        MoveTowardsDino();
    }

    void MoveTowardsDino()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
    public void Active()
    {
        var fxx=Instantiate(FX,transform.position=transform.position,Quaternion.identity);
        fxx.AddComponent<destroyer>();
        fxx.GetComponent<destroyer>().timeDestroyer=5;
        Destroy(gameObject);
    }
}