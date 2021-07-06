Shader "Unlit/PlaneMarching"
{
    Properties
    {
        _CellularTex ("Texture", 3D) = "" {}
        _Epsilon("Step Size", float) = 0.01

    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100
        Cull Off
        ZTest LEqual
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texcoord: TEXCOORD0;
            };

            struct v2f
            {
                float3 wPos :TEXCOORD0;
                float4 pos  :SV_POSITION;
                float3 texcoord :TEXCOORD1;
            };

            float mincomp(in float3 p) { return min(p.x, min(p.y, p.z)); }

            sampler3D _CellularTex;
            float4 _CellularTex_ST;
            float4 _CellularTex_TexelSize;

            float maxDistToCheck = 12800.0f;
            float _Epsilon;
           
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.texcoord = v.texcoord;
                return o;
            }
            float4 BlendUnder(float4 color, float4 newColor)
            {
                color.rgb += (1.0 - color.a) * newColor.a * newColor.rgb;
                color.a += (1.0 - color.a) * newColor.a;
                return color;
            }
            float4 PlaneMarch(float3 p0, float3 d) {
                float4 color = float4(0, 0, 0, 0);

                float t = 0;
                [loop]
                while (t <= maxDistToCheck) {
                    
                    float3 p = p0 + d * t;
                    float4 c = tex3D(_CellularTex, p / _CellularTex_TexelSize.w);
                    
                    if (c.a > 0.0) {
                        color =  BlendUnder(color,c);
                       // break;
                    }

                    float3 deltas = (step(0, d) - frac(p)) / d;
                    t += max(mincomp(deltas), _Epsilon);
                }

                return color;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                 float3 rayOrigin = i.wPos;
                // float3 viewDir = WorldSpaceViewDir(i.pos).xyz;

                 float3 rayDirection =  normalize(i.wPos - _WorldSpaceCameraPos);
                 
                 half4  color =   PlaneMarch(rayOrigin, rayDirection);

                 if (color.a == 0 ) {
                     discard;
                 }
                 return  color;
            }
            ENDCG
        }
    }
}
