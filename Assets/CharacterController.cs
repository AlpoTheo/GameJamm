using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // run-idle gecisi icin
    public float speed = 0.0f;
    private Rigidbody2D r2d;
    private Animator _animator;
    
    
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed = 1.0f;
        }
        else
        {
            speed = 0.0f;
        }
        
        _animator.SetFloat("speed", speed);
    }
}
