using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class TimeMeasuringServiceApp
    {
        public const int TIME_PER_QUESTION_IN_FULL_SECONDS = 15;
        private readonly System.Threading.Timer timer;
        private int timeLeftInFullSeconds;
        private bool isTimerRunning; // Dodajemy flagę do śledzenia czy timer jest włączony

        public TimeMeasuringServiceApp()
        {
            timeLeftInFullSeconds = TIME_PER_QUESTION_IN_FULL_SECONDS;
            timer = new System.Threading.Timer(TimerElapsed, null, Timeout.Infinite, 1000);
            isTimerRunning = false; // Ustawiamy początkową wartość flagi na false
        }

        public void StartTimer()
        {
            if (!isTimerRunning) // Uruchamiamy timer tylko jeśli nie jest już uruchomiony
            {
                timer.Change(0, 1000); // Rozpocznij odliczanie czasu
                isTimerRunning = true; // Aktualizujemy flagę
            }
        }

        public void StopTimer()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite); // Zatrzymaj odliczanie czasu
            isTimerRunning = false; // Aktualizujemy flagę
        }

        public void ResetTimer()
        {
            timeLeftInFullSeconds = TIME_PER_QUESTION_IN_FULL_SECONDS;
        }

        public int GetElapsedTime()
        {
            return TIME_PER_QUESTION_IN_FULL_SECONDS - timeLeftInFullSeconds;
        }

        private void TimerElapsed(object? state)
        {
            timeLeftInFullSeconds--;

            if (timeLeftInFullSeconds <= 0)
            {
                StopTimer();
                OnTimeElapsed();
            }
            else
            {
                Console.Write($"\rCzas pozostały: {timeLeftInFullSeconds} sekund     "); // Nadpisanie aktualnej linii konsoli
            }
        }

        protected virtual void OnTimeElapsed()
        {
            TimeElapsed?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler? TimeElapsed;
    }
}
    
