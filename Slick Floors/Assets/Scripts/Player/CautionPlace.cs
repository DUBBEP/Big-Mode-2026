using UnityEngine;

public class CautionPlace : MonoBehaviour
{
    public GameObject cautionSign;
    public MopHandle mopHandleControls;
    [SerializeField] float placeSpeed = 10f;
    [SerializeField] float spawnHeight = 1f;
    [SerializeField] private AudioClip placeSignSoundFXClip;

    private Animator anim;
    private int _signCount = 0;
    private bool _isHolding;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AddSign()
    {
        _signCount++;
        gameObject.SetActive(true);
    }

    public void HandleInput(bool isPressed)
    {
        if (isPressed)
        {
            if (!_isHolding && _signCount > 0)
                StartHolding();
        }
        else
        {
            if (_isHolding)
                StopHolding();
        }
    }

    private void StartHolding()
    {
        _isHolding = true;
        mopHandleControls.holdSign(gameObject.transform);
        anim.SetBool("Place", true);
    }

    private void StopHolding()
    {
        _isHolding = false;
        mopHandleControls.releaseSign();
        anim.SetBool("Place", false);
        anim.SetBool("PlaceLeft", false);
        anim.SetBool("PlaceRight", false);
    }

    public void spin()
    {
        if (mopHandleControls.handsInversed)
        {
            anim.SetBool("PlaceLeft", true);
        }
        else
        {
            anim.SetBool("PlaceRight", true);
        }
    }
    public void placeSign()
    {
        if (cautionSign == null) return;

        // Perform spawn
        Vector3 spawnPos = transform.position;
        spawnPos.y -= spawnHeight;

        GameObject sign = Instantiate(cautionSign, spawnPos, Quaternion.identity);
        if (sign != null)
        {
            Transform signChild = sign.transform.Find("Sign");
            if (signChild != null)
            {
                Rigidbody2D rb = signChild.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 1f;
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.linearVelocity = Vector2.down * placeSpeed;
                    SoundFXManager.Instance.playSoundFXClip(placeSignSoundFXClip, this.transform, volume: 2.0f);
                }

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
            }
        }

        _signCount--;
        StopHolding();

        if (_signCount <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
