using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public floar sensibilidadeMouse = 100f;
    public float anguloMinimo = -90f;
    public float anguloMaximo = 90f;

    public Transform transformPlayer;

    float rotacao = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse * Time.deltaTime;

        rotacao -= mouseY;
        rotacao = Mathf.Clamp(rotacao, anguloMinimo, anguloMaximo);
        transformPlayer.localRotation = Quaternion.Euler(rotacao, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}
