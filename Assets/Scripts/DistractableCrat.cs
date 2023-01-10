using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractableCrat : MonoBehaviour
{
    [SerializeField] Transform crateDestroyedPrefab;

    public static System.EventHandler OnAnyDestoryed;
    public GridPosition gridPosition{get; private set;}

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
       gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
    }
    public void Damage()
    {
        var test=Instantiate(crateDestroyedPrefab,transform.position,transform.rotation);
        ApplyExplosion(test,150f,transform.position,10f);
        OnAnyDestoryed?.Invoke(this,System.EventArgs.Empty);
        Destroy(gameObject);
    }

    private void ApplyExplosion(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if (TryGetComponent<Rigidbody>(out Rigidbody myRd))
            {
                myRd.AddExplosionForce(explosionForce,explosionPosition,explosionRadius);
            }

            ApplyExplosion(child,explosionForce,explosionPosition,explosionRadius);
        }
    }
}
