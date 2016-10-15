using UnityEngine;
using System.Collections;

public class ShootingController : MonoBehaviour
{

    [SerializeField] private GameObject Bullet;
    [SerializeField] private float Speed;
    [SerializeField] private float FireRate;
    private float _fireNext = 0.0f;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.fixedTime > _fireNext)
	    {
	        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.L))
	        {
	            _fireNext = Time.fixedTime + FireRate;

	            GameObject projectile = (GameObject) Instantiate(Bullet, gameObject.transform.position, Quaternion.identity);
	            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

	            rb.velocity = new Vector2(Speed, 0);
	        }
	    }
	}
}
