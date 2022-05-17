using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Rigidbody2D rb;

    [Space]
    [Header("CursorVaribles")]
    //cursor varibles
    private Vector3 CurrentPos;
    private Vector3 pos; //transform.position but updated through update()
    private Vector3 direction; //direction between mouse cursor and anchor
    private Vector3 directionLocalised; //direction between ghost cursor and player transform
    private Vector3 newLocation;
    public float cursorRadius = 5f;
    public float minCursorRadius = 0.5f;
    private bool cursorCanMove = false;


    [Space]
    [Header("jumping stuff")]
    //jumping stuff
    public float jumpingPower = 280f;
    public float slowMoSpeed = 0.5f;
    public float grav; //the value to reset gravity back to from 0 when it's modified
    public int jumpCount;
    public int maxJumpCount = 1;
    public float canSlashJump;
    public Text slashJumptext;

    private GameObject AnchorToSpawn;
    private GameObject CursorToSpawn;
    public GameObject GhostCursorToSpawn;


    public static PlayerDragJump Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of the player!");
            return;
        }

        Instance = this;
    }


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //cam = Camera.main;
        col = GetComponent<Collision>();
        spr = GetComponent<SpriteRenderer>();

        lr = gameObject.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = Color.clear;

        grav = rb.gravityScale;

        ObjectCreation();

    }


    void Update()
    {
        AddMomentum();
        WallGrab();

        if (canSlashJump >= 0)
        {
            canSlashJump -= Time.deltaTime;
        }

        slashJumptext.text = "" + canSlashJump;
    }

    void ObjectCreation()
    {
        AnchorToSpawn = new GameObject("AnchorInst");
        CursorToSpawn = new GameObject("CursorInst");
        //GhostCursorToSpawn = new GameObject("GhostCursorInst");
    }

    void AddMomentum()
    {
        pos = transform.position;
        if (cursorCanMove == true)
        {
            GhostCursorToSpawn.transform.position = newLocation;
        }
        else
        {
            GhostCursorToSpawn.transform.position = transform.position;
        }
        CursorUpdate(); //updates the position of the created cursor object

        //Line renderer update
        Vector3[] positions = new Vector3[2];
        positions[0] = pos;
        positions[1] = GhostCursorToSpawn.transform.position;
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);

        //this is the vector between the Anchor and Cursor, which is the direction the player wants to/will move
        direction = CursorToSpawn.transform.position - AnchorToSpawn.transform.position;
        directionLocalised = GhostCursorToSpawn.transform.position - transform.position;

    }

    void CursorUpdate() //update function for cursor spesifically, seperated into a function so its easier to read
    {

        var p = Input.mousePosition;
        var depth = 1f;
        CurrentPos = cam.ScreenToWorldPoint(new Vector3(p.x, p.y, depth));
        CursorToSpawn.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);



        newLocation = direction + pos; //get the direction vector of "direction" and centers it on the player transform
        Vector3 centerPosition = transform.localPosition; //center of player position
        float distance = Vector3.Distance(newLocation, centerPosition); //distance from ghost cursor to the maxium radius

        if (distance > cursorRadius) //If the distance is less than the radius, it is already within the radius.
        {
            Vector3 fromOriginToObject = newLocation - centerPosition; //~cursor position~ - *radius center*
            fromOriginToObject *= cursorRadius / distance; //Multiply by radius //Divide by Distance
            newLocation = centerPosition + fromOriginToObject; //*player position with radius* + all that Math
        }

        if (Input.GetMouseButtonDown(0))
        {
            AnchorUpdate();
            lr.material.color = Color.magenta;
            cursorCanMove = true;
            if (col.onCeiling == false && col.onGround == false && col.onWall == false)
            {
                Time.timeScale = slowMoSpeed;
            }
            AnchorUpdate();
            lr.material.color = Color.magenta;

        }

        if (Input.GetMouseButtonUp(0))
        {
            lr.material.color = Color.clear;

            Time.timeScale = 1f;
            if (direction.x > 0) { spr.flipX = true; }
            if (direction.x <= 0) { spr.flipX = false; }


            if (directionLocalised.x > minCursorRadius || directionLocalised.x < -minCursorRadius || directionLocalised.y > minCursorRadius || directionLocalised.y < -minCursorRadius)
            {
                rb.gravityScale = grav;
                rb.velocity = new Vector3(0, 0, 0);
                rb.AddForce(jumpingPower * directionLocalised);

                jumpCount -= 1;
            }
            cursorCanMove = false;

            lr.material.color = Color.clear;

            rb.AddForce(400 * direction);
        }
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
            rb.gravityScale = grav;
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

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && canSlashJump >= 0)
        {
            StartCoroutine(WaitAndPrint(collision));
        }
    }

    IEnumerator WaitAndPrint(Collider2D collision)
    {
        collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        rb.velocity = new Vector3(0, 0, 0);
        rb.AddForce(jumpingPower * direction);
        canSlashJump = 0;
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(1);
        collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        

    }

}
