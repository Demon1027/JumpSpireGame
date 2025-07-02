Shader "UI/CircleFadeDual"
{
    Properties
    {
        _Color ("Fade Color", Color) = (0, 0, 0, 1)
        _Cutoff ("Cutoff", Float) = 0.5
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Invert ("Invert", Float) = 0
        [HideInInspector] _MainTex ("Sprite Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Cutoff;
            float4 _Center;
            float _Invert;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 중심 기준 거리 계산
                float dist = distance(i.uv, _Center.xy);

                // 원형 마스크 생성
                float mask = step(_Cutoff, dist);

                // Invert 시 반전
                if (_Invert > 0.5) mask = 1.0 - mask;

                // 검정색 또는 페이드 색상으로 보간
                fixed4 fadeColor = _Color;
                fadeColor.a *= mask;

                return fadeColor;
            }
            ENDCG
        }
    }
}
