using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player physics
    private float speedNormalization = 2.0f;
    public Rigidbody2D rb;
    public float playerSpeed;
    // Player -> balloon physics
    public Rigidbody2D balloon;
    public Text distanceText;
    public float minStretchDistance;
    public float maxStretchDistance;
    // Player controls
    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    // Attacks
    public GameObject swordSwingPrefab;
    public float attackCooldown;
    private bool attacked;
    private bool canAttack;
    

    private void Start()
    {
        canAttack = true;
        attacked = false;
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
        Instantiate(swordSwingPrefab, transform.position, transform.rotation);
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
        distanceText.text = Vector2.Distance(transform.position, balloon.position).ToString();


        rb.SetRotation(playerRotation);

        rb.velocity = rb.velocity + curMovement;


        if (attacked && canAttack)
        {
            Attack();
        }
    }
}
