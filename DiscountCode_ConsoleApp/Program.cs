using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

class Program
{
    private static HubConnection _connection;  
    private static TaskCompletionSource<bool> _generateCodesCompletionSource;
    private static TaskCompletionSource<bool> _useCodeCompletionSource;

    private static void SetupEventHandlers()
    {
        _connection.On<bool>("ReceiveGeneratedCodesResult", result =>
        {
            _generateCodesCompletionSource?.SetResult(result);
        });

        _connection.On<bool>("ReceiveCodeUsageResult", result =>
        {
            _useCodeCompletionSource?.SetResult(result);
        });
    }
    
    private static async Task<string> Ping()
    {
        try
        {
            var response = await _connection.InvokeAsync<string>("Ping");
            return $"Ping Response: {response}";
        }
        catch (Exception ex)
        {
            return $"Error calling Ping: {ex.Message}";
        }
    }
    private static async Task<string> GenerateCodes()
    {
        try
        {
            Console.Write("Enter Count: ");
            var countInput = Console.ReadLine();
            Console.Write("Enter Length (7 or 8): ");
            var lengthInput = Console.ReadLine();

            if (!ushort.TryParse(countInput, out ushort count) || (count < 1 || count > 2000))
            {
                return "Invalid Count. Must be between 1 and 2000.";
            }

            if (!byte.TryParse(lengthInput, out byte length) || (length != 7 && length != 8))
            {
                return "Invalid Length. Must be 7 or 8.";
            }

            _generateCodesCompletionSource = new TaskCompletionSource<bool>();

            await _connection.InvokeAsync("GenerateCodes", count, length);

            var result = await _generateCodesCompletionSource.Task;
            return result ? "Codes generated successfully." : "Failed to generate codes.";
        }
        catch (Exception ex)
        {
            return $"Error calling GenerateCodes: {ex.Message}";
        }
    }
    private static async Task<string> UseCode()
    {
        try
        {
            Console.Write("Enter Code (7 or 8 characters): ");
            var code = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(code) || (code.Length != 7 && code.Length != 8))
            {
                return "Invalid Code. Must be 7 or 8 characters.";
            }

            _useCodeCompletionSource = new TaskCompletionSource<bool>();

            await _connection.InvokeAsync("UseCode", code);

            var result = await _useCodeCompletionSource.Task;
            return result ? "Code used successfully." : "Failed to use code.";
        }
        catch (Exception ex)
        {
            return $"Error calling UseCode: {ex.Message}";
        }
    }
    private static async Task<string> GetUsedCodes()
    {
        try
        {
            var usedCodes = await _connection.InvokeAsync<List<DiscountCode>>("GetUsedCodes");
            if (usedCodes == null || usedCodes.Count == 0) return "No used codes found.";

            var result = "Used Codes:";
            foreach (var code in usedCodes)
            {
                result += $"\n- Code: {code.Code}, DiscountPercentage: {code.DiscountPercentage}% , UsedAt: {code.UsedAt}";
            }
            return result;
        }
        catch (Exception ex)
        {
            return $"Error calling GetUsedCodes: {ex.Message}";
        }
    }
    private static async Task<string> GetUnusedCodes()
    {
        try
        {
            var unusedCodes = await _connection.InvokeAsync<List<DiscountCode>>("GetUnusedCodes");
            if (unusedCodes == null || unusedCodes.Count == 0) return "No unused codes found.";

            var result = "Unused Codes:";
            foreach (var code in unusedCodes)
            {
                result += $"\n- Code: {code.Code}, DiscountPercentage: {code.DiscountPercentage}% , CreatedAt: {code.CreatedAt}";
            }
            return result;
        }
        catch (Exception ex)
        {
            return $"Error calling GetUnusedCodes: {ex.Message}";
        }
    }



    static async Task Main(string[] args)
    {
        // Establish the SignalR connection
        _connection = new HubConnectionBuilder()
            .WithUrl("https://discountcode-be.onrender.com/discountHub")
            .WithAutomaticReconnect()
            .Build();

        try
        {
            await _connection.StartAsync();
            SetupEventHandlers();
            Console.WriteLine("Connected to SignalR Hub!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR Hub: {ex.Message}");
            return;
        }

        string lastResult = string.Empty; // Store the last operation result to display

        // Show options
        while (true)
        {
            Console.Clear();
            
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1 - Call Ping");
            Console.WriteLine("2 - Call GenerateCodes (Enter Count & Length)");
            Console.WriteLine("3 - Call UseCode (Enter Code)");
            Console.WriteLine("4 - Call GetUsedCodes");
            Console.WriteLine("5 - Call GetUnusedCodes");
            Console.WriteLine("0 - Exit");

            if (!string.IsNullOrWhiteSpace(lastResult))
            {
                Console.WriteLine("\nLast Result:");
                Console.WriteLine(lastResult);
            }

            Console.Write("\nEnter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    lastResult = await Ping();
                    break;
                case "2":
                    lastResult = await GenerateCodes();
                    break;
                case "3":
                    lastResult = await UseCode();
                    break;
                case "4":
                    lastResult = await GetUsedCodes();
                    break;
                case "5":
                    lastResult = await GetUnusedCodes();
                    break;
                case "0":
                    Console.WriteLine("Exiting...");
                    await _connection.StopAsync();
                    return;
                default:
                    lastResult = "Invalid choice. Please try again.";
                    break;
            }
        }
    }

}

public class DiscountCode
{
    public string Code { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public int DiscountPercentage { get; set; }
}
