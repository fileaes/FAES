using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAES_GUI.CustomControls
{
    [DefaultEvent("Click")]
    public partial class SubMenuButton : UserControl
    {
        protected SubMenuButton[] _buttons;
        public SubMenuButton()
        {
            InitializeComponent();
            WireAllControls(this);
        }

        public void registerDetoggles(SubMenuButton[] buttons)
        {
            _buttons = buttons;
        }

        private void WireAllControls(Control cont)
        {
            foreach (Control ctl in cont.Controls)
            {
                ctl.Click += buttonLabel_Click;
                if (ctl.HasChildren)
                {
                    WireAllControls(ctl);
                }
            }
        }

        private void setColors(Control cont, Color color)
        {
            this.BackColor = color;
            foreach (Control ctl in cont.Controls)
            {
                if (ctl != selectedIndicator || selectedIndicator.BackColor != Color.DarkBlue) ctl.BackColor = color;
                if (ctl.HasChildren)
                {
                    setColors(ctl, color);
                }
            }
        }

        private void buttonLabel_MouseEnter(object sender, EventArgs e)
        {
            setColors(this, Color.Silver);
        }

        private void buttonLabel_MouseHover(object sender, EventArgs e)
        {
            setColors(this, Color.Silver);
        }

        private void buttonLabel_MouseLeave(object sender, EventArgs e)
        {
            setColors(this, Color.Transparent);
        }

        private void buttonLabel_MouseDown(object sender, MouseEventArgs e)
        {
            setColors(this, Color.DimGray);
        }

        private void buttonLabel_MouseUp(object sender, MouseEventArgs e)
        {
            setColors(this, Color.Silver);
        }

        private void buttonLabel_Click(object sender, EventArgs e)
        {
            Selected = true;

            foreach (SubMenuButton button in _buttons)
                button.Selected = false;

            this.InvokeOnClick(this, EventArgs.Empty);
        }

        [Description("If the button is selected"), Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Selected
        {
            get
            {
                if (selectedIndicator.BackColor == Color.DarkBlue) return true;
                else return false;
            }
            set
            {
                if (value) selectedIndicator.BackColor = Color.DarkBlue;
                else selectedIndicator.BackColor = Color.Transparent;
            }
        }
        [Description("Text displayed on the button"), Category("Data"), Browsable(true), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                return buttonLabel.Text;
            }
            set
            {
                buttonLabel.Text = value;
            }
        }
    }
}
