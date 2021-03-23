void BloomNearColor_float(float4 input, float4 target, 
	float sensitivity, float4 bloom, out float4 output) 
{
	bool doBloom = true;
	output = input;

	for (int i = 0; doBloom && i < 4; i++) {
		//Get the difference in values
		float v = input[i] - target[i];
		//Absolute that value for a positive version
		if (v < 0) v = v * -1;
		//If the difference is higher than sensitivity
		if (v > sensitivity)
		{
			//No bloom
			doBloom = false;
		}
	}

	if (doBloom) {
		output = input + bloom;
	}
}