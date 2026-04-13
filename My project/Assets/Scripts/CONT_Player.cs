using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CONT_Player : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Slider staminaBar;
    [SerializeField] float staminaMin = 3;
    [SerializeField] float staminaMax = 10;
    [SerializeField] float stamina;
    public float Stamina { get { return stamina; } set { if (value < staminaMax && value > staminaMin) stamina = value; UpdateStamina(); } }
    Coroutine StaminaDecay;

    InputSystem.InputSystem controls;
    public InputSystem.InputSystem Controls { get { return controls; } private set { controls = value; } }
    Vector3 movement = Vector3.zero;
    Vector2 Movement { set { movement = new Vector3(value.x, 0.0f, value.y); forceMovementUpdate = true; } }
    bool forceMovementUpdate;

    private void Awake()
    {
        Controls = new();
        Controls.Player.Move.performed += context => Movement = Controls.Player.Move.ReadValue<Vector2>();
        Controls.Player.Move.canceled += context => Movement = Vector2.zero;
        agent.updateRotation = false;
        agent.speed = stamina;
    }

    private void OnEnable()
    {
        Controls.Enable();
        StaminaDecay = StartCoroutine(ReduceStamina());
    }

    private void OnDisable()
    {
        Controls.Disable();
        StopCoroutine(StaminaDecay);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(movement * 5.0f);
    }

    void MovePlayer(Vector3 movement)
    {
        agent.updatePosition = !(movement == Vector3.zero);
        if (Vector3.Distance(agent.pathEndPosition, agent.transform.position) > 1.0f || forceMovementUpdate)
        {
            agent.SetDestination(agent.transform.position + movement);
            forceMovementUpdate = false;
        }
    }

    IEnumerator ReduceStamina()
    {
        while (true)
        {
            Stamina -= 0.01f;
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void UpdateStamina()
    {
        agent.speed = stamina;
        staminaBar.value = (stamina - staminaMin) / (staminaMax - staminaMin);
    }
}
