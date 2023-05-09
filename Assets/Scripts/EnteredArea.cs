using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag + " Entered the area");
    }
}
