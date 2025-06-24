namespace Danom
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a collection of <see cref="ResultError"/> instances.
    /// </summary>
    public sealed class ResultErrors : IEnumerable<ResultError>
    {
        private readonly List<ResultError> _errors = new List<ResultError>();

        /// <summary>
        /// Creates a new empty instance of <see cref="ResultErrors"/>.
        /// </summary>
        public ResultErrors() { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// errors.
        /// </summary>
        /// <param name="errors"></param>/
        public ResultErrors(ResultErrors errors)
        {
            if (errors != null)
            {
                _errors.AddRange(errors);
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the
        /// specified error.
        /// </summary>
        /// <param name="error"></param>
        public ResultErrors(ResultError error)
            : this()
        {
            if (error != null)
            {
                _errors.Add(error);
            }
        }


        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// errors.
        /// </summary>
        /// <param name="errors"></param>
        public ResultErrors(IEnumerable<ResultError> errors)
        {
            if (errors != null)
            {
                _errors.AddRange(errors);
            }
        }


        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// error.
        /// </summary>
        /// <param name="error"></param>
        public ResultErrors(string error)
            : this(new ResultError(error)) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// key and error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        public ResultErrors(string key, string error)
            : this(new ResultError(key, error)) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// key and errors.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        public ResultErrors(string key, params string[] errors)
            : this(new ResultError(key, errors)) { }

        /// <summary>
        /// Creates a new instance of <see cref="ResultErrors"/> from the specified
        /// error strings.
        /// </summary>
        /// <param name="errors"></param>
        public ResultErrors(params string[] errors)
            : this(errors.Select(m => new ResultError(m))) { }

        /// <summary>
        /// Adds a new error to the collection.
        /// </summary>
        /// <param name="error"></param>
        public void Add(ResultError error) =>
            _errors.Add(error);

        /// <summary>
        /// Returns the enumerator for the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ResultError> GetEnumerator() =>
            _errors.GetEnumerator();

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
            var errors = string.Join(", ", _errors);
            return string.Concat("[ ", errors, " ]");
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
