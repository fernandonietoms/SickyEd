Shader "Custom/FogCurtainAdvancedDirectional"
{
    Properties
    {
        _NoiseTex ("Noise Texture", 2D) = "white" {}

        _Color ("Fog Color", Color) = (0,0,0,1)

        _Speed1 ("Speed Layer 1", Range(0,3)) = 0.2
        _Speed2 ("Speed Layer 2", Range(0,3)) = 0.4

        _Intensity ("Fog Density", Range(0,3)) = 1.2
        _Transparency ("Transparency", Range(0,1)) = 0.7

        _Height ("Fog Position", Range(0,1)) = 0.3
        _Smoothness ("Edge Smoothness", Range(0.01,1)) = 0.4

        _Scale ("Noise Scale", Range(0.5,6)) = 2

        _Mode ("Fog Mode (0 Bottom, 1 Top, 2 Center)", Range(0,2)) = 0
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

            sampler2D _NoiseTex;

            float _Speed1;
            float _Speed2;

            float _Intensity;
            float _Transparency;

            float _Height;
            float _Smoothness;

            float _Scale;

            float _Mode;

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


            float sampleFog(float2 uv)
            {
                float2 uv1 = uv * _Scale;
                float2 uv2 = uv * (_Scale * 1.7);

                uv1 += float2(_Time.y * _Speed1, 0);
                uv2 += float2(-_Time.y * _Speed2, _Time.y * 0.15);

                float noise1 = tex2D(_NoiseTex, uv1).r;
                float noise2 = tex2D(_NoiseTex, uv2).r;

                return (noise1 + noise2) * 0.5;
            }


            float verticalMask(float y)
            {
                if (_Mode < 0.5)
                {
                    // Bottom
                    return smoothstep(
                        _Height,
                        _Height + _Smoothness,
                        y
                    );
                }
                else if (_Mode < 1.5)
                {
                    // Top
                    return smoothstep(
                        1 - _Height,
                        1 - (_Height + _Smoothness),
                        y
                    );
                }
                else
                {
                    // Center
                    float bottom = smoothstep(
                        0.5 - _Height,
                        0.5 - (_Height + _Smoothness),
                        y
                    );

                    float top = smoothstep(
                        0.5 + _Height,
                        0.5 + (_Height + _Smoothness),
                        y
                    );

                    return bottom * top;
                }
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float fogNoise = sampleFog(uv);

                float mask = verticalMask(uv.y);

                float fog = fogNoise * mask * _Intensity;

                float alpha = fog * _Transparency;

                return fixed4(_Color.rgb, alpha);
            }

            ENDCG
        }
    }
}