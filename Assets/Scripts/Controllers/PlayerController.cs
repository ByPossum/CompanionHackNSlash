using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ControllerInput)), RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private ControllerInput ci_input;
    private Rigidbody rb;
    private Animator anim;
    private float f_inputTime = 0;


    [Header("Movement Modifiers")]
    [SerializeField, Tooltip("The base walking speed")]
    private float f_walkSpeed;
    [SerializeField, Tooltip("The base running speed")]
    private float f_runSpeed;
    [Header("Movement Curves")]
    [SerializeField, Tooltip("The walking speed over time to move")]
    private AnimationCurve ac_walkCurve;
    [SerializeField, Tooltip("The running speed over time to move")]
    private AnimationCurve ac_runCurve;
    [SerializeField, Tooltip("The time it takes to decelerate")]
    private AnimationCurve ac_decelCurve;

    // Start is called before the first frame update
    void Start()
    {
        ci_input = GetComponent<ControllerInput>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ci_input.Direction != Vector2.zero)
        {
            f_inputTime += Time.deltaTime;
            float x = ci_input.Direction.x * ac_walkCurve.Evaluate(f_inputTime);
            float y = ci_input.Direction.y * ac_walkCurve.Evaluate(f_inputTime);
            rb.velocity = new Vector3(x, rb.velocity.y, y).normalized * f_walkSpeed;
            anim.SetFloat("Walkspeed", 1f);
        }
        else
        {
            f_inputTime = 0f;
            rb.velocity = Vector3.zero + Physics.gravity;
            anim.SetFloat("Walkspeed", 0f);
            //f_inputTime = Mathf.Repeat(f_inputTime-=Time.deltaTime, ac_decelCurve.keys[ac_decelCurve.keys.Length-1].time);
            //float x = ac_decelCurve.Evaluate(f_inputTime);
            //float y = ac_decelCurve.Evaluate(f_inputTime);
            //rb.velocity = new Vector3(x, rb.velocity.y, y);
        }
    }
}
