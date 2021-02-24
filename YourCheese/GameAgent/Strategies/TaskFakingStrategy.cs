using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.Strategies
{
    class TaskFakingStrategy : Strategy
    {

        SkeldMap map;
        Navigator navigator;
        double confidence = 1;
        public List<GameTask> taskPositions;
        public List<GameTask> doneTasks;

        public TaskFakingStrategy(Navigator navigator, SkeldMap map, List<GameTask> doneTasks)
        {
            this.navigator = navigator;
            this.map = map;
            this.doneTasks = doneTasks;
        }

        public void run()
        {
            taskPositions = new TaskManager().getTaskPositions();
            taskPositions.RemoveAll(x => doneTasks.Contains(x));

            while (taskPositions.Count > 0 && new Random().NextDouble() < confidence)
            {
                doTask();
            }
        }

        private void doTask()
        {
            var task = getClosestTask(taskPositions);
            navigator.setDestination(task.position);
            System.Threading.Thread.Sleep(task.fakeTime+500);
            doneTasks.Add(task);
            confidence -= 0.25;
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
            return $"Faking tasks";
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
