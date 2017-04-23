﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    const int LEFT = -1, RIGHT = 1;

    int prev_dir = 1;

    public float speed = 40;
    public float jumpspeed = 1000;
    public int maxhealth = 10;

	public BulletController bullet;
	public float bulletSpeed = 4;

    private bool grounded = false;
	private int facing = 1;
    private int health;

    private float old_pos;


    private int sprintFrames = 0;
	// Use this for initialization
	void Start () {
        health = maxhealth;
        old_pos = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if(old_pos == transform.position.x && prev_dir ==facing)
        {
            sprintFrames = 0;
            print("still");
        }
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(CheckGrounded())
            {
                sprintFrames++;
            }
            
            if (sprintFrames >= 90)
            {
                body.AddForce(Vector2.left * (speed+15));
                print("Sprinting");
            }
            else
            {
                body.AddForce(Vector2.left * speed);
            }
            
			facing = LEFT;
            
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (CheckGrounded())
            {
                sprintFrames++;
            }
            if (sprintFrames >= 90)
            {
                body.AddForce(Vector2.right * (speed + 15));
                print("Sprinting");
            }
            else
            {
                body.AddForce(Vector2.right * speed);
            }
            facing = RIGHT;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && CheckGrounded())
        {
            body.AddForce(Vector2.up * jumpspeed);
            sprintFrames = 0;
        }
		if (Input.GetKeyDown(KeyCode.Space))
		{
            Shoot();
		}
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = facing == LEFT;
        if (!CheckGrounded() && body.position.y < -25)
        {
            Die();
        }

        old_pos = transform.position.x;
    }

    private bool CheckGrounded()
    {
        return GetComponent<Rigidbody2D>().Cast(Vector2.down, new RaycastHit2D[1], 0.02f) > 0;
        //Bounds bounds = GetComponent<Collider2D>().bounds;
        //RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, 0.02f);
        //return grounded = hit.collider != null;
    }

    public void Hit(int damage)
    {
        //print(name + " hit: " + damage + " damage");
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        print(name + " died!!1");
        SceneManager.LoadScene("death screen");
    }

    private void Shoot()
    {
        BulletController bullet2 = Instantiate(bullet, transform.position + new Vector3(.7f * facing, .1f), Quaternion.identity);
        bullet2.velocity = new Vector2(bulletSpeed * facing, 0);
    }
}

public enum DashState
{
    Ready,
    Dashing,
    Cooldown
}