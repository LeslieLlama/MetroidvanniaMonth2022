using UnityEngine;
using System.Collections;

public class CrawlerBehaviour : MonoBehaviour {

	public float moveSpeed = .2f;
	//private CircleCollider2D collider;
	private float crawlerRadius;
    //private Vector2 crawlerCenter;
    public float raycastLength;
	public LayerMask obstacles;
	private Vector3 previousPosition;
	private bool hasStuck = false;
    CircleCollider2D col;
    Rigidbody2D rb;

	void Start () {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
		crawlerRadius = col.radius;
		//previousPosition = transform.position;
		//crawlerCenter = collider.center;
	}
/*
	void OnCollisionEnter2D(Collision2D coll) {
		//if (coll.gameObject.layer != "Obstacle") {
		//	return;
		//}
		Debug.Log ("###" + coll.contacts.Length);
		var point = coll.contacts[0];
		//Debug.Log ("###" + point);
		Debug.DrawRay (point.point, point.normal, Color.black, 10000);
		Vector2 pos = transform.position;
		var vect = point.point - pos;
		var angle = Vector3.Angle (-transform.up, vect);
		Debug.Log ("###" + angle);
//		if (angle >= 80) {
//		}
	}
*/	
	void Update () {
		Vector2 up = transform.up;

		Vector2 frontPoint = transform.position;
		//frontPoint.x += (crawlerRadius * transform.lossyScale.x);

		RaycastHit2D hit = Physics2D.Raycast (frontPoint, -up*raycastLength, crawlerRadius, obstacles);
		Debug.DrawRay (frontPoint, -up * raycastLength, Color.red);

		if (hit.collider != null) {
			//just debug info
			Debug.DrawRay (hit.point, hit.normal, Color.yellow);
			Vector3 moveDirection = Quaternion.Euler (0, 0, -90) * hit.normal;
			Debug.DrawRay (frontPoint, moveDirection * raycastLength, Color.white);

			//var collisions = Physics2D.OverlapCircleAll(transform.position, crawlerRadius);
			//Debug.Log (collisions.);

			//stick to the obstacle
			rb.AddForce (-2 * hit.normal);
			//move forward
			rb.AddForce(moveSpeed * moveDirection);


			var len = crawlerRadius * transform.lossyScale.x * 2;
			RaycastHit2D hit2 = Physics2D.Raycast (frontPoint, moveDirection * raycastLength, len, obstacles);
			if(hit2.collider != null){
				//Debug.Log ("### Obstacle forward. Rotating");
				Quaternion targetRotation2 = Quaternion.FromToRotation (Vector3.up, hit2.normal);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation2, Time.deltaTime);

            } else {
				//rotate crawler according to obstacle angle under it
				Quaternion targetRotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime);
				//Debug.Log ("### No obstcacle...");
			}


		} else {
			//no ground under object - falling.
			rb.AddForce(Vector3.down);
            transform.rotation = new Quaternion(0,0,0,0);
		}



/*
		Debug.Log ("###" + transform.position + ";" + previousPosition);
		//previousPosition = transform.position;
		if (previousPosition != transform.position) {
			previousPosition = transform.position;
		}
*/
/*
		if (transform.position == previousPosition) {
			hasStuck = true;
		} else {
			previousPosition = transform.position;
			hasStuck = false;
		}
	*/
	}

	void FixedUpdate () {
		//rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);
	}

}
