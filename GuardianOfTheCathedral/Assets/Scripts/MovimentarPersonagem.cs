using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentarPersonagem : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float alturaPulo = 6f;
    public float gravidade = -20f;

    public Transform checaChao;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    Vector3 velocidadeCai;

    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado, velocidadeAbaixar;

    // Start is called before the first frame update
    void Start()
    {
        controle = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checaChao.position, raioEsfera);
    }
    // Update is called once per frame
    void Update()
    {
        // cria uma de raio esfera que verifica se o personagem esta no chao
        // se estah em contato com chaoMask, entao retorna true
        estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;

        controle.Move(mover * velocidade * Time.deltaTime);

        if (estaNoChao && Input.GetButtonDown("Jump"))
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }

        if (!estaNoChao)
        {
            velocidadeCai.y += gravidade * Time.deltaTime;
        }

        controle.Move(velocidadeCai * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AgacharLevantar();
        }

        ChecarBloqueioAbaixado();

        if (!levantarBloqueado && estaNoChao && Input.GetButtonDown("Jump"))
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }
    }

    private void AgacharLevantar()
    {
        estahAbaixado = !estahAbaixado;
        if (levantarBloqueado && estahAbaixado)
        {
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
        }
        else
        {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0);
        }

    }

    private void ChecarBloqueioAbaixado()
    {
        //Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);
        RaycastHit hit;
        levantarBloqueado = Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f);
    }
}
