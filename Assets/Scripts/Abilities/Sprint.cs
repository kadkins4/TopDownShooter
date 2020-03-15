using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hero.Abilities
{
    using Hero.Player;
    public class Sprint : MonoBehaviour
    {
        Player _playerRef;
        float additionalSprintSpeed = 7.0f;
        float sprintDuration = 1.0f;
        bool isSprinting = false;
        [SerializeField] private float cooldown = 7.0f;
        private bool isOnCooldown = false;

        private void Awake()
        {
            _playerRef = GetComponent<Player>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) _Sprint();
        }

        private IEnumerator _StartCooldown()
        {
            print("cd");
            yield return new WaitForSeconds(cooldown);
            isOnCooldown = false;
        }

        private void _Sprint()
        {
            if (isSprinting) return; // don't allow double sprint
            if (isOnCooldown) return;

            isSprinting = true;
            isOnCooldown = true;

            float currentSpeed = _playerRef.GetCurrentSpeed();
            float targetSpeed = currentSpeed + additionalSprintSpeed;

            _playerRef.SetSpeed(targetSpeed);
            StartCoroutine(_StartCooldown());
            StartCoroutine(_EndSprint(currentSpeed));
        }

        private IEnumerator _EndSprint(float originalSpeed)
        {
            print("sprinting");
            yield return new WaitForSeconds(sprintDuration);
            _playerRef.SetSpeed(originalSpeed);
            isSprinting = false;
        }
    }
}