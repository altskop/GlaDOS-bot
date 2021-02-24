using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent
{

    class VotingVector
    {
        public Vector2 initialButton;
        public Vector2 confirmButton;

        public VotingVector(Vector2 initialButton, Vector2 confirmButton)
        {
            this.initialButton = initialButton;
            this.confirmButton = confirmButton;
        }

        public VotingVector(Vector2 pos)
        {
            this.initialButton = pos;
            this.confirmButton = pos;
        }
    }
    class VotingDriver
    {

        List<VotingVector> votingButtons = new List<VotingVector>() { new VotingVector(new Vector2(395, 939), new Vector2(570, 937)),
                                                                      new VotingVector(new Vector2(714, 265)), new VotingVector(new Vector2(1367, 265)),
                                                                      new VotingVector(new Vector2(714, 406)), new VotingVector(new Vector2(1367, 406)),
                                                                      new VotingVector(new Vector2(714, 544)), new VotingVector(new Vector2(1367, 544)),
                                                                      new VotingVector(new Vector2(714, 686)), new VotingVector(new Vector2(1367, 686)),
                                                                      new VotingVector(new Vector2(714, 819)), new VotingVector(new Vector2(1367, 819))
                                                                    };

        public void vote(int livingPlayers)
        {
            System.Threading.Thread.Sleep(30000);
            List<VotingVector> options = votingButtons.GetRange(0, livingPlayers+1);
            int randomIndex = new Random().Next(options.Count);
            VotingVector randomChoice = options[randomIndex];

            var taskInput = new TaskInput();
            
            taskInput.mouseClick(randomChoice.initialButton);
            System.Threading.Thread.Sleep(500);
            taskInput.mouseClick(randomChoice.confirmButton);
        }
    }
}
