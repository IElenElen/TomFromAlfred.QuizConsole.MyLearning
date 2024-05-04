namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Moje kolejne... przeszłe już kroki w nauce konsoli c#.");
            Console.WriteLine();

            Console.WriteLine("Test Quizu");

            //Poniżej daję info użytkownikowi
            Console.WriteLine("Pytania poniższego quizu są jednokrotnego wyboru. Po zapoznaniu się z treścią pytania naciśnij a, b lub c");
            Console.WriteLine("wybierając odpowiedź według Ciebie poprawną.");
            Console.WriteLine("Za poprawną odpowiedż otrzymasz jeden punkt, za błędną brak punktu.");

            // Inicjalizuję obiekty dla pytań, wyborów i weryfikacji odpowiedzi
            QuestionsSet questionsSet = new QuestionsSet();
            ChoicesCollection choicesCollection = new ChoicesCollection();
            AnswerVerifier answerVerifier = new AnswerVerifier();

            // Zmienna przechowująca łączną liczbę punktów uzyskanych przez użytkownika
            int totalPoints = 0;

            // Tworzę pętlę przechodzącą przez każde pytanie w zestawie
            for (int i = 0; i < questionsSet.Questions.Count; i++)
            {
                var question = questionsSet.Questions[i];
                var choices = choicesCollection.GetChoicesForQuestion(i);

                // Wyświetlanie pytania
                Console.WriteLine($"Pytanie {question.QuestionNumber + 1}: {question.QuestionContent}");

                // Wyświetlanie dostępnych wyborów w pętli
                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                // Pobieranie wyboru od użytkownika
                char userChoice = GetUserChoice();
                Console.WriteLine($"Wybrana odpowiedź: {userChoice}");

                // Następuje weryfikacja odpowiedzi i przyznawanie punktów
                int points = answerVerifier.GetPointsForAnswer(question.QuestionNumber, userChoice);

                // Wyświetlanie informacji o poprawności odpowiedzi
                if (points == 1)
                {
                    Console.WriteLine("Poprawna odpowiedź. Zdobywasz 1 punkt.");
                }
                else
                {
                    Console.WriteLine("Odpowiedź błędna. Brak punktu.");
                }
                if (i < questionsSet.Questions.Count - 1)
                {
                    Console.WriteLine($"Aktualna liczba punktów: {totalPoints}");
                    Console.WriteLine("Naciśnij Enter, aby przejść do kolejnego pytania.");

                    // Czekanie na gotowość użytkownika przed przejściem do następnego pytania (jeśli nie jest to ostatnie pytanie)
                    Console.ReadLine();
                }
            }
            Console.WriteLine($"Twój wynik końcowy: {totalPoints} punktów.");  // Wyświetlanie końcowego wyniku 
        }

        // Metoda do pobierania wyboru od użytkownika
        static char GetUserChoice()
        {
            Console.Write("Twój wybór (wpisz a, b lub c): ");
            char userChoice = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();

            // Dodatkowa walidacja wyboru użytkownika
            while (userChoice != 'a' && userChoice != 'b' && userChoice != 'c')
            {
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                Console.WriteLine();
                Console.Write("Twój wybór (wpisz a, b lub c): ");
                userChoice = char.ToLower(Console.ReadKey().KeyChar);
            }
            Console.WriteLine(); // Nowa linia po wprowadzeniu wyboru
            return userChoice;
        }
    }
}
