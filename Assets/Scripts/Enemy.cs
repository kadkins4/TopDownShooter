using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.collider.name);
    }
}
