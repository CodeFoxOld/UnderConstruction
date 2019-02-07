Shader "Custom/SpawnShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Range(0,1)) = 0.5
        _BlinkSpeed("Blink Frequency", Range(0,200)) = 50
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

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _EmissionColor;
        half _EmissionStrength;
        half _BlinkSpeed;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed positiveTime = abs(sin(_Time * _BlinkSpeed));
            fixed4 c = _EmissionColor;
            o.Albedo = _EmissionColor.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = _EmissionColor.a * _EmissionStrength * positiveTime;
            o.Emission = _EmissionColor * _EmissionStrength * positiveTime;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
