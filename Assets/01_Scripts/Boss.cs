using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxLife = 3;
    private float life;
    public float speed = 0.5f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float timeBtwShoot = 1f;
    private float shootTimer = 0;

    private int currentPhase = 1;
    private bool isEntering = true;
    private float minX, maxX;

    void Start()
    {
        life = maxLife;
        UpdatePhase();

        Camera cam = Camera.main;
        minX = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        maxX = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
    }

    void Update()
    {
        if (isEntering)
        {
            EnterFromTop();
        }
        else
        {
            MoveHorizontally();
            Shoot();
        }
    }

 
    void EnterFromTop()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (transform.position.y <= Camera.main.ViewportToWorldPoint(new Vector2(0, 0.8f)).y)
        {
            isEntering = false;
        }
    }

    void MoveHorizontally()
    {
        float moveDirection = Mathf.PingPong(Time.time * speed, 1);
        Vector3 newPosition = transform.position;

        newPosition.x = Mathf.Lerp(minX + 1f, maxX - 1f, moveDirection);

        transform.position = newPosition;
    }

    void Shoot()
    {
        if (shootTimer < timeBtwShoot)
        {
            shootTimer += Time.deltaTime;
        }
        else
        {
            shootTimer = 0;
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;

        if (life <= maxLife * 0.75f && currentPhase == 1)
        {
            currentPhase = 2;
            UpdatePhase();
        }
        else if (life <= maxLife * 0.5f && currentPhase == 2)
        {
            currentPhase = 3;
            UpdatePhase();
        }
        else if (life <= maxLife * 0.25f && currentPhase == 3)
        {
            currentPhase = 4;
            UpdatePhase();
        }

        if (life <= 0)
        {
            WinGame();
        }
    }

    void UpdatePhase()
    {
        switch (currentPhase)
        {
            case 1:
                speed = 2f;
                timeBtwShoot = 1f;
                break;
            case 2:
                speed = 3f;
                break;
            case 3:
                timeBtwShoot = 0.5f;
                break;
            case 4:
                speed = 4f;
                timeBtwShoot = 0.3f;
                break;
        }
    }

    void WinGame()
    {
        Debug.Log("¡Ganaste!");
    }
    void MoveForward()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

}
