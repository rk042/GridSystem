using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] Transform ragdollRoot;

    public void SetUp(Transform originalRootBon)
    {
        MatchAllChildTranforms(originalRootBon,ragdollRoot);
        Vector3 randomDir=new Vector3(Random.Range(-1f,+1f),0,Random.Range(-1f,+1f));
        ApplyExplosionToRagdoll(ragdollRoot,300f,transform.position + randomDir,10f);
    }

    private void MatchAllChildTranforms(Transform root,Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild=clone.Find(child.name);
            if (cloneChild!=null)
            {
                cloneChild.position=child.position;
                cloneChild.rotation=child.rotation;

                MatchAllChildTranforms(child,cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if (TryGetComponent<Rigidbody>(out Rigidbody myRd))
            {
                myRd.AddExplosionForce(explosionForce,explosionPosition,explosionRadius);
            }

            ApplyExplosionToRagdoll(child,explosionForce,explosionPosition,explosionRadius);
        }
    }
}
