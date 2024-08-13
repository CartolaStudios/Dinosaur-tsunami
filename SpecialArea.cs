using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialArea : MonoBehaviour
{
    public int maxDinoCount = 5; // Quantidade máxima de dinossauros
    public List<GameObject> humans; // Lista de prefabs de humanos diferentes
    public int numberOfHumansToLaunch = 2; // Quantidade de humanos a serem lançados
    public GameObject destructionFX; // Efeito visual para destruição
    public AudioClip destructionSound; // Som para destruição
    public TextMeshProUGUI QuantidadeText;
    public List<Dino> collidingDinos = new List<Dino>();
    private AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = destructionSound;
        QuantidadeText.text = maxDinoCount.ToString();
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



    public void AddDino(Dino dino)
    {

        if (!collidingDinos.Contains(dino))
        {
            collidingDinos.Add(dino);
            CheckDinoCount();
        }
    }

    public void RemoveDino(Dino dino)
    {
        if (collidingDinos.Contains(dino))
        {
            collidingDinos.Remove(dino);
            QuantidadeText.text = collidingDinos.Count.ToString() + "/ " + maxDinoCount.ToString();
        }
    }

    private void CheckDinoCount()
    {
        QuantidadeText.text = collidingDinos.Count.ToString() + "/ " + maxDinoCount.ToString();
        if (collidingDinos.Count >= maxDinoCount)
        {
            for (int i = 0; i < collidingDinos.Count; i++)
            {
                collidingDinos[i].Barrado(false);

                tag = "Untagged";
            }
            ActivateSpecialEffect();

            Destroy(gameObject);
        }
    }
    public void DestroyeImediat()
    {
            for (int i = 0; i < collidingDinos.Count; i++)
            {
                collidingDinos[i].Barrado(false);

                tag = "Untagged";
            }
            ActivateSpecialEffect();

            Destroy(gameObject);
        }

    private  void ActivateSpecialEffect()
    {
        // Instanciar o efeito especial
        if (destructionFX != null)
        {
            var fxInstance = Instantiate(destructionFX, transform.position, Quaternion.identity);
            fxInstance.AddComponent<destroyer>(); // Adiciona o script destroyer ao efeito
        }

        // Reproduzir o som de destruição
        if (audioSource != null && destructionSound != null)
        {
            audioSource.Play();
        }

        // Lançar humanos aleatórios
        for (int i = 0; i < numberOfHumansToLaunch; i++)
        {
            GameObject humanPrefab = humans[Random.Range(0, humans.Count)];
            GameObject humanInstance = Instantiate(humanPrefab, transform.position, Quaternion.identity);
            Rigidbody rb = humanInstance.GetComponent<Rigidbody>();
           
        }


        
    }

}