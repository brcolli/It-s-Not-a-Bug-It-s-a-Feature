﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ShootingController : MonoBehaviour
{

    [SerializeField] private GameObject Arrow;
    [SerializeField] private float Speed = 10.0f;
    private bool _charging = false;
    private float _chargeTimer = 1.0f;
    [SerializeField] private float FireRate = 100.0f;
    private float _fireNext = 0.0f;

    private Animator _anim;
    private Rigidbody2D _rb;
    private LineRenderer _lineRenderer;
    private GameObject _projectile;

    // Use this for initialization
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        FacingRight = GetComponent<PlayerController>().FacingRight; // Get current direction
        
        _anim.SetBool("Charging", _charging);
        _anim.SetBool("Shooting", false);
        if (Time.fixedTime > _fireNext)
        {
            if (Input.GetMouseButton(0))
            {
                _charging = true;
                _chargeTimer += Time.deltaTime;

                // Gets mouse location in world coordinates
                Vector2 currMouse = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

                Vector2 currPos = new Vector2(transform.position.x, transform.position.y);

                // If mouse behind player, turn player
                if ((currMouse.x < currPos.x && FacingRight) || (currMouse.x > currPos.x && !FacingRight))
                    GetComponent<PlayerController>().Flip();

                Vector2 currDir = currMouse - currPos;
                currDir.Normalize();

                float currPower = Speed*_chargeTimer*3.4f;
                if (currPower > 70.0f)
                    currPower = 70.0f;

                // Trace path of projectile
                //_tracer.UpdateTrajectory(transform.position, currDir, currPower, 0.1f, 100.0f);
                UpdateTrajectory(currPos, currDir*currPower, 1.0f, Physics2D.gravity, Vector3.zero);
            }
            else if (_charging)
            {
                _anim.SetBool("Shooting", true);
                _charging = !_charging;
                _fireNext = Time.fixedTime + FireRate;

                // Gets mouse location in world coordinates
                Vector2 target =
                    Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

                // Gets position of character
                Vector2 myPos =
                    new Vector2(transform.position.x, transform.position.y);

                // Get direction based off of mouse position and player location
                Vector2 direction = target - myPos;
                direction.Normalize(); // Normalize

                // If mouse behind player, turn player
                if ((target.x < myPos.x && FacingRight) || (target.x > myPos.x && !FacingRight))
                    GetComponent<PlayerController>().Flip();

                // Make projectile's initial direction point to mouse
                Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg);

                // Instantiate projectile and give velocity
                _projectile = (GameObject) Instantiate(Arrow, myPos, rotation);
                _rb = _projectile.GetComponent<Rigidbody2D>();
                _projectile.GetComponent<ProjectileController>().Player = this;

                Vector2 velocity = _rb.velocity;
                float angle = Mathf.Atan2(velocity.y, velocity.x)*Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Power = Speed*_chargeTimer*3.4f;
                if (Power > 70.0f)
                    Power = 70.0f; // Max power

                _rb.velocity = transform.TransformDirection(direction*Power);
                _chargeTimer = 1.0f;
                _lineRenderer.SetVertexCount(0); // Remove line renderer
            }
        }
    }

    // Print out a predicted trajectory
    private void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, float mass, Vector3 gravity, Vector3 wind)
    {
        int numSteps = 500; // for example
        float timeDelta = 1.0f / initialVelocity.magnitude; // for example
        
        // For drag, if desired
        Vector3 gravityWind = gravity + (wind / mass);

        // Get LineRenderer component and initialize
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetVertexCount(numSteps);

        // Initialize position and velocity tracers
        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;

        // Trace
        for (int i = 0; i < numSteps; ++i)
        {
            _lineRenderer.SetPosition(i, position);

            position += velocity * timeDelta + 0.5f * gravityWind * timeDelta * timeDelta;
            velocity += gravityWind * timeDelta;
        }
    }

    /* Getters and Setters */

    public float Power { get; set; }

    public bool FacingRight { get; set; }
}
