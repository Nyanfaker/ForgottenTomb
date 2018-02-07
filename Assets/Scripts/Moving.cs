using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Moving : MonoBehaviour {

    public float moveTime = 0.1f;           
    public LayerMask m_blockingLayer;         
    BoxCollider2D boxCollider;      
    Rigidbody2D rb2D;               
    float inverseMoveTime;

    protected SpritePositionSetter sps;
    protected SpritePositionSetter weaponSPS;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        sps = GetComponentInChildren<SpritePositionSetter>();
        weaponSPS = GameObject.Find("Weapon").GetComponent<SpritePositionSetter>();

        inverseMoveTime = 1f / moveTime;
    }

    public RaycastHit2D Hit(int xDir, int yDir, LayerMask blockingLayer)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        RaycastHit2D hit;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        return hit;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, m_blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        if (hit.transform == null)
            return;
        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null) {
            OnCantMove(hitComponent);
        }
            
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
