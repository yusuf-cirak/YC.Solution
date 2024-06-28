﻿namespace YC.Result;

/// <summary>
/// Represents a record struct that contains error information.
/// </summary>
public readonly record struct Error : IEquatable<Error>
{
    /// <summary>
    /// Gets the title of the error.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the detail of the error.
    /// </summary>
    public string Detail { get; }

    /// <summary>
    /// Gets the status code of the error.
    /// </summary>
    public int Status { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> record struct.
    /// </summary>
    /// <param name="title">The title of the error.</param>
    /// <param name="detail">The detail of the error.</param>
    /// <param name="status">The status code of the error.</param>
    internal Error(string title, string detail, int status)
    {
        Title = title;
        Detail = detail;
        Status = status;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Error"/> is equal to the current <see cref="Error"/>.
    /// </summary>
    /// <param name="other">The error to compare with the current error.</param>
    /// <returns><c>true</c> if the specified error is equal to the current error; otherwise, <c>false</c>.</returns>
    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Title == other.Value.Title && Detail == other.Value.Detail && Status == other.Value.Status;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> struct with the specified title, detail, and status.
    /// </summary>
    /// <param name="title">The title of the error.</param>
    /// <param name="detail">The detail of the error.</param>
    /// <param name="status">The status code of the error. Default is 400.</param>
    /// <returns>A new instance of the <see cref="Error"/> record struct.</returns>
    public static Error Create(string title, string detail, int status = 400) => new(title, detail, status);

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> struct with the specified detail and a default status of 400.
    /// </summary>
    /// <param name="detail">The detail of the error.</param>
    /// <returns>A new instance of the <see cref="Error"/> record struct.</returns>
    public static Error Create(string detail) => new(string.Empty, detail, 400);

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> struct with the specified status and empty title and detail.
    /// </summary>
    /// <param name="status">The status code of the error. Default is 400.</param>
    /// <returns>A new instance of the <see cref="Error"/> record struct.</returns>
    public static Error Create(int status = 400) => new(string.Empty, string.Empty, status);
}
