using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hero.Abilities
{
    using Hero.Player;
    public class Dash : MonoBehaviour
    {
        private Player _playerRef;
        private Rigidbody2D _rb;

        [Header("Cast")]
        [SerializeField] [Range(1.0f, 10.0f)] private float dashSpeed;
        // the dashSpeed is just to make the number manageable
        // we multiply times dashMultiplier to make the desired force
        private float dashMultiplier = 100f;
        [SerializeField] private float dashTime = 0.25f; // duration of dash
        [SerializeField] private float startDashTime = 0.5f; // charge up time
        private int direction; // player is locked in this direction -- last direction of mouse
        float dashBuildUpDuration = 0.5f; // time leading up to dash


        [Header("Stats")]
        [SerializeField] private float cooldown = 7.0f;
        bool isOnCooldown = false;
        [SerializeField] private float energyCost = 10.0f;
        [SerializeField] private float damageOnImpact = 10.0f;
        [SerializeField] private float stunDuration = 10.0f;
        [SerializeField] private float knockBackDistance = 20.0f;
        [SerializeField] private float affectedUnitSpeed; // SERIALIZED ONLY for development

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerRef = GetComponent<Player>();
            affectedUnitSpeed = _playerRef.GetCurrentSpeed();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) StartCoroutine(_startDash());
        }

        private IEnumerator _startDash()
        {

            if (_CanCastDash()) {

                _playerRef.SetSpeed(0.0f);
                _playerRef.SetPlayerMovementStatus(false); // lock position
                _playerRef.SetPlayerRotationStatus(false); // lock rotation

                StartCoroutine(StartDashCooldown());

                yield return new WaitForSeconds(dashBuildUpDuration);
                _playerRef.SetSpeed(affectedUnitSpeed);
                StartCoroutine(_Dash());
            }
        }

        private IEnumerator _Dash()
        {
            // move player forward set amount of distance
            float totalDashSpeed = affectedUnitSpeed + (dashSpeed * dashMultiplier);
            _rb.AddForce(transform.right * totalDashSpeed, ForceMode2D.Force);

            yield return new WaitForSeconds(dashTime);
            _rb.AddForce(transform.right / dashSpeed);

            _playerRef.SetPlayerMovementStatus(true);
            _playerRef.SetPlayerRotationStatus(true);
        }

        private bool _CanCastDash ()
        {
            if (isOnCooldown) return false;
            // TODO
            // subtract energy from core if enough exists
            return true;
        }

        private IEnumerator StartDashCooldown()
        {
            isOnCooldown = true;

            yield return new WaitForSeconds(cooldown);
            print("dash off cooldown");
            isOnCooldown = false;
        }

    }
}