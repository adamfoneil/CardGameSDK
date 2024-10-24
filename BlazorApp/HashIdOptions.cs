namespace BlazorApp;

public class HashIdOptions
{
	public string Salt { get; set; } = default!;
	public int MinLength { get; set; }
}
