using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public Vector3 Size;
    public Color GizmoColor;
    // Start is called before the first frame update

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawWireCube(transform.position, Size);
    }

}
