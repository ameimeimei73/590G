using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh, FallofMap};
    public DrawMode draw_mode;

    const int map_chunk_size = 241;
    [Range(0, 6)]
    public int level_of_detail;
    public float noise_scale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool use_falloff;

    public float mesh_height_multiplier;
    public AnimationCurve  mesh_height_curve;

    public bool auto_update;

    public TerrainType[] regions;

    float[,] falloff_map;

    void Awake() {
        falloff_map = FalloffGenerator.generate_falloff_map(map_chunk_size);
    }

    public void draw_map_in_editor() {
        MapData map_data = generate_map_data();

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (draw_mode == DrawMode.NoiseMap)
            display.draw_texture(TextureGenerator.texture_from_height_map(map_data.height_map));
        else if (draw_mode == DrawMode.ColorMap)
            display.draw_texture(TextureGenerator.texture_from_color_map(map_data.color_map, map_chunk_size, map_chunk_size));
        else if (draw_mode == DrawMode.Mesh)
            display.draw_mesh(MeshGenerator.generate_terrain_mesh(map_data.height_map, mesh_height_multiplier, mesh_height_curve, level_of_detail), TextureGenerator.texture_from_color_map(map_data.color_map, map_chunk_size, map_chunk_size));
        else if (draw_mode == DrawMode.FallofMap)
            display.draw_texture(TextureGenerator.texture_from_height_map(FalloffGenerator.generate_falloff_map(map_chunk_size)));
    }

    MapData generate_map_data() {
        float[,] noise_map = Noise.generate_noise_map(map_chunk_size, map_chunk_size, seed, noise_scale, octaves, persistance, lacunarity, offset);
        Color[] color_map = new Color[map_chunk_size * map_chunk_size];

        for (int y = 0; y < map_chunk_size; y++) {
            for (int x = 0; x < map_chunk_size; x++) {
                if (use_falloff)
                    noise_map[x, y] = Mathf.Clamp01(noise_map[x, y] - falloff_map[x, y]);
                float current_height = noise_map[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (current_height <= regions[i].height) {
                        color_map[y*map_chunk_size+x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        return new MapData(noise_map, color_map);
    }

    void OnValidate() {
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;

        falloff_map = FalloffGenerator.generate_falloff_map(map_chunk_size);
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}

public struct MapData {
    public float[,] height_map;
    public Color[] color_map;

    public MapData (float[,] height_map, Color[] color_map) {
        this.height_map = height_map;
        this.color_map = color_map;
    }
}