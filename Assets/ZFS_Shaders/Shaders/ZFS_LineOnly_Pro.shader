// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,ufog:True,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|emission-2476-OUT;n:type:ShaderForge.SFN_DepthBlend,id:5,x:35164,y:32709|DIST-1022-OUT;n:type:ShaderForge.SFN_OneMinus,id:884,x:34998,y:32709|IN-5-OUT;n:type:ShaderForge.SFN_Multiply,id:890,x:33420,y:32726|A-1876-RGB,B-1665-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1022,x:35329,y:32709,ptlb:Edge_Detection_Distance,ptin:_Edge_Detection_Distance,glob:False,v1:3;n:type:ShaderForge.SFN_Append,id:1590,x:34817,y:32768|A-884-OUT,B-1618-V;n:type:ShaderForge.SFN_TexCoord,id:1618,x:34998,y:32829,uv:0;n:type:ShaderForge.SFN_Tex2d,id:1620,x:34640,y:32768,ptlb:Gradient_Edge_Detection,ptin:_Gradient_Edge_Detection,ntxv:0,isnm:False|UVIN-1590-OUT;n:type:ShaderForge.SFN_TexCoord,id:1652,x:33776,y:32948,uv:0;n:type:ShaderForge.SFN_Append,id:1654,x:33595,y:32888|A-1665-OUT,B-1652-V;n:type:ShaderForge.SFN_Tex2d,id:1656,x:33414,y:32888,ptlb:Gradient_Color,ptin:_Gradient_Color,ntxv:0,isnm:False|UVIN-1654-OUT;n:type:ShaderForge.SFN_Clamp,id:1665,x:33790,y:32784|IN-2450-OUT,MIN-2647-OUT,MAX-2648-OUT;n:type:ShaderForge.SFN_Vector1,id:1666,x:34025,y:32914,v1:0.9;n:type:ShaderForge.SFN_Vector1,id:1667,x:34025,y:32860,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:1771,x:34455,y:32768,ptlb:Edge_Detection,ptin:_Edge_Detection,on:True|A-1772-OUT,B-1620-R;n:type:ShaderForge.SFN_Vector1,id:1772,x:34650,y:32689,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:1858,x:33234,y:32811,ptlb:Gradient_Or_Solid_Color,ptin:_Gradient_Or_Solid_Color,on:True|A-890-OUT,B-1656-RGB;n:type:ShaderForge.SFN_Color,id:1876,x:33627,y:32660,ptlb:Solid_Color,ptin:_Solid_Color,glob:False,c1:0.1764706,c2:0.5229208,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:2450,x:34025,y:32716|A-1771-OUT,B-2452-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2452,x:34252,y:32833,ptlb:Intensity,ptin:_Intensity,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:2476,x:33048,y:32811|A-1858-OUT,B-2477-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2477,x:33234,y:32955,ptlb:Brightness,ptin:_Brightness,glob:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2647,x:34046,y:33034,ptlb:Value_Clamp_Minimum,ptin:_Value_Clamp_Minimum,glob:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:2648,x:34046,y:33110,ptlb:Value_Clamp_Maximum,ptin:_Value_Clamp_Maximum,glob:False,v1:0.95;proporder:2477-2452-1858-1656-1876-1771-1022-1620-2647-2648;pass:END;sub:END;*/

Shader "ZFS Shaders/ZFS_LineOnly_Pro" {
    Properties {
        _Brightness ("Brightness", Float ) = 1
        _Intensity ("Intensity", Float ) = 1
        [MaterialToggle] _Gradient_Or_Solid_Color ("Gradient_Or_Solid_Color", Float ) = 1
        _Gradient_Color ("Gradient_Color", 2D) = "white" {}
        _Solid_Color ("Solid_Color", Color) = (0.1764706,0.5229208,1,1)
        [MaterialToggle] _Edge_Detection ("Edge_Detection", Float ) = 1
        _Edge_Detection_Distance ("Edge_Detection_Distance", Float ) = 3
        _Gradient_Edge_Detection ("Gradient_Edge_Detection", 2D) = "white" {}
        _Value_Clamp_Minimum ("Value_Clamp_Minimum", Float ) = 0
        _Value_Clamp_Maximum ("Value_Clamp_Maximum", Float ) = 0.95
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float _Edge_Detection_Distance;
            uniform sampler2D _Gradient_Edge_Detection; uniform float4 _Gradient_Edge_Detection_ST;
            uniform sampler2D _Gradient_Color; uniform float4 _Gradient_Color_ST;
            uniform fixed _Edge_Detection;
            uniform fixed _Gradient_Or_Solid_Color;
            uniform float4 _Solid_Color;
            uniform float _Intensity;
            uniform float _Brightness;
            uniform float _Value_Clamp_Minimum;
            uniform float _Value_Clamp_Maximum;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float2 node_1590 = float2((1.0 - saturate((sceneZ-partZ)/_Edge_Detection_Distance)),i.uv0.g);
                float node_1665 = clamp((lerp( 0.0, tex2D(_Gradient_Edge_Detection,TRANSFORM_TEX(node_1590, _Gradient_Edge_Detection)).r, _Edge_Detection )*_Intensity),_Value_Clamp_Minimum,_Value_Clamp_Maximum);
                float2 node_1654 = float2(node_1665,i.uv0.g);
                float3 emissive = (lerp( (_Solid_Color.rgb*node_1665), tex2D(_Gradient_Color,TRANSFORM_TEX(node_1654, _Gradient_Color)).rgb, _Gradient_Or_Solid_Color )*_Brightness);
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
