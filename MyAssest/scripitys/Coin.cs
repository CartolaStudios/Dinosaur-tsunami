using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public GameObject coin;
    public GameObject FX;
    void Update()
    {
        MoveTowardsDino();
        Destroy(gameObject,10);
    }

    void MoveTowardsDino()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        coin.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); 
    }
    public void Active()
    {
        var fxx = Instantiate(FX, transform.position = transform.position, Quaternion.identity);
        fxx.AddComponent<destroyer>();
        fxx.GetComponent<destroyer>().timeDestroyer = 5;
        Destroy(gameObject);
    }
}