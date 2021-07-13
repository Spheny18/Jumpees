using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public bool clingPlatforms = true;
    public bool clingRotation = true;
    public bool clingRigidBody = true;
    Vector2 lastVelocity;
    [HideInInspector]
    public Vector2 postUpdateVelocity;
    public LayerMask ignore;
    public Dictionary<string,bool> collisionss;
    public Dictionary<string,ColInfo> collisions;
    public ColInfoBuilder[] collisionsDefiner;
    public ColInfo otherCollisions;
    public float skinWidth = 0.01f;

    void Start()
    {
        collisions = new Dictionary<string,ColInfo>();
        foreach(ColInfoBuilder cib in collisionsDefiner){
            collisions.Add(cib.key, new ColInfo(cib));
            // Debug.Log(cib.key);
        }
        otherCollisions = new ColInfo();

        // collisions = new Dictionary<string,bool>();
        boxCollider = GetComponent<BoxCollider2D>();
        // ignore =~ LayerMask.GetMask("Pink");
        // ignore
        lastVelocity = Vector2.zero;
        postUpdateVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //TODO fix missing collisions when on a fast moving platform
    void LateUpdate(){
        postUpdateVelocity = Vector2.zero;
        if(!clingPlatforms){
            return;
        }

        foreach(KeyValuePair<string, ColInfo> entry in collisions)
        {
            foreach (oldPositions op in collisions[entry.Key].oldPos){
                // if(op.obj.GetComponent<Rigidbody2D>()){
                    // Vector3 v = op.obj.GetComponent<Rigidbody2D>().velocity;
                    // v.x = v.x > 0 ? v.x * collisions[entry.Key].positiveXMoveInfluence: v.x * collisions[entry.Key].negativeXMoveInfluence;
                    // v.y = v.y > 0 ? v.y * collisions[entry.Key].positiveYMoveInfluence: v.y * collisions[entry.Key].negativeYMoveInfluence;
                    // postUpdateVelocity += new Vector2(v.x,v.y);
                    // RawMove(v);

                // } else {
                // Debug.Log(op.obj.name + " | " + (op.obj.transform.position - op.oldPos).ToString("F4"));
                    Vector3 v = op.obj.transform.position - op.oldPos;
                    v.x = v.x > 0 ? v.x * collisions[entry.Key].positiveXMoveInfluence: v.x * collisions[entry.Key].negativeXMoveInfluence;
                    v.y = v.y > 0 ? v.y * collisions[entry.Key].positiveYMoveInfluence: v.y * collisions[entry.Key].negativeYMoveInfluence;
                    postUpdateVelocity += new Vector2(v.x,v.y);
                    RawMove(v);
                // }
            }
        } 
            //leaving the influence logic incase I Decide to make other colliders be exposed and editable
        foreach (oldPositions op in otherCollisions.oldPos){
                // if(op.obj.GetComponent<Rigidbody2D>()){
                //     continue;
                // }
                // Debug.Log(op.obj.name + " | " + (op.obj.transform.position - op.oldPos).ToString("F4"));
                Vector3 v = op.obj.transform.position - op.oldPos;
                v.x = v.x > 0 ? v.x * otherCollisions.positiveXMoveInfluence: v.x * otherCollisions.negativeXMoveInfluence;
                v.y = v.y > 0 ? v.y * otherCollisions.positiveYMoveInfluence: v.y * otherCollisions.negativeYMoveInfluence;
                postUpdateVelocity += new Vector2(v.x,v.y);
                RawMove(v);
        }

        if(!clingRotation){
            return;
        }
        foreach(KeyValuePair<string, ColInfo> entry in collisions)
        {
            foreach (oldPositions op in collisions[entry.Key].oldPos){
                // Debug.Log(op.obj.transform.rotation.eulerAngles.ToString("F4") + " | " + op.oldRot.eulerAngles.ToString("F4"));
                Quaternion diffRotation = op.obj.transform.rotation * Quaternion.Inverse(op.oldRot);
                // Debug.Log(diffRotation.eulerAngles.ToString("F4"));
                float theta = diffRotation.eulerAngles.z * Mathf.Deg2Rad; 
                if(theta == 0 ){
                    continue;
                }
                // ox, oy = origin
                //     px, py = point

                //     qx = ox + math.cos(angle) * (px - ox) - math.sin(angle) * (py - oy)
                //     qy = oy + math.sin(angle) * (px - ox) + math.cos(angle) * (py - oy)
                //  Debug.Log("transform.position " + transform.position.ToString("F4") + " | obj.position " + op.obj.transform.position.ToString("F4"));
                Vector3 newPos = Vector3.zero;
                // v.x = Mathf.Cos(theta) * (transform.position.x - op.obj.transform.position.x) - Mathf.Sin(theta) * (transform.position.y - op.obj.transform.position.y) + op.obj.transform.position.x;
                // v.y = Mathf.Sin(theta) * (transform.position.x - op.obj.transform.position.x) + Mathf.Cos(theta) * (transform.position.y - op.obj.transform.position.y) + op.obj.transform.position.y;
                newPos.x = (op.obj.transform.position.x) + (Mathf.Cos(theta) * (transform.position.x - op.obj.transform.position.x)) - (Mathf.Sin(theta) * (transform.position.y - op.obj.transform.position.y));
                newPos.y = (op.obj.transform.position.y) + (Mathf.Sin(theta) * (transform.position.x - op.obj.transform.position.x)) + (Mathf.Cos(theta) * (transform.position.y - op.obj.transform.position.y));
                
                Vector3 v = newPos - transform.position;
                // Debug.Log("theta" + theta + " | v" + v);
                postUpdateVelocity += new Vector2(v.x,v.y);
                RawMove(v);
            }
        } 

    }

    void Collisions(){
        foreach(KeyValuePair<string, ColInfo> entry in collisions)
        {
            collisions[entry.Key].oldPos.Clear();
            collisions[entry.Key].hits.Clear();
            collisions[entry.Key].isContact = false;
        } 
        otherCollisions.isContact = false;
        otherCollisions.hits.Clear();
        otherCollisions.oldPos.Clear();

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0,ignore);//,ignore);
        
        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider )
		        continue;
            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);
            //  Debug.Log("collider distance: " + colliderDistance.distance);

                
            // if (colliderDistance.isOverlapped)
            if (colliderDistance.distance < skinWidth)
            {
                // Debug.Log(hit.gameObject.name + " | " + Vector2.SignedAngle(colliderDistance.normal, Vector2.up));
                
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                bool added = false;
                foreach(KeyValuePair<string, ColInfo> entry in collisions)
                {
                    float angle = Vector2.SignedAngle(colliderDistance.normal, Vector2.up);
                    if(angle <= entry.Value.maxAngle && angle >= entry.Value.minAngle){
                        // if(((entry.Value.exclusiveMask && entry.Value.mask != hit.gameObject.layer) || (!entry.Value.exclusiveMask && entry.Value.mask == hit.gameObject.layer)))
                        collisions[entry.Key].isContact = true;
                        collisions[entry.Key].hits.Add(hit);
                        collisions[entry.Key].oldPos.Add(new oldPositions(hit));
                        added = true;
                    }
                    // Debug.Log(entry.Key + " | " + collisions[entry.Key].isContact);
                }   
                if(!added){
                    otherCollisions.isContact = true;
                    otherCollisions.hits.Add(hit);
                    otherCollisions.oldPos.Add(new oldPositions(hit));
                }
            }
        }
    }

    public void Move(Vector2 v){
        this.lastVelocity = v;
        Vector3 PrevPos = transform.position;
        // Debug.Log(v.ToString("F4"));
        SafeMove(v);
        Collisions();
        // lastVelocity = PrevPos - transform.position;
    }


    void SafeMove(Vector2 velocity){
        // Debug.Log("we movingf");
        RaycastHit2D hit = Physics2D.BoxCast(transform.position,transform.lossyScale,0,velocity,velocity.magnitude,ignore);
        if(hit){
            // Debug.Log("your goin a lil fast buddy");
            velocity = new Vector3(hit.centroid.x,hit.centroid.y,0) - transform.position;
        }
            // if(velocity.magnitude >= minimumMove){
                transform.Translate(velocity);
            // }
        // }
    }

    public void RawMove(Vector2 velocity){
        transform.Translate(velocity);
    }

    // void FixedUpdate(){
    //     foreach(KeyValuePair<string, ColInfo> entry in collisions)
    //     {
    //         foreach (oldPositions op in collisions[entry.Key].oldPos){
    //             if(op.obj.GetComponent<Rigidbody2D>()){
    //                 Vector3 v = op.obj.GetComponent<Rigidbody2D>().velocity;
    //                 v.x = v.x > 0 ? v.x * collisions[entry.Key].positiveXMoveInfluence: v.x * collisions[entry.Key].negativeXMoveInfluence;
    //                 v.y = v.y > 0 ? v.y * collisions[entry.Key].positiveYMoveInfluence: v.y * collisions[entry.Key].negativeYMoveInfluence;
    //                 postUpdateVelocity += new Vector2(v.x,v.y);
    //                 RawMove(v);

    //             }
    //         }
    //     }
    // }
}

public struct oldPositions{
    public GameObject obj;
    public Vector3 oldPos;
    public Quaternion oldRot;
    public oldPositions(Collider2D col){
        obj = col.gameObject;
        oldPos = col.gameObject.transform.position;
        oldRot = col.gameObject.transform.rotation;
    }
}

public class ColInfo{
    public bool isContact;
    public float minAngle;
    public float maxAngle;
    public List<Collider2D> hits;
    public List<oldPositions> oldPos;
    public float positiveXMoveInfluence;
    public float negativeXMoveInfluence;
    public float positiveYMoveInfluence;
    public float negativeYMoveInfluence;
    public LayerMask mask;
    public bool exclusiveMask;
    public ColInfo(ColInfoBuilder cib){
        this.minAngle = cib.minAngle;
        this.maxAngle = cib.maxAngle;
        isContact = false;
        hits = new List<Collider2D>();
        oldPos = new List<oldPositions>();
        this.positiveXMoveInfluence = cib.positiveXMoveInfluence;
        this.negativeXMoveInfluence  = cib.negativeXMoveInfluence;
        this.positiveYMoveInfluence = cib.positiveYMoveInfluence;
        this.negativeYMoveInfluence = cib.negativeYMoveInfluence;
        this.mask = cib.mask;
        this.exclusiveMask = cib.exclusiveMask;
    }
    public ColInfo(){
        this.minAngle = -181;
        this.maxAngle = 181;
        isContact = false;
        hits = new List<Collider2D>();
        oldPos = new List<oldPositions>();
        this.positiveXMoveInfluence = 1;
        this.negativeXMoveInfluence  = 1;
        this.positiveYMoveInfluence = 1;
        this.negativeYMoveInfluence = 1;
        this.mask = LayerMask.GetMask("Default");
        this.exclusiveMask = true;
    }
}

[System.Serializable]
public struct ColInfoBuilder{
    // [System.SerializeField]
    public string key;
    [HideInInspector]
    public bool isContact;
    public float minAngle;
    public float maxAngle;
    [Range(0,1)]
    public float positiveXMoveInfluence;
    [Range(0,1)]
    public float negativeXMoveInfluence;
    [Range(0,1)]
    public float positiveYMoveInfluence;
    [Range(0,1)]
    public float negativeYMoveInfluence;
    public LayerMask mask;
    public bool exclusiveMask;
}


