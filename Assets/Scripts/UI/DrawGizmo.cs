using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour {

    public short m_type = 0;
    private Transform[] points;

    private void OnDrawGizmos() {
        Vector3 last = Vector3.zero;
        switch (m_type) {
            case 0:
                points = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 1; i < points.Length; i++) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(points[i].position, new Vector3(10 , 10, 10));
                }
            break;
            case 1:
                points = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 1; i < points.Length; i++) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(points[i].position, new Vector3(8 , 8, 8));
                }
            break;
            case 2:
                points = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 1; i < points.Length; i++) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(points[i].position, 10);
                }
            break;
            case 3:
                points = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 1; i < points.Length; i++) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(points[i].position, 3);
                    if (last != Vector3.zero) {
                        Gizmos.DrawLine(last, points[i].position);                        
                    }
                    last = points[i].position;
                }            
            break;
            case 4:
                points = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 1; i < points.Length; i++) {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(points[i].position, 3);
                    if (last != Vector3.zero) {
                        Gizmos.DrawLine(last, points[i].position);                        
                    }
                    last = points[i].position;
                }            
            break;
            default:
                points = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 1; i < points.Length; i++) {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(points[i].position, 3);
                    last = points[i].position;
                }
            break;
        }
    }
}
