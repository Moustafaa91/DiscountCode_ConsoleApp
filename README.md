# Discount Code SignalR Console Client

This is a **C# Console Application** that connects to a SignalR WebSocket hub to interact with a backend service for managing discount codes. The application provides a menu-driven interface to perform the following operations:

1. Call the `Ping` method to test connectivity.
2. Generate discount codes with configurable count and length.
3. Use a discount code.
4. Retrieve a list of used discount codes.
5. Retrieve a list of unused discount codes.

The backend WebSocket hub is hosted at: `https://discountcode-be.onrender.com/discountHub`. The source code for the backend is available at [DiscountCode_BE](https://github.com/Moustafaa91/DiscountCode_BE).

---

## Technologies Used

- **C#** (Console Application)
- **.NET 6**
- **SignalR** for WebSocket communication
- **Microsoft.AspNetCore.SignalR.Client** package

---

## Installation

### Prerequisites
1. **.NET SDK** (version 6 or higher): [Download .NET](https://dotnet.microsoft.com/download)
2. A terminal or IDE capable of running .NET projects (e.g., Visual Studio or Visual Studio Code).

### Clone the Repository
```bash
git clone https://github.com/Moustafaa91/DiscountCode_ConsoleApp.git
cd DiscountCode_ConsoleApp
```

### Install Required Packages
Restore the dependencies:
```bash
dotnet restore
```

### Run the Application
Run the application locally:
```bash
dotnet run
```

---

## How to Use

### Menu Options
When you start the application, you will see the following menu:

```
Choose an action:
1 - Call Ping
2 - Call GenerateCodes (Enter Count & Length)
3 - Call UseCode (Enter Code)
4 - Call GetUsedCodes
5 - Call GetUnusedCodes
0 - Exit
```

### Operations
1. **Ping**:
   - Tests the connectivity to the SignalR hub.
   - Returns a simple response like `Hello, world!`.

2. **Generate Codes**:
   - Prompts for two inputs:
     - **Count**: Number of codes to generate (1 to 2000).
     - **Length**: Length of each code (7 or 8).
   - Displays whether the operation was successful.

3. **Use Code**:
   - Prompts for a single input:
     - **Code**: A valid discount code (7 or 8 characters).
   - Displays whether the code was successfully used.

4. **Get Used Codes**:
   - Retrieves a list of all used discount codes.

5. **Get Unused Codes**:
   - Retrieves a list of all unused discount codes.

---

## Backend Details

This application communicates with a SignalR WebSocket hub published at:
`https://discountcode-be.onrender.com/discountHub`

The backend source code is available at [DiscountCode_BE](https://github.com/Moustafaa91/DiscountCode_BE).

---

## Contributions

Contributions are welcome! Feel free to fork this repository and submit pull requests for any improvements or bug fixes.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.


