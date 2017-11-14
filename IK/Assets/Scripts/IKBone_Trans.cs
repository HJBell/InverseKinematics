using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class IKBone_Trans : MonoBehaviour {

    public Vector3 pStartNode { get { return this.transform.position; } }
    public Vector3 pBoneVector { get { return this.transform.forward * Length; } }
    public Vector3 pEndNode { get { return pStartNode + pBoneVector; } }

    [SerializeField]
    private float Length = 1f;
    [SerializeField]
    [Range(0f, 90f)]
    private float UpConstraint = 45f;
    [SerializeField]
    [Range(0f, 90f)]
    private float DownConstraint = 45f;
    [SerializeField]
    [Range(0f, 90f)]
    private float RightConstraint = 45f;
    [SerializeField]
    [Range(0f, 90f)]
    private float LeftConstraint = 45f;


    // TESTING
    public IKBone_Trans childIKBone;


    private LineRenderer mLineRenderer;


    //-----------------------------------Unity Functions-----------------------------------

    private void Start()
    {
        mLineRenderer = this.GetComponent<LineRenderer>();
        mLineRenderer.positionCount = 2;
    }

    private void Update()
    {
        mLineRenderer.SetPositions(new Vector3[2] { pStartNode, pEndNode });
    }

    private void OnDrawGizmos()
    {
        if (childIKBone == null) return;

        var up = this.transform.TransformVector(Vector3.up);
        var down = this.transform.TransformVector(Vector3.down);
        var right = this.transform.TransformVector(Vector3.right);
        var left = this.transform.TransformVector(Vector3.left);

        float axesGizmoSize = 0.35f;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pEndNode, pEndNode + up * axesGizmoSize);
        Gizmos.DrawLine(pEndNode, pEndNode + down * axesGizmoSize * 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pEndNode, pEndNode + right * axesGizmoSize);
        Gizmos.DrawLine(pEndNode, pEndNode + left * axesGizmoSize * 0.5f);

        Gizmos.color = Color.cyan;

        var upC = Vector3.Lerp(pBoneVector, up, UpConstraint / 90f).normalized;
        var downC = Vector3.Lerp(pBoneVector, down, DownConstraint / 90f).normalized;
        var rightC = Vector3.Lerp(pBoneVector, right, RightConstraint / 90f).normalized;
        var leftC = Vector3.Lerp(pBoneVector, left, LeftConstraint / 90f).normalized;

        var upCEnd = pEndNode + upC * axesGizmoSize;
        var downCEnd = pEndNode + downC * axesGizmoSize;
        var rightCEnd = pEndNode + rightC * axesGizmoSize;
        var leftCEnd = pEndNode + leftC * axesGizmoSize;

        Gizmos.DrawLine(pEndNode, upCEnd);
        Gizmos.DrawLine(pEndNode, downCEnd);
        Gizmos.DrawLine(pEndNode, rightCEnd);
        Gizmos.DrawLine(pEndNode, leftCEnd);

        Gizmos.DrawLine(upCEnd, rightCEnd);
        Gizmos.DrawLine(rightCEnd, downCEnd);
        Gizmos.DrawLine(downCEnd, leftCEnd);
        Gizmos.DrawLine(leftCEnd, upCEnd);
    }


    //-----------------------------------Public Functions----------------------------------

    public void PositionStartNode(Vector3 pos)
    {
        var currentEndNode = pEndNode;
        this.transform.position = pos;
        this.transform.LookAt(currentEndNode);
    }

    public void PositionEndNode(Vector3 pos)
    {
        this.transform.LookAt(pos);
        transform.Translate(Vector3.forward * ((pos - pStartNode).magnitude - Length));
    }
}
