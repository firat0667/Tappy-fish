using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float Speed;
    int angle;
    int maxAngle = 20;
    int minAngle = -60;
    public Score score;
    public GameManager GameManager;
    public ObstacleSpawner ObstacleSpawner;
    public Sprite FishDied;
    SpriteRenderer sp;
    Animator anim;
    [SerializeField] private AudioSource swim, hit, point;

    bool touchedGround;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        FishSwim();
    }

    private void FixedUpdate()
    {
        FishRotation();
    }

    void FishSwim()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.gameOver == false)
        {
            swim.Play();
            if (GameManager.gameStarted == false)
            {
                _rb.gravityScale = 5f;
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, Speed);
                ObstacleSpawner.InstantiateObstacle();
                GameManager.GameHasStarted();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _rb.velocity = new Vector2(_rb.velocity.x, Speed);
            }    
        }
    }

    void FishRotation()
    {
        if (_rb.velocity.y > 0)
        {
            if (angle <= maxAngle)
            {
                angle = angle + 4;
            }
        }
        else if (_rb.velocity.y < -2.5f)
        {
            if (angle > minAngle)
            {
                angle = angle - 2;
            }
        }

        if (touchedGround == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            score.Scored();
            point.Play();
        }
        else if (collision.CompareTag("Column") && GameManager.gameOver == false)
        {
           GameManager.GameOver();
           FishDieEffect();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (GameManager.gameOver == false)
            {
                // game over
                GameManager.GameOver();
                GameOver();
                FishDieEffect();
            }
        }
    }

    void FishDieEffect()
    {
        hit.Play();
    }

    void GameOver()
    {
        touchedGround = true;
        sp.sprite = FishDied;
        anim.enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        
    }

}
