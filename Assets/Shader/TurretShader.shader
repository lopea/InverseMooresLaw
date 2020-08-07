Shader "Unlit/TurretShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,1,1,1)
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
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            //u a g, iq
            float cubeDist(float2 pos, float2 size)
            {
                float2 d = abs(pos) - size;
                return length(max(d, 0)) + min(max(d.x,d.y), 0.0);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                i.uv *= 5;
                
                float dist = cubeDist(i.uv -2.5, 0.5);
                dist = (dist + _Time * 10) % .8;
                dist = 1-dist + 0.3;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col = col * dist;
                col.a = 1;
                return col * _Color; 
            }
            ENDCG
        }
    }
}
