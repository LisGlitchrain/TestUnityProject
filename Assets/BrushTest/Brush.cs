using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public Color color;
    public float radius;
    bool merged = true;
    public Vector3 uvWorld;
    GameObject target;
    public Material baseMaterial;

    public RenderTexture canvasTexture;
    int brushCounter = 0; //Increases every brush instantiated
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Debug.Log(HitTestUVPosition(ref uvWorld));
            merged = false;
        }
        else if(!merged)
        {
            merged = true;
        }
    }

    bool HitTestUVPosition(ref Vector3 uvWorldPosition)
    {
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        Vector3 cursorPos = new Vector3(mousePos.x, mousePos.y, 0.0f);
        Ray cursorRay = Camera.main.ScreenPointToRay(cursorPos);
        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return false;
            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            Texture2D tempTexture = new Texture2D(hit.collider.gameObject.GetComponent<MeshRenderer>().material.mainTexture.width, 
                                                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.mainTexture.height);
            //tempTexture = hit.collider.gameObject.GetComponent<MeshRenderer>().material.mainTexture
            //tempTexture.set
            //hit.collider.gameObject.GetComponent<MeshRenderer>().material.mainTexture.
            uvWorldPosition.x = pixelUV.x;
            uvWorldPosition.y = pixelUV.y;
            uvWorldPosition.z = 0.0f;
            return true;
        }
        else
        {
            return false;
        }
    }



    void MergeTexture()
    {
        RenderTexture.active = canvasTexture;
        int width = canvasTexture.width;
        int height = canvasTexture.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base
        foreach (Transform child in this.transform)
        {//Clear brushes
            Destroy(child.gameObject);
        }
        brushCounter = 0; //Reset how many brushes in the scene
    }
}
