using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
