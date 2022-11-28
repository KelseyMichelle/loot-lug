using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player physics
    private float speedNormalization = 2.0f;
    public Rigidbody2D rb;
    public float playerSpeed = 30f;
    public float speed;
    public float maxSpeed;

    // Player controls
    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    
    // Player -> balloon physics
    private Rigidbody2D balloon;
    private Text distanceText;
    public float minStretchDistance = 3;
    public float maxStretchDistance = 5;
    public float pullStrength = 0.1f;
    public float maxStrength = 0.5f;

    // Attacks
    public GameObject swordSwingPrefab;
    public float attackCooldown;
    private bool attacked;
    private bool canAttack;

    private void Start()
    {
        canAttack = true;
        attacked = false;
        balloon = GameObject.Find("Balloon").GetComponent<Rigidbody2D>();
        distanceText = GameObject.Find("Distance").GetComponent<Text>();

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        attacked = context.action.triggered;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Attack()
    {
        canAttack = false;
        var swordSwing = Instantiate(swordSwingPrefab, transform.position, transform.rotation);
        swordSwing.transform.parent = gameObject.transform;
        StartCoroutine(AttackCooldown());
    }


    IEnumerator AttackCooldown()
    {

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void FixedUpdate()
    {
        Vector2 curMovement = movementInput * speedNormalization * Time.deltaTime * playerSpeed;
        Vector2 curRotation = new Vector2(lookInput.y, -lookInput.x);
        Quaternion playerRotation = Quaternion.LookRotation(curRotation, Vector3.forward);

        var distanceFromBalloon = Vector2.Distance(transform.position, balloon.position);
        distanceText.text = distanceFromBalloon.ToString();


        rb.SetRotation(playerRotation);


        if (distanceFromBalloon > minStretchDistance)
        {
            var moveDirection = (rb.position - balloon.position).normalized;
            var forceStrength = (float)Math.Pow(Math.Min(pullStrength * (distanceFromBalloon - minStretchDistance), maxStrength),2);
            var forceApplied = forceStrength * (new Vector2(moveDirection.x, moveDirection.y));
            balloon.velocity += forceApplied * Time.deltaTime;
            rb.velocity -= forceApplied*Time.deltaTime;
        }

        rb.velocity = rb.velocity + curMovement;


        if (attacked && canAttack)
        {
            Attack();
        }

    }
}
