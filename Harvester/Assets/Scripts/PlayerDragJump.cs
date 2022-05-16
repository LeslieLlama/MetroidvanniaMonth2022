using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDragJump : MonoBehaviour
{
    // The target marker.
    public Transform target;
    public Camera cam;
    public LineRenderer lr;
    private Collision col;
    private SpriteRenderer spr;

    // Speed in units per sec.
    public float speed;
    public float friction = 0.9f;
    private int state = 1; // 1= freefall, 2=Cursor active, 
    private Rigidbody2D rb;

    //cursor varibles

    private Vector3 StartPos;
    public Vector3 CurrentPos;
    private Vector3 pos;
    private Vector3 offset;
    private Vector3 direction;

    private GameObject AnchorToSpawn;
    private GameObject CursorToSpawn;
    private GameObject GhostCursorToSpawn;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //cam = Camera.main;
        col = GetComponent<Collision>();
        spr = GetComponent<SpriteRenderer>();

        lr = gameObject.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = Color.clear;

        ObjectCreation();

    }



    void Update()
    {
        pos = transform.position;


        CursorUpdate(); //updates the position of the created cursor object


        //Line renderer update
        Vector3[] positions = new Vector3[2];
        positions[0] = pos;
        positions[1] = GhostCursorToSpawn.transform.position;
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);

        //this is the vector between the Anchor and Cursor, which is the direction the player wants to/will move
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - AnchorToSpawn.transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            AnchorUpdate();
            lr.material.color = Color.magenta;
        }

        if (Input.GetMouseButtonUp(0))
        {
            lr.material.color = Color.clear;

            rb.AddForce(400 * direction);
        }

        WallGrab();

    }

    void ObjectCreation()
    {
        AnchorToSpawn = new GameObject("AnchorInst");
        CursorToSpawn = new GameObject("CursorInst");
        GhostCursorToSpawn = new GameObject("GhostCursorInst");
    }


    void CursorUpdate()
    {
        var p = Input.mousePosition;
        var depth = 1f;
        CurrentPos = cam.ScreenToWorldPoint(new Vector3(p.x, p.y, depth));

        CursorToSpawn.transform.position = CurrentPos;
        GhostCursorToSpawn.transform.position = direction + pos;

    }
    void AnchorUpdate()
    {
        var p = Input.mousePosition;
        var depth = 1f;
        CurrentPos = cam.ScreenToWorldPoint(new Vector3(p.x, p.y, depth));
        AnchorToSpawn.transform.position = CurrentPos;
    }

    
    void WallGrab()
    {
        if (col.onGround == true || col.onCeiling == true || col.onWall == true)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }

        if (col.onRightWall == true) { spr.flipX = true; };
        if (col.onLeftWall == true) { spr.flipX = false; };

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) //6 is the ground layer here
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            rb.AddForce(400 * direction);
        }
    }

}
