using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinyKey : MonoBehaviour
{
    private bool b_obtained = false;
    public bool Obtained;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<GoapController>() || collision.transform.GetComponent<PlayerController>())
        {
            b_obtained = true;
            gameObject.SetActive(false);
        }
    }
}
