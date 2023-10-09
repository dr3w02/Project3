using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSkull : MonoBehaviour
{


    public int skullIndex;
    public SkullSmash currentSkull;


    public int playerIndex;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] PlayerInput _playerInput;
    public float speed;

    public Animator hammer2;

    public Animator hammer1;


    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        //Subscribe to Event
        SoulFireInputSystem.current.players[playerIndex].onSouthPressed += Wack;
    }

    private void OnDisable()
    {
        //Unsubscribe to Event
        SoulFireInputSystem.current.players[playerIndex].onSouthPressed -= Wack;
    }

    private void FixedUpdate()
    {

        //Get the analog movement
        Vector2 moveDirection = SoulFireInputSystem.current.players[playerIndex].movementDirection;
        moveDirection *= Time.deltaTime * speed;

        _rb.MovePosition(moveDirection + (Vector2)transform.position);

    }


    public void Wack()
    {
        Debug.Log($"Wack - Player {playerIndex}");

        if (currentSkull != null)
        {
            currentSkull.HitSkull(skullIndex);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Smash")
        {
            currentSkull = collision.GetComponent<SkullSmash>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Smash")
        {
            currentSkull = null;
        }
    }




}



