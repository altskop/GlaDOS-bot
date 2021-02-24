using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YourCheese.GameAgent.Forms
{
    public partial class BotStatus : Form
    {

        public BehaviorDriver behaviorDriver;
        
        public BotStatus()
        {
            InitializeComponent();
        }

        public void update(BehaviorDriver behavior)
        {
            MethodInvoker mi = delegate () {
                isImposterLabel.Text = behavior.botInfo.isImposter.ToString();
                if (behavior.currentStrategy != null)
                modeLabel.Text = behavior.currentStrategy.getMode();
                inEmergencyMeetingLabel.Text = behavior.inEmergencyMeeting.ToString();
            };
            this.Invoke(mi);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
