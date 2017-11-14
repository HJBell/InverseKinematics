using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKRoot : MonoBehaviour {

    [SerializeField]
    private Transform TargetTrans;
    [SerializeField]
    private int NodeCount = 5;
    [SerializeField]
    private float Length = 1.0f;
    [SerializeField]
    private bool AdvancedGizmos = false;

    private List<IKBone> mBones = new List<IKBone>();

    private IKBone pEffector { get { return mBones[mBones.Count - 1]; } }


    //-----------------------------------Unity Functions-----------------------------------

    private void Start()
    {
        float boneLength = Length / NodeCount;
        for (int i = 0; i < NodeCount; i++)
            mBones.Add(new IKBone(boneLength));
    }

    private void Update()
    {
        Vector3 targetPos = TargetTrans.position;

        for (int i = mBones.Count - 1; i >= 0; i--)
        {
            mBones[i].PositionEndNode(targetPos);
            targetPos = mBones[i].pStartNode;
        }

        for (int i = 0; i < mBones.Count; i++)
        {
            Vector3 parentEndPos = (i == 0) ? this.transform.position : mBones[i-1].pEndNode;
            mBones[i].PositionStartNode(parentEndPos);
        }

        foreach (var bone in mBones)
            bone.Render();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < mBones.Count; i++)
        {
            Color boneCol = (i % 2 == 0) ? Color.red : Color.magenta;
            if (AdvancedGizmos) mBones[i].RenderGizmos(boneCol);
            else mBones[i].RenderGizmosBasic(boneCol);
        }
    }
}