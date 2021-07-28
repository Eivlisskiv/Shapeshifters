void CreateBeam_float(float4 UV, float Width, float Power, out float Out) 
{
	float green = UV[1];

	Width = abs(Width);
	Power = abs(Power);

	float up = pow(1 - green, Width);

	float down = pow(green, Width);

	float beam = 1 - (up + down);

	Out = pow(beam, Width / Power);
}

void AngleOffset_float(float2 Speed, float Time,
	float Mult, out float Tilt)
{
	Tilt = (Speed[0] + Speed[1]) * (Mult * Time);
}