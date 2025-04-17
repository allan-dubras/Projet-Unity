using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement horizontale")]
    [SerializeField] private PlayerControl controls; // On Déclare le Input System 
    [SerializeField] private Rigidbody2D rb; // On Déclare le rigibody2D 
    [SerializeField] private Vector2 moveInput; // C'est le Raw input receptionné depuis la manette
    [SerializeField] private float speed = 10; // Déclaration de la variable speed pour les mouvements du joueur
    [SerializeField] private float speedSprint = 15; // Déclaration de la variable speed pour les mouvements du joueur
    [SerializeField] private float endurance = 100; // Déclaration de la variable speed pour les mouvements du joueur
    [SerializeField] private Slider enduranceSlider;
    [SerializeField] private Animator animator;


    [Header("Saut")]
    [SerializeField] private float JumpForce = 6;// Déclaration de la variable pour la puissance de saut
    [SerializeField] private bool isGrounded;// Bool qui vérifie si il est bien au sol
    [SerializeField] private LayerMask Ground;// Layer pour permettre de détecter le sol
    [SerializeField] private Transform player;//avoir le transform du joueur 
    [SerializeField] private float distance = 0.6f;// distance du raycast

    public float Endurance { get => endurance; set => endurance = value; }

    private void Awake()
    {

        enduranceSlider = FindObjectOfType<Slider>();
        //On Initialise le playercontrol et le rigidbody2d
        controls = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        //Inscrit le Move
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();  //lambda expression
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;



        //Inscrit le Jump
        controls.Player.Jump.performed += ctx => Jump();

        //Enable l'input action 
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void Jump()
    {
        if (isGrounded && endurance >= 10)
        {
            //on multiplie le vecteur par une jumpforce et on mets un ForceMode2D en impulse
            rb.AddForce(new Vector2(0, 1) * JumpForce, ForceMode2D.Impulse);
            endurance -= 10;
            animator.SetBool("isJumping", true);
        }

    }
    private void Update()
    {
        if (!isGrounded) 
        {
            animator.SetBool("isJumping", false);
        }
        enduranceSlider.value = endurance;
        if (endurance > 100)
        {
            endurance = 100;
        }
        isGrounded = CheckCollision(Vector2.down, player, distance, Ground);
        Vector2 move = controls.Player.Move.ReadValue<Vector2>();

        bool isSprinting = controls.Player.Sprint.IsPressed() && endurance > 0 && move != Vector2.zero;
        float currentSpeed = isSprinting ? speedSprint : speed;
        transform.Translate(move.x * currentSpeed * Time.deltaTime, 0, 0);
        if (isSprinting)
        {
            endurance -= 20 * Time.deltaTime;
            animator.SetBool("isSprint", true);
        }
        else
        {
            animator.SetBool("isSprint", false);
        }
        if (moveInput.x != 0 && !isSprinting)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }



    private bool CheckCollision(Vector2 direction, Transform checkPoint, float distance, LayerMask layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, direction, distance, layerMask);  // Affiche le raycast en rouge s'il ne touche rien, en vert s'il touche le sol
        Color rayColor = hit.collider != null ? Color.green : Color.red;                           // Expression Terniaire
        Debug.DrawRay(checkPoint.position, direction * distance, rayColor);                       // Dessine un trait pour avoir le visuel sur la scene 
        return hit.collider != null;
    }
}
