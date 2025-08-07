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
        private readonly Dictionary<string, ResultError> _resultErrors =
            new Dictionary<string, ResultError>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the error collection for the specified key.
        /// If the key does not exist, a <see cref="KeyNotFoundException"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public ResultError this[string key] =>
            _resultErrors.TryGetValue(key, out var resultError)
                ? resultError
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
        public ResultErrors(string key, IEnumerable<string> errors) =>
            Add(key, errors);

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// key and error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public ResultErrors(string key, string error)
            : this(key, new List<string>() { error }) { }

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
        public ResultErrors(IEnumerable<string> errors)
            : this(string.Empty, errors) { }

        /// <summary>
        /// Adds a new error to the collection with the specified key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public void Add(string key, IEnumerable<string> errors) =>
            Add(new ResultError(key, errors));

        /// <summary>
        /// Adds a new error to the collection with the specified key and error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public void Add(string key, string error) =>
            Add(new ResultError(key, new List<string>() { error }));

        /// <summary>
        /// Adds a new error to the collection with the specified key.
        /// </summary>
        /// <param name="error"></param>
        public void Add(string error) =>
            Add(new ResultError(string.Empty, error));

        /// <summary>
        /// Adds a range of errors to the collection.
        /// </summary>
        /// <param name="errors"></param>
        public void Add(IEnumerable<string> errors) =>
            Add(new ResultError(string.Empty, errors));

        /// <summary>
        /// Adds a new error to the collection with the specified key and errors.
        /// </summary>
        /// <param name="error"></param>
        public void Add(ResultError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error), "Error cannot be null.");
            }

            if (_resultErrors.TryGetValue(error.Key, out var existingErrors))
            {
                existingErrors.Add(error);
            }
            else
            {
                _resultErrors.Add(error.Key, error);

            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// collection of <see cref="ResultError"/> instances.
        /// </summary>
        /// <param name="errors"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(ResultErrors errors)
        {
            foreach (var error in errors)
            {
                Add(error);
            }
        }

        /// <summary>
        /// Returns the enumerator for the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ResultError> GetEnumerator() =>
            _resultErrors.Select(x => x.Value).GetEnumerator();

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

            return string.Concat(
                "[ ",
                string.Join("; ", _resultErrors.Select(x => x.Value)),
                " ]"
            );
        }

    }

    /// <summary>
    /// Represents a collection of optionally keyed errors.
    /// </summary>
    public sealed class ResultError
    {
        private readonly List<string> _errors;

        /// <summary>
        /// Creates a new instance of <see cref="ResultError"/> from the
        /// specified key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public ResultError(string key, IEnumerable<string> errors)
        {
            if (key == null) // we allow empty key (i.e.. "")
            {
                throw new ArgumentException("Key cannot be null.", nameof(key));
            }

            Key = key;
            _errors = new List<string>(errors);
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
        public ResultError(IEnumerable<string> errors)
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
        public IReadOnlyList<string> Errors => _errors;

        /// <summary>
        /// Adds a new error to the collection with the specified key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public void Add(string key, IEnumerable<string> errors) =>
            Add(new ResultError(key, errors));

        /// <summary>
        /// Adds a new error to the collection with the specified key and error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public void Add(string key, string error) =>
            Add(new ResultError(key, error));

        /// <summary>
        /// Adds a new error to the collection with the specified error using
        /// the empty key.
        /// </summary>
        /// <param name="error"></param>
        public void Add(string error) =>
            Add(new ResultError(error));

        /// <summary>
        /// Adds a range of errors to the collection using the empty key.
        /// </summary>
        /// <param name="errors"></param>
        public void Add(IEnumerable<string> errors) =>
            Add(new ResultError(errors));

        /// <summary>
        /// Appends another <see cref="ResultError"/> to this instance.
        /// </summary>
        /// <param name="next"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Add(ResultError next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next), "Next error cannot be null.");
            }

            if (!string.Equals(next.Key, Key, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Cannot append errors with different keys.");
            }

            _errors.AddRange(next.Errors);
        }

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
