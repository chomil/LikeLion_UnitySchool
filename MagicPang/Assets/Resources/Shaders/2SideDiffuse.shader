Shader "SpritesDiffuse2Sided"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
        _Lighting("Lighting", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        // Shadow Caster Pass
        Pass
        {
            Tags
            {
                "LightMode"="ShadowCaster"
            }

            ZWrite On
            Blend One OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

        // Main rendering Pass
        CGPROGRAM
        #pragma surface surf TwoSidedLambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile_local _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"

        struct Input
        {
            float2 uv_MainTex;
            float4 color;
        };

        uniform float _Lighting;
        uniform float _Tint;

        float4 LightingTwoSidedLambert(SurfaceOutput s, float3 lightDir, float atten)
        {
            float NdotL = abs(dot(s.Normal, lightDir));
            float3 litColor = s.Albedo * _LightColor0.rgb * (NdotL * atten);
            float3 finalColor = lerp(s.Albedo * (NdotL * atten), litColor, _Lighting);

            float4 c;
            c.rgb = finalColor;
            c.a = s.Alpha;
            return c;
        }

        void vert(inout appdata_full v, out Input o)
        {
            // Handle flipping
            if (_Flip.x < 0) v.vertex.x = -v.vertex.x;
            if (_Flip.y < 0) v.vertex.y = -v.vertex.y;

            #if defined(PIXELSNAP_ON)
                v.vertex = UnityPixelSnap(v.vertex);
            #endif

            v.normal.z *= -1;

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            float4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
            o.Albedo = c.rgb * c.a;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Transparent/VertexLit"
}