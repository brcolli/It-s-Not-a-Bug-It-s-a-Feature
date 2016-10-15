using UnityEngine;
using System.Collections;

public class StrikeController : MonoBehaviour
{

    private Animator _anim;

	// Use this for initialization
	void Start ()
	{
	    _anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(1))
	    {
	        _anim.SetBool("Striking", true);
	    }
        else
            _anim.SetBool("Striking", false);
	}
}
