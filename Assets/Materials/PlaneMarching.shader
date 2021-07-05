Shader "Unlit/PlaneMarching"
{
    Properties
    {
        _CellularTex ("Texture", 3D) = "white" {}
        _Epsilon("Step Size", float) = 0.01

    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
        
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
                float3 wPos :TEXCOORD0;
                float4 pos  :SV_POSITION;

            };

            float mincomp(in float3 p) { return min(p.x, min(p.y, p.z)); }

            sampler3D _CellularTex;
            float4 _CellularTex_ST;
            float4 _CellularTex_TexelSize;

            float maxDistToCheck = 128.0f;
            float _Epsilon;
           
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            float4 PlaneMarch(float3 p0, float3 d) {

                float t = 0;
                [loop]
                while (t <= maxDistToCheck) {
                    
                    float3 p = p0 + d * t;
                    float4 c = tex3D(_CellularTex, p / _CellularTex_TexelSize.z);
                    
                    if (c.a > 0.9) {
                        return c;
                    }

                    float3 deltas = (step(0, d) - frac(p)) / d;
                    t += max(mincomp(deltas), _Epsilon);
                }

                return float4(0,0,0,0);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                 float3 rayOrigin = i.wPos;
                 float3 rayDirection = normalize(i.wPos - _WorldSpaceCameraPos);
                 
                 float4  color =  PlaneMarch(rayOrigin, rayDirection);

                 if (color.a == 0) {
                     discard;
                 }
                 return  color;
            }
            ENDCG
        }
    }
}
