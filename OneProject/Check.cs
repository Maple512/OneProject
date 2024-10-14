namespace OneProject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

[StackTraceHidden]
[DebuggerStepThrough]
public static class Check
{
    public static void ThrowIfNotBetween<T>(
        T value,
        T min,
        T max,
        [CallerArgumentExpression(nameof(value))]
        string? paramName = null)
        where T : IComparable<T>
    {
        if(value.CompareTo(max) > 0 || value.CompareTo(min) < 0)
        {
            ThrowNotBetween(value, min, max, paramName);
        }
    }

    [DoesNotReturn]
    private static void ThrowNotBetween<T>(T value, T min, T max, string? paramName = null)
        => throw new ArgumentOutOfRangeException(paramName, $"{paramName}'{value}' must be between {min} and {max}.");

    [return: NotNull]
    public static string NotNullOrEmpty(
        string? value,
        [CallerArgumentExpression(nameof(value))]
        string? argumentExpression = null)
        => string.IsNullOrEmpty(value) ? throw new ArgumentNullException(argumentExpression) : value;

    [return: NotNull]
    public static string NotNullOrWhiteSpace(
        string? value,
        [CallerArgumentExpression(nameof(value))]
        string? argumentExpression = null)
        => string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException(argumentExpression) : value;

    [return: NotNull]
    public static T NotNull<T>(
        T? value,
        [CallerArgumentExpression(nameof(value))]
        string? argumentExpression = null)
        => value == null ? throw new ArgumentNullException(argumentExpression) : value;
}
