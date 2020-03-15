using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hero.Abilities
{
    using Hero.Player;
    public class Blink : MonoBehaviour
    {
        [SerializeField] private float blinkDistance;
        Rigidbody2D _rbReference;
        Collider2D _collider;
        Vector3 NewPosition;

        [Header("Stats")]
        [SerializeField] private float cooldown = 7.0f;
        float timeToRefresh; // this is the actual time we will decrement
        [SerializeField] private float energyCost = 10.0f;
        int maxCharges = 3;
        int availableCharges = 3;
        bool isRefreshing = false;

        private void Awake()
        {
            _rbReference = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            Physics2D.queriesStartInColliders = false; // prevents raycast from hitting self
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) _Blink();

            // TODO this may be better off outside of the UPDATE
            if (!isRefreshing && maxCharges != availableCharges)
            {
                timeToRefresh = cooldown;
                StartCoroutine(RefreshCharge());
            }
        }

        private IEnumerator RefreshCharge()
        {
            isRefreshing = true;
            yield return new WaitForSeconds(timeToRefresh);

            int index = Mathf.Abs(availableCharges - maxCharges) - 1;
            // if negative, all has been replenished
            if (index < 0)
            {
                index = 0;
            } else
            {
                availableCharges += 1;
            }

            isRefreshing = false;
        }

        private void _Blink()
        {
            if (availableCharges <= 0) return;
            // TODO energy check
            availableCharges -= 1;

            Vector3 currentPosition = transform.position;
            Vector2 destinationDirection = GetComponent<Player>().GetMousePosition();
            Vector2 lookDirection = destinationDirection - _rbReference.position;
            float safeBlinkDistance = blinkDistance;

            // first check to see if obstacle is in the way...
            safeBlinkDistance = _FindSafeBlinkDistance(lookDirection, safeBlinkDistance);

            // no obstacle
            transform.position = Vector3.MoveTowards(currentPosition, destinationDirection, safeBlinkDistance);
        }

        private float _FindSafeBlinkDistance(Vector2 dir, float desiredBlinkDist)
        {
            float safeBlinkDistance = desiredBlinkDist;
            safeBlinkDistance = _CheckForWalls(dir, desiredBlinkDist);

            return safeBlinkDistance;
        }

        private float _CheckForWalls(Vector2 dir, float desiredDistance)
        {
            float maxDistance = desiredDistance;
            bool isSafeToBlink = false;

            while (!isSafeToBlink)
            {
                isSafeToBlink = true;
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, maxDistance);

                foreach (var hit in hits)
                {
                    Debug.Log(hit.distance);
                    if (hit.collider.name == "Wall" || hit.collider.name == "Impassable")
                    {
                        isSafeToBlink = false;
                        maxDistance -= 0.25f;
                    }
                }
            }

            return maxDistance;
        }
    }
}