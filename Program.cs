using System.Text.RegularExpressions;

Console.WriteLine();
Console.WriteLine("Skriv in produkter. Avsluta med 'exit' eller visa listan med 'lista'");
Console.WriteLine();
Console.WriteLine("Accepterade värden: A-ö, 0-9 i format ABC-123. Sifferdel min 200, max 500.");


string[] produktArray = new string[0];
int index = 0;

//Godkänn endast A-ö (Programmet är på svenska, så ÅÄÖ är inkluderade) A-ö är samma som A-Öa-ö.
Regex letterRegex = new Regex(@"[^A-ö]");

//Godkänn endast siffror.
Regex digitRegex = new Regex(@"[\D]");

while(true)
{
    Console.ResetColor(); //Istället för en ResetColor per utskrift återställer jag i början av varje loop.
    Console.Write("Ange produkt: ");
    string input = Console.ReadLine().Trim();
    //Väljer att trimma eventuella spaces eftersom de kan vara svåra att se i konsol. Risk att man ej förstår varför det blir fel.
    //Gäller dock endast spaces före/efter korrekt format.

    if (input.ToLower() == "exit")
    {
        break; //Bryt loop
    }
    else if (input.ToLower() == "lista") //Ville ha möjlighet att se listan utan att behöva avsluta.
    {
        Console.WriteLine();
        Console.WriteLine("Lista efter inmatning:");

        foreach (string produkt in produktArray)
        {
            Console.WriteLine($"> {produkt}");
        }

        Console.Write("Fortsätt inmatning? 'ja' / 'nej': ");
        string input2 = Console.ReadLine();

        if(input2.ToLower().Trim() == "ja") //Fortsätt inmatning
        {
            continue; //Hoppa till nästa loop
        } 
        else if (input2.ToLower().Trim() == "nej") //Avsluta program
        {
            break;
        } 
        else
        {
            Console.WriteLine($"Oklart kommando '{input2}', fortsätter."); //Användaren skrev något konstigt, default continue
            continue;
        }
    }

    //Kontrollera om input har '-'
    if(input.Contains("-"))
    {
        string[] components = input.Split("-");

        //Kontrollera om antalet delar är 2. (Problematiskt att Split skapar tomma strings från intet om input saknas före eller efter -)
        //Fick implementera ytterliggare en condition om att innehållet per del ej är "" då regex ej matchar i en tom string.
        if (components.Length == 2)
        {
            string inputLeft = components[0];
            string inputRight = components[1];
            bool valueRight = int.TryParse(inputRight, out int rightValue);

            //Kontrollera värde 1 -> endast bokstäver
            if (letterRegex.Count(inputLeft) > 0 || inputLeft == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Felaktigt format del 1: >{inputLeft}< (Endast bokstäver. A-ö)");
                continue;
            }

            //Kontrollerar värde 2 -> endast siffror samt intervall
            if (digitRegex.Count(inputRight) > 0 || inputRight == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Felaktigt format del 2: >{inputRight}< (Endast siffror. 0-9)");
                continue;
            } 
            else if (rightValue > 500 || rightValue < 200) //Om TryParse misslyckas blir värdet 0 hos rightValue, vilket också faller utanför intervallet.
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Felaktigt värde: >{rightValue}< (Ange värde mellan 200 och 500)");
                continue;
            }

            //Lägg till produkt.
            Array.Resize(ref produktArray, produktArray.Length + 1);
            produktArray[index] = input.ToUpper();
            index++;
            Console.ForegroundColor = ConsoleColor.Green; //Bekräftelse i grön text med vad som läggs till på listan.
            if (produktArray.Length > 1) 
            {
                Console.WriteLine($"> {input.ToUpper()} tillagd. Totalt {produktArray.Length} produkter.");
            } 
            else
            {
                Console.WriteLine($"> {input.ToUpper()} tillagd. Totalt {produktArray.Length} produkt.");
            }
        } 
        //Om antalet delar inte är 2 i components, d.v.s. mer än 1 '-' vid Split('-')
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Felaktigt format: För många bindestreck(-). (Ange i format: ABC-123)");
        }
    }
    //Om '-' saknas.
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Felaktigt format: bindestreck(-) saknas. (Ange i format: ABC-123)");
    }
}

Console.ResetColor(); //Återställer även textfärg här när man bryter ur loop.
Array.Sort(produktArray);
Console.WriteLine();
Console.WriteLine("Alfabetisk lista av totalt {0} produkter:", produktArray.Length);
Console.WriteLine();

foreach (string produkt in produktArray)
{
    Console.WriteLine($"> {produkt}");
}
