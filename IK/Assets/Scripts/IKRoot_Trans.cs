using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IKRoot_Trans : MonoBehaviour {

    [SerializeField]
    private Transform TargetTrans;
    [SerializeField]
    private List<IKBone_Trans> mBones = new List<IKBone_Trans>();


    //-----------------------------------Unity Functions-----------------------------------

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
            Vector3 parentEndPos = (i == 0) ? this.transform.position : mBones[i - 1].pEndNode;
            mBones[i].PositionStartNode(parentEndPos);
        }
    }
}
