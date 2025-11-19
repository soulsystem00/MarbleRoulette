using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class UniqueColorGenerator
{
    private static List<Color> generatedColors = new List<Color>();
    private static float minDistance = 0.2f; // 색상 간 최소 RGB 거리

    /// <summary>
    /// 중복되지 않는 랜덤 색상 반환
    /// </summary>
    public static Color GetUniqueColor()
    {
        Color newColor;
        int tries = 0;

        do
        {
            float hue = Random.value; // 0~1
            float saturation = Random.Range(0.7f, 1f);
            float value = Random.Range(0.7f, 1f);

            newColor = Color.HSVToRGB(hue, saturation, value);
            tries++;

            // 무한루프 방지: 100번 시도 후 그냥 반환
            if (tries > 100) break;

        } while (IsTooSimilar(newColor));

        generatedColors.Add(newColor);
        return newColor;
    }

    /// <summary>
    /// 이미 생성된 색상과 유사한지 확인
    /// </summary>
    private static bool IsTooSimilar(Color color)
    {
        foreach (var c in generatedColors)
        {
            if (ColorDistance(color, c) < minDistance)
                return true;
        }
        return false;
    }

    /// <summary>
    /// RGB 거리 계산
    /// </summary>
    private static float ColorDistance(Color a, Color b)
    {
        return Mathf.Sqrt(
            Mathf.Pow(a.r - b.r, 2) +
            Mathf.Pow(a.g - b.g, 2) +
            Mathf.Pow(a.b - b.b, 2)
        );
    }

    /// <summary>
    /// 색상 기록 초기화
    /// </summary>
    public static void Reset()
    {
        generatedColors.Clear();
    }
}
