using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    public static MeshData generate_terrain_mesh(float[,] height_map, float height_multiplier, AnimationCurve height_curve, int level_of_detail) {
        int width = height_map.GetLength(0);
        int height = height_map.GetLength(1);

        // make terrain centered
        float top_left_x = (width-1)/-2.0f;
        float top_left_z = (height-1)/2.0f;

        int mesh_simplification_increment = (level_of_detail == 0)? 1 : level_of_detail * 2;
        int verticesPerLine = (width - 1) / mesh_simplification_increment + 1;

        MeshData mesh_data = new MeshData(verticesPerLine, verticesPerLine);
        int vertex_idx = 0;

        for (int y = 0; y < height; y += mesh_simplification_increment) {
            for (int x = 0; x < width; x += mesh_simplification_increment) {
                mesh_data.vertices[vertex_idx] = new Vector3(top_left_x + x, height_curve.Evaluate(height_map[x,y]) * height_multiplier, top_left_z - y);
                mesh_data.uvs[vertex_idx] = new Vector2(x/(float)width, y/(float)height);

                if (x < width - 1 && y < height - 1) {
                    mesh_data.add_triangle(vertex_idx, vertex_idx + verticesPerLine + 1, vertex_idx + verticesPerLine); // bottom left
                    mesh_data.add_triangle(vertex_idx + verticesPerLine + 1, vertex_idx, vertex_idx + 1); // top right
                }

                vertex_idx++;
            }
        }

        return mesh_data;
    }

}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangle_index;

    public MeshData(int mesh_width, int mesh_height) {
        vertices = new Vector3[mesh_width*mesh_height];
        triangles = new int[(mesh_width-1)*(mesh_height-1)*6];
        uvs = new Vector2[mesh_width*mesh_height];
    }

    public void add_triangle(int a, int b, int c) {
        triangles[triangle_index] = a;
        triangles[triangle_index+1] = b;
        triangles[triangle_index+2] = c;
        triangle_index += 3;
    }

    Vector3[] calculate_normals() {
        Vector3[] vertex_normals = new Vector3[vertices.Length];
        int triangle_count = triangles.Length / 3;
        for (int i = 0; i < triangle_count; i++) {
            int normal_triangle_idx = i*3;
            int vertex_idx_a = triangles[normal_triangle_idx];
            int vertex_idx_b = triangles[normal_triangle_idx+1];
            int vertex_idx_c = triangles[normal_triangle_idx+2];

            Vector3 triangle_normal = surface_normal_from_indices(vertex_idx_a, vertex_idx_b, vertex_idx_c);
            vertex_normals[vertex_idx_a] += triangle_normal;
            vertex_normals[vertex_idx_b] += triangle_normal;
            vertex_normals[vertex_idx_c] += triangle_normal;
        }

        for (int i = 0; i < vertex_normals.Length; i++)
            vertex_normals[i].Normalize();

            return vertex_normals;
    }

    Vector3 surface_normal_from_indices(int idx_a, int idx_b, int idx_c) {
        Vector3 point_a = vertices[idx_a];
        Vector3 point_b = vertices[idx_b];
        Vector3 point_c = vertices[idx_c];

        Vector3 side_ab = point_b - point_a;
        Vector3 side_ac = point_c - point_a;

        return Vector3.Cross(side_ab, side_ac).normalized;
    }

    public Mesh create_mesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = calculate_normals();

        return mesh;
    }
}
