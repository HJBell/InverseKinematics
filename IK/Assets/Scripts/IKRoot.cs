using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IKNode
{
    public Transform Trans;
    [HideInInspector]
    public Transform Parent;
    [HideInInspector]
    public float Length;
}

public class IKRoot : MonoBehaviour {

    [SerializeField]
    private Transform TargetTrans;

    [SerializeField]
    private float NodeTranslationSpeed = 10.0f;

    [SerializeField]
    private List<IKNode> Nodes = new List<IKNode>();

    private IKNode Effector { get { return Nodes[Nodes.Count - 1]; } }


    //-----------------------------------Unity Functions-----------------------------------

    private void Start()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Nodes[i].Parent = ((i == 0) ? this.transform : Nodes[i - 1].Trans);
            Nodes[i].Length = Vector3.Distance(Nodes[i].Trans.position, Nodes[i].Parent.position);
        }
    }

    private void Update()
    {
        Vector3 targetPos = TargetTrans.position;

        // Looping from the effector node to the root node.
        for(int i = Nodes.Count -1; i >= 0; i--)
        {
            // Translating the node to its target pos.
            var translation = targetPos - Nodes[i].Trans.position;
            var translationMultiplier = Mathf.Clamp(Time.deltaTime * NodeTranslationSpeed, 0.0f, translation.magnitude);
            Nodes[i].Trans.Translate(translation.normalized * translationMultiplier);

            // Calculating point on the vector that is the bone length from the current node in
            // the direction of its parent and setting this to be the parents target pos.
            Vector3 nodeToParent = (Nodes[i].Trans.position - Nodes[i].Parent.position).normalized;
            targetPos = Nodes[i].Trans.position - nodeToParent * Nodes[i].Length;
        }

        // Looping from the root node to the effector node.
        for (int i = 0; i < Nodes.Count; i++)
        {
            // Moving the node back towards their parent so that they 
            // are no more than the maximum distance away from them.
            Vector3 parentToNode = (Nodes[i].Trans.position - Nodes[i].Parent.position).normalized;
            Nodes[i].Trans.position = Nodes[i].Parent.position + parentToNode * Nodes[i].Length;
        }

    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position, 0.2f);
        for (int i = 0; i < Nodes.Count; i++)
        {
            Gizmos.DrawSphere(Nodes[i].Trans.position, 0.1f);
            Vector3 lineDirection = (Nodes[i].Trans.position - Nodes[i].Parent.position).normalized;
            Gizmos.DrawLine(Nodes[i].Trans.position, Nodes[i].Trans.position - lineDirection * Nodes[i].Length);
        }
    }
}
