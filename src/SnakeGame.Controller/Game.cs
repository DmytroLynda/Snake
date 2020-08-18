using SnakeGame.Controller.ExternalInterfaces;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame.Controller
{
    public class Game
    {
        private IUpdater Updater { get; }

        public Game(IUpdater updater)
        {
            Updater = updater ?? throw new ArgumentNullException(nameof(updater));
        }

        public void Start(int delayBetweenUpdates)
        {
            if (delayBetweenUpdates < 0)
            {
                delayBetweenUpdates = 0;
            }

            Stopwatch sleepTimer = new Stopwatch();

            while (true)
            {
                ExecuteWithSpecifiedDelay(
                    () => Updater.Update(),
                    delayBetweenUpdates,
                    sleepTimer);
            }
        }

        public async Task StartAsync(int delayBetweenUpdates)
        {
            await Task.Run(() => Start(delayBetweenUpdates));
        }

        private static void ExecuteWithSpecifiedDelay(Action act, int delay, Stopwatch sleepTimer)
        {
            #region Check value and for Null

            Debug.Assert(act != null, nameof(act) + " != null");
            Debug.Assert(sleepTimer != null, nameof(sleepTimer) + " != null");
            Debug.Assert(delay > 0, $"{nameof(delay)} must be > 0.");

            #endregion

            sleepTimer.Start();

            long startProcessing = sleepTimer.ElapsedMilliseconds;

            act.Invoke();

            int remainingDelay = CalculateRemainingDelay(delay, sleepTimer, startProcessing);
            
            Thread.Sleep(remainingDelay);

            sleepTimer.Stop();
        }

        private static int CalculateRemainingDelay(int delay, Stopwatch sleepTimer, long startProcessing)
        {
            #region Check forn null

            Debug.Assert(sleepTimer != null, nameof(sleepTimer) + " != null");
            Debug.Assert(startProcessing <= sleepTimer.ElapsedMilliseconds, $"The start processing time must got after invoking the act.");
            Debug.Assert(delay > 0, $"{nameof(delay)} must be > 0.");

            #endregion

            long processingTime = sleepTimer.ElapsedMilliseconds - startProcessing;
            int remainingDelay = delay - (int)processingTime;
            if (remainingDelay < 0) remainingDelay = 0;
            return remainingDelay;
        }
    }
}
