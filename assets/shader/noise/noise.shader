shader_type spatial;

uniform sampler2D noise;
uniform float speed: hint_range(0, 100) = 0.5f;
uniform float intensity : hint_range(0, 1) = 0.5f;
uniform float layers : hint_range(1, 50) = 5;

float gradient(vec2 uv){
    return (0.5f - distance(vec2(uv.x, uv.y), vec2(0.5, 0.5)));
   }

vec3 colorize(vec3 albedo){
    vec3 palete = vec3(albedo.r, albedo.r, albedo.r);
    return palete;
   }

void fragment(){
    vec4 bg = texture(noise, UV - TIME * speed);
    
    ALBEDO = bg.rgb;
    ALBEDO.r = gradient(UV);
    ALBEDO.r = clamp(ALBEDO.r * bg.r * intensity * 10f, 0f, 1f);
    ALBEDO.r = floor(ALBEDO.r * layers) / layers;
    ALBEDO = colorize(ALBEDO);
}