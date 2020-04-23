using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer texture_renderer;
    public MeshFilter mesh_filter;
    public MeshRenderer mesh_renderer;
    public MeshCollider mesh_collider;

    public void draw_texture(Texture2D texture) {
        // Apply texture to the texture renderer
        texture_renderer.sharedMaterial.mainTexture = texture;
        texture_renderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void draw_mesh(MeshData mesh_data, Texture2D texture) {
        Mesh mesh = mesh_data.create_mesh();
        mesh_renderer.sharedMaterial.mainTexture = texture;
        mesh_filter.sharedMesh = mesh;
        mesh_collider.sharedMesh = mesh;
    }

}
