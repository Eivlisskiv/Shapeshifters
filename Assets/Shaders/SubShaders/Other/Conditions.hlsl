void IfAdd_float(bool Condition, float Value, float Addition, out float Out)
{
	Out = Value;
	if (Condition)
	{
		Out = Value + Addition;
	}
}