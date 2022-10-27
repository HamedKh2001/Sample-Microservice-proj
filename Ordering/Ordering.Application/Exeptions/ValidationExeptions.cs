using FluentValidation.Results;

namespace Ordering.Application.Excpetions
{
	public class ValidationExeptions : ApplicationException
	{
		public ValidationExeptions()
			: base("One or more validation failures have occured")
		{
			Errors = new Dictionary<string, string[]>();
		}
		public ValidationExeptions(IEnumerable<ValidationFailure> failures) : this()
		{
			Errors = failures
				.GroupBy(f => f.PropertyName, f => f.ErrorMessage)
				.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
		}

		public Dictionary<string, string[]> Errors { get; }
	}
}
