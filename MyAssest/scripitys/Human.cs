using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public GameObject fx;
    public float moveSpeed = 5f;

    void Update()
    {
        MoveTowardsDino();
    }

    void MoveTowardsDino()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
   public  void Active()
    {
        var fxx = Instantiate(fx, transform.position = transform.position, Quaternion.identity);
        fxx.AddComponent<destroyer>();
        fxx.GetComponent<destroyer>().timeDestroyer = 5;
        Destroy(gameObject);
    }
}