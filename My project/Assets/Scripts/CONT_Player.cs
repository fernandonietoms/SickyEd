using UnityEngine;
using UnityEngine.AI;

public class CONT_Player : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

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
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(movement);
    }

    void MovePlayer(Vector3 movement)
    {
        agent.SetDestination(agent.transform.position + ((movement).normalized));
    }
}
