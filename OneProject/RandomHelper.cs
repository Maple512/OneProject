namespace OneProject;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

[StackTraceHidden]
[DebuggerStepThrough]
public static class RandomHelper
{
    public static string GetString(int length) => string.Create(length, length, RandomString);

    private static void RandomString(Span<char> buffer, int count)
    {
        for(var i = 0; i < count; i++)
        {
            var type = RandomNumberGenerator.GetInt32(3);

            if(type == 0)
            {
                buffer[i] = (char)RandomNumberGenerator.GetInt32(48, 57);
            }
            else if(type == 1)
            {
                buffer[i] = (char)RandomNumberGenerator.GetInt32(65, 90);
            }
            else if(type == 2)
            {
                buffer[i] = (char)RandomNumberGenerator.GetInt32(97, 122);
            }
        }
    }

    public static int GetRandom(int minValue, int maxValue) => RandomNumberGenerator.GetInt32(minValue, maxValue);

    public static int GetRandom(int maxValue) => RandomNumberGenerator.GetInt32(maxValue);

    public static bool GetBool() => RandomNumberGenerator.GetInt32(2) == 0;

    public static T GetRandomOf<T>(params T[] source)
    {
        _ = Check.NotNull(source);

        return source.Length is 1 ? source[0] : source[GetRandom(0, source.Length)];
    }

    // public static T GetRandomOfList<T>(IEnumerable<T> source)
    // {
    //     _ = Check.NotNullOrEmpty(source);
    //
    //     return source.ElementAt(GetRandom(0, source.Count()));
    // }
    //
    // public static List<T> GenerateRandomizedList<T>(IEnumerable<T> source)
    // {
    //     _ = Check.NotNull(source);
    //
    //     var currentList = new List<T>(source);
    //     var randomList = new List<T>(currentList.Count);
    //
    //     while(currentList.Count != 0)
    //     {
    //         var randomIndex = GetRandom(0, currentList.Count);
    //
    //         randomList.Add(currentList[randomIndex]);
    //
    //         currentList.RemoveAt(randomIndex);
    //     }
    //
    //     return randomList;
    // }
}
