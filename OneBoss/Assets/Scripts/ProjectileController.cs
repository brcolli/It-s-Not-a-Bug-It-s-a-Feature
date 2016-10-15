using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
    // Object that instantiated projectile
    public ShootingController Player;

    /* For detecting platform collisions */
    private const float PlatformCheckRadius = 0.2f;
    [SerializeField] private Transform PlatformCheck;
    [SerializeField] private LayerMask DefinePlatform;

    private Rigidbody2D _rb;
    [SerializeField] private GameObject ArrowSprite;

	// Use this for initialization
    private void Start()
    {
        Destroy(gameObject, 5.0f); // Destroy after 5 seconds

        _rb = GetComponent<Rigidbody2D>();

        // Set damage based on power of shot
        if (Player.Power >= 70.0f)
            Damage = 30.0f;
        else
            Damage = 10.0f;

        // Say whether enemy shot or friendly shot
        if (Player.CompareTag("Player"))
            gameObject.tag = "PlayerProjectile";
        else
            gameObject.tag = "EnemyProjectile";
    }

    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(transform.position - (Vector3)_rb.velocity.normalized);

        // Rotate based on direction
        if (Player.FacingRight)
            transform.Rotate(0, -90, 0);
        else
            transform.Rotate(0, 90, 0);
        
        // Rotate arrow according to path
        /*if ((Player.FacingRight && _rb.velocity.normalized.x > 0.0f) || (!Player.FacingRight && _rb.velocity.normalized.x > 0.0f))
            transform.up = Vector3.Slerp(transform.up, _rb.velocity.normalized, Time.deltaTime);
        else
            transform.up = Vector3.Slerp(transform.up, -_rb.velocity.normalized, Time.deltaTime);*/
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Hit enemy
        if (collision2D.gameObject.layer.Equals(12))
            Destroy(gameObject);
        else if (collision2D.gameObject.layer.Equals(10)) // Hit platform
        {
            Instantiate(ArrowSprite, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    /* Getters and Setters */

    // Damage
    public float Damage { get; set; }
}
