using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// JSON converter for handling <see cref="DateOnly"/> values.
/// Converts JSON strings into DateOnly objects and back.
/// </summary>
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    /// <summary>
    /// Reads a JSON string and converts it to a <see cref="DateOnly"/> instance.
    /// </summary>
    /// <param name="reader">The UTF8 JSON reader.</param>
    /// <param name="typeToConvert">The target type.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>A parsed <see cref="DateOnly"/> value.</returns>
    public override DateOnly Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
        => DateOnly.Parse(reader.GetString()!);

    /// <summary>
    /// Writes the <see cref="DateOnly"/> value to JSON as a formatted string (yyyy-MM-dd).
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">Serializer options.</param>
    public override void Write(
        Utf8JsonWriter writer,
        DateOnly value,
        JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}

/// <summary>
/// JSON converter for handling <see cref="TimeOnly"/> values.
/// Converts JSON strings into TimeOnly objects and back.
/// </summary>
public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    /// <summary>
    /// Reads a JSON string and converts it to a <see cref="TimeOnly"/> instance.
    /// </summary>
    /// <param name="reader">The UTF8 JSON reader.</param>
    /// <param name="typeToConvert">The target type.</param>
    /// <param name="options">Serializer options.</param>
    /// <returns>A parsed <see cref="TimeOnly"/> value.</returns>
    public override TimeOnly Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
        => TimeOnly.Parse(reader.GetString()!);

    /// <summary>
    /// Writes the <see cref="TimeOnly"/> value to JSON as a formatted string (HH:mm:ss).
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">Serializer options.</param>
    public override void Write(
        Utf8JsonWriter writer,
        TimeOnly value,
        JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("HH:mm:ss"));
}