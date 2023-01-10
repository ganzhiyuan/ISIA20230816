using System;
using System.Collections.Generic;
using TAP;
namespace ISIA.INTERFACE.ARGUMENTSPACK
{

    [Serializable]
    public class ProcessDailyInputArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();
        #region Fields
        private string _Workshop;
        private string _Shift;
        private string _Process;
        private string _Prod_type;
        private string _Last_qty;
        private string _Last_return_qty;
        private string _In_qty;
        private string _In_other_qty;
        private string _Out_qty;
        private string _Out_other_qty;
        private string _Broken_weight;
        private string _Return_qty;
        private string _Return_target_rate;
        private string _Remain_qty;
        private string _Remain_qualified_qty;
        private string _Remain_return_qty;
        private string _Broken_qty;
        private string _Remark1;
        private string _Remark2;
        private string _In_other_broken_qty;
        private string _Cid;
        private string _Cdt;
        private string _Mid;
        private string _Mdt;
        private string _Report_date;
        private string _Id;
        private string _Approval_flag;
        private string _Apprever;
        private string _Approval_dt;
        private string _Other1_mu_out_qty;
        private string _Other1_mu_sui_qty;
        private string _Other1_lid_test_qty;
        private string _Other1_lid_sui_qty;
        private string _Other1_el_test_qty;
        private string _Other1_el_sui_qty;
        private string _Other2_qumo_qty;
        private string _Other2_no_qumo_qty;
        private string _Test_in_hege_qty;
        private string _Test_in_dixiao_qty;
        private string _Test_in_fm_qty;
        private string _Test_in_irev1shu_qty;
        private string _Test_in_irev1el_qty;
        private string _Test_in_reban_qty;
        private string _Test_in_hpsc_qty;
        private string _Test_in_yc_qty;
        private string _Test_in_qj_qty;
        private string _Test_stock_all_qty;
        private string _Test_stock_dixiao_qty;
        private string _Test_stock_irev1shu_qty;
        private string _Test_stock_irev1el_qty;
        private string _Test_stock_reban_qty;
        private string _Test_stock_hpsc_qty;
        private string _Test_stock_yc_qty;
        private string _Test_stock_qj_qty;
        private string _Test_stock_w_test_qty;
        private string _Test_stock_w_lid_qty;
        private string _Test_stock_w_el_qty;
        private string _Test_stock_w_dian_qty;
        private string _Test_stock_w_dian1_qty;
        private string _Test_stock_w_select_qty;
        private string _Test_stock_w_in_qty;
        private string _Test_stock_w_in1_qty;
        private string _Test_chupan_all_qty;
        private string _Test_chupan_dixiao_qty;
        private string _Test_chupan_fm_qty;
        private string _Test_chupan_irev1shu_qty;
        private string _Test_chupan_irev1el_qty;
        private string _Test_chupan_reban_qty;
        private string _Test_chupan_hpsc_qty;
        private string _Test_chupan_yc_qty;
        private string _Test_chupan_qj_qty;
        private string _Test_chupan_jiangji_qty;
        private string _Test_stock_fm_qty;
        private string _Other1_xiaolv;
        private string _Yici_last_qty;
        private string _Yici_lingliao;
        private string _Yici_return_qty;
        private string _Yici_all_qty;
        private string _In_other_broken_qty1;
        private string _Test_in_return_qty;
        private string _Remain_no_module_qty;
        private string _Test_siyin_return_qty;
        private string _Test_nmr_uncut;
        private string _Test_nmr_cutable;
        private string _Test_stock_uncut;
        private string _Test_stock_cutable;
        private string _DateTimeStart = null;
        private string _DateTimeEnd = null;
        private string _Item_no = null;
        private bool _OpenUpdate = false;
        



        private string _Broken_rate = null;
        private string _Return__rate = null;
        private string _Profit_and_loss = null;
        private List<ProcessDailyInputArgsPack> _processDailyInputArgsPacks = new List<ProcessDailyInputArgsPack>();


        #endregion

        #region Properties

        public string Workshop
        {
            get
            {
                return _Workshop;
            }

            set
            {
                _Workshop = value;
            }
        }

        public string Shift
        {
            get
            {
                return _Shift;
            }

            set
            {
                _Shift = value;
            }
        }

        public string Process
        {
            get
            {
                return _Process;
            }

            set
            {
                _Process = value;
            }
        }

        public string Prod_type
        {
            get
            {
                return _Prod_type;
            }

            set
            {
                _Prod_type = value;
            }
        }

        public string Last_qty
        {
            get
            {
                return _Last_qty;
            }

            set
            {
                _Last_qty = value;
            }
        }

        public string Last_return_qty
        {
            get
            {
                return _Last_return_qty;
            }

            set
            {
                _Last_return_qty = value;
            }
        }

        public string In_qty
        {
            get
            {
                return _In_qty;
            }

            set
            {
                _In_qty = value;
            }
        }

        public string In_other_qty
        {
            get
            {
                return _In_other_qty;
            }

            set
            {
                _In_other_qty = value;
            }
        }

        public string Out_qty
        {
            get
            {
                return _Out_qty;
            }

            set
            {
                _Out_qty = value;
            }
        }

        public string Out_other_qty
        {
            get
            {
                return _Out_other_qty;
            }

            set
            {
                _Out_other_qty = value;
            }
        }

        public string Broken_weight
        {
            get
            {
                return _Broken_weight;
            }

            set
            {
                _Broken_weight = value;
            }
        }

        public string Return_qty
        {
            get
            {
                return _Return_qty;
            }

            set
            {
                _Return_qty = value;
            }
        }

        public string Return_target_rate
        {
            get
            {
                return _Return_target_rate;
            }

            set
            {
                _Return_target_rate = value;
            }
        }

        public string Remain_qty
        {
            get
            {
                return _Remain_qty;
            }

            set
            {
                _Remain_qty = value;
            }
        }

        public string Remain_qualified_qty
        {
            get
            {
                return _Remain_qualified_qty;
            }

            set
            {
                _Remain_qualified_qty = value;
            }
        }

        public string Remain_return_qty
        {
            get
            {
                return _Remain_return_qty;
            }

            set
            {
                _Remain_return_qty = value;
            }
        }

        public string Broken_qty
        {
            get
            {
                return _Broken_qty;
            }

            set
            {
                _Broken_qty = value;
            }
        }

        public string Remark1
        {
            get
            {
                return _Remark1;
            }

            set
            {
                _Remark1 = value;
            }
        }

        public string Remark2
        {
            get
            {
                return _Remark2;
            }

            set
            {
                _Remark2 = value;
            }
        }

        public string In_other_broken_qty
        {
            get
            {
                return _In_other_broken_qty;
            }

            set
            {
                _In_other_broken_qty = value;
            }
        }

        public string Cid
        {
            get
            {
                return _Cid;
            }

            set
            {
                _Cid = value;
            }
        }

        public string Cdt
        {
            get
            {
                return _Cdt;
            }

            set
            {
                _Cdt = value;
            }
        }

        public string Mid
        {
            get
            {
                return _Mid;
            }

            set
            {
                _Mid = value;
            }
        }

        public string Mdt
        {
            get
            {
                return _Mdt;
            }

            set
            {
                _Mdt = value;
            }
        }

        public string Report_date
        {
            get
            {
                return _Report_date;
            }

            set
            {
                _Report_date = value;
            }
        }

        public string Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }

        public string Approval_flag
        {
            get
            {
                return _Approval_flag;
            }

            set
            {
                _Approval_flag = value;
            }
        }

        public string Apprever
        {
            get
            {
                return _Apprever;
            }

            set
            {
                _Apprever = value;
            }
        }

        public string Approval_dt
        {
            get
            {
                return _Approval_dt;
            }

            set
            {
                _Approval_dt = value;
            }
        }

        public string Other1_mu_out_qty
        {
            get
            {
                return _Other1_mu_out_qty;
            }

            set
            {
                _Other1_mu_out_qty = value;
            }
        }

        public string Other1_mu_sui_qty
        {
            get
            {
                return _Other1_mu_sui_qty;
            }

            set
            {
                _Other1_mu_sui_qty = value;
            }
        }

        public string Other1_lid_test_qty
        {
            get
            {
                return _Other1_lid_test_qty;
            }

            set
            {
                _Other1_lid_test_qty = value;
            }
        }

        public string Other1_lid_sui_qty
        {
            get
            {
                return _Other1_lid_sui_qty;
            }

            set
            {
                _Other1_lid_sui_qty = value;
            }
        }

        public string Other1_el_test_qty
        {
            get
            {
                return _Other1_el_test_qty;
            }

            set
            {
                _Other1_el_test_qty = value;
            }
        }

        public string Other1_el_sui_qty
        {
            get
            {
                return _Other1_el_sui_qty;
            }

            set
            {
                _Other1_el_sui_qty = value;
            }
        }

        public string Other2_qumo_qty
        {
            get
            {
                return _Other2_qumo_qty;
            }

            set
            {
                _Other2_qumo_qty = value;
            }
        }

        public string Other2_no_qumo_qty
        {
            get
            {
                return _Other2_no_qumo_qty;
            }

            set
            {
                _Other2_no_qumo_qty = value;
            }
        }

        public string Test_in_hege_qty
        {
            get
            {
                return _Test_in_hege_qty;
            }

            set
            {
                _Test_in_hege_qty = value;
            }
        }

        public string Test_in_dixiao_qty
        {
            get
            {
                return _Test_in_dixiao_qty;
            }

            set
            {
                _Test_in_dixiao_qty = value;
            }
        }

        public string Test_in_fm_qty
        {
            get
            {
                return _Test_in_fm_qty;
            }

            set
            {
                _Test_in_fm_qty = value;
            }
        }

        public string Test_in_irev1shu_qty
        {
            get
            {
                return _Test_in_irev1shu_qty;
            }

            set
            {
                _Test_in_irev1shu_qty = value;
            }
        }

        public string Test_in_irev1el_qty
        {
            get
            {
                return _Test_in_irev1el_qty;
            }

            set
            {
                _Test_in_irev1el_qty = value;
            }
        }

        public string Test_in_reban_qty
        {
            get
            {
                return _Test_in_reban_qty;
            }

            set
            {
                _Test_in_reban_qty = value;
            }
        }

        public string Test_in_hpsc_qty
        {
            get
            {
                return _Test_in_hpsc_qty;
            }

            set
            {
                _Test_in_hpsc_qty = value;
            }
        }

        public string Test_in_yc_qty
        {
            get
            {
                return _Test_in_yc_qty;
            }

            set
            {
                _Test_in_yc_qty = value;
            }
        }

        public string Test_in_qj_qty
        {
            get
            {
                return _Test_in_qj_qty;
            }

            set
            {
                _Test_in_qj_qty = value;
            }
        }

        public string Test_stock_all_qty
        {
            get
            {
                return _Test_stock_all_qty;
            }

            set
            {
                _Test_stock_all_qty = value;
            }
        }

        public string Test_stock_dixiao_qty
        {
            get
            {
                return _Test_stock_dixiao_qty;
            }

            set
            {
                _Test_stock_dixiao_qty = value;
            }
        }

        public string Test_stock_irev1shu_qty
        {
            get
            {
                return _Test_stock_irev1shu_qty;
            }

            set
            {
                _Test_stock_irev1shu_qty = value;
            }
        }

        public string Test_stock_irev1el_qty
        {
            get
            {
                return _Test_stock_irev1el_qty;
            }

            set
            {
                _Test_stock_irev1el_qty = value;
            }
        }

        public string Test_stock_reban_qty
        {
            get
            {
                return _Test_stock_reban_qty;
            }

            set
            {
                _Test_stock_reban_qty = value;
            }
        }

        public string Test_stock_hpsc_qty
        {
            get
            {
                return _Test_stock_hpsc_qty;
            }

            set
            {
                _Test_stock_hpsc_qty = value;
            }
        }

        public string Test_stock_yc_qty
        {
            get
            {
                return _Test_stock_yc_qty;
            }

            set
            {
                _Test_stock_yc_qty = value;
            }
        }

        public string Test_stock_qj_qty
        {
            get
            {
                return _Test_stock_qj_qty;
            }

            set
            {
                _Test_stock_qj_qty = value;
            }
        }

        public string Test_stock_w_test_qty
        {
            get
            {
                return _Test_stock_w_test_qty;
            }

            set
            {
                _Test_stock_w_test_qty = value;
            }
        }

        public string Test_stock_w_lid_qty
        {
            get
            {
                return _Test_stock_w_lid_qty;
            }

            set
            {
                _Test_stock_w_lid_qty = value;
            }
        }

        public string Test_stock_w_el_qty
        {
            get
            {
                return _Test_stock_w_el_qty;
            }

            set
            {
                _Test_stock_w_el_qty = value;
            }
        }

        public string Test_stock_w_dian_qty
        {
            get
            {
                return _Test_stock_w_dian_qty;
            }

            set
            {
                _Test_stock_w_dian_qty = value;
            }
        }

        public string Test_stock_w_dian1_qty
        {
            get
            {
                return _Test_stock_w_dian1_qty;
            }

            set
            {
                _Test_stock_w_dian1_qty = value;
            }
        }

        public string Test_stock_w_select_qty
        {
            get
            {
                return _Test_stock_w_select_qty;
            }

            set
            {
                _Test_stock_w_select_qty = value;
            }
        }

        public string Test_stock_w_in_qty
        {
            get
            {
                return _Test_stock_w_in_qty;
            }

            set
            {
                _Test_stock_w_in_qty = value;
            }
        }

        public string Test_stock_w_in1_qty
        {
            get
            {
                return _Test_stock_w_in1_qty;
            }

            set
            {
                _Test_stock_w_in1_qty = value;
            }
        }

        public string Test_chupan_all_qty
        {
            get
            {
                return _Test_chupan_all_qty;
            }

            set
            {
                _Test_chupan_all_qty = value;
            }
        }

        public string Test_chupan_dixiao_qty
        {
            get
            {
                return _Test_chupan_dixiao_qty;
            }

            set
            {
                _Test_chupan_dixiao_qty = value;
            }
        }

        public string Test_chupan_fm_qty
        {
            get
            {
                return _Test_chupan_fm_qty;
            }

            set
            {
                _Test_chupan_fm_qty = value;
            }
        }

        public string Test_chupan_irev1shu_qty
        {
            get
            {
                return _Test_chupan_irev1shu_qty;
            }

            set
            {
                _Test_chupan_irev1shu_qty = value;
            }
        }

        public string Test_chupan_irev1el_qty
        {
            get
            {
                return _Test_chupan_irev1el_qty;
            }

            set
            {
                _Test_chupan_irev1el_qty = value;
            }
        }

        public string Test_chupan_reban_qty
        {
            get
            {
                return _Test_chupan_reban_qty;
            }

            set
            {
                _Test_chupan_reban_qty = value;
            }
        }

        public string Test_chupan_hpsc_qty
        {
            get
            {
                return _Test_chupan_hpsc_qty;
            }

            set
            {
                _Test_chupan_hpsc_qty = value;
            }
        }

        public string Test_chupan_yc_qty
        {
            get
            {
                return _Test_chupan_yc_qty;
            }

            set
            {
                _Test_chupan_yc_qty = value;
            }
        }

        public string Test_chupan_qj_qty
        {
            get
            {
                return _Test_chupan_qj_qty;
            }

            set
            {
                _Test_chupan_qj_qty = value;
            }
        }

        public string Test_chupan_jiangji_qty
        {
            get
            {
                return _Test_chupan_jiangji_qty;
            }

            set
            {
                _Test_chupan_jiangji_qty = value;
            }
        }

        public string Test_stock_fm_qty
        {
            get
            {
                return _Test_stock_fm_qty;
            }

            set
            {
                _Test_stock_fm_qty = value;
            }
        }

        public string Other1_xiaolv
        {
            get
            {
                return _Other1_xiaolv;
            }

            set
            {
                _Other1_xiaolv = value;
            }
        }

        public string Yici_last_qty
        {
            get
            {
                return _Yici_last_qty;
            }

            set
            {
                _Yici_last_qty = value;
            }
        }

        public string Yici_lingliao
        {
            get
            {
                return _Yici_lingliao;
            }

            set
            {
                _Yici_lingliao = value;
            }
        }

        public string Yici_return_qty
        {
            get
            {
                return _Yici_return_qty;
            }

            set
            {
                _Yici_return_qty = value;
            }
        }

        public string Yici_all_qty
        {
            get
            {
                return _Yici_all_qty;
            }

            set
            {
                _Yici_all_qty = value;
            }
        }

        public string In_other_broken_qty1
        {
            get
            {
                return _In_other_broken_qty1;
            }

            set
            {
                _In_other_broken_qty1 = value;
            }
        }

        public string Test_in_return_qty
        {
            get
            {
                return _Test_in_return_qty;
            }

            set
            {
                _Test_in_return_qty = value;
            }
        }

        public string Remain_no_module_qty
        {
            get
            {
                return _Remain_no_module_qty;
            }

            set
            {
                _Remain_no_module_qty = value;
            }
        }

        public string Test_siyin_return_qty
        {
            get
            {
                return _Test_siyin_return_qty;
            }

            set
            {
                _Test_siyin_return_qty = value;
            }
        }

        public string Test_nmr_uncut
        {
            get
            {
                return _Test_nmr_uncut;
            }

            set
            {
                _Test_nmr_uncut = value;
            }
        }

        public string Test_nmr_cutable
        {
            get
            {
                return _Test_nmr_cutable;
            }

            set
            {
                _Test_nmr_cutable = value;
            }
        }

        public string Test_stock_uncut
        {
            get
            {
                return _Test_stock_uncut;
            }

            set
            {
                _Test_stock_uncut = value;
            }
        }

        public string Test_stock_cutable
        {
            get
            {
                return _Test_stock_cutable;
            }

            set
            {
                _Test_stock_cutable = value;
            }
        }

        public string DateTimeStart
        {
            get
            {
                return _DateTimeStart;
            }

            set
            {
                _DateTimeStart = value;
            }
        }

        public string DateTimeEnd
        {
            get
            {
                return _DateTimeEnd;
            }

            set
            {
                _DateTimeEnd = value;
            }
        }

        public List<ProcessDailyInputArgsPack> ProcessDailyInputArgsPacks
        {
            get
            {
                return _processDailyInputArgsPacks;
            }

            set
            {
                _processDailyInputArgsPacks = value;
            }
        }

        public string Item_no
        {
            get
            {
                return _Item_no;
            }

            set
            {
                _Item_no = value;
            }
        }

        public string Broken_rate
        {
            get
            {
                return _Broken_rate;
            }

            set
            {
                _Broken_rate = value;
            }
        }

        public string Return__rate
        {
            get
            {
                return _Return__rate;
            }

            set
            {
                _Return__rate = value;
            }
        }

        public string Profit_and_loss
        {
            get
            {
                return _Profit_and_loss;
            }

            set
            {
                _Profit_and_loss = value;
            }
        }

        public bool OpenUpdate
        {
            get
            {
                return _OpenUpdate;
            }

            set
            {
                _OpenUpdate = value;
            }
        }


        #endregion

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(ProcessDailyInputArgsPack), this);
            return argsPack;
        }
    }
}
