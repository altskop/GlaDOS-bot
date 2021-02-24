using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourCheese.GameAgent.Strategies
{
    public interface Strategy
    {
        double getConfidence();
        void setConfidence(double t);

        void abort();
        void run();

        String getAsString();
        String getMode();

        void setNavigator(Navigator navigator);
        void update(GameDataContainer gameDataContainer);
    }
}
