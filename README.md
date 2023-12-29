# TodoApi Tester
## Projektbeskrivning
Projektet är en ASP.NET-baserad ToDo-applikation med tillhörande enhetstester. Applikationen erbjuder ett API för att hantera ToDo-uppgifter, inklusive att lägga till, hämta, uppdatera och ta bort uppgifter.

## Setup av projektet
För att starta projektet behöver du .NET Core på din dator. Dessutom måste du ha tillgång till en databas för att kunna ansluta. I detta projekt används en SQL Server-databas. För att använda en egen databas behöver du ändra connection stringen i appsettings.json-filen. När du har bytt connection string måste du köra följande kommando i Package Manager Console:
```
update-database
```
