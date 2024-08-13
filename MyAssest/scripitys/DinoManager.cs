using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DinoManager : MonoBehaviour
{
    public GameObject dinoPrefab;
    public Transform formationPoint;
    public float horizontalSpacing = 1.0f;
    public float spacing = 1.0f;
    public float jumpDelay = 0.01f;
    public float maxJumpForce = 20f;
    public float maxChargeTime = 3f;

    public List<Dino> dinos = new List<Dino>();
    public float jumpCharge = 0f;
    public float jumpChargeSpeed = 0.5f;
    private bool isCharging = false;
    private float coinCurrent;
    private PowerUpManager powerUpManager;
    public Image CurrentjumpImagem;

    void Start()
    {
        powerUpManager = FindObjectOfType<PowerUpManager>();
    }

    public void updateFillImagemJump()
    {
        CurrentjumpImagem.fillAmount = jumpCharge / maxChargeTime;
    }

    void Update()
    {
        if (dinos.Count > 0)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space))
            {
                isCharging = true;
                jumpCharge = 0f;
            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetKeyUp(KeyCode.Space))
            {
                isCharging = false;
                float jumpForce = Mathf.Lerp(0, maxJumpForce, jumpCharge / maxChargeTime);
                StartCoroutine(JumpSequence(jumpForce));
                jumpCharge = 0f;
                updateFillImagemJump();
            }

            if (isCharging)
            {
                jumpCharge += jumpChargeSpeed * Time.deltaTime;
                jumpCharge = Mathf.Clamp(jumpCharge, 0, maxChargeTime);
                updateFillImagemJump();
            }
        }
    }

    public void AddCoin()
    {
        coinCurrent++;
    }

    public void AddDino()
    {
        Dino newDino = Instantiate(dinoPrefab, formationPoint.position, Quaternion.identity).GetComponent<Dino>();
        newDino.SetManager(this);
        dinos.Add(newDino);
        UpdateDinoPositions();

        // Notificar o PowerUpManager sobre o novo dinossauro
        powerUpManager.OnDinoAdded(newDino);
    }

    public void RemoveDino(Dino dino)
    {
        dinos.Remove(dino);
        Destroy(dino.gameObject);
        UpdateDinoPositions();
        Lose();
    }

    public void Lose()
    {
        if (dinos.Count <= 0)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }

    public void UpdateDinoPositions()
    {
        if (dinos.Count == 0) return;

        Dino dino1 = dinos[0];
        dino1.StartMoveToPosition(formationPoint.position);
        dino1.GetComponent<Dino>().AddDino = true;
        var dinoFilho = dino1.transform.GetChild(0);
        dinoFilho.GetComponent<AudioSource>().enabled = true;

        float maxHorizontalDistance = 1.0f;
        float maxZDistance = 2.0f;

        for (int i = 1; i < dinos.Count; i++)
        {
            float horizontalOffset = Random.Range(-maxHorizontalDistance, maxHorizontalDistance);
            float zOffset = -Mathf.Min(i * spacing, maxZDistance);

            Vector3 targetPosition = formationPoint.position + new Vector3(horizontalOffset, 0, zOffset);
            if (powerUpManager.powerUpActive != PowerUpName.BigDino)
            {
                dinos[i].StartMoveToPosition(targetPosition);
            }
        }
    }

    public IEnumerator JumpSequence(float jumpForce)
    {
        if (dinos.Count > 0)
        {
            for (int i = 0; i < dinos.Count; i++)
            {
                dinos[i].jumpForce = jumpForce; // Certifique-se de que a força do pulo seja passada
                dinos[i].Jump(jumpForce);
                yield return new WaitForSeconds(jumpDelay);
            }
        }
    }
}