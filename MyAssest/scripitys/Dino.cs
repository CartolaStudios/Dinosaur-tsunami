using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : MonoBehaviour
{
    private DinoManager dinoManager;
    private Rigidbody rb;
    public GameObject Morto;
    public bool AddDino;
    public bool imortal;
    public bool voando;
    public bool atirandor;
    public Animator animator;
    private float moveDuration = 1.0f;
    public float hitDistance = 1.0f;
    public bool isGrounded;
    public float sphereRadius = 0.5f;
    public Vector3 localSphereOffset = Vector3.down;
    private bool isJumpChargedInAir;
    private Vector3 PreviuPosition;
    public float jumpForce; // Define jumpForce como público para ser setado no DinoManager
    public bool barrado;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dinoManager = FindObjectOfType<DinoManager>();
    }
    public void Barrado(bool b)
    {
        barrado = b;
    }




    public void SetManager(DinoManager manager)
    {
        dinoManager = manager;
    }
    public void Update()
    {
        CheckGroundStatus();
        if (barrado)
        {
            MoveTowardsDino();
        }
    }

    void MoveTowardsDino()
    {
        transform.Translate(Vector3.left * 5 * Time.deltaTime);
    }
    public void Jump(float force)
    {
        if (isGrounded && animator && rb)
        {
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            animator.Play("Jump");
            isGrounded = false; // Atualiza isGrounded ao pular
        }
    }

    public void CheckGroundStatus()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("Dino");
        Vector3 origin = transform.position + localSphereOffset; // Ajuste aqui

        if (Physics.SphereCast(origin, sphereRadius, Vector3.down, out hit, hitDistance, layerMask))
        {
            if (!isGrounded && isJumpChargedInAir)
            {
                dinoManager.StartCoroutine(dinoManager.JumpSequence(jumpForce));
                isJumpChargedInAir = false;
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SpecialArea"))
        {
            SpecialArea specialArea = collision.GetComponent<SpecialArea>();
            if (specialArea != null)
            {
                Barrado(true);
                specialArea.AddDino(this);
            }
            if (imortal)
            {
                specialArea.DestroyeImediat();
            }
        }
        if (collision.gameObject.CompareTag("Human"))
        {
            if (!imortal)
            {
                animator.Play("Attack");
            }
            if (AddDino)
            {
                Invoke("timeAddDino", 0);
                Destroy(collision.gameObject);
                collision.GetComponent<Human>().Active();
            }
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            dinoManager.AddCoin();
            collision.GetComponent<Coin>().Active();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            collision.GetComponent<PowerUp>().Active();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {

          
            if (collision.GetComponent<Trap>())
            {
                collision.GetComponent<Trap>().Active();
                if (!imortal)
                {
                    dinoManager.RemoveDino(this);
                    var morto = Instantiate(Morto, transform.position, Quaternion.identity);
                    morto.AddComponent<destroyer>();
                    morto.GetComponent<destroyer>().timeDestroyer = 5;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("SpecialArea"))
        {
            SpecialArea specialArea = collision.GetComponent<SpecialArea>();
            if (specialArea != null)
            {
                Barrado(false); specialArea.RemoveDino(this);
                StartMoveToPosition(PreviuPosition);
            }
        }
    }







    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + localSphereOffset, sphereRadius);
    }
    public void timeAddDino()
    {
        dinoManager.AddDino();
    }

    public void StartMoveToPosition(Vector3 targetPosition)
    {
        if (!barrado)
        {
            PreviuPosition = targetPosition;
            StartCoroutine(MoveToPosition(targetPosition));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float currentY = transform.position.y;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            newPosition.y = currentY;
            transform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetPosition.y = transform.position.y;
        transform.position = targetPosition;
    }
}