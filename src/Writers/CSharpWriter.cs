﻿using Devbeat.DTE.JsonToCSharp.Represesentation;

namespace Devbeat.DTE.JsonToCSharp.Writers;

internal static class CSharpWriter
{
    private static string NEWLINE => Environment.NewLine;

    internal static string Write(
        CSharpCode codeRepresentation
        , CSharpWriteOptions? options = null)
    {
        var sb = new StringBuilder();
        using var writer = new StringWriter(sb);

        options ??= CSharpWriteOptions.Default;

        WriteNamespace(writer, codeRepresentation.Namespace);

        foreach (var classObject in codeRepresentation.Classes)
        {
            WriteBeginClass(writer, classObject.Name);

            foreach (var propertyObject in classObject.Properties)
            {
                WriteProperty(
                    writer
                    , propertyObject.Type
                    , propertyObject.Name
                    , propertyObject.IsCollection
                    , propertyObject.IsNullable);
            }

            WriteEndClass(writer);
        }

        return sb.ToString();
    }

    private static void WriteProperty(
        StringWriter writer, string type, string name, bool isCollection, bool isNullable)
    {
        writer.WriteLine($"\tpublic {type}{(isNullable ? "?" : "")}{(isCollection ? "[]" : "")} {name} {{ get; set; }}");
    }

    private static void WriteBeginClass(StringWriter writer, string name)
    {
        writer.WriteLine($"{NEWLINE}public class {name}{NEWLINE}{{");
    }

    private static void WriteEndClass(StringWriter writer)
    {
        writer.WriteLine("}");
    }

    private static void WriteNamespace(StringWriter writer, string name)
    {
        writer.WriteLine($"// auto-generated by Devbeat JsonToCSharp for DevToys{NEWLINE}{NEWLINE}namespace {name};");
    }
}
