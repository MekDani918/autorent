# Rendszerfejlesztés - Autorent (37. csapat)

Alapértelmezett felhasználó:
| Felhasználónév | Jelszó |
| ------ | ------ |
| user | 123 |

## Telepítés

Kliens alkalmazás működéséhez szükséges a [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.3-windows-x64-installer), továbbá telepíteni kell még a [Node.js](https://nodejs.org/en/download) javascript futtatási környezetet is

Ezek után a [Releases](https://github.com/MekDani918/autorent/releases) oldalon a legfrissebb kiadást kell letölteni és kicsomagolni

Node modulok telepítése:
```sh
cd Szerver/
npm install
```
    
## Futtatás

#### Szerver:
```sh
cd Szerver/
node server.js
```
#### Kliens:
Kliens mappában `autorent.exe`

## Konfiguráció

#### Szerver:

Megadható egy `.env` fájlban hogy az API milyen porton legyen elérhető
```env
PORT=3000
```
Ebben az esetben a futtatás:
```sh
node --env-file=.env server.js
```

#### Kliens

Az `autorent.dll.config` fájlban módosítható az API címe
```xml
  <setting name="API_URL" serializeAs="String">
      <value>http://127.0.0.1:3000</value>
  </setting>
```

## Tech Stack

**Client:** Windows Presentation Foundation

**Server:** Node, Express, Sequelize, SQLite


## Készítők
- [Auerbach Dávid](https://www.github.com/david01978) (KCE9TF)
- [Koronczai Hont](https://www.github.com/Klaszfm) (AUCHWW)
- [Mátics Dániel](https://www.github.com/MekDani918) (HA12JZ)
