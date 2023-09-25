using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chefao : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    public float distanciaDoAtaque = 2.5f;
    public int vida = 20;
    private EfeitoRagdoll efeitoRagdoll;
    public AudioClip somMorte;
    public AudioClip somPasso;
    public AudioClip grunhido;
    private AudioSource audioSrc;
    private bool grunhiu = false;
    private FieldOfView fov;
    private PatrulharAleatorio pal;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        efeitoRagdoll = GetComponent<EfeitoRagdoll>();


        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();

        efeitoRagdoll.init();
    }

    void Update()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);



        if (vida <= 0)
        {
            Morrer();
        }

        if (fov.podeVerPlayer)
        {
            Perseguir();
        }
        else
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
            agente.isStopped = false;
            pal.Patrulhar();
        }

        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            Atacar();
        }
        else if (distanciaDoPlayer < 10f && !grunhiu)
        {
            Grunhir();
        }
    }

    private void Perseguir()
    {
        agente.isStopped = false;
        agente.SetDestination(player.transform.position);
        CorrigirRigiSair();
    }

    private void Olhar()
    {
        Vector3 direcao = player.transform.position - transform.position;
        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, Time.deltaTime * 10f);
    }

    private void CorrigirRigiEntrar()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Atacar()
    {
        agente.isStopped = true;
        anim.SetTrigger("atacar");
        DarDano();
        CorrigirRigiEntrar();
    }

    private void Grunhir()
    {
        audioSrc.PlayOneShot(grunhido);
        grunhiu = true;
    }

    private void Morrer()
    {
        agente.isStopped = true;
        anim.SetBool("podeAndar", false);

        audioSrc.clip = somMorte;
        audioSrc.Play();

        efeitoRagdoll.Ativar();
        this.enabled = false;
    }

    public void LevarDano(int dano)
    {
        vida -= dano;
        anim.SetTrigger("levouTiro");
        if (vida <= 0)
        {
            Morrer();
        }
    }

    public void DarDano()
    {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-20);
    }

    public int GetVida()
    {
        return vida;
    }
}
