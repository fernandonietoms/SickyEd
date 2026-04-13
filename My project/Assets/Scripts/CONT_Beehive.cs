using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CONT_Beehive : MonoBehaviour
{
    [SerializeField] NavMeshAgent bees;
    [SerializeField] float maxDistanceBees = 10.0f;
    [SerializeField] float beeDamage = 0.5f;
    Coroutine TrackPlayer;

    CONT_Player playerController;

    private void Awake()
    {
        bees.gameObject.SetActive(false);
        bees.updateRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Player!");
            if (collision.gameObject.TryGetComponent(out playerController))
            {
                if (TrackPlayer != null) StopCoroutine(TrackPlayer);
                TrackPlayer = StartCoroutine(FollowPlayer());
            }
        }
    }

    IEnumerator FollowPlayer()
    {
        bool followPlayer = true;
        bees.gameObject.SetActive(true);
        while (true)
        {
            if (followPlayer)
            {
                // Stop following condition
                if (Vector3.Distance(bees.transform.position, transform.position) > maxDistanceBees)
                {
                    Debug.Log("Retreat");
                    followPlayer = false;
                    bees.SetDestination(transform.position);
                }
                // Follow and damage player
                else
                {
                    bees.SetDestination(playerController.transform.position);
                    Debug.Log(Vector3.Distance(bees.transform.position, playerController.transform.position));
                    if (Vector3.Distance(bees.transform.position, playerController.transform.position) < 1.5f)
                    {
                        bees.SetDestination(bees.transform.position);
                        playerController.Stamina -= beeDamage;
                    }
                }
            }

            // Disable bees after stop condition
            if (!followPlayer && Vector3.Distance(bees.transform.position, transform.position) < 2.0f)
            {
                bees.gameObject.SetActive(false);
                StopCoroutine(TrackPlayer);
                TrackPlayer = null;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
