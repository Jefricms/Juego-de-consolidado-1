using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        // Si el objetivo se perdió (por cambio de escena o reinicio), vuelve a buscarlo
        if (target == null)
        {
            GameObject jugador = GameObject.FindWithTag("Player");
            if (jugador != null)
            {
                target = jugador.transform;
            }
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, offset.z);
    }
}