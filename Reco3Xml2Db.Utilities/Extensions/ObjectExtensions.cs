using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Utilities.Extensions {
  public static class ObjectExtensions {
    /// <summary>
    /// Gets the specified field and initialized it if needed.
    /// </summary>
    /// <typeparam name="TField">Field type.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="field">The field.</param>
    /// <param name="initializer">The initializer.</param>
    /// <returns>Field value.</returns>
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Generic object functionality.")]
    public static TField Get<TField>(this object value, ref TField field, Func<TField> initializer) {
      if (field.IsNull()) {
        field = initializer();
      }

      return field;
    }

    /// <summary>
    /// Indicates that the specified reference is not a null reference
    /// </summary>
    /// <typeparam name="T">Current Type.</typeparam>
    /// <param name="value">Reference to be tested</param>
    /// <returns>
    /// True, if specified value is not a null reference; Otherwise False.
    /// </returns>
    public static Boolean IsNotNull<T>(this T value) {
      return !ReferenceEquals(value, null);
    }

    /// <summary>
    /// Execute a action if T Not null.
    /// </summary>
    /// <typeparam name="T">Current Type.</typeparam>
    /// <param name="value">Reference to be tested</param>
    /// <param name="action">Action to execute if value Not null.</param>
    public static void IfNotNull<T>(this T value, Action action) {
      if (IsNotNull(value)) {
        action();
      }
    }

    /// <summary>
    /// Execute a action if T Not null.
    /// </summary>
    /// <typeparam name="T">Current Type.</typeparam>
    /// <param name="value">Reference to be tested</param>
    /// <param name="action">Action to execute if value Not null.</param>
    public static void IfNotNull<T>(this T value, Action<T> action) {
      if (IsNotNull(value)) {
        action(value);
      }
    }

    /// <summary>
    /// Execute a func if T Not null.
    /// </summary>
    /// <typeparam name="T">Current Type.</typeparam>
    /// <typeparam name="TOut">Type for out parameter.</typeparam>
    /// <param name="value">Reference to be tested</param>
    /// <param name="fn">Func to execute if value Not null.</param>
    /// <returns>If value not null, return the result of the Func; Otherwise return Default(TOut).</returns>
    public static TOut IfNotNull<T, TOut>(this T value, Func<T, TOut> fn) {
      return !EqualityComparer<T>.Default.Equals(value, default) ? fn(value) : default;
    }

    /// <summary>
    /// Indicates that the specified reference is a null reference
    /// </summary>
    /// <typeparam name="T">Current Type.</typeparam>
    /// <param name="value">Reference to be tested</param>
    /// <returns>True, if specified value is a null reference; Otherwise False.</returns>
    public static bool IsNull<T>(this T value) {
      return ReferenceEquals(value, null);
    }
  }
}
