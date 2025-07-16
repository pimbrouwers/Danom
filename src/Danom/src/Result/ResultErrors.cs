namespace Danom
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a collection of <see cref="ResultError"/> instances.
    /// </summary>
    public sealed class ResultErrors : IEnumerable<ResultError>
    {
        private readonly Dictionary<string, List<string>> _resultErrors =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the error collection for the specified key.
        /// If the key does not exist, a <see cref="KeyNotFoundException"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public ResultError this[string key] =>
            _resultErrors.TryGetValue(key, out var errors)
                ? new ResultError(key, errors.ToArray())
                : throw new KeyNotFoundException($"No errors found for key '{key}'.");

        /// <summary>
        /// Creates a new empty instance of <see cref="ResultErrors"/>.
        /// </summary>
        public ResultErrors() { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public ResultErrors(string key, params string[] errors) =>
            Add(key, errors);

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// key and error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public ResultErrors(string key, string error)
            : this(key, new[] { error }) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// error.
        /// </summary>
        /// <param name="error"></param>
        public ResultErrors(string error)
            : this(string.Empty, error) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// error strings.
        /// </summary>
        /// <param name="errors"></param>
        public ResultErrors(params string[] errors)
            : this(string.Empty, errors) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// collection of <see cref="ResultError"/> instances.
        /// </summary>
        /// <param name="errors"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ResultErrors(IEnumerable<ResultError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors), "Errors cannot be null.");
            }

            foreach (var error in errors)
            {
                Add(error.Key, error.Errors);
            }
        }

        /// <summary>
        /// Adds a new error to the collection with the specified key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public void Add(string key, params string[] errors)
        {
            if (key == null) // we allow empty key (i.e.. "")
            {
                throw new ArgumentException("Key cannot be null.", nameof(key));
            }

            if (errors == null)
            {
                throw new ArgumentException("Errors cannot be null.", nameof(errors));
            }

            if (!_resultErrors.TryGetValue(key, out var existingErrors))
            {
                _resultErrors.Add(key, errors.ToList());
            }
            else
            {
                foreach (var error in errors)
                {
                    existingErrors.Add(error);
                }
            }
        }

        /// <summary>
        /// Adds a new error to the collection with the specified key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public void Add(string key, IEnumerable<string> errors) =>
            Add(key, errors.ToArray());

        /// <summary>
        /// Adds a new error to the collection with the specified key and error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public void Add(string key, string error) =>
            Add(key, new[] { error });

        /// <summary>
        /// Adds a new error to the collection with the specified key.
        /// </summary>
        /// <param name="error"></param>
        public void Add(string error) =>
            Add(string.Empty, error);

        /// <summary>
        /// Adds a range of errors to the collection.
        /// </summary>
        /// <param name="errors"></param>
        public void Add(params string[] errors) =>
            Add(string.Empty, errors);

        /// <summary>
        /// Returns the enumerator for the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ResultError> GetEnumerator() =>
            _resultErrors.Select(x => new ResultError(x.Key, x.Value.ToArray())).GetEnumerator();

        /// <summary>
        /// Returns the enumerator for the collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Returns a string representation of the <see cref="ResultErrors"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_resultErrors.Count == 0)
            {
                return "[]";
            }

            var formattedErrors = _resultErrors.Select(x =>
                string.IsNullOrWhiteSpace(x.Key)
                    ? string.Join(", ", x.Value)
                    : string.Concat(x.Key, " - ", string.Join(", ", x.Value))
            );

            return string.Concat("[ ", string.Join("; ", formattedErrors), " ]");
        }

    }

    /// <summary>
    /// Represents a collection of optionally keyed errors.
    /// </summary>
    public sealed class ResultError
    {
        /// <summary>
        /// Creates a new instance of <see cref="ResultError"/> from the
        /// specified key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public ResultError(string key, params string[] errors)
        {
            if (key == null) // we allow empty key (i.e.. "")
            {
                throw new ArgumentException("Key cannot be null.", nameof(key));
            }

            Key = key;
            Errors = errors;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ResultError"/> from the specified
        /// key and error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public ResultError(string key, string error)
            : this(key, new[] { error }) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultError"/> from the specified
        /// errors.
        /// </summary>
        /// <param name="errors"></param>
        public ResultError(params string[] errors)
            : this(string.Empty, errors) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultError"/> from the specified
        /// error.
        /// </summary>
        /// <param name="error"></param>
        public ResultError(string error)
            : this(new[] { error }) { }

        /// <summary>
        /// Gets the key associated with the error, if any.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the list of errors associated with the result.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Returns a string representation of the <see cref="ResultError"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var errors = string.Join(", ", Errors);
            return string.IsNullOrWhiteSpace(Key) ?
                errors :
                string.Concat(Key, " - ", errors);
        }
    }
}
