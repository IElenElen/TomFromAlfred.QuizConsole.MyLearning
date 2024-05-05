using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Refakto - marcowe zmagania.");
            Console.WriteLine("Refaktoryzacja - wstępna przymiarka 16.02.24.");
            Console.WriteLine("Moje kolejne... przeszłe już kroki w nauce konsoli c#.");
            Console.WriteLine();

            Console.WriteLine("Test Quizu");
            Console.WriteLine();

            //Poniżej daję info użytkownikowi
            Console.WriteLine("Pytania poniższego quizu są jednokrotnego wyboru. Po zapoznaniu się z treścią pytania naciśnij a, b lub c,");
            Console.WriteLine("wybierając odpowiedź według Ciebie poprawną.");
            Console.WriteLine("Za poprawną odpowiedż otrzymasz jeden punkt, za błędną brak punktu.");
            Console.WriteLine();

            // Inicjalizuję obiekty dla pytań, wyborów i weryfikacji odpowiedzi
            QuestionsManagerApp questionsService = new QuestionsManagerApp();
            ChoicesManagerApp choicesService = new ChoicesManagerApp();
            AnswerVerifierServiceApp answerVerifierManager = new AnswerVerifierServiceApp();

            // Zmienna przechowująca łączną liczbę punktów uzyskanych przez użytkownika
            int totalPoints = 0;

            // Tworzę pętlę przechodzącą przez każde pytanie w zestawie
            for (int i = 0; i < questionsService.Questions.Count; i++)
            {
                var question = questionsService.Questions[i];
                var choices = choicesService.GetChoicesForQuestion(i);

                // Wyświetlanie pytania
                Console.WriteLine($"Pytanie {question.QuestionNumber + 1}: {question.QuestionContent}");

                // Wyświetlanie dostępnych wyborów w pętli
                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                // Pobieranie wyboru od użytkownika
                UsersManagerApp usersService = new UsersManagerApp();
                char userChoice = usersService.GetUserChoice();
                Console.WriteLine();

                // Następuje weryfikacja odpowiedzi i przyznawanie punktów
                bool result = answerVerifierManager.GetPointsForAnswer(question.QuestionNumber, userChoice); 
                Console.WriteLine();

                // Wyświetlanie informacji o poprawności odpowiedzi
                if (result)
                {
                    totalPoints++;
                    Console.WriteLine("Poprawna odpowiedź. Zdobywasz 1 punkt.");
                }
                else
                {
                    Console.WriteLine("Odpowiedź błędna. Brak punktu.");
                }
                if (i < questionsService.Questions.Count - 1)
                {
                    Console.WriteLine($"Aktualna liczba punktów: {totalPoints}");
                    Console.WriteLine("Naciśnij Enter, aby przejść do kolejnego pytania.");

                    // Czekanie na gotowość użytkownika przed przejściem do następnego pytania (jeśli nie jest to ostatnie pytanie)

                    Console.WriteLine("Jeżeli zaś chcesz zakończyć zabawę z quiz naciśnij k, nastepnie Enter."); //zakończenie quizu na żądanie
                    string? userInputX = Console.ReadLine();

                    if (userInputX == "k" || userInputX == "K")
                    {
                        Console.WriteLine("Quiz został zatrzymany.");
                        break;
                    }
                    Console.ReadLine();
                    Console.WriteLine();
                }
            }
            Console.WriteLine($"Twój wynik końcowy: {totalPoints} pkt.");  // Wyświetlanie końcowego wyniku 
        }
    }
}
