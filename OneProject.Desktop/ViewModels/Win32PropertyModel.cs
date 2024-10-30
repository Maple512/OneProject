namespace OneProject.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

public class Win32PropertyModel
{
    public Win32PropertyModel(PropertyData data)
    {
        IsArray = data.IsArray;
        IsLocal = data.IsLocal;
        Name = data.Name;
        Type = GetTypeString(data.Type);
        Value = data.Value;
    }
    public bool IsArray { get; }
    public bool IsLocal { get; }
    public string? Name { get; }
    public string? Type { get; }
    public object? Value { get; }

    static string GetTypeString(CimType type) => type switch
    {
        CimType.None => nameof(CimType.None),
        CimType.SInt16 => nameof(CimType.SInt16),
        CimType.SInt32 => nameof(CimType.SInt32),
        CimType.Real32 => nameof(CimType.Real32),
        CimType.Real64 => nameof(CimType.Real64),
        CimType.String => nameof(CimType.String),
        CimType.Boolean => nameof(CimType.Boolean),
        CimType.Object => nameof(CimType.Object),
        CimType.SInt8 => nameof(CimType.SInt8),
        CimType.UInt8 => nameof(CimType.UInt8),
        CimType.UInt16 => nameof(CimType.UInt16),
        CimType.UInt32 => nameof(CimType.UInt32),
        CimType.SInt64 => nameof(CimType.SInt64),
        CimType.UInt64 => nameof(CimType.UInt64),
        CimType.DateTime => nameof(CimType.DateTime),
        CimType.Reference => nameof(CimType.Reference),
        CimType.Char16 => nameof(CimType.Char16),
        _ => nameof(CimType.None),
    };
}
