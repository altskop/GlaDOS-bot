using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.Strategies
{
    class TaskDoingStrategy : Strategy
    {
        SkeldMap map;
        Navigator navigator;
        double confidence = 1;
        public List<GameTask> taskPositions;
        public List<GameTask> doneTasks = new List<GameTask>();
        TaskIdentifier currentTaskIdentifier;

        public TaskDoingStrategy(Navigator navigator, SkeldMap map)
        {
            this.navigator = navigator;
            this.map = map;
        }

        public void run()
        {
            taskPositions = new TaskManager().getTaskPositions();

            while (taskPositions.Count > 0 && new Random().NextDouble() < confidence)
            {
                doTask();
            }
        }

        private void doTask()
        {
            System.Threading.Thread.Sleep(500);
            taskPositions = new TaskManager().getTaskPositions();
            var task = getClosestTask(taskPositions);
            if (task != null)
            {
                //Console.WriteLine($"{task.position.x},{task.position.y}");
                navigator.setDestination(task.position);
                currentTaskIdentifier = new TaskIdentifier();
                currentTaskIdentifier.doTask();
                System.Threading.Thread.Sleep(500);
                doneTasks.Add(task);
            }
            confidence -= 0.1;
        }

        private GameTask getClosestTask(List<GameTask> taskPositions)
        {
            float distance = 99999;
            GameTask closestTask = null;
            foreach (var task in taskPositions)
            {
                float temp = Vector2.Distance(navigator.botPos, task.position);
                if (temp < distance)
                {
                    distance = temp;
                    closestTask = task;
                }
            }
            return closestTask;
        }

        public double getConfidence()
        {
            return confidence;
        }

        public void setConfidence(double t)
        {
            this.confidence = t;
        }

        public String getAsString()
        {
            return $"Doing my tasks";
        }

        public String getMode()
        {
            return $"Doing tasks";
        }

        public void abort()
        {
            confidence = 0;
            navigator.abort();

        }

        public void update(GameDataContainer gameDataContainer)
        {

        }

        public void setNavigator(Navigator navigator)
        {
            this.navigator = navigator;
        }
    }
}
