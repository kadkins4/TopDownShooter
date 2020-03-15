using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hero.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Attributes")]
        public float maxHealth = 100f;
        public float currentHealth = 100f;
        public float maxMana = 100f;
        public float currentMana = 100f;
        public float maxStamina = 100f;
        public float currentStamina = 100f;
        public float agility = 2f;
        public float strength = 2f;
        public float intellect = 2f;

        private Rigidbody2D _rb;
        public Animator playerAnimator;

        [Header("Movement")]
        private Vector2 _movement;
        public float movementSpeed = 7f;
        private bool playerCanMove = true;

        [Header("Attack")]
        private Vector2 _mousePosition;
        private bool playerCanRotate = true;
        public GameObject firePoint;
        // TODO temp property -- will change to weapon
        public Bullet bulletPrefab;

        public int magazineCapacity = 10;
        public int roundsRemaining = 10;

        public float rateOfAttack = 0.5f;
        public bool canAttack = true;
        public float timeToReload = 2.5f;

        // Start is called before the first frame update
        void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            // Inputs
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            // find mouse position
            _mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

            // TODO maybe create a player controller that will trigger all movement/abilities
            if (Input.GetButtonDown("Fire1")) _Attack();
            if (Input.GetKeyDown(KeyCode.R)) _ReloadWeapon();
        }

        private void FixedUpdate()
        {
            // Movement
            if (playerCanMove) // player is not stunned, charging nor other movement disabling status
            {
                _rb.velocity = _movement * movementSpeed;
                // _rb.MovePosition(_rb.position + _movement * movementSpeed * Time.fixedDeltaTime);
                playerAnimator.SetFloat("moveSpeed", _movement.sqrMagnitude);
            } 

            // Direction
            if (playerCanRotate)
            {
                Vector2 lookDirection = _mousePosition - _rb.position;
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

                _rb.rotation = angle;
            }

        }

        private void _Attack()
        {
            if (canAttack && roundsRemaining >= 1)
            {
                canAttack = false;
                _FireWeapon();
                roundsRemaining -= 1;
                StartCoroutine(StartTimeBetweenAttacks());
            }
        }

        private IEnumerator StartTimeBetweenAttacks()
        {
            yield return new WaitForSeconds(rateOfAttack);
            canAttack = true;
        }

        private void _FireWeapon()
        {
            Instantiate(
                bulletPrefab,
                firePoint.transform.position,
                Quaternion.identity
            );
        }

        private void _ReloadWeapon()
        {
            canAttack = false;
            StartCoroutine(StartTimeToReload());
        }

        private IEnumerator StartTimeToReload()
        {
            yield return new WaitForSeconds(timeToReload);
            roundsRemaining = magazineCapacity;
            canAttack = true;
        }

        public Vector2 GetMousePosition()
        {
            return _mousePosition;
        }

        public void SetSpeed(float newSpeed)
        {
            movementSpeed = newSpeed;
        }

        public float GetCurrentSpeed()
        {
            return movementSpeed;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public void SetCurrentHealth(float newHealth)
        {
            currentHealth = newHealth;
        }

        public float GetCurrentMana()
        {
            return currentMana;
        }

        public void SetCurrentMana(float newMana)
        {
            currentMana = newMana;
        }

        public float GetCurrentStamina()
        {
            return currentStamina;
        }

        public bool GetPlayerMovementStatus() // TODO change name
        {
            return playerCanMove;
        }

        public void SetPlayerMovementStatus(bool newPlayerMovementStatus)
        {
            playerCanMove = newPlayerMovementStatus;
            // TODO clean this up
            if (!newPlayerMovementStatus) {
                _rb.velocity = _movement * 0.0f;
            }
        }
        public bool GetPlayerRotationStatus() // TODO change name
        {
            return playerCanRotate;
        }

        public void SetPlayerRotationStatus(bool newPlayerRotationStatus)
        {
            playerCanRotate = newPlayerRotationStatus;
        }
    }
}