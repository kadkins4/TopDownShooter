using Hero.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float bulletForce = 100f;

    public GameObject bulletPrefab;

    Vector2 _mousePosition;

    // Start is called before the first frame update
    void Awake()
    {
        _mousePosition = FindObjectOfType<Player>().GetMousePosition();
    }

    void Start()
    {
        _RotateBullet();
        _AccelerateBullet();
    }

    // DEBUG
    private void _RotateBullet()
    {
        // find the amount of rotation needed to face mouse position
        var angle = Mathf.Atan2(_mousePosition.x, _mousePosition.y) * Mathf.Rad2Deg;
        // add amount needed to current rotation
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }

    private void _AccelerateBullet()
    {
        rb.AddForce(transform.up * bulletForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.name);
        if (other.collider.name != "Player") Destroy(gameObject);
    }
}
