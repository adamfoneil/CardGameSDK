namespace BlazorApp.Extensions;

public static class Dropdown
{
	public static DropdownValue<TEnum>[] ForEnum<TEnum>()
		where TEnum : Enum =>
		Enum.GetValues(typeof(TEnum))
			.Cast<TEnum>()
			.Select(value => new DropdownValue<TEnum>(value, value.ToString()))
			.ToArray();
}

public record DropdownValue<TValue>(TValue Value, string Text);