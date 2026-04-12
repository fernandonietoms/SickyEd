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
    float Stamina { get { return stamina; } set { if (value < staminaMax && value > staminaMin) stamina = value; } }

    InputSystem.InputSystem controls;
    public InputSystem.InputSystem Controls { get { return controls; } private set { controls = value; } }
    Vector3 movement = Vector3.zero;
    Vector2 Movement { set { movement = new Vector3(value.x, 0.0f, value.y); }}

    private void Start()
    {
        Controls = new();
        Controls.Enable();
        Controls.Player.Move.performed += context => Movement = Controls.Player.Move.ReadValue<Vector2>();
        Controls.Player.Move.canceled += context => Movement = Vector2.zero;

        agent.updateRotation = false;

        agent.speed = stamina;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(movement);
        Stamina -= 0.01f;
        agent.speed = stamina;
        staminaBar.value = stamina / staminaMax;
    }

    void MovePlayer(Vector3 movement)
    {
        agent.SetDestination(agent.transform.position + ((movement).normalized));
    }
}
