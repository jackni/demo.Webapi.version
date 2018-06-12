using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Example.Common.CommandValidation
{
	public static class CommandValidator
	{
		public static EntityValidationResult GetValidationResult<T>(object command) where T : class, new()
		{
			var validationResults = new List<ValidationResult>();
			var commandType = command.GetType();
			var commandPropes = commandType.GetProperties();
			var target = new T();
			var targetProps = target.GetType().GetProperties();

			foreach (var commandProp in commandPropes)
			{
				var targetProp = targetProps.FirstOrDefault(tp => tp.Name == commandProp.Name);
				if (targetProp != null)
				{
					var value = commandProp.GetValue(command);
					targetProp.SetValue(target, value);
				}
			}
			var vc = new ValidationContext(target);
			Validator.TryValidateObject(target, vc, validationResults, true);
			return new EntityValidationResult(validationResults);
		}
	}
}
