Shader "Autosterogram/Noise"
{
    Properties
    {
        [HideInInspector] 
        _MainTex ("Main texture", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _bwBlend ("Black & White blend", Range (0, 1)) = 0
        _pixels ( "Number of pixels", Range( 0, 256 ) ) = 123.
        _blend ( "anisotropic", Range( 0, 1 ) ) = 1
        _Color ( "color", Color ) = (1,1,0,1)
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
            sampler2D _Noise;
            fixed _pixels ;
            fixed _blend ;
            float4 _Color ; 
            

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
              return (hashOld33( float3( floor( _pixels*uv)/237., sin(_Time.y*0.003 ) ) )
                +  hashOld33( float3( floor( _pixels*uv)/237., sin( (_Time.y - unity_DeltaTime.x )*0.003 ) )))/2.   ;

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
              return lerp( depth, depth*depth, _bwBlend)     ;
            }

            //From all all code of mine
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col ; 
                float width = 150. ;
              	float maxDisplacement = 75. ;                

                //Screen Uv coordinates 
                float2 uv_original = i.uv ;
                float2 uv_offset = uv_original  ;
                //Square Grid Coordinate
                float2 gv ;                
                gv.y = frac( round( .5*_ScreenParams.x / width) * uv_original.y );                
                gv.x = frac( ( uv_original.x * _ScreenParams.x ) / width  ) * width  ;

                //Cumulative Displacement Algorithm
                float displacement = 0. ;
                float iteration = _ScreenParams.x / width + 1 ;
                while( iteration > 0. ) {
                  float l = normalizeDepth ( tex2D(  _CameraDepthTexture, uv_offset  ).x ) ; 
                  if( uv_offset.x > 0. ) {
                    displacement += l *maxDisplacement ;
                    uv_offset.x -= (width - maxDisplacement * l ) / _ScreenParams.x  ;
                  }
                  iteration -- ;
                }

                
                //décalage
                gv.x += displacement ;
                gv.x /= width ;
                // Map to texture coordinate                    
                gv = frac( gv ) ;

                //Sample 2 point in the noise texture for color shifting 
                float2 coord1 =  float2(sin( _Time.x*.01453+ .12987612),sin( _Time.x*.07852) ) ;
                float2 coord2 =  float2(sin( _Time.x*.02353),cos( _Time.x*.05123 + .12987612) ) ;
                float2 noise =  floor(N23( gv ).xy + 0.5);
                noise = noise.xy * ( 1 - noise.yx ) ;
                //Bicolor + warning color for the black pixels 
                col.rgb = noise.x * tex2D(  _Noise, coord1 ) + noise.y * tex2D(  _Noise, coord2 ) + (1- noise.x - noise.y)  * _Color; 
                //Debug or easy mode
                col.rgb = lerp( col.rgb , normalizeDepth(tex2D(  _CameraDepthTexture, uv_original ).x), _blend )  ; 
                return col;
            }
            ENDCG
        }
    }
}
