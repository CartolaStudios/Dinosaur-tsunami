using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lifetime = 10f; // Tempo que o pr�dio ficar� ativo antes de ser destru�do

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroi o pr�dio ap�s o tempo de vida
    }

    void Update()
    {
        MoveBuilding();
    }

    void MoveBuilding()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }
}