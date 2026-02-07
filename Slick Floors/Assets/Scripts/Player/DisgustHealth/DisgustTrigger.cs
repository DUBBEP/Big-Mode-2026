using UnityEngine;

public class DisgustTrigger : MonoBehaviour
{
    [SerializeField] private DisgustController disgustController;
    [SerializeField] private PlayerMovement movement;
    [Tooltip("Seconds until tick damage applies")]
    [SerializeField] private float disgustTickRate;
    private bool tickDamageActive;
    private float tickTimer;
    private GameObject cachedTickObject;
    private System.Collections.Generic.HashSet<GameObject> processedPickups = new System.Collections.Generic.HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cachedTickObject = collision.gameObject;
        tickDamageActive = OtherHasDamageComponent(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) =>
        tickDamageActive = OtherHasDamageComponent(collision.gameObject);

    private void OnTriggerExit2D(Collider2D collision) =>
        tickDamageActive = false;

    private bool OtherHasDamageComponent(GameObject obj) => obj.GetComponent<IDamageSource>() != null;

    private void FixedUpdate()
    {
        if (!tickDamageActive)
        {
            if (tickTimer > 0) tickTimer -= Time.deltaTime;
            return;
        }

        tickTimer += Time.deltaTime;

        if (tickTimer > disgustTickRate && 
            cachedTickObject != null &&
            cachedTickObject.TryGetComponent<IDamageSource>(out IDamageSource src))
        {
            TakeDamage(src);
            tickTimer = 0;
        }
    }

    private void TakeDamage(IDamageSource sourceObject)
    {
        DamageSource src = sourceObject.GetDamageSource();
        src.recievingObject = gameObject;
        disgustController.AddDisgust(src);
    }
}