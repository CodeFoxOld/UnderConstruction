Shader "Custom/IndicatorShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Noise("Noise Map", 2D) = "white" {}
        _GradientMap ("Gradient Map", 2D) = "white" {}
        _Normal ("Normal Map", 2D) = "white" {}
        _EmissiveMap("Emissive Map", 2D) = "white" {}
        _EmissionColor("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength("Emission Intensity", Range(0,1)) = 0
        _ScrollSpeedX ("Scroll Speed X", Range(-10,10)) = 0
        _ScrollSpeedY ("Scroll Speed Y", Range(-10,10)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:blend

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Normal;
        sampler2D _Noise;
        sampler2D _GradientMap;
        sampler2D _EmissiveMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Normal;
            float2 uv_Noise;
            float2 uv_GradientMap;
            float2 uv_EmissiveMap;
        };
        
        half _ScrollSpeedX;
        half _ScrollSpeedY;
        half _Glossiness;
        half _Metallic;
        half _EmissionStrength;
        fixed4 _Color;
        fixed4 _EmissionColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float gradient = tex2D (_GradientMap, IN.uv_GradientMap);
            
            half2 noiseposition = IN.uv_Noise;
            noiseposition.x += _Time * _ScrollSpeedX;
            noiseposition.y += _Time * _ScrollSpeedY;        
            float noise = tex2D(_Noise, noiseposition);
            half3 normal = tex2D(_Normal, noiseposition);
            
            fixed4 e = tex2D(_MainTex, IN.uv_EmissiveMap) * (_EmissionColor * _EmissionStrength);
            
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = gradient * (noise * c.a);
            o.Normal = normal;
            o.Emission = e;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
