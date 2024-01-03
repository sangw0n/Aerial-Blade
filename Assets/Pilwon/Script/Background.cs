using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private MeshRenderer mesh;
    private float offset;
    public float speed;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
        mesh.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
