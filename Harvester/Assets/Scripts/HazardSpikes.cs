using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpikes : MonoBehaviour
{
    public int id;
    public int damage = 1;
    public Transform resetPoint;

    private void Awake()
    {
        GameEvents.Instance.onHazardDamage += ResetPlayer;
    }

    void ResetPlayer(int id)
    {
        if (id == this.id)
        {
            StartCoroutine(MovePlayer());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameEvents.Instance.HazardDamage(id);
            GameEvents.Instance.TakeDamage(damage);
            GameEvents.Instance.FadeInOut();
        }
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onHazardDamage -= ResetPlayer;
    }

    IEnumerator MovePlayer()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerDragJump.Instance.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        PlayerDragJump.Instance.gameObject.transform.position = resetPoint.transform.position;
    }
}
