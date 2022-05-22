using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCrawl : MonoBehaviour
{
    public float moveSpeed;
    public float raycastLength;

    private Rigidbody2D rb;

    Vector2 down;
    Vector2 right;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        down = new Vector2(0,-1);
        right = new Vector2(1,0);
    }

    // Update is called once per frame
    void Update()
    {
        

        

        RaycastHit2D grouundCheck = Physics2D.Raycast(transform.position, down, raycastLength);
        Debug.DrawRay(transform.position, down * raycastLength, Color.red);

        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, right, raycastLength);
        Debug.DrawRay(transform.position, right * raycastLength, Color.yellow);

        

        
        

        
        if (wallCheck.collider != null)
        {
            float hitAngle = Vector2.Angle(wallCheck.normal, transform.up);
            Debug.Log(hitAngle);
            transform.rotation = new Quaternion(0,0,hitAngle,0);
        }

        //rb.velocity = right * moveSpeed;
        
    }
}
