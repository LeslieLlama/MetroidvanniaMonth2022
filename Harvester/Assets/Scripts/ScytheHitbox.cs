using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheHitbox : MonoBehaviour
{
    public BoxCollider2D col;
    private GameObject player;
    private float playerDist;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        player = PlayerDragJump.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        //find the angle of the player cursor, set roation to it
        

        //set collider Y offset to half of the distance between player and cursor
        //set Y size equeal to distance between 

        playerDist = (player.transform.position - gameObject.transform.position).magnitude;

        col.offset = new Vector2(0, -playerDist/2);
        col.size = new Vector2(2,playerDist);

        //transform.up = transform.position - player.transform.position;



        // LookAt 2D
        // get the angle
        Vector3 norTar = (transform.position - player.transform.position).normalized;
        float angle = Mathf.Atan2(norTar.y, norTar.x) * Mathf.Rad2Deg;
        // rotate to angle
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0, angle - 90);
        transform.rotation = rotation;




    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy")
        {
            print("oh yeah");
            PlayerDragJump.Instance.canSlashJump = 2.5f;
        }
    }
}
