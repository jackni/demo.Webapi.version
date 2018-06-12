using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Example.Common.CommandValidation
{
	public class EntityValidationResult
	{
		public IList<ValidationResult> Errors { get; private set; }

		public bool IsValid => Errors.Count < 1;

		public EntityValidationResult(IList<ValidationResult> errors = null)
		{
			Errors = errors ?? new List<ValidationResult>();
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			foreach (var line in Errors)
				sb.AppendLine(line.ToString());

			return sb.ToString();
		}
	}
}
