using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract
{
    public interface IUserInputReader //interfejs dla odczytywania ruchu użytkownika

     /* Metoda ReadKey() zwraca obiekt typu ConsoleKeyInfo, który zawiera informacje o naciśniętym klawiszu, takie jak:
       Key = Właściwość określająca naciśnięty klawisz.
       KeyChar = Znak reprezentujący naciśnięty klawisz, jeśli dotyczy (np.litery, cyfry).
       Modifiers = Informacje o modyfikatorach, takich jak Shift, Alt czy Ctrl. */

    {
        ConsoleKeyInfo ReadKey();
    }
}
