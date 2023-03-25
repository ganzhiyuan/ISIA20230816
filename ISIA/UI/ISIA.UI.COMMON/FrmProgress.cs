using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ISIA.UI.COMMON
{
    public partial class FrmProgress : Form
    {
        //Cancel 버튼이 눌러졌을 경우 처리를 위한 Cancel 이벤트
        public event CancelEventHandler Cancel;

        //Cancel 처리를 위한 델리게이트
        public delegate void CancelEventHandler(object sender, CancelEventArgs e);

        //취소 시 취소 시점까지 진행된 내용을 유지할 것인지 지정하기 위한 이벤트 핸들러
        public class CancelEventArgs : System.EventArgs
        {
            //취소 시점까지 진행된 내용을 유지할 것인지 지정
            public bool ReserveResult = false;
        }

        public FrmProgress()
        {
            InitializeComponent();
            helpTip.SetToolTip(chkReserve, "Specifies whether to show images to be downloaded by the time the user cancels.");
        }

        public FrmProgress(string title, bool canCancel) : this(title, canCancel, true)
        {

        }

        //제목 및 취소 가능 여부를 지정하기 위한 생성자
        public FrmProgress(string title, bool canCancel, bool canReserve) : this()
        {
            if (title != string.Empty)
                this.Text = title;
            
            if(canCancel)
            {
                chkReserve.Visible = true;
                if(canCancel == false)
                {
                    chkReserve.Visible = false;
                    btnCancel.Location = new Point(btnCancel.Location.X, 30);
                    this.Height -= btnCancel.Height;
                }
            }
            else
            {
                btnCancel.Visible = chkReserve.Visible = false;
                this.Height -= btnCancel.Height;
            }
        }

        //Proress 처리 및 메세지 처리를 위한 메소드
        public void SetValue(int value, string message)
        {
            pbProgress.Value = value;
            lblMsg.Text = message;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //현재 작업을 취소하도록 이벤트를 발생한다.
            if(Cancel != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                args.ReserveResult = chkReserve.Checked;
                Cancel(this, args);
            }
        }

        private void FrmProgress_FormClosed(object sender, FormClosedEventArgs e)
        {
            //사용자가 강제로 창을 닫은 경우
            if(e.CloseReason == CloseReason.UserClosing)
            {
                btnCancel_Click(btnCancel, EventArgs.Empty);
            }
        }
    }
}
