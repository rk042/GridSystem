using UnityEngine;

public class BulletProjectTrail : MonoBehaviour
{
    private Vector3 targetPos;
    private float moveSpeed=200f;
    [SerializeField] ParticleSystem myPs;
    public void SetUpPosition(Vector3 targetPos)
    {
        this.targetPos=new Vector3{ x=targetPos.x,y=targetPos.y+1.4f,z=targetPos.z};
    }

    private void Update()
    {
        var moveDir=(targetPos-transform.position).normalized;
        float distaneBeforeMoving=Vector3.Distance(transform.position,targetPos);
        transform.position += moveDir*moveSpeed*Time.deltaTime;
        float distaneAfterMoving=Vector3.Distance(transform.position,targetPos);

        if (distaneBeforeMoving<distaneAfterMoving)
        {
            transform.DetachChildren();
            Destroy(gameObject);
            Instantiate(myPs.gameObject,transform.position,Quaternion.identity);
        }
    }
}