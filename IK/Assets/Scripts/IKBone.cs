using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBone {

    public Vector3 pStartNode { get { return mStartNode; } }
    public Vector3 pEndNode { get { return mEndNode; } }
    public Vector3 pBoneVector { get { return mEndNode - mStartNode; } }

    private Vector3 mStartNode = Vector3.zero;
    private Vector3 mEndNode = Vector3.zero;
    private float mLength = 0.0f;


    //-----------------------------------Public Functions----------------------------------

    public IKBone(float length)
    {
        mLength = length;
    }

    public void PositionStartNode(Vector3 pos)
    {
        PositionNode(ref mStartNode, pos, ref mEndNode, -1.0f);
    }

    public void PositionEndNode(Vector3 pos)
    {
        PositionNode(ref mEndNode, pos, ref mStartNode, 1.0f);
    }

    public void Render()
    {
        MonoBehaviour.FindObjectOfType<PostRender>().DrawLine(mStartNode, mEndNode);
    }

    public void RenderGizmosBasic(Color col)
    {
        Gizmos.color = col;
        Gizmos.DrawLine(mStartNode, mEndNode);
    }

    public void RenderGizmos(Color col)
    {
        RenderGizmosBasic(col);
        Gizmos.color = col;
        Gizmos.DrawWireSphere(pStartNode, 0.1f);
        Gizmos.DrawWireSphere(pEndNode, 0.05f);
    }


    //-----------------------------------Private Functions----------------------------------

    private void PositionNode(ref Vector3 node, Vector3 pos, ref Vector3 offsetNode, float offsetMultiplier)
    {
        node = pos;
        offsetNode += pBoneVector.normalized * (pBoneVector.magnitude - mLength) * offsetMultiplier;
    }
}
