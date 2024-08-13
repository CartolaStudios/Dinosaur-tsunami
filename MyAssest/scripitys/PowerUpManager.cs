using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    private DinoManager dinoManager;
    public GameObject bigDinoPrefab; // Prefab do dinossauro grande
    private GameObject activeBigDino;
    private bool isBigDinoActive = false;
    private Dino originalFirstDino;
    public PowerUpName powerUpActive;
    public AudioClip AlertFinal;
    AudioSource AudioSource;
    void Start()
    {
        dinoManager = FindObjectOfType<DinoManager>();
        gameObject.AddComponent<AudioSource>();
        AudioSource = GetComponent<AudioSource>();
    }

    public void ActivateBigDinoPowerUp(float duration, PowerUpName name)
    {
        StartCoroutine(BigDinoPowerUp(duration));
        powerUpActive = name;
    }

    private IEnumerator BigDinoPowerUp(float duration)
    {
        isBigDinoActive = true;

        // Desativar todos os dinossauros atuais
        foreach (var dino in dinoManager.dinos)
        {
            dino.gameObject.SetActive(false);
        }

        // Spawn do dinossauro grande
        activeBigDino = Instantiate(bigDinoPrefab, dinoManager.formationPoint.position, Quaternion.identity);
        Dino bigDinoScript = activeBigDino.GetComponent<Dino>();
        bigDinoScript.SetManager(dinoManager);

        // Substituir o primeiro dinossauro pelo dinossauro grande na lista
        originalFirstDino = dinoManager.dinos[0];
        dinoManager.dinos[0] = bigDinoScript;

        // Aguardar a duração do power-up
        yield return new WaitForSeconds(duration - 5);
        var durationFinalTime = duration - 5 - duration;
        AudioSource.clip = AlertFinal;
        AudioSource.Play();
        yield return new WaitForSeconds(-durationFinalTime);
        // Reativar os dinossauros normais
        foreach (var dino in dinoManager.dinos)
        {
            dino.gameObject.SetActive(true);
        }

        powerUpActive = PowerUpName.none;

        // Restaurar o estado original
        dinoManager.dinos[0] = originalFirstDino;
        originalFirstDino.gameObject.SetActive(true);
        Destroy(activeBigDino);
        isBigDinoActive = false;
        dinoManager.UpdateDinoPositions();
    }

    public void OnDinoAdded(Dino newDino)
    {
        if (isBigDinoActive)
        {
            newDino.gameObject.SetActive(false);
        }
    }
}