using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpName
{
    none,
    BigDino // Adicionei o BigDino aqui
}

public class PowerUp : MonoBehaviour
{
    public PowerUpName powerUpType; // Tipo do power-up
    public float Duration=10; // Tipo do power-up
    private PowerUpManager powerUpManager;

    void Start()
    {
        powerUpManager = FindObjectOfType<PowerUpManager>();
        Destroy(gameObject, 10);
    }
    public void Update()
    {
        MoveTowardsDino();

    }
   
    void MoveTowardsDino()
    {
        transform.Translate(Vector3.left * 5 * Time.deltaTime);
    }
    public void Active()
    {
        if (powerUpType == PowerUpName.BigDino)
        {
            powerUpManager.ActivateBigDinoPowerUp(Duration, powerUpType); // Duração de 10 segundos
        }
        Destroy(gameObject);
    }
}