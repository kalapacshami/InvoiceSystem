# InvoiceSystem

Egyszerű számlázó rendszer egy állásinterjú házi feladataként. Termékeket, ügyfeleket és rendeléseket tárol, és bármelyik rendeléshez le tud generálni egy PDF számlát – a végösszeget automatikusan számolja, a kedvezményes és veszélyes termékeket pedig külön jelöli a számlán.

.NET 8 Web API, EF Core + SQLite, QuestPDF a PDF-hez.

## Futtatás

Kell hozzá a .NET 8 SDK, más nem – az adatbázis egy sima SQLite fájl, nem kell külön szervert telepíteni.

```bash
git clone <repo-url>
cd InvoiceSystem
dotnet restore
dotnet ef database update --project src/InvoiceSystem.Infrastructure --startup-project src/InvoiceSystem.Api
dotnet run --project src/InvoiceSystem.Api
```

Ha nincs meg a `dotnet ef` parancs: `dotnet tool install --global dotnet-ef`

Elindulás után a `/swagger` végponton kipróbálható minden endpoint. Az `invoicesystem.db` automatikusan létrejön az API mappájában.

## Endpointok

- `POST /api/Customers`, `GET /api/Customers`, `GET /api/Customers/{id}`
- `POST /api/Products`, `GET /api/Products`, `GET /api/Products/{id}`
- `POST /api/Orders`, `GET /api/Orders/{id}`
- `GET /api/Orders/{id}/invoice` – PDF letöltés

## A kedvezmény mértéke

Az `appsettings.json`-ban állítható, nincs a kódba égetve:

```json
"DiscountSettings": {
  "DiscountPercentage": 10
}
```

## Pár döntés, amit menet közben hoztam

A leírás sok mindent szabadon hagyott, ezekben döntöttem:

- **SQLite**, hogy ne kelljen semmit telepíteni/konfigurálni a futtatáshoz.
- **Nincs külön Invoice tábla** – a PDF mindig a rendelés aktuális adataiból generálódik, nincs redundáns adat.
- **A tétel ára rendeléskor rögzül** (`UnitPriceAtOrderTime`), hogy egy régi rendelés számlája ne változzon meg, ha egy termék ára közben módosul.
- **A kedvezmény soronként számít**, nem az egész rendelésre – így nem torzítja el egy olcsó kedvezményes tétel a drágább, nem kedvezményes tételek árát.
- **Érvénytelen ProductId esetén az egész rendelés elutasításra kerül**, nem jöhet létre félig hibás rendelés.
- **A válaszokban nem a nyers EF Core entitásokat adom vissza**, mert a kétirányú navigációs property-k (Order↔Customer) körkörös hivatkozást okoznának JSON-nál. Ehelyett egy sima DTO-t adok vissza, ez egyben a végösszeg-számítás helye is.

## SQL

A `sql` mappában van a teljes séma (`schema.sql`, EF migrationből generálva) és a feladatban kért két lekérdezés (`queries.sql`): top 3 termék mennyiség szerint, illetve veszélyes terméket tartalmazó rendelések.
