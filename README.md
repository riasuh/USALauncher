# USA LIFE | Launcher

## Vorwort

Danke, dass du dir das Repository zu unserem Launcher ansiehst. Da sich bei der Ausführung in manchen Fällen Windows Defender beschweren kann und damit so manch Skepsis erzeugt, haben wir uns entschieden, den Quellcode zu veröffentlichen. Bei Bedarf kann der Launcher auch selbst gebaut werden. Außerdem geben wir euch so die Möglichkeit, selbst Änderungen vorzunehmen. Beachte jedoch, dass wir den Launcher nur noch für Arma 3 verwenden werden. Daher wird sich vermutlich auch nicht mehr viel ändern.

## Einführung

Der USA Launcher ist eine WinForms-App, die mit C# entwickelt wurde. Hauptziel des Launchers ist, den Gameserver zu entlasten sowie schnellere Ladezeiten zu ermöglichen (durch Download der Missionfile via Fileserver). Zusätzlich wird per Launcher exklusiv eine Texturemod angeboten, in der sämtliche Texturen für den Server in hoher Auflösung vorhanden sind.

Anfangs basierte die App auf dem .NET Framework 4.5.2 bzw. später auf dem .NET Framework 4.6.1. Vor kurzem wurde die App jedoch auf [.NET 8](https://learn.microsoft.com/de-de/dotnet/core/whats-new/dotnet-8/overview) migriert, um eine einzige ausführbare Datei zu ermöglichen. Der Vorteil dabei ist, dass keine zusätzlichen Anforderungen für die Nutzer bestehen – es müssen keine zusätzlichen .NET-Runtimes, Frameworks oder .dlls installiert werden. Dies ist auch der Grund, warum die Größe der .exe im Vergleich etwas zugenommen hat. Die Spieleranzahl des Servers wird mithilfe des integrierten [SteamQuery.NET-Pakets](https://www.nuget.org/packages/SteamQuery.NET) abgewickelt.

## Einrichten der Entwicklungsumgebung

1. Installiere Visual Studio. Der Code wurde mit VS 2022 getestet.\
  <https://visualstudio.microsoft.com/vs/>
2. Installiere benötigte Komponenten und Erweiterungen
    - .NET-Desktopentwicklung
    - Entwicklung für die universelle Windows-Plattform
    - Erweiterungen
        - Microsoft Visual Studio Installer Projects 2022\
          <https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2022InstallerProjects>
        - ML.NET Model Builder 2022\
          <https://marketplace.visualstudio.com/items?itemName=MLNET.ModelBuilder2022>
3. Öffne die [./USALauncher.sln](./USALauncher.sln) mit VS 2022.
4. Build und Debug sollten jetzt „Out-of-the-box“ funktionieren.

## Links zu Version und Dateien

Der Launcher nutzt `.txt` Dateien, die unter <https://download.usa-life.net> gehostet sind, um die aktuelle Version zu überprüfen. Nachfolgende Dateien werden derzeit immer vom Launcher verwendet, da für die Entwicklung keine separate Umgebung eingerichtet wurde.

- <https://download.usa-life.net/launcherversion.txt>
- <https://download.usa-life.net/mod.txt>
- <https://download.usa-life.net/mission.txt>
- <https://download.usa-life.net/radio.txt>

## Mögliche Weiterentwicklung

Alternativ können die Daten aus unserer Datenbank abgerufen werden. Dazu muss folgende [GraphQL Query](https://graphql.org/) an unseren öffentlichen Endpunkt geschickt werden:

GraphQL Endpunkt: <https://hasura.usa-life.net/v1/graphql>

```gql
query A3ConfigVersionByPk {
  A3ConfigVersionByPk(lock: "X") {
    launcher
    mission
    mod
    modBillboard
    modHighResTex
    modObjectOverwrites
    radio
  }
}
```

Als Resultat wird folgendes JSON Object zurückgegeben:

```json
{
  "data": {
    "A3ConfigVersionByPk": {
      "launcher": "1.1.4.0",
      "mission": "v3",
      "mod": "1.1.0.0",
      "modBillboard": "1.1.0.0",
      "modHighResTex": "1.1.0.0",
      "modObjectOverwrites": "1.1.0.0",
      "radio": "1.0.0.0"
    }
  }
}
```
## Mitwirkende
**Milozz** (aka ria/riasuh) - Code\
**Larry** (aka Tim) - Code/Git\
**L03ff3l** (aka Loeffel) - Design