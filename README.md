```
 _______                       __               ______                      
|       \                     |  \             /      \                     
| $$$$$$$\  ______   _______  | $$   __       |  $$$$$$\  ______    ______  
| $$__/ $$ |      \ |       \ | $$  /  \      | $$__| $$ /      \  /      \ 
| $$    $$  \$$$$$$\| $$$$$$$\| $$_/  $$      | $$    $$|  $$$$$$\|  $$$$$$\
| $$$$$$$\ /      $$| $$  | $$| $$   $$       | $$$$$$$$| $$  | $$| $$  | $$
| $$__/ $$|  $$$$$$$| $$  | $$| $$$$$$\       | $$  | $$| $$__/ $$| $$__/ $$
| $$    $$ \$$    $$| $$  | $$| $$  \$$\      | $$  | $$| $$    $$| $$    $$
 \$$$$$$$   \$$$$$$$ \$$   \$$ \$$   \$$       \$$   \$$| $$$$$$$ | $$$$$$$ 
                                                        | $$      | $$      
                                                        | $$      | $$      
                                                         \$$       \$$      
```
Av: Mikael Ekroth

## För krav
- Se till att ha DotNet 9 installerad.
- Se till att ha MongoDb 4.2+ installerad.
- Se till att ha Docker installerad för att köra integrations testerna.

## Projekt struktur
src
 - Mekroth.BankApp - Konsole application
 - Mekroth.BankApp.Core
   - Entiteter
   - Interfaces
 - Mekroth.BankApp.Application
   - Innehåller affärslogiken
   - Implementerar services interfaces ifrån Mekroth.BankApp.Core
 - Mekroth.BankApp.Infrastructure
   - Innehåller implementationen av datakällor, i detta fallet MongoDb.
     
tests 
 - Mekroth.BankApp.Application.UnitTests
   - Innehåller enhetstester rörande affärslogiken.
 - Mekroth.BankApp.Infrastructure.IntegrationTests
   - Innehåller tests direkt kopplat till implementationen av MongoDb
   
## Ramverk:
- DotNet: 9.0.4
- MongoDb.Driver: 3.3.0

## Test ramverk
- FluentAssertions
- NSubstitute
- xUnit
- TestContainer.MongoDb (Docker krävs)
