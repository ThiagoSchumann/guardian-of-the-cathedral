using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EfeitoRagdoll : MonoBehaviour
{
    private Rigidbody myRigid;
    private List<Collider> colls = new List<Collider>();
    private List<Rigidbody> rigs = new List<Rigidbody>();
    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    public void init()
    {
        Rigidbody[] rigids = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigid in rigids)
        {
            if (rigid == myRigid)
            {
                continue;
            }

            rigs.Add(rigid);
            rigid.isKinematic = true;

            Collider coll = rigid.gameObject.GetComponent<Collider>();
            coll.enabled = false;
            colls.Add(coll);



        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Ativar()
    {
        for (int i = 0; i < rigs.Count; i++)
        {
            rigs[i].isKinematic = false;
            colls[i].enabled = true;
        }
        myRigid.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        StartCoroutine(FinalizarAnimacao());
    }

    IEnumerator FinalizarAnimacao()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
    }
}
