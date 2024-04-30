using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    Rigidbody2D Rigid;
    [SerializeField] private float Movement_Speed = 10f;
    private float Movement = 0;
    [SerializeField] private float Max_Movement_Speed = 10f;
    private Vector3 Player_LocalScale;

    [SerializeField] private Sprite[] Spr_Player = new Sprite[2];

    void Start () 
    {
        Rigid = GetComponent<Rigidbody2D>();
        Player_LocalScale = transform.localScale;
    }
    
    void Update () 
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0) // Проверяем, нажата ли левая кнопка мыши или есть ли касание на экране
        {
            Vector3 position;
    
            // Если есть касание, используем его позицию, иначе используем позицию мыши
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                position = touch.position;
            }
            else
            {
                position = Input.mousePosition;
            }
    
            position.x = Mathf.Clamp(position.x, 0, Screen.width);
            position.y = Mathf.Clamp(position.y, 0, Screen.height);
        
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            Movement = (worldPosition.x - transform.position.x) * Movement_Speed;
            Movement = Mathf.Clamp(Movement, -Max_Movement_Speed, Max_Movement_Speed);
        
            if (Movement > 0)
                transform.localScale = new Vector3(Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
            else if (Movement < 0)
                transform.localScale = new Vector3(-Player_LocalScale.x, Player_LocalScale.y, Player_LocalScale.z);
        }
    }

    void FixedUpdate()
    {
        Vector2 Velocity = Rigid.velocity;
        Velocity.x = Movement;
        Rigid.velocity = Velocity;

        if (Velocity.y < 0)
        {
            GetComponent<SpriteRenderer>().sprite = Spr_Player[0];
            Propeller_Fall();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Spr_Player[1];
        }

        Vector3 Top_Left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        float Offset = 0.5f;

        if (transform.position.x > -Top_Left.x + Offset)
            transform.position = new Vector3(Top_Left.x - Offset, transform.position.y, transform.position.z);
        else if(transform.position.x < Top_Left.x - Offset)
            transform.position = new Vector3(-Top_Left.x + Offset, transform.position.y, transform.position.z);
    }

    void Propeller_Fall()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Active", false);
            transform.GetChild(0).GetComponent<Propeller>().Set_Fall(gameObject);
            transform.GetChild(0).parent = null;
        }
    }
}