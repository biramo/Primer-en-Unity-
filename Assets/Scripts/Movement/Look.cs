using UnityEngine;

public class Look : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch; // ángulo vertical acumulado de la cámara

    //Funcion para girar la camara/vista
    public void Rotate(Vector2 delta)//delta:valor de cuanto se movio el raton
    {
        transform.Rotate(0f, delta.x * sensitivity, 0f);//lo multiplicamos por la sensibilidad para obtener la rotacion

        // Pitch: se acumula, se limita y se aplica solo a la cámara
        pitch -= delta.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);//Evita que la camara de la vuelta completa
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}