using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace twitchBot
{
    public class ChibiController
    {
        private PictureBox pictureBox;
        private Form form;
        public bool boolHeadwear = true;
        public bool boolRightSide = false;

        public ChibiController(PictureBox pictureBox,Form form)
        {
            this.pictureBox = pictureBox;
            this.form=form;
        }

        public void ChibiReset()
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.Invoke(new Action(ChibiReset));
            }
            else
            {
                //變更圖片
                if(boolHeadwear)
                {
                    pictureBox.Image = Image.FromFile(@"..\..\image\chibiWait.gif");
                }
                else
                {
                    pictureBox.Image = Image.FromFile(@"..\..\image\chibiWait_NH.gif");
                }
            }
        }
        public void ChibiWalk()
        {
            if(pictureBox.InvokeRequired)
            {
                pictureBox.Invoke(new Action(ChibiWalk));
            }
            else
            {
                if (pictureBox.Location.X < 20 && !boolRightSide)
                {
                    boolRightSide=true;
                }
                else if(pictureBox.Location.X + pictureBox.Width > form.Width-20 && boolRightSide)
                {
                    boolRightSide = false;
                }
                if (boolHeadwear)
                {
                    if(boolRightSide)
                    {
                        pictureBox.Image = Image.FromFile(@"..\..\image\chibiwalkR.gif");
                    }
                    else
                    {
                        pictureBox.Image = Image.FromFile(@"..\..\image\chibiwalk.gif");
                    }
                }
                else
                {
                    if(boolRightSide)
                    {
                        pictureBox.Image = Image.FromFile(@"..\..\image\chibiwalkR_NH.gif");
                    }
                    else
                    {
                        pictureBox.Image = Image.FromFile(@"..\..\image\chibiwalk_NH.gif");
                    }
                }
                
            }
        }
        public void ChibiConfuse()
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.Invoke(new Action(ChibiConfuse));
            }
            else
            {
                if (boolHeadwear)
                {
                    pictureBox.Image = Image.FromFile(@"..\..\image\chibiConfuse.gif");
                }
                else
                {
                    pictureBox.Image = Image.FromFile(@"..\..\image\chibiConfuse_NH.gif");
                }
            }
        }
        public void ChibiHeadwear()
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.Invoke(new Action(ChibiHeadwear));
            }
            else
            {
                if (boolHeadwear)
                {
                    pictureBox.Image = Image.FromFile(@"..\..\image\chibiNoHeadwear.gif");
                    boolHeadwear = false;
                }
                else
                {
                    pictureBox.Image = Image.FromFile(@"..\..\image\chibiHeadwear.gif");
                    boolHeadwear = true;
                }
            }
        }
    }
}
