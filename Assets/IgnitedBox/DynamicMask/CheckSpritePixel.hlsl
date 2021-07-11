void InsideMask_float4(float4 mask, float4 sprite) {
	if (mask[3] = < 0) sprite[3] = 0;
}

void OutsideMask_float4(float4 mask, float4 sprite) {
	if (mask[3] > 0) sprite[3] = 0;
}
