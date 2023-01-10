using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TAP;
using TAP.Models;
namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]

    public class MROArgsPack
    {
        private List<MROArgsPack> _mROArgsPacks = new List<MROArgsPack>();

        private ArgumentPack argsPack = new ArgumentPack();
        private string _Region;
        private string _Factory;
        private string _Money;
        private string _documentNo;
        private string _facility;
        private string _documentType;
        private string _agent;
        private string _documentuser;
        private string _comment;
        private string _orderTime;
        private string _sparePartsType;
        private string _sparePartsCode;
        private string _parentCode;
        private string _description;
        private string _model;
        private string _serialNumber;
        private string _sapCode;
        private string _defaultShelf;
        private string _countingUnit;
        private string _quantity;
        private string _price;
        private string _abcClass;
        private string _useProcess;
        private string _category;
        private string _stockInStatus;
        private string _wareHouse;
        private string _imageName;
        private string _insertTime;
        private string _updateTime;
        private string _insertUser;
        private string _updateUser;
        private string _isReturn;
        private string _isDisable;
        private string _isAlive;
        private string _DocumentTime;
        private string _sqltype;
        private string _normalNumber;
        private string _repairNumber;
        private string _startTime;
        private string _endtTime;
        private string _operationType;
        private string _OldDocumentNo;
        private string _moveWareHouse;
        private string _moveShelf;
        private string _PO;
        private string _Supplier;
        private string _Code;
        private string _Mingcheng;
        private string _Area;
        private string _Type;
        private string _LINE;
        private string _eqpcode;
        private string _eqpdescribe;
        private string _Remarks;
        private string _Reqty;
        public string Reqty
        {
            get
            {
                return _Reqty;
            }

            set
            {
                _Reqty = value;
            }
        }
        public string eqpdescribe
        {
            get
            {
                return _eqpdescribe;
            }

            set
            {
                _eqpdescribe = value;
            }
        }
        public string eqpcode
        {
            get
            {
                return _eqpcode;
            }

            set
            {
                _eqpcode = value;
            }
        }
        public string LINE
        {
            get
            {
                return _LINE;
            }

            set
            {
                _LINE = value;
            }
        }
        public string Type
        {
            get
            {
                return _Type;
            }

            set
            {
                _Type = value;
            }
        }
        public string Area
        {
            get
            {
                return _Area;
            }

            set
            {
                _Area = value;
            }
        }
        public string Mingcheng
        {
            get
            {
                return _Mingcheng;
            }

            set
            {
                _Mingcheng = value;
            }
        }
        public string Code
        {
            get
            {
                return _Code;
            }

            set
            {
                _Code = value;
            }
        }
        public string Supplier
        {
            get
            {
                return _Supplier;
            }

            set
            {
                _Supplier = value;
            }
        }
        public string PO
        {
            get
            {
                return _PO;
            }

            set
            {
                _PO = value;
            }
        }
        public string OldDocumentNo
        {
            get
            {
                return _OldDocumentNo;
            }

            set
            {
                _OldDocumentNo = value;
            }
        }
        public string DocumentNo
        {
            get
            {
                return _documentNo;
            }

            set
            {
                _documentNo = value;
            }
        }

        public string DocumentType
        {
            get
            {
                return _documentType;
            }

            set
            {
                _documentType = value;
            }
        }

        public string Agent
        {
            get
            {
                return _agent;
            }

            set
            {
                _agent = value;
            }
        }

        public string Documentuser
        {
            get
            {
                return _documentuser;
            }

            set
            {
                _documentuser = value;
            }
        }

        public string Comment
        {
            get
            {
                return _comment;
            }

            set
            {
                _comment = value;
            }
        }

        public string OrderTime
        {
            get
            {
                return _orderTime;
            }

            set
            {
                _orderTime = value;
            }
        }

        public string SparePartsType
        {
            get
            {
                return _sparePartsType;
            }

            set
            {
                _sparePartsType = value;
            }
        }

        public string SparePartsCode
        {
            get
            {
                return _sparePartsCode;
            }

            set
            {
                _sparePartsCode = value;
            }
        }

        public string ParentCode
        {
            get
            {
                return _parentCode;
            }

            set
            {
                _parentCode = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public string Model
        {
            get
            {
                return _model;
            }

            set
            {
                _model = value;
            }
        }

        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }

            set
            {
                _serialNumber = value;
            }
        }

        public string SapCode
        {
            get
            {
                return _sapCode;
            }

            set
            {
                _sapCode = value;
            }
        }

        public string DefaultShelf
        {
            get
            {
                return _defaultShelf;
            }

            set
            {
                _defaultShelf = value;
            }
        }

        public string CountingUnit
        {
            get
            {
                return _countingUnit;
            }

            set
            {
                _countingUnit = value;
            }
        }

        public string Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                _quantity = value;
            }
        }

        public string Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
            }
        }

        public string AbcClass
        {
            get
            {
                return _abcClass;
            }

            set
            {
                _abcClass = value;
            }
        }

        public string UseProcess
        {
            get
            {
                return _useProcess;
            }

            set
            {
                _useProcess = value;
            }
        }

        public string Category
        {
            get
            {
                return _category;
            }

            set
            {
                _category = value;
            }
        }

        public string StockInStatus
        {
            get
            {
                return _stockInStatus;
            }

            set
            {
                _stockInStatus = value;
            }
        }

        public string WareHouse
        {
            get
            {
                return _wareHouse;
            }

            set
            {
                _wareHouse = value;
            }
        }
        public string InsertTime
        {
            get
            {
                return _insertTime;
            }

            set
            {
                _insertTime = value;
            }
        }

        public string UpdateTime
        {
            get
            {
                return _updateTime;
            }

            set
            {
                _updateTime = value;
            }
        }

        public string InsertUser
        {
            get
            {
                return _insertUser;
            }

            set
            {
                _insertUser = value;
            }
        }

        public string UpdateUser
        {
            get
            {
                return _updateUser;
            }

            set
            {
                _updateUser = value;
            }
        }

        public string IsReturn
        {
            get
            {
                return _isReturn;
            }

            set
            {
                _isReturn = value;
            }
        }

        public string IsDisable
        {
            get
            {
                return _isDisable;
            }

            set
            {
                _isDisable = value;
            }
        }

        public string IsAlive
        {
            get
            {
                return _isAlive;
            }

            set
            {
                _isAlive = value;
            }
        }

        public string ImageName
        {
            get
            {
                return _imageName;
            }

            set
            {
                _imageName = value;
            }
        }
        public bool OpenUpdate { get; set; }

        public string Facility
        {
            get
            {
                return _facility;
            }

            set
            {
                _facility = value;
            }
        }

        public List<MROArgsPack> MROArgsPacks
        {
            get
            {
                return _mROArgsPacks;
            }

            set
            {
                _mROArgsPacks = value;
            }
        }

        public string Region
        {
            get
            {
                return _Region;
            }

            set
            {
                _Region = value;
            }
        }

        public string Factory
        {
            get
            {
                return _Factory;
            }

            set
            {
                _Factory = value;
            }
        }

        public string Money
        {
            get
            {
                return _Money;
            }

            set
            {
                _Money = value;
            }
        }

        public string DocumentTime
        {
            get
            {
                return _DocumentTime;
            }

            set
            {
                _DocumentTime = value;
            }
        }

        public string Sqltype
        {
            get
            {
                return _sqltype;
            }

            set
            {
                _sqltype = value;
            }
        }

        public string NormalNumber
        {
            get
            {
                return _normalNumber;
            }

            set
            {
                _normalNumber = value;
            }
        }

        public string RepairNumber
        {
            get
            {
                return _repairNumber;
            }

            set
            {
                _repairNumber = value;
            }
        }

        public string StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                _startTime = value;
            }
        }

        public string EndtTime
        {
            get
            {
                return _endtTime;
            }

            set
            {
                _endtTime = value;
            }
        }

        public string OperationType
        {
            get
            {
                return _operationType;
            }

            set
            {
                _operationType = value;
            }
        }

         

        public string MoveWareHouse
        {
            get
            {
                return _moveWareHouse;
            }

            set
            {
                _moveWareHouse = value;
            }
        }

        public string MoveShelf
        {
            get
            {
                return _moveShelf;
            }

            set
            {
                _moveShelf = value;
            }
        }

        public string Remarks
        {
            get
            {
                return _Remarks;
            }

            set
            {
                _Remarks = value;
            }
        }

        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(MROArgsPack), this);
            return argsPack;
        }
    }   
}