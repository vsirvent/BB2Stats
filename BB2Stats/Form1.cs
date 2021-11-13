using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BB2Stats
{
    public partial class BB2StatsForm : Form
    {
        int neg_total = 0;
        int one_total = 0;
        int two_total = 0;
        int three_total = 0;
        int skull_total = 0;
        int push_total = 0;
        int dodge_total = 0;
        int block_total = 0;
        int pow_total = 0;
        int dices_total = 0;
        int npows = 0;
        int nstuns = 0;
        int nkos = 0;
        int ninjuries = 0;
        int nbreaks = 0;
        int total_dodge = 0;
        int total_catch = 0;
        int total_ap = 0;

        public BB2StatsForm()
        {
            InitializeComponent();
        }

        private void updateTotalDices()
        {
            dices_total = neg_total + one_total + two_total + three_total;
            updateSkullValues();
            updatePushValues();
            updateDodgeValues();
            updateBlockValues();
            updatePowValues();

            if (neg_total > 0)
            {
                negPercentSkull.Text = ((int)((double)negDiceSkull.Value * 100.0 / (double)neg_total)).ToString();
                negPercentPush.Text = ((int)((double)negDicePush.Value * 100.0 / (double)neg_total)).ToString();
                negPercentDodge.Text = ((int)((double)negDiceDodge.Value * 100.0 / (double)neg_total)).ToString();
                negPercentBlock.Text = ((int)((double)negDiceBlock.Value * 100.0 / (double)neg_total)).ToString();
                negPercentPow.Text = ((int)((double)negDicePow.Value * 100.0 / (double)neg_total)).ToString();
            }
            if (one_total > 0)
            {
                onePercentSkull.Text = ((int)((double)oneDiceSkull.Value * 100.0 / (double)one_total)).ToString();
                onePercentPush.Text = ((int)((double)oneDicePush.Value * 100.0 / (double)one_total)).ToString();
                onePercentDodge.Text = ((int)((double)oneDiceDodge.Value * 100.0 / (double)one_total)).ToString();
                onePercentBlock.Text = ((int)((double)oneDiceBlock.Value * 100.0 / (double)one_total)).ToString();
                onePercentPow.Text = ((int)((double)oneDicePow.Value * 100.0 / (double)one_total)).ToString();
            }
            if (two_total > 0)
            {
                twoPercentSkull.Text = ((int)((double)twoDiceSkull.Value * 100.0 / (double)two_total)).ToString();
                twoPercentPush.Text = ((int)((double)twoDicePush.Value * 100.0 / (double)two_total)).ToString();
                twoPercentDodge.Text = ((int)((double)twoDiceDodge.Value * 100.0 / (double)two_total)).ToString();
                twoPercentBlock.Text = ((int)((double)twoDiceBlock.Value * 100.0 / (double)two_total)).ToString();
                twoPercentPow.Text = ((int)((double)twoDicePow.Value * 100.0 / (double)two_total)).ToString();
            }
            if (three_total > 0)
            {
                threePercentSkull.Text = ((int)((double)threeDiceSkull.Value * 100.0 / (double)three_total)).ToString();
                threePercentPush.Text = ((int)((double)threeDicePush.Value * 100.0 / (double)three_total)).ToString();
                threePercentDodge.Text = ((int)((double)threeDiceDodge.Value * 100.0 / (double)three_total)).ToString();
                threePercentBlock.Text = ((int)((double)threeDiceBlock.Value * 100.0 / (double)three_total)).ToString();
                threePercentPow.Text = ((int)((double)threeDicePow.Value * 100.0 / (double)three_total)).ToString();
            }
            totalDice.Text = dices_total.ToString();
            if (dices_total > 0)
            {
                powsPercent.Text = ((int)(((double)npows * 100.0) / (double)dices_total)).ToString();
            }
        }

        private void negDice_ValueChanged(object sender, EventArgs e)
        {
            neg_total = (int)(negDiceSkull.Value + negDicePush.Value + negDiceDodge.Value + negDiceBlock.Value + negDicePow.Value);
            negTotal.Text = neg_total.ToString();
            updateTotalDices();
        }

        private void oneDice_ValueChanged(object sender, EventArgs e)
        {
            one_total = (int)(oneDiceSkull.Value + oneDicePush.Value + oneDiceDodge.Value + oneDiceBlock.Value + oneDicePow.Value);
            oneTotal.Text = one_total.ToString();
            updateTotalDices();
        }

        private void twoDice_ValueChanged(object sender, EventArgs e)
        {
            two_total = (int)(twoDiceSkull.Value + twoDicePush.Value + twoDiceDodge.Value + twoDiceBlock.Value + twoDicePow.Value);
            twoTotal.Text = two_total.ToString();
            updateTotalDices();
        }

        private void threeDice_ValueChanged(object sender, EventArgs e)
        {
            three_total = (int)(threeDiceSkull.Value + threeDicePush.Value + threeDiceDodge.Value + threeDiceBlock.Value + threeDicePow.Value);
            threeTotal.Text = three_total.ToString();
            updateTotalDices();
        }

        private void updateSkullValues()
        {
            skull_total = (int)(negDiceSkull.Value + oneDiceSkull.Value + twoDiceSkull.Value + threeDiceSkull.Value);
            skullTotal.Text = skull_total.ToString();
            if (dices_total > 0)
            {
                skullPercent.Text = ((int)(((double)skull_total * 100.0) / (double)dices_total)).ToString();
            }
        }

        private void updatePushValues()
        {
            push_total = (int)(negDicePush.Value + oneDicePush.Value + twoDicePush.Value + threeDicePush.Value);
            pushTotal.Text = push_total.ToString();
            if (dices_total > 0)
            {
                pushPercent.Text = ((int)(((double)push_total * 100.0) / (double)dices_total)).ToString();
            }
        }
        private void updateDodgeValues()
        {
            dodge_total = (int)(negDiceDodge.Value + oneDiceDodge.Value + twoDiceDodge.Value + threeDiceDodge.Value);
            dodgeTotal.Text = dodge_total.ToString();
            if (dices_total > 0)
            {
                dodgePercent.Text = ((int)(((double)dodge_total * 100.0) / (double)dices_total)).ToString();
            }
        }

        private void updateBlockValues()
        {
            block_total = (int)(negDiceBlock.Value + oneDiceBlock.Value + twoDiceBlock.Value + threeDiceBlock.Value);
            blockTotal.Text = block_total.ToString();
            if (dices_total > 0)
            {
                blockPercent.Text = ((int)(((double)block_total * 100.0) / (double)dices_total)).ToString();
            }
        }

        private void updatePowValues()
        {
            pow_total = (int)(negDicePow.Value + oneDicePow.Value + twoDicePow.Value + threeDicePow.Value);
            powTotal.Text = pow_total.ToString();
            if (dices_total > 0)
            {
                powPercent.Text = ((int)(((double)pow_total * 100.0) / (double)dices_total)).ToString();
            }
        }
        private void updateBreaks()
        {
            nbreaks = nstuns + nkos + ninjuries;
            totalBreak.Text = nbreaks.ToString();
            if (npows > 0)
            {
                percentBreak.Text = ((int)((double)nbreaks * 100.0 / npows)).ToString();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            npows = (int)pows.Value;
            updateBreaks();
            if (dices_total > 0)
            {
                powsPercent.Text = ((int)(((double)npows * 100.0) / (double)dices_total)).ToString();
            }
        }

        private void stun_ValueChanged(object sender, EventArgs e)
        {
            nstuns = (int)stun.Value;
            updateBreaks();
        }

        private void ko_ValueChanged(object sender, EventArgs e)
        {
            nkos = (int)ko.Value;
            updateBreaks();
        }

        private void injury_ValueChanged(object sender, EventArgs e)
        {
            ninjuries = (int)injury.Value;
            updateBreaks();
        }
        private void pass_ValueChanged(object sender, EventArgs e)
        {
            total_dodge = (int)(failDodge.Value + okDodge.Value);
            totalDodge.Text = total_dodge.ToString();
            if (total_dodge > 0)
            {
                percentDodge.Text = (((int)okDodge.Value * 100) / total_dodge).ToString();
            }
        }
        private void catch_ValueChanged(object sender, EventArgs e)
        {
            total_catch = (int)(failCatch.Value + okCatch.Value);
            totalCatch.Text = total_catch.ToString();
            if (total_catch > 0)
            {
                percentCatch.Text = (((int)okCatch.Value * 100) / total_catch).ToString();
            }
        }
        private void ap_ValueChanged(object sender, EventArgs e)
        {
            total_ap = (int)(failAp.Value + okAp.Value);
            totalAp.Text = total_ap.ToString();
            if (total_ap > 0)
            {
                percentAp.Text = (((int)okAp.Value * 100) / total_ap).ToString();
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            stun.Value++;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ko.Value++;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void negTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void oneTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void twoTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void threeTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void totalDice_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            injury.Value++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            negDiceSkull.Value++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            oneDiceSkull.Value++;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            twoDiceSkull.Value++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            threeDiceSkull.Value++;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            negDicePush.Value++;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            oneDicePush.Value++;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            twoDicePush.Value++;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            threeDicePush.Value++;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            negDiceDodge.Value++;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            oneDiceDodge.Value++;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            twoDiceDodge.Value++;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            threeDiceDodge.Value++;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            negDiceDodge.Value++;
            pows.Value++;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            oneDiceDodge.Value++;
            pows.Value++;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            twoDiceDodge.Value++;
            pows.Value++;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            threeDiceDodge.Value++;
            pows.Value++;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            negDiceBlock.Value++;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            oneDiceBlock.Value++;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            twoDiceBlock.Value++;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            threeDiceBlock.Value++;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            negDiceBlock.Value++;
            pows.Value++;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            oneDiceBlock.Value++;
            pows.Value++;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            twoDiceBlock.Value++;
            pows.Value++;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            threeDiceBlock.Value++;
            pows.Value++;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            negDicePow.Value++;
            pows.Value++;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            oneDicePow.Value++;
            pows.Value++;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            twoDicePow.Value++;
            pows.Value++;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            threeDicePow.Value++;
            pows.Value++;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            okDodge.Value++;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            okCatch.Value++;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            okAp.Value++;
        }
    }
}
