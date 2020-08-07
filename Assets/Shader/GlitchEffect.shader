Shader "Hidden/GlitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amount ("Amount", Float) = 10
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Amount;

            //2D (returns 0 - 1)
            float random2d(float2 n) { 
                return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
            }

            float randomRange (in float2 seed, in float min, in float max) {
            		return min + random2d(seed) * (max - min);
            }

            // return 1 if v inside 1d range
            float insideRange(float v, float bottom, float top) {
               return step(bottom, v) - step(top, v);
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float3 color = tex2D(_MainTex, i.uv).rgb;
                float time = _Time.x * 0.05f;

                for(float j = 0; j < _Amount; j += 2)
                {
                    float vertical = random2d(float2(time, 53.3532 + (float)j));
                    float horizontal = random2d(float2(time, 237.3263 + (float)j)) * .25;
                    float offset = randomRange(float2(time, 42.324 + (float)j), -2, 2);
                    
                    float2 uv = i.uv;
                    uv += float2(-offset, offset);

                    if(insideRange(i.uv.y, vertical, frac(vertical + horizontal)) == 1) 
                    {
                        color.r = tex2D(_MainTex, uv - random2d(float2(-horizontal, vertical))).r;
                        color.g = tex2D(_MainTex, uv + random2d(float2(horizontal, vertical))).g;
                        color.b = tex2D(_MainTex, uv+ random2d(float2(horizontal, -vertical))).b;
                        color *=1.3;
                    }
                    
                    
                }

                //color.r = tex2D(_MainTex, i.uv + float2(randomRange(i.uv + time, )))
                return float4(color, 1);
            }
            ENDCG
        }
    }
}
