﻿namespace DeveloperStore.Sales.Application.Response;

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;

}
