// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-1179-RGB;n:type:ShaderForge.SFN_Tex2d,id:1179,x:32427,y:32723,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_1179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:66c6f01d415b2e94887207684a08c12f,ntxv:0,isnm:False|UVIN-2914-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:7381,x:31527,y:32659,ptovrint:False,ptlb:Angle,ptin:_Angle,varname:node_7381,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Time,id:6834,x:31527,y:32976,varname:node_6834,prsc:2;n:type:ShaderForge.SFN_Rotator,id:2914,x:32181,y:32723,varname:node_2914,prsc:2|UVIN-621-UVOUT,PIV-203-OUT,ANG-5951-OUT;n:type:ShaderForge.SFN_RemapRange,id:5951,x:31912,y:32769,varname:node_5951,prsc:2,frmn:0,frmx:1,tomn:0,tomx:6.28|IN-3649-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:8651,x:31527,y:32791,ptovrint:False,ptlb:RotDir,ptin:_RotDir,varname:node_8651,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-3319-OUT,B-7541-OUT;n:type:ShaderForge.SFN_Vector1,id:3319,x:31321,y:32741,varname:node_3319,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:7541,x:31321,y:32825,varname:node_7541,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:3649,x:31731,y:32769,varname:node_3649,prsc:2|A-7381-OUT,B-8651-OUT,C-6834-TSL;n:type:ShaderForge.SFN_Vector2,id:203,x:31912,y:32999,varname:node_203,prsc:2,v1:0.5,v2:0.5;n:type:ShaderForge.SFN_ScreenPos,id:621,x:31908,y:32456,varname:node_621,prsc:2,sctp:1;proporder:1179-7381-8651;pass:END;sub:END;*/

Shader "Plaid/PlaidRotate" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Angle ("Angle", Float ) = 0.2
        [MaterialToggle] _RotDir ("RotDir", Float ) = -1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _Angle;
            uniform fixed _RotDir;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
////// Lighting:
////// Emissive:
                float4 node_6834 = _Time + _TimeEditor;
                float node_2914_ang = ((_Angle*lerp( (-1.0), 1.0, _RotDir )*node_6834.r)*6.28+0.0);
                float node_2914_spd = 1.0;
                float node_2914_cos = cos(node_2914_spd*node_2914_ang);
                float node_2914_sin = sin(node_2914_spd*node_2914_ang);
                float2 node_2914_piv = float2(0.5,0.5);
                float2 node_2914 = (mul(float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg-node_2914_piv,float2x2( node_2914_cos, -node_2914_sin, node_2914_sin, node_2914_cos))+node_2914_piv);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_2914, _Texture));
                float3 emissive = _Texture_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
