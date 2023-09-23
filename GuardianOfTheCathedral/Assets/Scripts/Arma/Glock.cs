using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : MonoBehaviour
{
    private Animator anim;
    private bool estahAtirando;
    private RaycastHit hit;
    public GameObject efeitoTiro;
    public GameObject posEfeitoTiro;
    public GameObject faisca;
    private AudioSource somTiro;

    // Start is called before the first frame update
    void Start()
    {
        estahAtirando = false;
        anim = GetComponent<Animator>();
        somTiro = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // enquanto a animação do tiro estiver processando...
            if (!estahAtirando)
            {
                estahAtirando = true;
                StartCoroutine(Atirando());
            }
        }
    }

    IEnumerator Atirando()
    {
        // encontrar o centro da tela
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        // definir um ponto até o centro da tela
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));
        anim.Play("AtirarGlock");
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
        }

        yield return new WaitForSeconds(0.3f);
        Destroy(efeitoTiroObj);
        Destroy(faiscaObj);

        estahAtirando = false;
    }
}
