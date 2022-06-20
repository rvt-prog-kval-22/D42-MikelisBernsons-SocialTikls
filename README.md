# sociālo diskusiju portāla datu uzskaites automatizēta sistēma
## https://breddit.azurewebsites.net/

## Apraksts:
Mūsdienās cilvēki izmanto daudz un dažādus sociālos tīklus. Šajā kvalifikācijas darbā ir paredzēts lietotājiem piedāvāt alternatīvu vai papildu sociālo tīklu, kuram pievienoties. Šis portāls sniedz dažādiem cilvēkiem iespēju veidot jaunas grupas par sev interesējošiem tematiem, piemēram lietotāji to var izveidot par kādu politisku tematu apspriešanu, dalīšanos ar kaķu bildēm, jaunas valodas apguvi vai jeb ko citu. Grupas var pārvaldīt ar grupas administratoriem, dalīties ar domām attēlu teksta rakstu un “Youtube” saišu formātiem. Grupas vienmēr ir arī iespēja pārvaldīt vietnes administratoram, sliktu grupu izveidošanas gadījumā. Rakstus ir iespējams vērtēt ar patīk un nepatīk, lai palielinātu to popularitāti un dotu lietotājiem iespēju kārtot rakstus pēc tāda kritērija. Kā arī tiek dota iespēja grupas meklēt pēc to nosaukuma un, ja atrod kādu grupu, kuras temats interesē, tad ir iespēja tai pievienoties un kļūt par tās biedru.  
Reģistrētiem lietotājiem, kas pievienojas grupām tiek parādīts šo grupu saraksts, kas ļauj uz tām viegli pārvietoties no galvenās lapas, kā arī tiek parādīts personalizēts rakstu saraksts galvenajā lapā tikai no grupām, kurās lietotājs ir pievienojies. Lietotājs savā profilā arī var apskatīt visus sevis veidotos rakstus.


## Galvenās funkcionalitātes:
1.	grupu izveide, rediģēšana un dzēšana;
2.	rakstu izveide, rediģēšana un dzēšana;
3.	rakstu vērtēšana ar patīk/nepatīk, lai dotu iespēju kārtot pēc popularitātes;
4.	komentāru pievienošana un dzēšana;
5.	filtrēta datu izvade pēc dažādiem kritērijiem;
6.	vietnes pārvaldības iespējas;
7.	vietnes apskatīšana viesiem jeb nereģistrētiem lietotājiem.

## Izmantotās tehnoloģijas:
HTML, CSS, JS, Bootstrap  
C#  
.NET 6 MVC  
Visual Studio  
Gmail SMTP + MailKit  
Azure:  
-Web app  
-SQL Server  
-Key Vault  

## Izstrādes vides uzstādīšana
1. Jelupielādēt Visual Studio 2022 Community un jāpievieno WEB development pakotni  
2. Klonē projektu git-client://clone?repo=https%3A%2F%2Fgithub.com%2FMKBernsons%2FBreddit  
3. Nuget package manager konsolē jāuzraksta update-database  
4. Ar F5 var palaist projektu  

## Avoti:  
1.	[C# dokumentācija](https://docs.microsoft.com/en-us/dotnet/csharp/) Apskatīts 24/11/2021  
2.	[Entity Framework pamati 1](https://www.youtube.com/watch?v=ZX7_12fwQLU) Apmeklēts 09/12/2021  
3.	[Entity framework pamati 2](https://www.youtube.com/watch?v=qkJ9keBmQWo) Apmeklēts 10/12/2021  
4.	[Identity pamati](https://www.youtube.com/watch?v=egITMrwMOPU) Apskatīts 20/12/2022  
5.	[ASP.NET Core MVC pamati](https://www.youtube.com/watch?v=hZ1DASYd9rk) Apskatīts 20/12/2022  
6.	[Bootstrap dokumentācija](https://getbootstrap.com/), [Bootswatch dokumentācija](https://bootswatch.com/solar/) Apskatīts 13/11/2021  
7.	[Identity dokumentācija](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio)  Apskatīts 25/12/2021  
8.	[Lietotāju video pamācība 1](https://www.youtube.com/watch?v=egITMrwMOPU&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=65) Apskatīts 11/11/2021  
9.	[Failu saglabāšana sistēma](https://www.youtube.com/watch?v=aoxEJii70_I&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=53) Apskatīts 05/03/2022  
10.	[Lietotāju video pamācība 2](https://www.youtube.com/watch?v=NV734cJdZts&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=77) Apskatīts 26/02/2022  
11.	[Lietotāju video pamācība 3](https://www.youtube.com/watch?v=TzhqymQm5kw&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=81) Apskatīts 26/02/2022  
12.	[Lietotāju video pamācība 4](https://www.youtube.com/watch?v=7ikyZk5fGzk&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=80) Apskatīts 26/02/2022  
13.	[Lietotāju video pamācība 5](https://www.youtube.com/watch?v=KGIT8P29jf4&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=79) Apskatīts 26/02/2022  
14.	[Lietotāju video pamācība 6](https://www.youtube.com/watch?v=TuJd2Ez9i3I&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=78) Apskatīts 26/02/2022  
15.	[Paroļu atjaunošanas pamācība](https://www.youtube.com/watch?v=0W0yAz7fu04&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=115) Apskatīts 11/03/2022  
