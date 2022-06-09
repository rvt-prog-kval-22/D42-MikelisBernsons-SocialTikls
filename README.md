# sociālo diskusiju portāla datu uzskaites automatizēta sistēma - [Links uz Github](https://github.com/MKBernsons/Breddit) - Pieejai jautāt caur mykoob.

Ja kāds skolotājs nāk apskatīt un redz, ka vēl neesmu commitojis reālus projekta failus tas ir tādēļ, ka projekts atrodas privātā repo. Links augstāk ^

## Apraksts:
Mājaslapa paredzēta lietotājiem, kas vēlas apspriest kopīgas intereses, dalīties ar domām un līdzīgām interesēm utml. ar citiem cilvēkiem.

## Plānotās funkcionalitātes:

### Reģistrēties un taisīt lietotāja profilu:

1. uzstādīt publisko lietotājvārdu - OK
2. mainīt paroli kad ir iegājis profilā - OK
3. atjaunināt paroli ar linku kuru nosūta uz e-pastu - OK

### Publiskas grupas:

1. grupas nosaukums un apraksts - OK
2. grupai var pievienoties/apskatīt jebkurš cilvēks (tās ir publiskas) - OK
3. katrā grupā ir moderātori/admini ar paaugstinātām privilēģijām (skatīt zemāk) - OK
4. parasti lietotāji kuri var taisīt postus iekš grupas - OK
5. Iespēja vērtēt citus postus - OK
7. komentēt zem postiem - OK
8. Meklēt grupas pēc nosaukuma - OK
9. Rediģēt grupas (aprakstu) - OK
10. Parādīt grupas kurām tu esi pievienojies, uzspiežot uz tās tu to atvērtu - OK
11. Spēja sakārtot grupas pēc kritērijiem (jaunākās un populārākās) - OK

### Taisīt post:

1. saturs - pagaidām virsraksts un teksta saturs, bet vēlāk pētīšu vairāk opcijas - OK  
  1.1 - Teksta post - OK  
  1.2 - Youtube linkA post - OK  
  1.3 - Foto attēla post - OK  
2. postiem citi reģistrēti lietotāji var komentēt - OK
3. Vērtēt ar patīk/nepatīk - OK
4. Spēja sakārtot post pēc kritērijiem (jaunākie un populārākie) - OK
5. Spēja atlasīt post pēc kritērijiem (šodien, pēdējā mēnesī, gadā izveidotie) - OK

## Globāls mājaslapas Administrators: - +/- OK

1. var izdzēst postus un grupas - OK
2. var banot lietotājus (varbūt)?
3. var rediģēt grupas - OK
4. var mainīt, pievienot, noņemt grupas moderatorus - OK
5. var pievienot un noņemt citus administratorus - OK

## grupas moderators

1. var banot lietotājus tajā grupā (varbūt) 
2. var izdzēst postus tajā grupā - OK
3. var izdzēst komentārus tajā grupā - OK
4. var piešķirt administratora tiesības citiem lietotājiem grupā kā arī tās atņemt - OK

## Lietotājs:

1. var taisīt grupas un postus - OK
2. var rediģēt savus postus - OK
3. var taisīt jaunu grupu - OK

# Viesis:

1. Parasts lietotājs kurš var apskatīt grupas, postus utt. - OK

## Pašlaik plānotās izmantotās tehnoloģijas:
HTML  
CSS  
JS  
C#  
ASP.NET  
MSSQL  

## Izmantotie avoti un kad tie izmantoti:
[C# Dokumentācija](https://docs.microsoft.com/en-us/dotnet/csharp/) šeit tiek ietverti ļot daudz linki (1 links uz visu) par oficiālo c# dokumentāciju.  
[Entity Framework datu saglabāšana 1](https://www.youtube.com/watch?v=ZX7_12fwQLU) 09.12.2021  
[Entity Framework datu saglabāšana 2](https://www.youtube.com/watch?v=qkJ9keBmQWo) 09.12.2021  
[Identity Serviss lietotāja profila datiem](https://www.youtube.com/watch?v=egITMrwMOPU) 14.12.2021  
[.NET CORE 6 MVC (2022)](https://www.youtube.com/watch?v=hZ1DASYd9rk)  
[Mājaslapas vizuālā tematika](https://bootswatch.com/solar/)  
[Bootstrap 5 dokumentācija](https://getbootstrap.com/)  
[Favicon](https://icons8.com/icons/set/favicon-cat)  
[Vairāki video no šī playlist](https://www.youtube.com/playlist?list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU)  
[ViewBag piemērs](https://www.tutorialsteacher.com/mvc/viewbag-in-asp.net-mvc)  
[Pamācība uz kuras pamata izveidoju e-pastu sūtīšanu](https://www.youtube.com/watch?v=qSeO9886nRM)  
[Gmail e-pastu sūtīšanas info](https://www.siteground.com/kb/gmail-smtp-server/)  

# Uzstādīšanas instrukcijas(Izmaiņu dēļ tās vairs nav atbilstošas projektam, atjaunināšu vēlāk):
  
1. [Lejuplādēt Visual Studio Community 2022 (iespējams var atvērt arī ar 2019 gada versiju vai vecāku, bet neesmu to testējis.](https://visualstudio.microsoft.com/downloads/)
2. Iegūt pieeju projektam kur tas tiek saglabāts (pajautāt caur mykoob), to lejuplādēt.
3. Projektu jāatver kā mākat, bet to arī var izdarīt atverot visual studio un klonējot repository ar linku.
4. Iekš visual studio jāatver solution explorer un jāatrod Bred.sln fails un to jāuzspiež 2x reizes - ![download (1)](https://user-images.githubusercontent.com/87810247/154998330-b7cbe69e-ae73-4ace-a354-6aedccde7472.png)  

5. Solution explorer jāatrod appsettings.json fails kurā jāievada jūsu administratora vēlamās login detaļas - ![image](https://user-images.githubusercontent.com/87810247/154995852-5ba755e5-5583-4ee7-908c-708af124c46c.png)  
6. atvērt nuget package manager console - ![image](https://user-images.githubusercontent.com/87810247/154998059-6634e7c4-6ec5-4fa3-b2b2-b8cb0cb5fd86.png)  

7. Atvērtajā konsolē jāuzraksta - "update-database" un jāspiež enter un nedaudz jāpagaida - ![image](https://user-images.githubusercontent.com/87810247/154997989-5869077f-85d0-42cf-bf45-2c0b64d12221.png)  

8. Jāstartē aplikācija - ja izvēlesieties IIS EXPRESS tad jāseko līdzi 9. punktam. ![image](https://user-images.githubusercontent.com/87810247/154996149-8bda529c-b81c-4411-8de5-888ffd72e0f8.png)  
9. Prasīs uzticēties sertifikātiem un jāspiež 2x reizes - piekrist/jā
10. Atveroties mājaslapai pie linka jāpieraksta klāt -> "/admin/setup" kas izveidos jūsu specificēto admina profilu.
