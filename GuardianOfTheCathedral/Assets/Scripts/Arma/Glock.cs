using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glock : MonoBehaviour
{
    public Text textoMunicao;

    private Animator anim;
    private bool estahAtirando;
    private RaycastHit hit;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
    public GameObject faisca;
    private AudioSource somTiro;
    public AudioClip somColeta;

    public AudioClip[] clips;
    private int carregador = 3;
    private int municao;
    public GameObject imgCursor;

    // Definindo a capacidade máxima do carregador
    private const int CAPACIDADE_CARREGADOR = 10;

    // Start is called before the first frame update
    void Start()
    {
        estahAtirando = false;
        anim = GetComponent<Animator>();
        somTiro = GetComponent<AudioSource>();
        municao = CAPACIDADE_CARREGADOR; // Inicializando a munição com a capacidade máxima
        AtualizarTextoMunicao();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("acaoOcorrendo"))
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!estahAtirando && municao > 0)
            {
                somTiro.clip = clips[0];
                municao--;
                estahAtirando = true;
                StartCoroutine(Atirando());
            }
            else if (!estahAtirando && municao == 0 && carregador > 0)
            {
                Recarregar();
            }
            else if (!estahAtirando && municao == 0)
            {
                somTiro.clip = clips[2];
                somTiro.time = 0;
                somTiro.Play();
            }
        }
        else if (Input.GetButtonDown("Recarregar") && carregador > 0 && municao < CAPACIDADE_CARREGADOR)
        {
            Recarregar();
        }
        AtualizarTextoMunicao();

        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("mirar", true);
            imgCursor.SetActive(false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
        }
        else
        {
            anim.SetBool("mirar", false);
            imgCursor.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
        }
    }

    private void Recarregar()
    {
        somTiro.clip = clips[1];
        somTiro.time = 1.05f;
        somTiro.Play();

        anim.Play("RecarregarGlock");
        carregador--;
        municao = CAPACIDADE_CARREGADOR; // Recarregando com a capacidade máxima
    }

    private void AtualizarTextoMunicao()
    {
        textoMunicao.text = municao.ToString() + "/" + carregador.ToString();
    }

    public void AddCarregador()
    {
        somTiro.PlayOneShot(somColeta); // Reproduz o som de coleta

        carregador++;
        AtualizarTextoMunicao();
    }

    IEnumerator Atirando()
    {
        // encontrar o centro da tela
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        // definir um ponto até o centro da tela
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));
        anim.Play("AtirarGlock");
        somTiro.time = 0;
        somTiro.Play();

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        GameObject faiscaObj = null;

        // o inimigo não precisa estar na mira exata para acertar o tiro
        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            faiscaObj = Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            if (hit.transform.tag == "Arrastar")
            {
                Vector3 direcaoBala = ray.direction;
                hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
            }
            else
            {
                if (hit.transform.tag == "LevarDano")
                {
                    ILevarDano levarDano = hit.transform.GetComponent<ILevarDano>();
                    levarDano.LevarDano(5);
                }
            }
        }

        yield return new WaitForSeconds(0.3f);
        Destroy(efeitoTiroObj);
        Destroy(faiscaObj);

        estahAtirando = false;
    }

}
