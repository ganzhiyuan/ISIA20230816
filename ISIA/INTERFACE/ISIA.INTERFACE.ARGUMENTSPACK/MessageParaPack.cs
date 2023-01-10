using System;
using System.Collections.Generic;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class MessageParaPack
    {
        #region Common
        private ArgumentPack argsPack = new ArgumentPack();

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(MessageParaPack), this);
            return argsPack;
        }

        private string eventUser = null;
        private string where = null;
        private string transactionType = null;
        public string EventUser { get { return eventUser; } set { eventUser = value; } }
        public string Where { get { return where; } set { where = value; } }
        public string TransactionType { get { return transactionType; } set { transactionType = value; } }
        #endregion

        #region FactorySetup
        private string factory = null;
        private string factoryDescription = null;
        private string factoryType = null;
        private string factorySequence = null;
        private string shift1Start = null;
        private string shift2Start = null;
        private string daysPerWeek = null;
        private string hoursPerDay = null;
        public string Factory { get { return factory; } set { factory = value; } }
        public string FactoryDescription { get { return factoryDescription; } set { factoryDescription = value; } }
        public string FactoryType { get { return factoryType; } set { factoryType = value; } }
        public string FactorySequence { get { return factorySequence; } set { factorySequence = value; } }
        public string Shift1Start { get { return shift1Start; } set { shift1Start = value; } }
        public string Shift2Start { get { return shift2Start; } set { shift2Start = value; } }
        public string DaysPerWeek { get { return daysPerWeek; } set { daysPerWeek = value; } }
        public string HoursPerDay { get { return hoursPerDay; } set { hoursPerDay = value; } }
        #endregion
        #region DeviceSetup
        private string device = null;
        private string deviceDescription = null;
        public string Device { get { return device; } set { device = value; } }
        public string DeviceDescription { get { return deviceDescription; } set { deviceDescription = value; } }
        #endregion
        #region FlowSetup
        private string flow = null;
        private string flowDescription = null;
        private string rework_Flow_YN = null;
        private string alter_Flow_YN = null;
        public string Flow { get { return flow; } set { flow = value; } }
        public string FlowDescription { get { return flowDescription; } set { flowDescription = value; } }
        public string Rework_Flow_YN { get { return rework_Flow_YN; } set { rework_Flow_YN = value; } }
        public string Alter_Flow_YN { get { return alter_Flow_YN; } set { alter_Flow_YN = value; } }
        #endregion
        #region OperationSetup
        private string operation = null;
        private string operationDescription = null;
        private string operationDisplaySequence = null;
        private string reworkYN = null;
        public string Operation { get { return operation; } set { operation = value; } }
        public string OperationDescription { get { return operationDescription; } set { operationDescription = value; } }
        public string OperationDisplaySequence { get { return operationDisplaySequence; } set { operationDisplaySequence = value; } }
        public string ReworkYN { get { return reworkYN; } set { reworkYN = value; } }
        #endregion
        #region Lot
        private string lot = null;
        private string qty = null;
        public string Lot { get { return lot; } set { lot = value; } }
        public string Qty { get { return qty; } set { qty = value; } }
        #endregion
        #region Wafer
        private string wafer = null;
        public string Wafer { get { return wafer; } set { wafer = value; } }
        #endregion
        #region Hold
        private string holdCode = null;
        private string holdComment = null;
        public string HoldCode { get { return holdCode; } set { holdCode = value; } }
        public string HoldComment { get { return holdComment; } set { holdComment = value; } }
        #endregion
        #region Release
        private string releaseCode = null;
        private string releaseComment = null;
        private string createTime = null;
        public string ReleaseCode { get { return releaseCode; } set { releaseCode = value; } }
        public string ReleaseComment { get { return releaseComment; } set { releaseComment = value; } }
        public string CreateTime { get { return createTime; } set { createTime = value; } }
        #endregion
        #region Eqp
        private string eqp = null;
        public string Eqp { get { return eqp; } set { eqp = value; } }
        #endregion
        #region Rework
        private string reworkCode = null;
        private string reworkReturnFlow = null;
        private string reworkReturnOperation = null;
        public string ReworkCode { get { return reworkCode; } set { reworkCode = value; } }
        public string ReworkReturnFlow { get { return reworkReturnFlow; } set { reworkReturnFlow = value; } }
        public string ReworkReturnOperation { get { return reworkReturnOperation; } set { reworkReturnOperation = value; } }
        #endregion
        #region Merge
        private string childLot = null;
        public string ChildLot { get { return childLot; } set { childLot = value; } }
        #endregion
        #region WorkOrder
        private string year = null;
        private string month = null;
        private string workOrder = null;
        public string Year { get { return year; } set { year = value; } }
        public string Month { get { return month; } set { month = value; } }
        public string WorkOrder { get { return workOrder; } set { workOrder = value; } }
        #endregion
        #region EDC
        private string paraName = null;
        private string paraValue = null;
        public string ParaName { get { return paraName; } set { paraName = value; } }
        public string ParaValue { get { return paraValue; } set { paraValue = value; } }
        #endregion
        #region BOM
        private string bomPartName = null;
        private string bomPartDescription = null;
        private string bomPartGroup = null;
        private string bomCodeName = null;
        private string bomCodeDescription = null;
        public string BomPartName { get { return bomPartName; } set { bomPartName = value; } }
        public string BomPartDescription { get { return bomPartDescription; } set { bomPartDescription = value; } }
        public string BomPartGroup { get { return bomPartGroup; } set { bomPartGroup = value; } }
        public string BomCodeName { get { return bomCodeName; } set { bomCodeName = value; } }
        public string BomCodeDescription { get { return bomCodeDescription; } set { bomCodeDescription = value; } }
        #endregion
        #region SPC
        private string spcRuleName = null;
        private string lcl = null;
        private string ucl = null;
        public string SpcRuleName { get { return spcRuleName; } set { spcRuleName = value; } }
        public string Lcl { get { return lcl; } set { lcl = value; } }
        public string Ucl { get { return ucl; } set { ucl = value; } }
        #endregion
    }
}
