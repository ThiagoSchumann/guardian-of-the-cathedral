using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovimentarPersonagem : MonoBehaviour
{
    private int vida = 100;
    public Slider sliderVida;
    public CharacterController controle;
    public float velocidade = 5f;
    public float alturaPulo = 6f;
    public float gravidade = -20f;
    public AudioClip somPulo;
    public AudioClip somPassos;
    public AudioSource audioSrc;
    public Transform checaChao;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    Vector3 velocidadeCai;

    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado, velocidadeAbaixar;

    public Text textoMortos; // Referência para o texto que exibirá o número de mortos
    private int numeroMortos = 0; // Contagem de 

    public void AtualizarVida(int novaVida)
    {
        vida = Mathf.CeilToInt(Mathf.Clamp(vida + novaVida, 0, 100));
        print("Vida: " + vida);

        sliderVida.value = vida;
    }

    // Start is called before the first frame update
    void Start()
    {
        controle = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        audioSrc = GetComponent<AudioSource>();
        estaNoChao = true;

        AtualizarTextoMortos();
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

        if (vida <= 0)
        {
            FimDeJogo();
            return;
        }

        MoverPersonagem();
        AplicarPulo();
        AplicarGravidade();
        AgacharOuLevantar();
        ChecarBloqueioAbaixado();
    }

    private void MoverPersonagem()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 mover = transform.right * x + transform.forward * z;
        controle.Move(mover * velocidade * Time.deltaTime);

        // Reproduzir som de passos
        if (estaNoChao && (x != 0 || z != 0))
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.clip = somPassos;
                audioSrc.loop = true; // Faça o som dos passos repetir
                audioSrc.Play();
            }
        }
        else
        {
            audioSrc.loop = false; // Pare de repetir o som
            audioSrc.Stop(); // Pare o som dos passos
        }
    }

    private void AplicarPulo()
    {
        estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);
        if (estaNoChao && Input.GetButtonDown("Jump") && !levantarBloqueado)
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            print("Pulou");
            audioSrc.clip = somPulo;
            audioSrc.Play();
        }
    }

    private void AplicarGravidade()
    {
        if (!estaNoChao)
        {
            velocidadeCai.y += gravidade * Time.deltaTime;
        }
        controle.Move(velocidadeCai * Time.deltaTime);
    }

    private void AgacharOuLevantar()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AgacharLevantar();
        }
    }

    private void AgacharLevantar()
    {
        if (levantarBloqueado || !estaNoChao)
        {
            return;
        }

        estahAbaixado = !estahAbaixado;
        if (estahAbaixado)
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

    private void FimDeJogo()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    // Método para aumentar a contagem de mortos
    public void IncrementarMortos()
    {
        numeroMortos++;
        AtualizarTextoMortos();
    }

    // Método para atualizar o texto
    private void AtualizarTextoMortos()
    {
        textoMortos.text = "Mortos: " + numeroMortos.ToString();
    }


}
