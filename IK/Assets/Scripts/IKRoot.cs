using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKNode
{
    public Vector3 Pos;
    public float Length;

    public Vector3 pParentPos { get { return mParent == null ? mParentTrans.position : mParent.Pos; } }
    public IKNode pParent { set { mParent = value; } }
    public Transform pParentTrans { set { mParentTrans = value; } }

    private IKNode mParent;
    private Transform mParentTrans;
}

public class IKRoot : MonoBehaviour
{

    [SerializeField]
    private Transform TargetTrans;
    [SerializeField]
    private float NodeTranslationSpeed = 10.0f;
    [SerializeField]
    private int NodeCount = 5;
    [SerializeField]
    private float ArmLength = 1.0f;

    private List<IKNode> mNodes = new List<IKNode>();
    private PostRender mPostRender;

    private IKNode pEffector { get { return mNodes[mNodes.Count - 1]; } }


    //-----------------------------------Unity Functions-----------------------------------

    private void Awake()
    {
        mPostRender = FindObjectOfType<PostRender>();
    }

    private void Start()
    {
        for (int i = 0; i < NodeCount; i++)
        {
            mNodes.Add(new IKNode());
            mNodes[i].Pos = Vector3.one;
            mNodes[i].Length = ArmLength / NodeCount;
            if (i == 0) mNodes[i].pParentTrans = this.transform;
            else mNodes[i].pParent = mNodes[i - 1];
        }
    }

    private void Update()
    {
        Vector3 targetPos = TargetTrans.position;

        // Looping from the effector node to the root node.
        for (int i = mNodes.Count - 1; i >= 0; i--)
        {
            // Translating the node to its target pos.
            var translation = targetPos - mNodes[i].Pos;
            var translationMultiplier = Mathf.Clamp(Time.deltaTime * NodeTranslationSpeed, 0.0f, translation.magnitude);
            mNodes[i].Pos += translation.normalized * translationMultiplier;

            // Calculating point on the vector that is the bone length from the current node in
            // the direction of its parent and setting this to be the parents target pos.
            Vector3 nodeToParent = (mNodes[i].Pos - mNodes[i].pParentPos).normalized;
            targetPos = mNodes[i].Pos - nodeToParent * mNodes[i].Length;
        }

        // Looping from the root node to the effector node.
        for (int i = 0; i < mNodes.Count; i++)
        {
            // Moving the node back towards their parent so that they 
            // are no more than the maximum distance away from them.
            Vector3 parentToNode = (mNodes[i].Pos - mNodes[i].pParentPos).normalized;
            mNodes[i].Pos = mNodes[i].pParentPos + parentToNode * mNodes[i].Length;
        }

        // Rendering the IK limb.
        foreach (var node in mNodes)
            mPostRender.DrawLine(node.Pos, node.pParentPos);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position, 0.2f);
        for (int i = 0; i < mNodes.Count; i++)
        {
            Gizmos.DrawSphere(mNodes[i].Pos, 0.1f);
            Vector3 lineDirection = (mNodes[i].Pos - mNodes[i].pParentPos).normalized;
            Gizmos.DrawLine(mNodes[i].Pos, mNodes[i].Pos - lineDirection * mNodes[i].Length);
        }
    }
}