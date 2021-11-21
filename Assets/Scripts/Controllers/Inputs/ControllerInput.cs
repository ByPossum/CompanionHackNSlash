using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ControllerInput : BaseInput
{
    protected PlayerAction pa_currentAction;
    public PlayerAction CurrentAction { get { return pa_currentAction; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(CallbackContext context)
    {
        v_direction = context.action.ReadValue<Vector2>();
    }

    private bool MoveTransition(PlayerAction _target)
    {
        switch (pa_currentAction)
        {
            case PlayerAction.none:
                return true;
            case PlayerAction.walk:
                return true;
            case PlayerAction.dodge:
                return false;
            case PlayerAction.jump:
                break;
            case PlayerAction.attack:
                break;
            case PlayerAction.die:
                return false;
        }
        return false;
    }
}

public enum PlayerAction
{
    none,
    walk,
    run,
    dodge,
    jump,
    attack,
    die
}