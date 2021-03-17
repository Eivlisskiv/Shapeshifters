void AddBruise_float(float4 Color, float3 BruiseColor, float Bruise, out float4 Out)
{
	Out = Color;
	if (Bruise > 0)
	{
		BruiseColor = BruiseColor * Bruise;
		float4 Fade = Color * (1 - Bruise);
		Out = float4(
			(Fade.x + BruiseColor.x),
			(Fade.y + BruiseColor.y),
			(Fade.z + BruiseColor.z),
			Color[3]);
	}
}