using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace BB2Stats
{
    public partial class OverlayTeam : Form
    {
        class DiceResult
        {
            public int neg_dice = 0;
            public int one_dice = 0;
            public int two_dice = 0;
            public int three_dice = 0;
        };

        class SkillResult
        {
            public int fail_count = 0;
            public int ok_count = 0;
        }

        class Data
        {
            public DiceResult skull = new DiceResult();
            public DiceResult push = new DiceResult();
            public DiceResult dodge = new DiceResult();
            public DiceResult block = new DiceResult();
            public DiceResult pow = new DiceResult();

            public int neg_total = 0;
            public int one_total = 0;
            public int two_total = 0;
            public int three_total = 0;

            public int skull_total = 0;
            public int push_total = 0;
            public int dodge_total = 0;
            public int block_total = 0;
            public int pow_total = 0;

            public int dices_total = 0;

            public int npows = 0;
            
            public int nstuns = 0;
            public int nkos = 0;
            public int ninjuries = 0;
            
            public int nbreaks = 0;

            public SkillResult dodge_skill = new SkillResult();
            public SkillResult catch_skill = new SkillResult();
            public SkillResult ap_skill = new SkillResult();

            public int total_dodge = 0;
            public int total_catch = 0;
            public int total_ap = 0;
        }

        Data data = new Data();
        PrivateFontCollection pfc = new PrivateFontCollection();
        int teamNum = 0;
        Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
        public OverlayTeam(int n)
        {
            InitializeComponent();
            teamNum = n;
            switch (teamNum)
            {
                case 1:
                    {
                        var temp = this.Location;
                        temp.X = -(this.Width);
                        this.Location = temp;
                    }
                    break;
                case 2:
                    {
                        var temp = this.Location;
                        temp.X = screenBounds.X + screenBounds.Width;
                        this.Location = temp;
                    }
                    break;
            }
            pfc.AddFontFile(".\\Nuffle.ttf");
            pfc.AddFontFile(".\\NuffleDice.ttf");
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
            label3.Parent = pictureBox1;
            label3.BackColor = Color.Transparent;
            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;
            label7.Parent = pictureBox1;
            label7.BackColor = Color.Transparent;
            label17.Parent = pictureBox1;
            label17.BackColor = Color.Transparent;
            label18.Parent = pictureBox1;
            label18.BackColor = Color.Transparent;
            label19.Parent = pictureBox1;
            label19.BackColor = Color.Transparent;
            label20.Parent = pictureBox1;
            label20.BackColor = Color.Transparent;
            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;
            label41.Parent = pictureBox1;
            label41.BackColor = Color.Transparent;
            label50.Parent = pictureBox1;
            label50.BackColor = Color.Transparent;
            label1.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label2.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label3.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label4.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label7.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label17.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label18.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label19.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label20.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label50.Font = new System.Drawing.Font(pfc.Families[1], 40, FontStyle.Regular);
            label4.Font = new System.Drawing.Font(pfc.Families[0], 16, FontStyle.Regular);
            label41.Font = new System.Drawing.Font(pfc.Families[0], 16, FontStyle.Regular);

            setupLabel(negDiceSkullLabel, pfc.Families[0], 25);
            setupLabel(oneDiceSkullLabel, pfc.Families[0], 25);
            setupLabel(twoDiceSkullLabel, pfc.Families[0], 25);
            setupLabel(threeDiceSkullLabel, pfc.Families[0], 25);
            setupLabel(totalDiceSkullLabel, pfc.Families[0], 25);

            setupLabel(negDicePushLabel, pfc.Families[0], 25);
            setupLabel(oneDicePushLabel, pfc.Families[0], 25);
            setupLabel(twoDicePushLabel, pfc.Families[0], 25);
            setupLabel(threeDicePushLabel, pfc.Families[0], 25);
            setupLabel(totalDicePushLabel, pfc.Families[0], 25);

            setupLabel(negDiceDodgeLabel, pfc.Families[0], 25);
            setupLabel(oneDiceDodgeLabel, pfc.Families[0], 25);
            setupLabel(twoDiceDodgeLabel, pfc.Families[0], 25);
            setupLabel(threeDiceDodgeLabel, pfc.Families[0], 25);
            setupLabel(totalDiceDodgeLabel, pfc.Families[0], 25);

            setupLabel(negDiceBlockLabel, pfc.Families[0], 25);
            setupLabel(oneDiceBlockLabel, pfc.Families[0], 25);
            setupLabel(twoDiceBlockLabel, pfc.Families[0], 25);
            setupLabel(threeDiceBlockLabel, pfc.Families[0], 25);
            setupLabel(totalDiceBlockLabel, pfc.Families[0], 25);

            setupLabel(negDicePowLabel, pfc.Families[0], 25);
            setupLabel(oneDicePowLabel, pfc.Families[0], 25);
            setupLabel(twoDicePowLabel, pfc.Families[0], 25);
            setupLabel(threeDicePowLabel, pfc.Families[0], 25);
            setupLabel(totalDicePowLabel, pfc.Families[0], 25);

            setupLabel(negDiceLabel, pfc.Families[0], 25);
            setupLabel(oneDiceLabel, pfc.Families[0], 25);
            setupLabel(twoDiceLabel, pfc.Families[0], 25);
            setupLabel(threeDiceLabel, pfc.Families[0], 25);
            setupLabel(totalDiceLabel, pfc.Families[0], 25);

            setupLabel(label21, pfc.Families[0], 16);
            setupLabel(breaks, pfc.Families[0], 25);
            setupLabel(stunsLabel, pfc.Families[0], 25);
            setupLabel(kosLabel, pfc.Families[0], 25);
            setupLabel(injuryLabel, pfc.Families[0], 25);
            setupLabel(dodgeLabel, pfc.Families[0], 25);
            setupLabel(catchLabel, pfc.Families[0], 25);
            setupLabel(apLabel, pfc.Families[0], 25);
    
        }

        bool fadeIn = false;
        bool fadeOut = false; 

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (fadeIn)
            {
                if (teamNum == 1)
                {
                    if (this.Location.X < 150)
                    {
                        int diff = (150 - this.Location.X) / 3;
                        if (diff < 5) { diff = 5; }
                        var temp = this.Location;
                        temp.X += diff;
                        this.Location = temp;
                    }
                    else
                    {
                        fadeIn = false;
                    }
                }else if (teamNum == 2)
                {
                    int target_pos = screenBounds.X + screenBounds.Width - 150 - this.Width;
                    if (this.Location.X > target_pos)
                    {
                        int diff = (this.Location.X - target_pos) / 3;
                        if (diff < 5) { diff = 5; }
                        var temp = this.Location;
                        temp.X -= diff;
                        this.Location = temp;
                    }
                    else
                    {
                        fadeIn = false;
                    }
                }
            }else if (fadeOut)
            {
                if (teamNum == 1)
                {
                    if (this.Location.X + this.Width > 0)
                    {
                        int target_pos = -this.Width;
                        int diff = (this.Location.X - target_pos) / 3;
                        if (diff < 5) { diff = 5; }
                        var temp = this.Location;
                        temp.X -= diff;
                        this.Location = temp;
                    }
                    else
                    {
                        fadeOut = false;
                    }
                }
                else if (teamNum == 2)
                {
                    int target_pos = screenBounds.X + screenBounds.Width;
                    if (this.Location.X < target_pos)
                    {
                        int diff = (target_pos - this.Location.X) / 3;
                        if (diff < 5) { diff = 5; }
                        var temp = this.Location;
                        temp.X += diff;
                        this.Location = temp;
                    }
                    else
                    {
                        fadeOut = false;
                    }
                }
            }
            else
            {
                timer1.Stop();
            }
        }

        public void FadeIn()
        {           
            fadeIn = true;
            timer1.Start();
        }

        public void FadeOut()
        {
            fadeOut = true;
            timer1.Start();
        }

        public void FaddToggle()
        {
            if (teamNum == 1)
            {
                if (this.Location.X > 0)
                {
                    FadeOut();
                }
                else
                {
                    FadeIn();
                }
            }else if (teamNum == 2)
            {
                if (this.Location.X < screenBounds.X + screenBounds.Width)
                {
                    FadeOut();
                }
                else
                {
                    FadeIn();
                }
            }
        }

        public void setupLabel(Label label, FontFamily font, int size)
        {
            label.Parent = pictureBox1;
            label.BackColor = Color.Transparent;
            label.Font = new System.Drawing.Font(font, size, FontStyle.Regular);
        }

        public string toJson()
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public void fromJson(string json)
        {
            data = JsonConvert.DeserializeObject<Data>(json);
            negDiceSkull.Value = data.skull.neg_dice;
            oneDiceSkull.Value = data.skull.one_dice;
            twoDiceSkull.Value = data.skull.two_dice;
            threeDiceSkull.Value = data.skull.three_dice;
            negDicePush.Value = data.push.neg_dice;
            oneDicePush.Value = data.push.one_dice;
            twoDicePush.Value = data.push.two_dice;
            threeDicePush.Value = data.push.three_dice;
            negDiceDodge.Value = data.dodge.neg_dice;
            oneDiceDodge.Value = data.dodge.one_dice;
            twoDiceDodge.Value = data.dodge.two_dice;
            threeDiceDodge.Value = data.dodge.three_dice;
            negDiceBlock.Value = data.block.neg_dice;
            oneDiceBlock.Value = data.block.one_dice;
            twoDiceBlock.Value = data.block.two_dice;
            threeDiceBlock.Value = data.block.three_dice;
            negDicePow.Value = data.pow.neg_dice;
            oneDicePow.Value = data.pow.one_dice;
            twoDicePow.Value = data.pow.two_dice;
            threeDicePow.Value = data.pow.three_dice;
          
            failDodge.Value = data.dodge_skill.fail_count;
            okDodge.Value = data.dodge_skill.ok_count;
            failCatch.Value = data.catch_skill.fail_count;
            okCatch.Value = data.catch_skill.ok_count;
            failAp.Value = data.ap_skill.fail_count;
            okAp.Value = data.ap_skill.ok_count;
            stun.Value = data.nstuns;
            ko.Value = data.nkos;
            injury.Value = data.ninjuries;
            pows.Value = data.npows;

            negDiceSkullLabel.Text = negDiceSkull.Value.ToString();
            oneDiceSkullLabel.Text = oneDiceSkull.Value.ToString();
            twoDiceSkullLabel.Text = twoDiceSkull.Value.ToString();
            threeDiceSkullLabel.Text = threeDiceSkull.Value.ToString();
            totalDiceSkullLabel.Text = skullTotal.Text;

            negDicePushLabel.Text = negDicePush.Value.ToString();
            oneDicePushLabel.Text = oneDicePush.Value.ToString();
            twoDicePushLabel.Text = twoDicePush.Value.ToString();
            threeDicePushLabel.Text = threeDicePush.Value.ToString();
            totalDicePushLabel.Text = pushTotal.Text;

            negDiceDodgeLabel.Text = negDiceDodge.Value.ToString();
            oneDiceDodgeLabel.Text = oneDiceDodge.Value.ToString();
            twoDiceDodgeLabel.Text = twoDiceDodge.Value.ToString();
            threeDiceDodgeLabel.Text = threeDiceDodge.Value.ToString();
            totalDiceDodgeLabel.Text = dodgeTotal.Text;

            negDiceBlockLabel.Text = negDiceBlock.Value.ToString();
            oneDiceBlockLabel.Text = oneDiceBlock.Value.ToString();
            twoDiceBlockLabel.Text = twoDiceBlock.Value.ToString();
            threeDiceBlockLabel.Text = threeDiceBlock.Value.ToString();
            totalDiceBlockLabel.Text = blockTotal.Text;

            negDicePowLabel.Text = negDicePow.Value.ToString();
            oneDicePowLabel.Text = oneDicePow.Value.ToString();
            twoDicePowLabel.Text = twoDicePow.Value.ToString();
            threeDicePowLabel.Text = threeDicePow.Value.ToString();
            totalDicePowLabel.Text = powTotal.Text;

            negDiceLabel.Text = negTotal.Text;
            oneDiceLabel.Text = oneTotal.Text;
            twoDiceLabel.Text = twoTotal.Text;
            threeDiceLabel.Text = threeTotal.Text;
            totalDiceLabel.Text = pows.Value.ToString() + " / " + totalDice.Text;

            breaks.Text = totalBreak.Text + " / " + pows.Value.ToString();
            stunsLabel.Text = stun.Value.ToString();
            kosLabel.Text = ko.Value.ToString();
            injuryLabel.Text = injury.Value.ToString();

            dodgeLabel.Text = okDodge.Value.ToString() + " / " + totalDodge.Text;
            catchLabel.Text = okCatch.Value.ToString() + " / " + totalCatch.Text;
            apLabel.Text = okAp.Value.ToString() + " / " + totalAp.Text;
        }

        private void updateTotalDices()
        {
            data.dices_total = data.neg_total + data.one_total + data.two_total + data.three_total;
            updateSkullValues();
            updatePushValues();
            updateDodgeValues();
            updateBlockValues();
            updatePowValues();

            if (data.neg_total > 0)
            {
                negPercentSkull.Text = ((int)((double)negDiceSkull.Value * 100.0 / (double)data.neg_total)).ToString();
                negPercentPush.Text = ((int)((double)negDicePush.Value * 100.0 / (double)data.neg_total)).ToString();
                negPercentDodge.Text = ((int)((double)negDiceDodge.Value * 100.0 / (double)data.neg_total)).ToString();
                negPercentBlock.Text = ((int)((double)negDiceBlock.Value * 100.0 / (double)data.neg_total)).ToString();
                negPercentPow.Text = ((int)((double)negDicePow.Value * 100.0 / (double)data.neg_total)).ToString();
            }
            if (data.one_total > 0)
            {
                onePercentSkull.Text = ((int)((double)oneDiceSkull.Value * 100.0 / (double)data.one_total)).ToString();
                onePercentPush.Text = ((int)((double)oneDicePush.Value * 100.0 / (double)data.one_total)).ToString();
                onePercentDodge.Text = ((int)((double)oneDiceDodge.Value * 100.0 / (double)data.one_total)).ToString();
                onePercentBlock.Text = ((int)((double)oneDiceBlock.Value * 100.0 / (double)data.one_total)).ToString();
                onePercentPow.Text = ((int)((double)oneDicePow.Value * 100.0 / (double)data.one_total)).ToString();
            }
            if (data.two_total > 0)
            {
                twoPercentSkull.Text = ((int)((double)twoDiceSkull.Value * 100.0 / (double)data.two_total)).ToString();
                twoPercentPush.Text = ((int)((double)twoDicePush.Value * 100.0 / (double)data.two_total)).ToString();
                twoPercentDodge.Text = ((int)((double)twoDiceDodge.Value * 100.0 / (double)data.two_total)).ToString();
                twoPercentBlock.Text = ((int)((double)twoDiceBlock.Value * 100.0 / (double)data.two_total)).ToString();
                twoPercentPow.Text = ((int)((double)twoDicePow.Value * 100.0 / (double)data.two_total)).ToString();
            }
            if (data.three_total > 0)
            {
                threePercentSkull.Text = ((int)((double)threeDiceSkull.Value * 100.0 / (double)data.three_total)).ToString();
                threePercentPush.Text = ((int)((double)threeDicePush.Value * 100.0 / (double)data.three_total)).ToString();
                threePercentDodge.Text = ((int)((double)threeDiceDodge.Value * 100.0 / (double)data.three_total)).ToString();
                threePercentBlock.Text = ((int)((double)threeDiceBlock.Value * 100.0 / (double)data.three_total)).ToString();
                threePercentPow.Text = ((int)((double)threeDicePow.Value * 100.0 / (double)data.three_total)).ToString();
            }
            totalDice.Text = data.dices_total.ToString();
            if (data.dices_total > 0)
            {
                powsPercent.Text = ((int)(((double)data.npows * 100.0) / (double)data.dices_total)).ToString();
            }
        }

        private void negDice_ValueChanged(object sender, EventArgs e)
        {
            data.neg_total = (int)(negDiceSkull.Value + negDicePush.Value + negDiceDodge.Value + negDiceBlock.Value + negDicePow.Value);
            negTotal.Text = data.neg_total.ToString();
            updateTotalDices();
        }

        private void oneDice_ValueChanged(object sender, EventArgs e)
        {
            data.one_total = (int)(oneDiceSkull.Value + oneDicePush.Value + oneDiceDodge.Value + oneDiceBlock.Value + oneDicePow.Value);
            oneTotal.Text = data.one_total.ToString();
            updateTotalDices();
        }

        private void twoDice_ValueChanged(object sender, EventArgs e)
        {
            data.two_total = (int)(twoDiceSkull.Value + twoDicePush.Value + twoDiceDodge.Value + twoDiceBlock.Value + twoDicePow.Value);
            twoTotal.Text = data.two_total.ToString();
            updateTotalDices();
        }

        private void threeDice_ValueChanged(object sender, EventArgs e)
        {
            data.three_total = (int)(threeDiceSkull.Value + threeDicePush.Value + threeDiceDodge.Value + threeDiceBlock.Value + threeDicePow.Value);
            threeTotal.Text = data.three_total.ToString();
            updateTotalDices();
        }

        private void updateSkullValues()
        {
            data.skull.neg_dice = (int)(negDiceSkull.Value);
            data.skull.one_dice = (int)(oneDiceSkull.Value);
            data.skull.two_dice = (int)(twoDiceSkull.Value);
            data.skull.three_dice = (int)(threeDiceSkull.Value);
            data.skull_total = (int)(negDiceSkull.Value + oneDiceSkull.Value + twoDiceSkull.Value + threeDiceSkull.Value);
            skullTotal.Text = data.skull_total.ToString();
            if (data.dices_total > 0)
            {
                skullPercent.Text = ((int)(((double)data.skull_total * 100.0) / (double)data.dices_total)).ToString();
            }
        }

        private void updatePushValues()
        {
            data.push.neg_dice = (int)(negDicePush.Value);
            data.push.one_dice = (int)(oneDicePush.Value);
            data.push.two_dice = (int)(twoDicePush.Value);
            data.push.three_dice = (int)(threeDicePush.Value);
            data.push_total = (int)(negDicePush.Value + oneDicePush.Value + twoDicePush.Value + threeDicePush.Value);
            pushTotal.Text = data.push_total.ToString();
            if (data.dices_total > 0)
            {
                pushPercent.Text = ((int)(((double)data.push_total * 100.0) / (double)data.dices_total)).ToString();
            }
        }
        private void updateDodgeValues()
        {
            data.dodge.neg_dice = (int)(negDiceDodge.Value);
            data.dodge.one_dice = (int)(oneDiceDodge.Value);
            data.dodge.two_dice = (int)(twoDiceDodge.Value);
            data.dodge.three_dice = (int)(threeDiceDodge.Value);
            data.dodge_total = (int)(negDiceDodge.Value + oneDiceDodge.Value + twoDiceDodge.Value + threeDiceDodge.Value);
            dodgeTotal.Text = data.dodge_total.ToString();
            if (data.dices_total > 0)
            {
                dodgePercent.Text = ((int)(((double)data.dodge_total * 100.0) / (double)data.dices_total)).ToString();
            }
        }

        private void updateBlockValues()
        {
            data.block.neg_dice = (int)(negDiceBlock.Value);
            data.block.one_dice = (int)(oneDiceBlock.Value);
            data.block.two_dice = (int)(twoDiceBlock.Value);
            data.block.three_dice = (int)(threeDiceBlock.Value);
            data.block_total = (int)(negDiceBlock.Value + oneDiceBlock.Value + twoDiceBlock.Value + threeDiceBlock.Value);
            blockTotal.Text = data.block_total.ToString();
            if (data.dices_total > 0)
            {
                blockPercent.Text = ((int)(((double)data.block_total * 100.0) / (double)data.dices_total)).ToString();
            }
        }
        private void updatePowValues()
        {
            data.pow.neg_dice = (int)(negDicePow.Value);
            data.pow.one_dice = (int)(oneDicePow.Value);
            data.pow.two_dice = (int)(twoDicePow.Value);
            data.pow.three_dice = (int)(threeDicePow.Value);
            data.pow_total = (int)(negDicePow.Value + oneDicePow.Value + twoDicePow.Value + threeDicePow.Value);
            powTotal.Text = data.pow_total.ToString();
            if (data.dices_total > 0)
            {
                powPercent.Text = ((int)(((double)data.pow_total * 100.0) / (double)data.dices_total)).ToString();
            }
        }
        private void updateBreaks()
        {
            data.nbreaks = data.nstuns + data.nkos + data.ninjuries;
            totalBreak.Text = data.nbreaks.ToString();
            if (data.npows > 0)
            {
                percentBreak.Text = ((int)((double)data.nbreaks * 100.0 / data.npows)).ToString();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            data.npows = (int)pows.Value;
            updateBreaks();
            if (data.dices_total > 0)
            {
                powsPercent.Text = ((int)(((double)data.npows * 100.0) / (double)data.dices_total)).ToString();
            }
        }

        private void stun_ValueChanged(object sender, EventArgs e)
        {
            data.nstuns = (int)stun.Value;
            updateBreaks();
        }

        private void ko_ValueChanged(object sender, EventArgs e)
        {
            data.nkos = (int)ko.Value;
            updateBreaks();
        }

        private void injury_ValueChanged(object sender, EventArgs e)
        {
            data.ninjuries = (int)injury.Value;
            updateBreaks();
        }
        private void pass_ValueChanged(object sender, EventArgs e)
        {
            data.dodge_skill.fail_count = (int)(failDodge.Value);
            data.dodge_skill.ok_count = (int)(okDodge.Value);
            data.total_dodge = (int)(failDodge.Value + okDodge.Value);
            totalDodge.Text = data.total_dodge.ToString();
            if (data.total_dodge > 0)
            {
                percentDodge.Text = (((int)okDodge.Value * 100) / data.total_dodge).ToString();
            }
        }
        private void catch_ValueChanged(object sender, EventArgs e)
        {
            data.catch_skill.fail_count = (int)(failCatch.Value);
            data.catch_skill.ok_count = (int)(okCatch.Value);
            data.total_catch = (int)(failCatch.Value + okCatch.Value);
            totalCatch.Text = data.total_catch.ToString();
            if (data.total_catch > 0)
            {
                percentCatch.Text = (((int)okCatch.Value * 100) / data.total_catch).ToString();
            }
        }
        private void ap_ValueChanged(object sender, EventArgs e)
        {
            data.ap_skill.fail_count = (int)(failAp.Value);
            data.ap_skill.ok_count = (int)(okAp.Value);
            data.total_ap = (int)(failAp.Value + okAp.Value);
            totalAp.Text = data.total_ap.ToString();
            if (data.total_ap > 0)
            {
                percentAp.Text = (((int)okAp.Value * 100) / data.total_ap).ToString();
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
