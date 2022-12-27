using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimiter : MonoBehaviour
{
    public float speedLimite;

    private Rigidbody2D _rb2d;
    // Start is called before the first frame update
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_rb2d.velocity.x > speedLimite || _rb2d.velocity.y > speedLimite)
        {
            Vector2 maxV = new Vector2(Mathf.Clamp(_rb2d.velocity.x, -speedLimite, speedLimite), Mathf.Clamp(_rb2d.velocity.y, -speedLimite, speedLimite));
            _rb2d.velocity = maxV;
        }
    }
}
