using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardTransform : MonoBehaviour
{
    public Transform target;
    public float angleOffset;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        float AngleRad = Mathf.Atan2((target.position.y) - transform.position.y, (target.position.x) - transform.position.x);

        float AngleDeg = (180 / Mathf.PI) *AngleRad + angleOffset;

        transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
}
