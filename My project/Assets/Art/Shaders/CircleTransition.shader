Shader "Custom/CircleTransition"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _Smoothness ("Edge Smoothness", Range(0.001, 0.5)) = 0.1
        _Radius ("Circle Radius", Range(0.0, 2.0)) = 0
        _CenterX ("Center X", Range(0.0, 1.0)) = 0.5
        _CenterY ("Center Y", Range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _Smoothness;
            float _Radius;
            float _CenterX;
            float _CenterY;

            fixed4 _Color;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };


            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            void drawCircle(float2 uv, float2 center, float radius, float smoothness, out float output)
            {
                float dist = distance(uv, center);

                // borde suave tipo humo
                output = smoothstep(radius, radius - smoothness, dist);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(_CenterX, _CenterY);

                float aspect = _ScreenParams.x / _ScreenParams.y;

                float2 uv = i.uv;

                // corrección de aspecto
                uv.x = (uv.x - center.x) * aspect + center.x;

                float alpha;

                drawCircle(uv, center, _Radius, _Smoothness, alpha);

                return fixed4(_Color.rgb, 1 - alpha);
            }

            ENDCG
        }
    }
}