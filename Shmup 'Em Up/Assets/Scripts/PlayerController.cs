using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerController : MonoBehaviour
{

    /* Health */
    public int TotalHealth = 100;
    private int CurrentHealth;

    /* Movement */
    [SerializeField] private float MaxSpeed;

    private Rigidbody2D _rb;

	// Use this for initialization
	void Start ()
	{
	    CurrentHealth = TotalHealth;

	    _rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (CurrentHealth <= 0)
            Destroy(gameObject);

        /*gameObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                                                    0);*/

        float moveH = Input.GetAxis("Horizontal");
	    float moveV = Input.GetAxis("Vertical");
        _rb.velocity = new Vector2(moveH * MaxSpeed, moveV * MaxSpeed);
    }
}
