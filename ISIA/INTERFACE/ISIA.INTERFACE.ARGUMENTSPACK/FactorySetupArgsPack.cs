using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{

    [Serializable]
    public class FactorySetupArgsPack
    {
        private ArgumentPack argsPack = new ArgumentPack();

        #region Fields
        private string _Name = null;
        private string _Description = null;
        private string _Factory_Type = null;
        private int _Factory_Sequence = 0;
        private string _Equipment_Use_Yn = null;
        private int _Days_Per_Week = 0;
        private int _Hours_Per_Day = 0;
        private string _Shift1_Start_Time = null;
        private string _Shift2_Start_Time = null;
        private string _Shift3_Start_Time = null;
        private string _Shift4_Start_Time = null;
        private string _Device_Group1 = null;
        private string _Device_Group2 = null;
        private string _Device_Group3 = null;
        private string _Device_Group4 = null;
        private string _Device_Group5 = null;
        private string _Operation_Group1 = null;
        private string _Operation_Group2 = null;
        private string _Operation_Group3 = null;
        private string _Operation_Group4 = null;
        private string _Operation_Group5 = null;
        private string _Flow_Group1 = null;
        private string _Flow_Group2 = null;
        private string _Flow_Group3 = null;
        private string _Flow_Group4 = null;
        private string _Flow_Group5 = null;
        private string _Bom_Group1 = null;
        private string _Bom_Group2 = null;
        private string _Bom_Group3 = null;
        private string _Bom_Group4 = null;
        private string _Bom_Group5 = null;
        private string _Equipment_Group1 = null;
        private string _Equipment_Group2 = null;
        private string _Equipment_Group3 = null;
        private string _Equipment_Group4 = null;
        private string _Equipment_Group5 = null;
        private string _Equipment_Prompt1 = null;
        private string _Equipment_Prompt2 = null;
        private string _Equipment_Prompt3 = null;
        private string _Equipment_Prompt4 = null;
        private string _Equipment_Prompt5 = null;
        private string _Equipment_Prompt6 = null;
        private string _Equipment_Prompt7 = null;
        private string _Equipment_Prompt8 = null;
        private string _Equipment_Prompt9 = null;
        private string _Equipment_Prompt10 = null;
        private string _Reserved_Field1 = null;
        private string _Reserved_Field2 = null;
        private string _Reserved_Field3 = null;
        private string _Reserved_Field4 = null;
        private string _Reserved_Field5 = null;
        private string _Update_Time = null;
        private string _Update_User = null;
        private List<FactorySetupArgsPack> _FacArgsPacks = new List<FactorySetupArgsPack>();
        #endregion

        #region Properties
        public string Name { get { return _Name; } set { _Name = value; } }
        public int Factory_Sequence { get { return _Factory_Sequence; } set { _Factory_Sequence = value; } }
        public string Equipment_Use_Yn { get { return _Equipment_Use_Yn; } set { _Equipment_Use_Yn = value; } }
        public int Days_Per_Week { get { return _Days_Per_Week; } set { _Days_Per_Week = value; } }
        public int Hours_Per_Day { get { return _Hours_Per_Day; } set { _Hours_Per_Day = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        public string Factory_Type { get { return _Factory_Type; } set { _Factory_Type = value; } }
        public string Shift1_Start_Time { get { return _Shift1_Start_Time; } set { _Shift1_Start_Time = value; } }
        public string Shift2_Start_Time { get { return _Shift2_Start_Time; } set { _Shift2_Start_Time = value; } }
        public string Shift3_Start_Time { get { return _Shift3_Start_Time; } set { _Shift3_Start_Time = value; } }
        public string Shift4_Start_Time { get { return _Shift4_Start_Time; } set { _Shift4_Start_Time = value; } }
        public string Device_Group1 { get { return _Device_Group1; } set { _Device_Group1 = value; } }
        public string Device_Group2 { get { return _Device_Group2; } set { _Device_Group2 = value; } }
        public string Device_Group3 { get { return _Device_Group3; } set { _Device_Group3 = value; } }
        public string Device_Group4 { get { return _Device_Group4; } set { _Device_Group4 = value; } }
        public string Device_Group5 { get { return _Device_Group5; } set { _Device_Group5 = value; } }
        public string Operation_Group1 { get { return _Operation_Group1; } set { _Operation_Group1 = value; } }
        public string Operation_Group2 { get { return _Operation_Group2; } set { _Operation_Group2 = value; } }
        public string Operation_Group3 { get { return _Operation_Group3; } set { _Operation_Group3 = value; } }
        public string Operation_Group4 { get { return _Operation_Group4; } set { _Operation_Group4 = value; } }
        public string Operation_Group5 { get { return _Operation_Group5; } set { _Operation_Group5 = value; } }
        public string Flow_Group1 { get { return _Flow_Group1; } set { _Flow_Group1 = value; } }
        public string Flow_Group2 { get { return _Flow_Group2; } set { _Flow_Group2 = value; } }
        public string Flow_Group3 { get { return _Flow_Group3; } set { _Flow_Group3 = value; } }
        public string Flow_Group4 { get { return _Flow_Group4; } set { _Flow_Group4 = value; } }
        public string Flow_Group5 { get { return _Flow_Group5; } set { _Flow_Group5 = value; } }
        public string Bom_Group1 { get { return _Bom_Group1; } set { _Bom_Group1 = value; } }
        public string Bom_Group2 { get { return _Bom_Group2; } set { _Bom_Group2 = value; } }
        public string Bom_Group3 { get { return _Bom_Group3; } set { _Bom_Group3 = value; } }
        public string Bom_Group4 { get { return _Bom_Group4; } set { _Bom_Group4 = value; } }
        public string Bom_Group5 { get { return _Bom_Group5; } set { _Bom_Group5 = value; } }
        public string Equipment_Group1 { get { return _Equipment_Group1; } set { _Equipment_Group1 = value; } }
        public string Equipment_Group2 { get { return _Equipment_Group2; } set { _Equipment_Group2 = value; } }
        public string Equipment_Group3 { get { return _Equipment_Group3; } set { _Equipment_Group3 = value; } }
        public string Equipment_Group4 { get { return _Equipment_Group4; } set { _Equipment_Group4 = value; } }
        public string Equipment_Group5 { get { return _Equipment_Group5; } set { _Equipment_Group5 = value; } }
        public string Equipment_Prompt1 { get { return _Equipment_Prompt1; } set { _Equipment_Prompt1 = value; } }
        public string Equipment_Prompt2 { get { return _Equipment_Prompt2; } set { _Equipment_Prompt2 = value; } }
        public string Equipment_Prompt3 { get { return _Equipment_Prompt3; } set { _Equipment_Prompt3 = value; } }
        public string Equipment_Prompt4 { get { return _Equipment_Prompt4; } set { _Equipment_Prompt4 = value; } }
        public string Equipment_Prompt5 { get { return _Equipment_Prompt5; } set { _Equipment_Prompt5 = value; } }
        public string Equipment_Prompt6 { get { return _Equipment_Prompt6; } set { _Equipment_Prompt6 = value; } }
        public string Equipment_Prompt7 { get { return _Equipment_Prompt7; } set { _Equipment_Prompt7 = value; } }
        public string Equipment_Prompt8 { get { return _Equipment_Prompt8; } set { _Equipment_Prompt8 = value; } }
        public string Equipment_Prompt9 { get { return _Equipment_Prompt9; } set { _Equipment_Prompt9 = value; } }
        public string Equipment_Prompt10 { get { return _Equipment_Prompt10; } set { _Equipment_Prompt10 = value; } }
       
        public string Update_Time { get { return _Update_Time; } set { _Update_Time = value; } }
        public string Update_User { get { return _Update_User; } set { _Update_User = value; } }
        public List<FactorySetupArgsPack> FacArgsPacks{ get{ return _FacArgsPacks; } set { _FacArgsPacks = value; } }
        #endregion
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(FactorySetupArgsPack), this);
            return argsPack;
        }
    }
}
