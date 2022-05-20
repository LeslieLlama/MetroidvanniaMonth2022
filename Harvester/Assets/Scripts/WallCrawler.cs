using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCrawler : MonoBehaviour
{

    Rigidbody2D rb;
    public float walkSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();     
    }

    // Update is called once per frame
    void Update()
    {

        rb.AddForce(transform.right * walkSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.rotation = collision.transform.rotation;
    }
}
