using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace System.Waf.Foundation
{
    /// <summary>Provides helper methods for validating objects. This class extends the <see cref="Validator"/> class.</summary>
    public static class ValidationHelper
    {
        private static readonly ConcurrentDictionary<Type, ValidationTypeCache> cache = new ConcurrentDictionary<Type, ValidationTypeCache>();

        /// <summary>Validate the specified object. It considers attached <see cref="ValidationAttribute"/>(s) and the <see cref="IValidatableObject"/> interface.</summary>
        /// <param name="instance">The object to validate.</param>
        /// <returns>All validation errors.</returns>
        /// <remarks>This method is similar to <see cref="Validator.TryValidateObject(object, ValidationContext, ICollection{ValidationResult}, bool)"/> but it validates 
        /// all rules and does not abort when some errors are found. See also: https://github.com/dotnet/runtime/issues/31882 </remarks>
        public static IEnumerable<ValidationResult> Validate(object instance)
        {
            if (instance is null) throw new ArgumentNullException(nameof(instance));
            var instanceType = instance.GetType();
            var info = cache.GetOrAdd(instanceType, CreateTypeCache);
            return ValidateCore(instance, info);
        }

        private static IEnumerable<ValidationResult> ValidateCore(object instance, ValidationTypeCache info)
        {
            foreach (var property in info.ValidationProperties)
            {
                var propertyContext = new ValidationContext(instance) { MemberName = property.Property.Name };
                var propertyValue = property.Property.GetValue(instance, null);
                foreach (var error in GetErrors(propertyValue, propertyContext, property.ValidationAttributes)) yield return error;
            }

            var context = new ValidationContext(instance);
            foreach (var error in GetErrors(instance, context, info.ValidationAttributes)) yield return error;

            if (instance is IValidatableObject validatable)
            {
                foreach (var result in validatable.Validate(context) ?? Array.Empty<ValidationResult>())
                {
                    if (result != ValidationResult.Success) yield return result;
                }
            }
        }

        private static IEnumerable<ValidationResult> GetErrors(object value, ValidationContext context, IReadOnlyList<ValidationAttribute> attributes)
        {
            var required = attributes.OfType<RequiredAttribute>().FirstOrDefault();
            if (required != null && TryGetError(value, context, required, out var result)) yield return result;
            foreach (var attribute in attributes)
            {
                if (attribute == required) continue;
                if (TryGetError(value, context, attribute, out result)) yield return result;
            }
        }

        private static bool TryGetError(object value, ValidationContext context, ValidationAttribute attribute, [NotNullWhen(true)] out ValidationResult? result)
        {
            var error = attribute.GetValidationResult(value, context);
            result = error == ValidationResult.Success ? null : error;
            return !(result is null);
        }

        private static ValidationTypeCache CreateTypeCache(Type instanceType)
        {
            var attributes = instanceType.GetCustomAttributes(true).OfType<ValidationAttribute>().ToArray();
            var propertyInfos = instanceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => !x.GetIndexParameters().Any());
            var properties = propertyInfos.Select(x => new ValidationPropertyCache(x, x.GetCustomAttributes(true).OfType<ValidationAttribute>().ToArray()))
                .Where(x => x.ValidationAttributes.Any()).ToArray();
            return new ValidationTypeCache(attributes, properties);
        }

        private sealed class ValidationTypeCache
        {
            public ValidationTypeCache(IReadOnlyList<ValidationAttribute> validationAttributes, IReadOnlyList<ValidationPropertyCache> validationProperties)
            {
                ValidationAttributes = validationAttributes;
                ValidationProperties = validationProperties;
            }

            public IReadOnlyList<ValidationAttribute> ValidationAttributes { get; }

            public IReadOnlyList<ValidationPropertyCache> ValidationProperties { get; }
        }

        private sealed class ValidationPropertyCache
        {
            public ValidationPropertyCache(PropertyInfo property, IReadOnlyList<ValidationAttribute> validationAttributes)
            {
                Property = property;
                ValidationAttributes = validationAttributes;
            }

            public PropertyInfo Property { get; }

            public IReadOnlyList<ValidationAttribute> ValidationAttributes { get; }
        }
    }
}
