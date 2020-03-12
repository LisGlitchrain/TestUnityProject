Shader "Stars/Unlit4"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "white" {}
        _FlatFactor("Flat Factor", Range(0, 1)) = 0
        _Color("Color", Color) = (1,1,1,1)
        _Size("Size", Range(0.0001, 0.05)) = 0.025
        _CutValue("Cut Value", Range(0.0001, 1)) = 0.025
        _MyCameraAspectRatio("Camera Aspect ", float) = 1.0
        _SizeX("Size X", float) = 0.05
        _SizeY("Size Y", float) = 0.05
    }

        SubShader
        {

            Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }
            Cull off

            Pass
            {
                Name "ForwardBase"
                Tags {"LightMode" = "ForwardBase"}

                CGPROGRAM

                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma geometry geom
                #pragma fragment frag

                #pragma multi_compile_instancing

                #pragma multi_compile_fwdbase
                #include "Lighting.cginc"
                #include "AutoLight.cginc"

                sampler2D _MainTex;
                float _FlatFactor;
                float4 _Color;
                float _Size;
                float _CutValue;
                float _CameraAspectRatio;
                float _SizeX;
                float _SizeY;

                struct v2g
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 vertex : POSITION1;
                    //float3 normal : NORMAL;
                    float4 color : COLOR1;
                };

                struct g2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    //fixed3 diff : COLOR0;
                    float4 color : COLOR1;
                };

                v2g vert(appdata_full i)
                {
                    v2g o;
                    o.vertex = i.vertex;
                    o.pos = UnityObjectToClipPos(i.vertex);
                    o.uv = i.texcoord;
                    //o.normal = i.normal;
                    o.color = i.color;
                    return o;
                }

                [maxvertexcount(6)]
                void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) //triangle
                {
                    g2f o;

                    // высчитываем плоскую нормаль
                   /* float3 vecA = IN[1].vertex - IN[0].vertex;
                    float3 vecB = IN[2].vertex - IN[0].vertex;
                    float3 flatNormal = cross(vecA, vecB);
                    flatNormal = normalize(mul(flatNormal, (float3x3) unity_WorldToObject));*/

                    //// высчитываем барицентрический юв/цвет
                    //float2 flatUv = (IN[0].uv + IN[1].uv + IN[2].uv) / 3;
                    //float4 flatColor = (IN[0].color + IN[1].color + IN[2].color) / 3;

                    //for (int i = 0; i < 3; i++) {
                    //    // высчитываем гладкую нормаль
                    //    float3 smoothNormal = normalize(mul(IN[i].normal, (float3x3) unity_WorldToObject));
                    //    // лерпим между гладкой и плоской
                    //    float3 lerpNormal = lerp(smoothNormal, flatNormal, _FlatFactor);
                    //    // задаем диффуз и амбиент
                    //    half nl = max(0, dot(lerpNormal, _WorldSpaceLightPos0.xyz));
                    //    o.diff = nl * _LightColor0.rgb;
                    //    o.diff.rgb += ShadeSH9(half4(lerpNormal,1));

                    //    // лерпим между гладкой и плоской юв/цветом
                    //    o.uv = lerp(IN[i].uv, flatUv, _FlatFactor);
                    //    o.color = lerp(IN[i].color, flatColor, _FlatFactor);

                    //    //задаем позицию точки
                    //    o.pos = IN[i].pos;
                    //    triStream.Append(o);
                    //}
                    o.color = _Color;
                    //float sizeX = _Size;// *_CameraAspectRatio;
                    //float sizeY = _Size;// / _CameraAspectRatio;
                    //o.color = _Color;
                    o.pos = IN[0].pos;               
                    o.uv = float2(0, 0);
                    //o.size = 1;
                    /*o.diff = float3(1,0,0);
                    o.diff.rgb = fixed3(1,1,1);
                    o.uv = float3(0,0,0);*/
                    triStream.Append(o);
                    
                    o.color = _Color;
                    o.uv = float2(1, 0);
                    o.pos = IN[0].pos + float4(_SizeX, 0, 0, 0); // float4(sizeX, 0, 0, 0);

                    //o.diff = float3(1, 1, 1);
                    //o.diff.rgb = fixed3(1, 1, 1);
                    //o.uv = float3(0, 0, 0);
                    triStream.Append(o);

                    o.color = _Color;
                    o.uv = float2(0, 1);
                    o.pos = IN[0].pos + float4(0, _SizeY, 0, 0);
                    //o.diff = float3(1, 1, 1);
                    //o.diff.rgb = fixed3(1, 1, 1);
                    //o.uv = float3(0, 0, 0);
                    triStream.Append(o);


                    triStream.RestartStrip();

                    o.color = _Color;

                    //o.color = _Color;
                    o.pos = IN[0].pos + float4(_SizeX, _SizeY, 0, 0);
                    o.uv = float2(1, 1);
                    //o.size = 1;
                    /*o.diff = float3(1,0,0);
                    o.diff.rgb = fixed3(1,1,1);
                    o.uv = float3(0,0,0);*/
                    triStream.Append(o);

                    o.color = _Color;
                    o.uv = float2(1, 0);
                    o.pos = IN[0].pos + float4(_SizeX, 0, 0, 0);

                    //o.diff = float3(1, 1, 1);
                    //o.diff.rgb = fixed3(1, 1, 1);
                    //o.uv = float3(0, 0, 0);
                    triStream.Append(o);

                    o.color = _Color;
                    o.uv = float2(0, 1);
                    o.pos = IN[0].pos + float4(0, _SizeY, 0, 0);
                    //o.diff = float3(1, 1, 1);
                    //o.diff.rgb = fixed3(1, 1, 1);
                    //o.uv = float3(0, 0, 0);
                    triStream.Append(o);

                    //o.color = _Color;
                    //o.pos = IN[0].pos + float4(0.025f, 0.025f, 0, 0);
                    ////o.diff = float3(1, 1, 1);
                    ////o.diff.rgb = fixed3(1, 1, 1);
                    ////o.uv = float3(0, 0, 0);
                    //triStream.Append(o);
                }

                float4 frag(g2f i, v2g ivg) : COLOR //
                {
                    //float4 col = tex2D(_MainTex, i.uv);

                    /*float4 col = float4(1, 1, 1, 1);*/
                    float4 col = 1;
                    //col.rgb *= i.diff * i.color;


                    float x = i.uv.x;
                    float y = i.uv.y;
                    //float dis = sqrt(pow((0.5 - x), 2) + pow((0.5 - y), 2));
                    float dis = sqrt(pow((0.5 - x), 2) + pow((0.5 - y), 2));

                    if (dis > _CutValue) {
                        discard;
                    }
                    else {
                        col = _Color;

                        //float innerRadius = (_ComponentWidth * 0.5 - _BoundWidth) / _ComponentWidth;
                        //if (dis > innerRadius) {
                        //    c = _BoundColor;
                        //    //c.a = c.a*antialias(_BoundWidth, dis, innerRadius);
                        //}
                        //else {
                        //    c = _BgColor;
                        //}
                    }
                    return col;

                }

                ENDCG
            }
        }
            //Fallback "Diffuse"
}