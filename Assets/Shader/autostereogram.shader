Shader "Autosterogram/Noise"
{
    Properties
    {
        _MainTex ("Main texture", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _bwBlend ("Black & White blend", Range (0, 1)) = 0
        _pixels ( "Number of pixels", Range( 0, 256 ) ) = 223.
        _blend ( "anisotropic", Range( 0, 1 ) ) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members depth)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float _bwBlend;
            sampler2D _CameraDepthTexture;
            fixed _pixels ;
            fixed _blend ;
            

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
            //Proablebly from shadertoy 
            float3 hashOld33( float3 p )
            {
            	p = float3( dot(p,float3(127.1,311.7, 74.7)),
            			  dot(p,float3(269.5,183.3,246.1)),
            			  dot(p,float3(113.5,271.9,124.6)));
            	return frac(sin(p)*43758.5453123);
            }
            //Proablebly from shadertoy 
            float3 N23( float2 uv ) {         
              return (hashOld33( float3( floor( 79.*uv)/237., sin(_Time.y*0.003 ) ) ));
              //   Smoooth noise between fram
              //   +  hashOld33( float3( floor( 79.*uv)/237., sin( (_Time.y - unity_DeltaTime.x )*0.003 ) )))/2.   ;

            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float normalizeDepth( float depth ) {
              depth = Linear01Depth(depth) ;
              depth = depth   ;
              depth = 1. - depth ;
              return lerp( depth, depth*depth, 0.5)     ;
            }

            float2 tile( float2 uv ) {
                float2 uv_t, gv_t, uv_s, gv_s  ;
                uv_t = frac( uv * _MainTex_ST.xy +  _MainTex_ST.zw) ;
                gv_t = ( _pixels != 0 ) ?  2.*floor(  _pixels*uv_t +.5 ) / _pixels : uv_t ;
                uv_s =  1.- 2.*abs( uv_t  - 0.5 ) ; ;
                gv_s = ( _pixels != 0 ) ? 2.*floor( _pixels * uv_s / 2. + .5 ) / _pixels   : uv_s ;
                float border = sign( gv_s.x * gv_s.y) * (length( gv_s ) ) ;
                return lerp( gv_s , N23( gv_t ).xy * border + N23( gv_s ).xy * (1-border), _blend  )   ;


            }

            //From all all code of mine
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col ; 
                float width = 150. ;
              	float maxDisplacement = 75. ;
                float displacement = 0. ;
                float2 gv ;
                //_ScreenParams
                float2 uv_original = i.uv ;
                float2 uv_offset = uv_original  ;
                float iteration = _ScreenParams.x / width + 1 ;
                while( iteration > 0. ) {
                  float l = normalizeDepth ( tex2D(  _CameraDepthTexture, uv_offset  ).x ) ; //* _ZBufferParams.z ;
                  if( uv_offset.x > 0. ) {
                    displacement += l *maxDisplacement ;
                    uv_offset.x -= (width - maxDisplacement * l ) / _ScreenParams.x  ;
                  }
                  iteration -- ;
                }

                //Square Grid Coordinate
                gv.y = frac( round( .5*_ScreenParams.x / width) * uv_original.y );                
                gv.x = frac( ( uv_original.x * _ScreenParams.x ) / width  ) * width  ;
                col = 0 ; 

                  //décalage
                gv.x += displacement ;
                gv.x /= width ;
                    
                gv = frac( gv ) ;

                
                //col =  tex2D(_MainTex, uv_original );    
                //float3 noise = N23(gv) ;// ( 30. /137. ) ;

                //col.rgb *= noise.x ;           
                //col.rg = noise.y ;
                //col = 0 ; 
                col.rg = tile( gv ) ; 
                return col;
            }
            ENDCG
        }
    }
}
