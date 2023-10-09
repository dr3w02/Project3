using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    public bool canMove;
    public float freezePlayerTimer = 1.0f;

    public int moleIndex;
    public int playerIndex;
    public Mole currentMole;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] PlayerInput _playerInput;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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

        if (canMove == false)
            return;

        //Get the analog movement
        Vector2 moveDirection = SoulFireInputSystem.current.players[playerIndex].movementDirection;
        moveDirection *= Time.deltaTime * speed;

        _rb.MovePosition(moveDirection + (Vector2)transform.position);

    }


    public void Wack()
    {
        if (canMove == false)
            return;

        Debug.Log($"Wack - Player {playerIndex}");

        if (currentMole != null)
        {
            currentMole.HitMole(moleIndex, this);
        }
    }

    public void FreezePlayer()
    {
        canMove = false;
        Invoke("UnFreezePlayer", freezePlayerTimer);
    }

    void UnFreezePlayer()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mole")
        {
            currentMole = collision.GetComponent<Mole>();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Mole")
        {
            currentMole = null;
        }
    }
}


