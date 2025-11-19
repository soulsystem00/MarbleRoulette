Shader "Custom/RadialFillSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0,1)) = 1
        _StartAngle ("Start Angle (deg)", Range(0,360)) = 0
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR; // SpriteRenderer color
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float _FillAmount;
            float _StartAngle;
            fixed4 _Color;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color; // SpriteRenderer Color 반영
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 center = float2(0.5, 0.5);
                float2 dir = i.uv - center;

                // 0~1로 정규화된 angle
                float angle = atan2(dir.y, dir.x) / (2 * UNITY_PI) + 0.5;

                // 시작 각도 offset 적용
                float start = _StartAngle / 360.0;
                angle -= start;
                if(angle < 0) angle += 1;

                if(angle > _FillAmount) discard;

                fixed4 col = tex2D(_MainTex, i.uv) * i.color; // Color 곱하기
                return col;
            }
            ENDCG
        }
    }
}