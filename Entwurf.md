Projektentwurf
Arbeitstitel: Production Time Analyzer

Grundidee
Die Anwendung ist eine webbasierte ASP.NET‑Core‑Anwendung, die Produktionszeiten von Produkten auf Maschinen erfasst, anzeigt und analysiert.
Sie richtet sich an B2B‑/Industrie‑Szenarien (Fertigung, Werkstücke, Maschinenlaufzeiten).

Die Anwendung läuft als eine monolithische Web‑App (Frontend + Backend in einem Projekt), nutzt eine echte ASP.NET‑Pipeline und eine SQL‑Server‑Datenbank.
Alle sicherheits‑ und transportrelevanten Aspekte (HTTPS/TLS) werden der Plattform überlassen.

Fachlicher Kontext
Ein Produkt (z. B. ein Werkstück oder Auftrag) wird auf einer oder mehreren Maschinen bearbeitet.
Während der Bearbeitung entstehen Zeitabschnitte, z. B.:

Rüstzeit
Produktionszeit
Stillstand
Nacharbeit
Diese Zeitabschnitte werden zeitlich erfasst und anschließend ausgewertet.

Ziel ist nicht Mitarbeiter‑Zeiterfassung, sondern transparente Produktionszeit‑Analyse.

Zentrale Fachobjekte (gedanklich, nicht technisch)
Produkt
(Name, Auftragsnummer)

Maschine
(Name, Typ wie CNC oder Drehbank)

Zeiterfassungseintrag
(Produkt, Maschine, Startzeit, Endzeit, Status)

Benutzerablauf (User Flow)
Der Benutzer meldet sich an (Standard‑Login, keine Sonderlogik).
Er öffnet eine Produktionsübersichtsseite.
Er wählt:
ein Startdatum
ein Enddatum
optional eine Maschine oder ein Produkt
Er klickt auf „Anzeigen“.
Das System zeigt:
eine tabellarische Übersicht der Zeitabschnitte
eine grafische Darstellung der Zeiten (z. B. Balken oder Zeitverlauf).
Der Benutzer kann:
auf einzelne Einträge klicken
eine Detailanalyse anfordern
Optional kann der Benutzer eine Frage an einen Analyse‑Agenten stellen (z. B. „Warum gab es hier Stillstand?“).
Darstellung im Frontend (HTML & JavaScript)
HTML / Razor ist zuständig für:
Seitenlayout
Formular (Datumsauswahl, Drop‑downs)
Tabellen‑Container
Diagramm‑Container
Platzhalter für Agent‑Antworten
HTML ist rein darstellend, keine Fachlogik.

JavaScript ist zuständig für:
Absenden des Formulars ohne Seitenreload
Abrufen der Daten vom Backend per fetch
Anzeige der gelieferten Daten:
Tabellen füllen
Diagramme zeichnen (z. B. Chart.js)
Interaktion:
Klick auf Datensätze
Nachladen von Detaildaten
Halten von UI‑Zustand (z. B. aktuell ausgewählter Zeitbereich)
JavaScript berechnet keine Zeiten und trifft keine fachlichen Entscheidungen.

Backend (ASP.NET Core)
Das Backend:

stellt HTTP‑Endpunkte bereit (GET/POST)
berechnet Zeiten und Gruppierungen
greift auf die SQL‑Server‑Datenbank zu
liefert fertig aufbereitete Ergebnisse an das Frontend
Alle fachlichen Berechnungen passieren ausschließlich hier.

Analyse‑Agent (bewusst einfach)
Der Agent ist:

ein Server‑Service
bekommt bereits aggregierte Daten
erzeugt textuelle Erklärungen
Beispielhafte Aufgabe:

„Erkläre auffällige Stillstandszeiten im gewählten Zeitraum.“

Der Agent:

ist lokal implementiert
darf langsam sein
ist klar über ein Interface abstrahiert
kann später ersetzt werden
Der Agent ist kein KI‑Marketing‑Feature, sondern ein Analyse‑Modul.

Datenhaltung
Microsoft SQL Server (LocalDB oder Express)
Verwaltung z. B. über SQL Server Management Studio
Entity Framework Core für den Zugriff
Die Datenbank ist real, nicht simuliert.

Architekturelle Leitlinien (wichtig)
echte ASP.NET‑Pipeline, aber keine künstliche Trennung
eine Anwendung, klar geschichtet
kein Angular, kein React
kein eigenes Security‑Protokoll
Fokus auf Verständlichkeit, Wartbarkeit und Realität
Ziel des Projekts
Nicht:

ein vollständiges Produktionssystem
Sondern:

ein realistisches, professionelles Beispielprojekt,
das zeigt, dass du:
Geschäftsdomänen verstehst
saubere Web‑Architektur entwerfen kannst
Zustände korrekt trennst
moderne Konzepte gezielt, nicht inflationär einsetzt
Ein Satz für dein eigenes Mindset
Ich baue eine kleine, reale Produktionszeit‑Analyse‑Anwendung – nicht perfekt, aber bewusst strukturiert.