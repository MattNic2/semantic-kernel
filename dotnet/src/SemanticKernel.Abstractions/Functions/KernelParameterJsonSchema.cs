﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel.Text;

#pragma warning disable IDE0130
// ReSharper disable once CheckNamespace - Using the main namespace
namespace Microsoft.SemanticKernel;

/// <summary>Represents JSON Schema for describing types used in <see cref="KernelFunction"/>s.</summary>
[JsonConverter(typeof(KernelParameterJsonSchema.JsonConverter))]
public sealed class KernelParameterJsonSchema
{
    /// <summary>The schema stored as a string.</summary>
    private string? _schemaAsString;

    /// <summary>Parses a JSON Schema for a parameter type.</summary>
    /// <param name="jsonSchema">The JSON Schema as a string.</param>
    /// <returns>A parsed <see cref="KernelParameterJsonSchema"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="jsonSchema"/> is null.</exception>
    /// <exception cref="JsonException">The JSON is invalid.</exception>
    public static KernelParameterJsonSchema Parse(string jsonSchema) => new(JsonSerializer.Deserialize<JsonElement>(jsonSchema));

    /// <summary>Parses a JSON Schema for a parameter type.</summary>
    /// <param name="jsonSchema">The JSON Schema as a sequence of UTF16 chars.</param>
    /// <returns>A parsed <see cref="KernelParameterJsonSchema"/>.</returns>
    /// <exception cref="JsonException">The JSON is invalid.</exception>
    public static KernelParameterJsonSchema Parse(ReadOnlySpan<char> jsonSchema) => new(JsonSerializer.Deserialize<JsonElement>(jsonSchema));

    /// <summary>Parses a JSON Schema for a parameter type.</summary>
    /// <param name="utf8JsonSchema">The JSON Schema as a sequence of UTF8 bytes.</param>
    /// <returns>A parsed <see cref="KernelParameterJsonSchema"/>.</returns>
    /// <exception cref="JsonException">The JSON is invalid.</exception>
    public static KernelParameterJsonSchema Parse(ReadOnlySpan<byte> utf8JsonSchema) => new(JsonSerializer.Deserialize<JsonElement>(utf8JsonSchema));

    /// <summary>Initializes a new instance from the specified <see cref="JsonElement"/>.</summary>
    /// <param name="jsonSchema">The schema to be stored.</param>
    private KernelParameterJsonSchema(JsonElement jsonSchema) => this.RootElement = jsonSchema;

    /// <summary>Gets a <see cref="JsonElement"/> representing the root element of the schema.</summary>
    public JsonElement RootElement { get; }

    /// <summary>Gets the JSON Schema as a string.</summary>
    public override string ToString() => this._schemaAsString ??= JsonSerializer.Serialize(this.RootElement, JsonOptionsCache.WriteIndented);

    /// <summary>Converter for reading/writing the schema.</summary>
    public sealed class JsonConverter : JsonConverter<KernelParameterJsonSchema>
    {
        /// <inheritdoc/>
        public override KernelParameterJsonSchema? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(JsonElement.ParseValue(ref reader));

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, KernelParameterJsonSchema value, JsonSerializerOptions options) =>
            value.RootElement.WriteTo(writer);
    }
}