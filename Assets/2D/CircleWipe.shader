Shader "Unlit/CircleWipe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_Radius ("Wipe Radius", Float) = 0
        _MaskTexture ("Texture", 2D) = "white" {}
        _Horizontal("Horizontal Ratio", Float) = 16
        _Vertical("Vertical Ratio", Float) = 9
        _RadiusSpeed("Radius Speed", Float) = 1
        _FadeColor("Fade Color", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTexture;
            float4 _MainTex_ST;
            float _Radius;
            float _RadiusSpeed;
            float _Horizontal;
            float _Vertical;
            fixed4 _FadeColor : COLOR;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MaskTexture, i.uv).a;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //return col;
                float3 pos = float3((i.uv.x - 0.5) / _Vertical, (i.uv.y - 0.5) / _Horizontal, 0);
                
                return length(pos) >  _Radius / _RadiusSpeed ? _FadeColor : col;
            }
            ENDCG
        }
    }
}
