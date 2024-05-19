using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.Tests;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Majowa wersja - bez opcji czasu: wersja gotowa do pracy z modułem 4: Testy.");

            // Poniżej daję info użytkownikowi
            Console.WriteLine("Pytania poniższego quizu są jednokrotnego wyboru. Po zapoznaniu się z treścią pytania naciśnij a, b lub c,");
            Console.WriteLine("wybierając odpowiedź według Ciebie poprawną.");
            Console.WriteLine("Za poprawną odpowiedż otrzymasz jeden punkt, za błędną brak punktu.");
            Console.WriteLine();

            // Inicjalizuję obiekty dla pytań, wyborów i weryfikacji odpowiedzi itd
            QuestionServiceApp questionServiceApp = new QuestionServiceApp();
            QuestionsListServiceApp questionsManagerApp = new QuestionsListServiceApp(questionServiceApp); // instancja QuestionsManagerApp i przekazuję questionServiceApp jako parametr

            ChoicesArraysServiceApp choicesService = new ChoicesArraysServiceApp();
            AnswerVerifierServiceApp answerVerifierManager = new AnswerVerifierServiceApp();

            IUserInputReader userInputReader = new FakeUserInputReader( new ConsoleKeyInfo()); // tworzę instancję implementacji IUserInputReader - na potrzeby testu
            UsersChoicesManagerApp usersService = new UsersChoicesManagerApp(userInputReader); // przekazuję implementację IUserInputReader do konstruktora

            // Zmienna przechowująca łączną liczbę punktów uzyskanych przez użytkownika 
            int totalPoints = 0; //zostawiam tutaj???

            /*// Pobranie wszystkich pytań
            List<Question> allQuestions = questionServiceApp.AllQuestions.ToList();

            // Tworzę pętlę przechodzącą przez każde pytanie w zestawie //do menagera?
            for (int i = 0; i < allQuestions.Count; i++)
            {
                var question = allQuestions[i];
                var choices = choicesService.GetChoicesForQuestion(i);

                // Wyświetlanie pytania
                Console.WriteLine($"Pytanie {question.QuestionNumber + 1}: {question.QuestionContent}");

                // Wyświetlanie dostępnych wyborów w pętli //do menagera?
                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }*/

                // Pobieranie wyboru od użytkownika //to bym tu zostawiła
                char userChoice = usersService.GetUserChoice();

                /*// Następuje weryfikacja odpowiedzi i przyznawanie punktów //to do serwisu? też raczej do managera
                bool result = answerVerifierManager.GetPointsForAnswer(question.QuestionNumber, userChoice);
                Console.WriteLine();

                // Wyświetlanie informacji o poprawności odpowiedzi //tu tutaj czy też do managera?
                if (result)
                {
                    totalPoints++;
                    Console.WriteLine("Poprawna odpowiedź. Zdobywasz 1 punkt.");
                }
                else
                {
                    Console.WriteLine("Odpowiedź błędna. Brak punktu.");
                }*/

                if (i < allQuestions.Count - 1)
                {
                    Console.WriteLine($"Aktualna liczba punktów: {totalPoints}");
                    Console.WriteLine();
                    Console.WriteLine("Naciśnij Enter, aby przejść do kolejnego pytania.");

                    // Czekanie na gotowość użytkownika przed przejściem do następnego pytania (jeśli nie jest to ostatnie pytanie)
                    Console.WriteLine("Jeżeli zaś chcesz zakończyć zabawę z quiz naciśnij k, nastepnie Enter."); //zakończenie quizu na żądanie
                    
                    /*string? userInputX = Console.ReadLine(); //czyli ten mechanizm do managera?
                    if (userInputX == "k" || userInputX == "K")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Quiz został zatrzymany.");
                        break;
                    }*/
                }
            }
            Console.WriteLine($"Twój wynik końcowy: {totalPoints} pkt.");  // wyświetlanie końcowego wyniku 
        }
    }   
}
