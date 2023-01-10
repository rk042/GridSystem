using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GranadeProjectTile : MonoBehaviour
{   
    public static event EventHandler OnAnyGranadeExploded;

    private Vector3 targetPosition;
    [SerializeField] private float radius=10f;
    [SerializeField] new ParticleSystem particleSystem;
    [SerializeField] AnimationCurve animationCurve;
    float totalDistance;
    Vector3 positionXZ;

    System.Action OnGranadeCompleteAction;

    public void Setup(GridPosition gridPosition,System.Action onGrandeCompelete)
    {
        OnGranadeCompleteAction=onGrandeCompelete;
        targetPosition=LevelGrid.instance.GetWorldPosition(gridPosition);
        positionXZ=transform.position;
        positionXZ.y=0;
        totalDistance=Vector3.Distance(positionXZ,targetPosition);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        Vector3 direction=(targetPosition-positionXZ).normalized;
        
        float moveSpeed = 10f;
        float distnace=Vector3.Distance(positionXZ,targetPosition);
        float normalizedDistance=1- distnace/totalDistance;

        float positionY=animationCurve.Evaluate(normalizedDistance);
        transform.position=new Vector3(positionXZ.x,positionY,positionXZ.z);

        positionXZ+=direction*moveSpeed*Time.deltaTime;
        
        if (Vector3.Distance(positionXZ,targetPosition)<0.2f)
        {
            var colliders= Physics.OverlapSphere(positionXZ,radius);
            foreach (var item in colliders)
            {
                if (item.TryGetComponent<Unit>(out Unit enemyAI))
                {
                    enemyAI.Damage(50);
                }
                else if (item.TryGetComponent<DistractableCrat>(out DistractableCrat crat))
                {
                    crat.Damage();
                }
            }
            var myPs=Instantiate(particleSystem,positionXZ,Quaternion.identity);
            myPs.Play();
            OnAnyGranadeExploded?.Invoke(null,EventArgs.Empty);
            OnGranadeCompleteAction?.Invoke();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
