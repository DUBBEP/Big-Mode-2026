using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;

public class CautionPlace : MonoBehaviour
{
    public GameObject cautionSign;
    public MopHandle mopHandleControls;
    [SerializeField] float placeSpeed = 10f;
    [SerializeField] float spawnHeight = 1f;
    private Animator anim;

    public void StartHolding()
    {
        mopHandleControls.holdSign(gameObject.transform);
    }

    public void spin()
    {
        anim = GetComponent<Animator>();
        if (anim.GetBool("Place"))
        {
            if (mopHandleControls.handsInversed)
            {
                anim.SetBool("PlaceLeft", true);
                anim.SetBool("Place", false);
            }
            else
            {
                anim.SetBool("PlaceRight", true);
                anim.SetBool("Place", false);
            }
        }
    }
    public void placeSign()
    {
        anim = GetComponent<Animator>();
        mopHandleControls.releaseSign();

        if (cautionSign == null) return;

        Vector3 spawnPos = transform.position;
        spawnPos.y -= spawnHeight;

        GameObject sign = Instantiate(cautionSign, spawnPos, Quaternion.identity);
        if (sign == null) return;

        Transform signChild = sign.transform.Find("Sign");
        if (signChild == null) return;

        Rigidbody2D rb = signChild.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.linearVelocity = Vector2.down * placeSpeed;
        }

        // Ignore collisions with player and mop temporarily
        Collider2D signCollider = signChild.GetComponent<Collider2D>();
        if (signCollider != null)
        {
            LayerMask obstacleMask = LayerMask.GetMask("Player", "Mop");
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, 5f, obstacleMask);

            foreach (Collider2D obstacle in obstacles)
            {
                Physics2D.IgnoreCollision(signCollider, obstacle, true);
            }
        }

        gameObject.SetActive(false);
    }
}
