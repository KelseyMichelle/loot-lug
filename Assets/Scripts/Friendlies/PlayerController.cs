using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Vector3 playerVelocity;

    public Rigidbody2D rb;
    public float playerSpeed = 2.0f;
    public float attackCooldown;
    public bool attacked;
    public bool canAttack;
    public float speed;
    public float maxSpeed;

    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

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
        rb.SetRotation(playerRotation);

        rb.velocity = rb.velocity + curMovement;

        // Changes the height position of the player..
        if (attacked && canAttack)
        {
            Attack();
        }
    }
}
