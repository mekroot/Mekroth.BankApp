namespace Mekroth.BankApp.Core.Models
{
	public sealed class Result<T>(IEnumerable<string> errors, T value)
	{
		public bool Success { get; } = errors is null || !errors.Any();
		public IEnumerable<string> Errors { get; } = errors;
		public T Value { get; } = value;
	}
}
