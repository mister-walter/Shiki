// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-1179-RGB;n:type:ShaderForge.SFN_Tex2d,id:1179,x:32427,y:32723,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_1179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:66c6f01d415b2e94887207684a08c12f,ntxv:0,isnm:False|UVIN-4773-OUT;n:type:ShaderForge.SFN_Append,id:7063,x:31447,y:32524,varname:node_7063,prsc:2|A-9665-OUT,B-3399-OUT;n:type:ShaderForge.SFN_Time,id:2963,x:31447,y:32672,varname:node_2963,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3097,x:31672,y:32524,varname:node_3097,prsc:2|A-7063-OUT,B-2963-T;n:type:ShaderForge.SFN_ScreenPos,id:7588,x:31447,y:32812,varname:node_7588,prsc:2,sctp:1;n:type:ShaderForge.SFN_Add,id:8092,x:31881,y:32608,varname:node_8092,prsc:2|A-3097-OUT,B-7588-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:7381,x:30682,y:33050,ptovrint:False,ptlb:Angle,ptin:_Angle,varname:node_7381,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Time,id:6834,x:31089,y:33206,varname:node_6834,prsc:2;n:type:ShaderForge.SFN_Multiply,id:810,x:31326,y:33117,varname:node_810,prsc:2|A-3649-OUT,B-6834-TSL;n:type:ShaderForge.SFN_Rotator,id:2914,x:31792,y:33045,varname:node_2914,prsc:2|UVIN-1285-UVOUT,PIV-203-OUT,ANG-5951-OUT,SPD-6544-OUT;n:type:ShaderForge.SFN_RemapRange,id:5951,x:31523,y:33117,varname:node_5951,prsc:2,frmn:0,frmx:1,tomn:0,tomx:6.28|IN-810-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:8651,x:30709,y:33201,ptovrint:False,ptlb:RotDir,ptin:_RotDir,varname:node_8651,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-3319-OUT,B-7541-OUT;n:type:ShaderForge.SFN_Vector1,id:3319,x:30503,y:33151,varname:node_3319,prsc:2,v1:-1;n:type:ShaderForge.SFN_Vector1,id:7541,x:30503,y:33235,varname:node_7541,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:3649,x:30949,y:33092,varname:node_3649,prsc:2|A-7381-OUT,B-8651-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6544,x:31297,y:33031,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_6544,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:100;n:type:ShaderForge.SFN_Vector2,id:203,x:31523,y:33347,varname:node_203,prsc:2,v1:0.5,v2:0.5;n:type:ShaderForge.SFN_Slider,id:9665,x:31050,y:32451,ptovrint:False,ptlb:Horizontal Scroll,ptin:_HorizontalScroll,varname:node_9665,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:3399,x:31050,y:32551,ptovrint:False,ptlb:Vertical Scroll,ptin:_VerticalScroll,varname:node_3399,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_ScreenPos,id:6089,x:31523,y:33455,varname:node_6089,prsc:2,sctp:0;n:type:ShaderForge.SFN_Add,id:4773,x:32070,y:32791,varname:node_4773,prsc:2|A-8092-OUT,B-2914-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1285,x:31083,y:32867,varname:node_1285,prsc:2,uv:0;proporder:1179-7381-8651-6544-9665-3399;pass:END;sub:END;*/

Shader "Plaid/PlaidBound" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Angle ("Angle", Float ) = 0.1
        [MaterialToggle] _RotDir ("RotDir", Float ) = -1
        _Speed ("Speed", Float ) = 100
        _HorizontalScroll ("Horizontal Scroll", Range(-1, 1)) = 0
        _VerticalScroll ("Vertical Scroll", Range(-1, 1)) = 0
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
            uniform float _Speed;
            uniform float _HorizontalScroll;
            uniform float _VerticalScroll;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float4 node_6834 = _Time + _TimeEditor;
                float node_2914_ang = (((_Angle*lerp( (-1.0), 1.0, _RotDir ))*node_6834.r)*6.28+0.0);
                float node_2914_spd = _Speed;
                float node_2914_cos = cos(node_2914_spd*node_2914_ang);
                float node_2914_sin = sin(node_2914_spd*node_2914_ang);
                float2 node_2914_piv = float2(0.5,0.5);
                float2 node_2914 = (mul(i.uv0-node_2914_piv,float2x2( node_2914_cos, -node_2914_sin, node_2914_sin, node_2914_cos))+node_2914_piv);
                float2 node_4773 = (((float2(_HorizontalScroll,_VerticalScroll)*node_2963.g)+float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).rg)+node_2914);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_4773, _Texture));
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
