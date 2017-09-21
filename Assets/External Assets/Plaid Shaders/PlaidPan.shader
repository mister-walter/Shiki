// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-1179-RGB;n:type:ShaderForge.SFN_Tex2d,id:1179,x:32427,y:32723,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_1179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:66c6f01d415b2e94887207684a08c12f,ntxv:0,isnm:False|UVIN-8092-OUT;n:type:ShaderForge.SFN_Append,id:7063,x:31816,y:32660,varname:node_7063,prsc:2|A-9665-OUT,B-3399-OUT;n:type:ShaderForge.SFN_Time,id:2963,x:31816,y:32808,varname:node_2963,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3097,x:31978,y:32723,varname:node_3097,prsc:2|A-7063-OUT,B-2963-T;n:type:ShaderForge.SFN_ScreenPos,id:7588,x:31978,y:32884,varname:node_7588,prsc:2,sctp:1;n:type:ShaderForge.SFN_Add,id:8092,x:32216,y:32723,varname:node_8092,prsc:2|A-3097-OUT,B-7588-UVOUT;n:type:ShaderForge.SFN_Slider,id:9665,x:31419,y:32587,ptovrint:False,ptlb:Horizontal Scroll,ptin:_HorizontalScroll,varname:node_9665,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.0607545,max:1;n:type:ShaderForge.SFN_Slider,id:3399,x:31419,y:32687,ptovrint:False,ptlb:Vertical Scroll,ptin:_VerticalScroll,varname:node_3399,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.04050335,max:1;proporder:1179-9665-3399;pass:END;sub:END;*/

Shader "Plaid/PlaidPan" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _HorizontalScroll ("Horizontal Scroll", Range(-1, 1)) = 0.0607545
        _VerticalScroll ("Vertical Scroll", Range(-1, 1)) = 0.04050335
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
            uniform float _HorizontalScroll;
            uniform float _VerticalScroll;
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
                float4 node_2963 = _Time + _TimeEditor;
                float2 node_8092 = ((float2(_HorizontalScroll,_VerticalScroll)*node_2963.g)+float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_8092, _Texture));
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
