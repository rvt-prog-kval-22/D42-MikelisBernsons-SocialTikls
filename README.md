# Sociālo diskusiju portāls ar automatizētu datu uzskaites sistēmu - Domātava

Janu kāds skolotājs nāk apskatīt un redz, ka vēl neesmu commitojis reālus projekta failus tas ir tādēļ, ka pašlaik patstāvīgi atsevišķos mazākos projektos cenšos iemācīties kā realizēt dažas šim projektam vajadzīgās funkcionalitātes.

## Apraksts:
Mājaslapa paredzēta lietotājiem, kas vēlas apspriest kopīgas intereses, dalīties ar domām un attēliem utml. ar citiem cilvēkiem.

## Plānotās funkcionalitātes:

### Reģistrēties un taisīt lietotāja profilu:

uzstādīt publisko lietotājvārdu,

mainīt paroli kad ir iegājis profilā,

atjaunināt paroli ar linku kuru nosūta uz e-pastu,

uzstādīt lietotāja bildi

### Taisīt publiskas grupas:

grupas nosaukums, apraksts

grupai var pievienoties jebkurš cilvēks (tās ir publiskas),

katrā grupā ir moderātori/admini ar paaugstinātām privilēģijām (skatīt zemāk)

parasti lietotāji kuri var taisīt postus iekš grupas kā arī vērtēt citus postus un komentēt zem tiem.

### Taisīt post:

satur - tekstu, linkus uz citām lapām, video un attēliem utml.

postiem citi reģistrēti lietotāji var komentēt un tos vērtēt.

Lietotāji var čatot ar citiem lietotājiem privāti.

Pieejams saraksts ar populārākajām grupām un postiem. varbūt varētu pievienot arī citus kārtošanas kritērijus.


## Vispārējais Administrators:

var izdzēst postus un grupas

var banot lietotājus

var rediģēt grupas

var mainīt, pievienot, noņemt grupas moderātorus

## grupas moderators

var banot lietotājus tajā grupā

var izdzēst postus tajā grupā

var izdzēst komentārus tajā grupā

var piešķirt administratora tiesības citiem lietotājiem grupā kā arī tās atņemt

## Lietotājs:

var taisīt grupas un postus

var rediģēt savus postus

var taisīt jaunu grupu kurā automātiski kļūst par moderatoru

# Viesis:

Parasts lietotājs kurš var apskatīt postus un komentārus


## Pašlaik plānotās izmantotās tehnoloģijas:
HTML,
CSS,
JS,
C#,
.NET,
ASP.NET

## Izmantotie avoti un kad tie izmantoti:
[C# Dokumentācija](https://docs.microsoft.com/en-us/dotnet/csharp/) noteikti tiks izmantota  
[Entity Framework datu saglabāšana 1](https://www.youtube.com/watch?v=ZX7_12fwQLU) 09.12.2021  
[Entity Framework datu saglabāšana 2](https://www.youtube.com/watch?v=qkJ9keBmQWo) 09.12.2021  
[Identity Serviss lietotāja profila datiem](https://www.youtube.com/watch?v=egITMrwMOPU) 14.12.2021  

# Uzstādīšanas instrukcijas:
Projekts tiks veidots uz .NET 5.0

nepieciešamās NuGet pakotnes:  
Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore  
Microsoft.EntityFrameworkCore.Design  
Microsoft.EntityFrameworkCore.SqlServer  
Microsoft.EntityFrameworkCore.Tools  
Microsoft.VisualStudio.Web.CodeGeneration.Design  
