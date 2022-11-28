using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector3 playerVelocity;

    public Rigidbody2D rb;
    public float playerSpeed = 30f;
    public float attackCooldown;
    public bool attacked;
    public bool canAttack;
    public float speed;
    public float maxSpeed;
    public float minStretchDistance = 3;
    public float maxStretchDistance = 5;
    public float pullStrength = 0.1f;
    public float maxStrength = 0.5f;

    private Rigidbody2D balloon;
    private Text distanceText;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    

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
        //Instantiate(SwordSwingPrefab, transform.position, transform.rotation);
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void FixedUpdate()
    {
        Vector2 curMovement = movementInput * playerSpeed * Time.deltaTime * speed;
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

        // Changes the height position of the   player..
        if (attacked && canAttack)
        {
            Attack();
        }

    }
}
