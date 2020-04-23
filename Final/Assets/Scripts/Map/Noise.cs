using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public static float[,] generate_noise_map(int m_width, int m_height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        float[,] noise_map = new float[m_width, m_height];

        // generate random starting point of the map by seed
        System.Random prng = new System.Random(seed);
        Vector2[] octave_offsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offset_x = prng.Next(-100000, 100000) + offset.x;
            float offset_y = prng.Next(-100000, 100000) + offset.y;
            octave_offsets[i] = new Vector2(offset_x, offset_y);
        }

        // handle devide by zero problem
        if (scale <= 0)
            scale = 0.0001f;

        float max_noise_height = float.MinValue;
        float min_noise_height = float.MaxValue;

        float half_width = m_width / 2.0f;
        float half_height = m_height / 2.0f;

        for (int y = 0; y < m_height; y++) {
            for (int x = 0; x < m_width; x++) {
                float amplitute = 1;
                float frequency = 1;
                float noise_height = 0;

                for (int i = 0; i < octaves; i++) {
                    float sample_x = (x - half_width) / scale * frequency + octave_offsets[i].x;
                    float sample_y = (y - half_height) / scale * frequency + octave_offsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sample_x, sample_y) * 2 - 1;
                    noise_height += perlinValue * amplitute;

                    amplitute *= persistance; // decrease each octaves
                    frequency *= lacunarity; // increase each octaves
                }

                if (noise_height > max_noise_height)
                    max_noise_height = noise_height;
                else if (noise_height < min_noise_height)
                    min_noise_height = noise_height;

                noise_map[x, y] = noise_height;
            }
        }

        // normalize noise map to [0.0, 1.0]
        for (int y = 0; y < m_height; y++) {
            for (int x = 0; x < m_width; x++) {
                noise_map[x, y] = Mathf.InverseLerp(min_noise_height, max_noise_height, noise_map[x, y]);
            }
        }

        return noise_map;
    }

}
