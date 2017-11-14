using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostRender : MonoBehaviour {

    [SerializeField]
    private Material LineMaterial;

    private Stack<Vector3> mLinesVerts = new Stack<Vector3>();


    //-----------------------------------Unity Functions-----------------------------------

    void OnPostRender()
    {
        if (!LineMaterial)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        while (mLinesVerts.Count > 0)
        {
            LineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(mLinesVerts.Pop());
            GL.Vertex(mLinesVerts.Pop());
            GL.End();
        }        
    }


    //----------------------------------Public Functions-----------------------------------

    public void DrawLine(Vector3 start, Vector3 end)
    {
        mLinesVerts.Push(start);
        mLinesVerts.Push(end);
    }
}
