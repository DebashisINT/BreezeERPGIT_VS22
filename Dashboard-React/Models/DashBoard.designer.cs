﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dashboard_React.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="BreezeERP_19082019")]
	public partial class DashBoardDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DashBoardDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["BreezeERP_19082019ConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DashBoardDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DashBoardDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DashBoardDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DashBoardDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<v_phoneCallDetail> v_phoneCallDetails
		{
			get
			{
				return this.GetTable<v_phoneCallDetail>();
			}
		}
		
		public System.Data.Linq.Table<v_FutureSaleDetail> v_FutureSaleDetails
		{
			get
			{
				return this.GetTable<v_FutureSaleDetail>();
			}
		}
		
		public System.Data.Linq.Table<v_TodaysAttendance> v_TodaysAttendances
		{
			get
			{
				return this.GetTable<v_TodaysAttendance>();
			}
		}
		
		public System.Data.Linq.Table<v_TodaysAttendanceCount> v_TodaysAttendanceCounts
		{
			get
			{
				return this.GetTable<v_TodaysAttendanceCount>();
			}
		}
		
		public System.Data.Linq.Table<tbl_CRMDb_ActHistory> tbl_CRMDb_ActHistories
		{
			get
			{
				return this.GetTable<tbl_CRMDb_ActHistory>();
			}
		}
		
		public System.Data.Linq.Table<v_followupDbHistory> v_followupDbHistories
		{
			get
			{
				return this.GetTable<v_followupDbHistory>();
			}
		}
		
		public System.Data.Linq.Table<CourtesyCall_Report> CourtesyCall_Reports
		{
			get
			{
				return this.GetTable<CourtesyCall_Report>();
			}
		}
		
		public System.Data.Linq.Table<V_ProjectList> V_ProjectLists
		{
			get
			{
				return this.GetTable<V_ProjectList>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.v_phoneCallDetails")]
	public partial class v_phoneCallDetail
	{
		
		private string _calldate;
		
		private string _note;
		
		private string _smanId;
		
		private System.DateTime _phd_callDate;
		
		private decimal _phd_id;
		
		private System.Nullable<int> _slv_nextActivityType;
		
		public v_phoneCallDetail()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_calldate", DbType="VarChar(10)")]
		public string calldate
		{
			get
			{
				return this._calldate;
			}
			set
			{
				if ((this._calldate != value))
				{
					this._calldate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_note", DbType="VarChar(500)")]
		public string note
		{
			get
			{
				return this._note;
			}
			set
			{
				if ((this._note != value))
				{
					this._note = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_smanId", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string smanId
		{
			get
			{
				return this._smanId;
			}
			set
			{
				if ((this._smanId != value))
				{
					this._smanId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_phd_callDate", DbType="DateTime NOT NULL")]
		public System.DateTime phd_callDate
		{
			get
			{
				return this._phd_callDate;
			}
			set
			{
				if ((this._phd_callDate != value))
				{
					this._phd_callDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_phd_id", DbType="Decimal(18,0) NOT NULL")]
		public decimal phd_id
		{
			get
			{
				return this._phd_id;
			}
			set
			{
				if ((this._phd_id != value))
				{
					this._phd_id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_slv_nextActivityType", DbType="Int")]
		public System.Nullable<int> slv_nextActivityType
		{
			get
			{
				return this._slv_nextActivityType;
			}
			set
			{
				if ((this._slv_nextActivityType != value))
				{
					this._slv_nextActivityType = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.v_FutureSaleDetails")]
	public partial class v_FutureSaleDetail
	{
		
		private System.Nullable<decimal> _phd_id;
		
		private System.Nullable<System.DateTime> _phd_callDate;
		
		private string _SalesManName;
		
		private string _CustomerName;
		
		private string _sProducts_Name;
		
		private string _phd_note;
		
		private string _call_dispositions;
		
		public v_FutureSaleDetail()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_phd_id", DbType="Decimal(19,0)")]
		public System.Nullable<decimal> phd_id
		{
			get
			{
				return this._phd_id;
			}
			set
			{
				if ((this._phd_id != value))
				{
					this._phd_id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_phd_callDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> phd_callDate
		{
			get
			{
				return this._phd_callDate;
			}
			set
			{
				if ((this._phd_callDate != value))
				{
					this._phd_callDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SalesManName", DbType="VarChar(201)")]
		public string SalesManName
		{
			get
			{
				return this._SalesManName;
			}
			set
			{
				if ((this._SalesManName != value))
				{
					this._SalesManName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustomerName", DbType="VarChar(201)")]
		public string CustomerName
		{
			get
			{
				return this._CustomerName;
			}
			set
			{
				if ((this._CustomerName != value))
				{
					this._CustomerName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sProducts_Name", DbType="VarChar(100)")]
		public string sProducts_Name
		{
			get
			{
				return this._sProducts_Name;
			}
			set
			{
				if ((this._sProducts_Name != value))
				{
					this._sProducts_Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_phd_note", DbType="NVarChar(4000)")]
		public string phd_note
		{
			get
			{
				return this._phd_note;
			}
			set
			{
				if ((this._phd_note != value))
				{
					this._phd_note = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_call_dispositions", DbType="VarChar(100)")]
		public string call_dispositions
		{
			get
			{
				return this._call_dispositions;
			}
			set
			{
				if ((this._call_dispositions != value))
				{
					this._call_dispositions = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.v_TodaysAttendance")]
	public partial class v_TodaysAttendance
	{
		
		private string _Emp_InternalId;
		
		private string _Name;
		
		private System.Nullable<System.DateTime> _In_Time;
		
		public v_TodaysAttendance()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Emp_InternalId", DbType="VarChar(10)")]
		public string Emp_InternalId
		{
			get
			{
				return this._Emp_InternalId;
			}
			set
			{
				if ((this._Emp_InternalId != value))
				{
					this._Emp_InternalId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(251)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_In_Time", DbType="DateTime")]
		public System.Nullable<System.DateTime> In_Time
		{
			get
			{
				return this._In_Time;
			}
			set
			{
				if ((this._In_Time != value))
				{
					this._In_Time = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.v_TodaysAttendanceCount")]
	public partial class v_TodaysAttendanceCount
	{
		
		private string _Emp_InternalId;
		
		private string _Name;
		
		private System.Nullable<int> _count;
		
		public v_TodaysAttendanceCount()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Emp_InternalId", DbType="VarChar(10)")]
		public string Emp_InternalId
		{
			get
			{
				return this._Emp_InternalId;
			}
			set
			{
				if ((this._Emp_InternalId != value))
				{
					this._Emp_InternalId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(251)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_count", DbType="Int")]
		public System.Nullable<int> count
		{
			get
			{
				return this._count;
			}
			set
			{
				if ((this._count != value))
				{
					this._count = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tbl_CRMDb_ActHistory")]
	public partial class tbl_CRMDb_ActHistory
	{
		
		private string _SMenId;
		
		private string _SmenName;
		
		private string _CustName;
		
		private string _ProdClass;
		
		private System.Nullable<long> _sls_id;
		
		private string _OutCome1;
		
		private string _Remarks1;
		
		private string _FeedBack1;
		
		private string _OutCome2;
		
		private string _Remarks2;
		
		private string _FeedBack2;
		
		private string _OutCome3;
		
		private string _Remarks3;
		
		private string _FeedBack3;
		
		private string _OutCome4;
		
		private string _Remarks4;
		
		private string _FeedBack4;
		
		private string _OutCome5;
		
		private string _Remarks5;
		
		private string _FeedBack5;
		
		private System.Nullable<long> _UserId;
		
		private System.Nullable<decimal> _Budget;
		
		private System.Nullable<System.DateTime> _OutcomeDate1;
		
		private System.Nullable<System.DateTime> _OutcomeDate2;
		
		private System.Nullable<System.DateTime> _OutcomeDate3;
		
		private System.Nullable<System.DateTime> _OutcomeDate4;
		
		private System.Nullable<System.DateTime> _OutcomeDate5;
		
		private System.Nullable<System.DateTime> _FeedBackDate1;
		
		private System.Nullable<System.DateTime> _FeedBackDate2;
		
		private System.Nullable<System.DateTime> _FeedBackDate3;
		
		private System.Nullable<System.DateTime> _FeedBackDate4;
		
		private System.Nullable<System.DateTime> _FeedBackDate5;
		
		public tbl_CRMDb_ActHistory()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SMenId", DbType="VarChar(20)")]
		public string SMenId
		{
			get
			{
				return this._SMenId;
			}
			set
			{
				if ((this._SMenId != value))
				{
					this._SMenId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SmenName", DbType="VarChar(100)")]
		public string SmenName
		{
			get
			{
				return this._SmenName;
			}
			set
			{
				if ((this._SmenName != value))
				{
					this._SmenName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustName", DbType="VarChar(100)")]
		public string CustName
		{
			get
			{
				return this._CustName;
			}
			set
			{
				if ((this._CustName != value))
				{
					this._CustName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProdClass", DbType="VarChar(100)")]
		public string ProdClass
		{
			get
			{
				return this._ProdClass;
			}
			set
			{
				if ((this._ProdClass != value))
				{
					this._ProdClass = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_sls_id", DbType="BigInt")]
		public System.Nullable<long> sls_id
		{
			get
			{
				return this._sls_id;
			}
			set
			{
				if ((this._sls_id != value))
				{
					this._sls_id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutCome1", DbType="VarChar(500)")]
		public string OutCome1
		{
			get
			{
				return this._OutCome1;
			}
			set
			{
				if ((this._OutCome1 != value))
				{
					this._OutCome1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks1", DbType="VarChar(500)")]
		public string Remarks1
		{
			get
			{
				return this._Remarks1;
			}
			set
			{
				if ((this._Remarks1 != value))
				{
					this._Remarks1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBack1", DbType="VarChar(500)")]
		public string FeedBack1
		{
			get
			{
				return this._FeedBack1;
			}
			set
			{
				if ((this._FeedBack1 != value))
				{
					this._FeedBack1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutCome2", DbType="VarChar(500)")]
		public string OutCome2
		{
			get
			{
				return this._OutCome2;
			}
			set
			{
				if ((this._OutCome2 != value))
				{
					this._OutCome2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks2", DbType="VarChar(500)")]
		public string Remarks2
		{
			get
			{
				return this._Remarks2;
			}
			set
			{
				if ((this._Remarks2 != value))
				{
					this._Remarks2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBack2", DbType="VarChar(500)")]
		public string FeedBack2
		{
			get
			{
				return this._FeedBack2;
			}
			set
			{
				if ((this._FeedBack2 != value))
				{
					this._FeedBack2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutCome3", DbType="VarChar(500)")]
		public string OutCome3
		{
			get
			{
				return this._OutCome3;
			}
			set
			{
				if ((this._OutCome3 != value))
				{
					this._OutCome3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks3", DbType="VarChar(500)")]
		public string Remarks3
		{
			get
			{
				return this._Remarks3;
			}
			set
			{
				if ((this._Remarks3 != value))
				{
					this._Remarks3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBack3", DbType="VarChar(500)")]
		public string FeedBack3
		{
			get
			{
				return this._FeedBack3;
			}
			set
			{
				if ((this._FeedBack3 != value))
				{
					this._FeedBack3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutCome4", DbType="VarChar(500)")]
		public string OutCome4
		{
			get
			{
				return this._OutCome4;
			}
			set
			{
				if ((this._OutCome4 != value))
				{
					this._OutCome4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks4", DbType="VarChar(500)")]
		public string Remarks4
		{
			get
			{
				return this._Remarks4;
			}
			set
			{
				if ((this._Remarks4 != value))
				{
					this._Remarks4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBack4", DbType="VarChar(500)")]
		public string FeedBack4
		{
			get
			{
				return this._FeedBack4;
			}
			set
			{
				if ((this._FeedBack4 != value))
				{
					this._FeedBack4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutCome5", DbType="VarChar(500)")]
		public string OutCome5
		{
			get
			{
				return this._OutCome5;
			}
			set
			{
				if ((this._OutCome5 != value))
				{
					this._OutCome5 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks5", DbType="VarChar(500)")]
		public string Remarks5
		{
			get
			{
				return this._Remarks5;
			}
			set
			{
				if ((this._Remarks5 != value))
				{
					this._Remarks5 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBack5", DbType="VarChar(500)")]
		public string FeedBack5
		{
			get
			{
				return this._FeedBack5;
			}
			set
			{
				if ((this._FeedBack5 != value))
				{
					this._FeedBack5 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="BigInt")]
		public System.Nullable<long> UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this._UserId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Budget", DbType="Decimal(18,2)")]
		public System.Nullable<decimal> Budget
		{
			get
			{
				return this._Budget;
			}
			set
			{
				if ((this._Budget != value))
				{
					this._Budget = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutcomeDate1", DbType="DateTime")]
		public System.Nullable<System.DateTime> OutcomeDate1
		{
			get
			{
				return this._OutcomeDate1;
			}
			set
			{
				if ((this._OutcomeDate1 != value))
				{
					this._OutcomeDate1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutcomeDate2", DbType="DateTime")]
		public System.Nullable<System.DateTime> OutcomeDate2
		{
			get
			{
				return this._OutcomeDate2;
			}
			set
			{
				if ((this._OutcomeDate2 != value))
				{
					this._OutcomeDate2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutcomeDate3", DbType="DateTime")]
		public System.Nullable<System.DateTime> OutcomeDate3
		{
			get
			{
				return this._OutcomeDate3;
			}
			set
			{
				if ((this._OutcomeDate3 != value))
				{
					this._OutcomeDate3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutcomeDate4", DbType="DateTime")]
		public System.Nullable<System.DateTime> OutcomeDate4
		{
			get
			{
				return this._OutcomeDate4;
			}
			set
			{
				if ((this._OutcomeDate4 != value))
				{
					this._OutcomeDate4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OutcomeDate5", DbType="DateTime")]
		public System.Nullable<System.DateTime> OutcomeDate5
		{
			get
			{
				return this._OutcomeDate5;
			}
			set
			{
				if ((this._OutcomeDate5 != value))
				{
					this._OutcomeDate5 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackDate1", DbType="DateTime")]
		public System.Nullable<System.DateTime> FeedBackDate1
		{
			get
			{
				return this._FeedBackDate1;
			}
			set
			{
				if ((this._FeedBackDate1 != value))
				{
					this._FeedBackDate1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackDate2", DbType="DateTime")]
		public System.Nullable<System.DateTime> FeedBackDate2
		{
			get
			{
				return this._FeedBackDate2;
			}
			set
			{
				if ((this._FeedBackDate2 != value))
				{
					this._FeedBackDate2 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackDate3", DbType="DateTime")]
		public System.Nullable<System.DateTime> FeedBackDate3
		{
			get
			{
				return this._FeedBackDate3;
			}
			set
			{
				if ((this._FeedBackDate3 != value))
				{
					this._FeedBackDate3 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackDate4", DbType="DateTime")]
		public System.Nullable<System.DateTime> FeedBackDate4
		{
			get
			{
				return this._FeedBackDate4;
			}
			set
			{
				if ((this._FeedBackDate4 != value))
				{
					this._FeedBackDate4 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FeedBackDate5", DbType="DateTime")]
		public System.Nullable<System.DateTime> FeedBackDate5
		{
			get
			{
				return this._FeedBackDate5;
			}
			set
			{
				if ((this._FeedBackDate5 != value))
				{
					this._FeedBackDate5 = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.v_followupDbHistory")]
	public partial class v_followupDbHistory
	{
		
		private long _id;
		
		private string _user_name;
		
		private System.Nullable<System.DateTime> _FollowDate;
		
		private string _FollowUsing;
		
		private string _Document;
		
		private System.Nullable<System.DateTime> _DocDate;
		
		private string _name;
		
		private string _openClsoe;
		
		private System.Nullable<System.DateTime> _NextFollowDate;
		
		private string _Remarks;
		
		public v_followupDbHistory()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="BigInt NOT NULL")]
		public long id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this._id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_user_name", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string user_name
		{
			get
			{
				return this._user_name;
			}
			set
			{
				if ((this._user_name != value))
				{
					this._user_name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FollowDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> FollowDate
		{
			get
			{
				return this._FollowDate;
			}
			set
			{
				if ((this._FollowDate != value))
				{
					this._FollowDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FollowUsing", DbType="VarChar(20)")]
		public string FollowUsing
		{
			get
			{
				return this._FollowUsing;
			}
			set
			{
				if ((this._FollowUsing != value))
				{
					this._FollowUsing = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Document", DbType="NVarChar(100)")]
		public string Document
		{
			get
			{
				return this._Document;
			}
			set
			{
				if ((this._Document != value))
				{
					this._Document = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DocDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> DocDate
		{
			get
			{
				return this._DocDate;
			}
			set
			{
				if ((this._DocDate != value))
				{
					this._DocDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", DbType="VarChar(251)")]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this._name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_openClsoe", DbType="VarChar(15)")]
		public string openClsoe
		{
			get
			{
				return this._openClsoe;
			}
			set
			{
				if ((this._openClsoe != value))
				{
					this._openClsoe = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NextFollowDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> NextFollowDate
		{
			get
			{
				return this._NextFollowDate;
			}
			set
			{
				if ((this._NextFollowDate != value))
				{
					this._NextFollowDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Remarks", DbType="VarChar(4000)")]
		public string Remarks
		{
			get
			{
				return this._Remarks;
			}
			set
			{
				if ((this._Remarks != value))
				{
					this._Remarks = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.CourtesyCall_Report")]
	public partial class CourtesyCall_Report
	{
		
		private string _TelleCaller;
		
		private string _Courtesy_CallDate___Time;
		
		private string _Lead_ID__Name;
		
		private System.Nullable<System.DateTime> _Last_Visit_Date_Time;
		
		private string _FOS;
		
		private string _Courtesy_Call_Feedback;
		
		private string _Visit_Outcome_Courtesy_Call_;
		
		private string _Visit_Outcome_Sales_Rep__;
		
		private decimal _TelleCallerId;
		
		private decimal _assignedByid;
		
		private string _Assign_By;
		
		public CourtesyCall_Report()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TelleCaller", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string TelleCaller
		{
			get
			{
				return this._TelleCaller;
			}
			set
			{
				if ((this._TelleCaller != value))
				{
					this._TelleCaller = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Courtesy CallDate / Time]", Storage="_Courtesy_CallDate___Time", DbType="NVarChar(50)")]
		public string Courtesy_CallDate___Time
		{
			get
			{
				return this._Courtesy_CallDate___Time;
			}
			set
			{
				if ((this._Courtesy_CallDate___Time != value))
				{
					this._Courtesy_CallDate___Time = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Lead ID /Name]", Storage="_Lead_ID__Name", DbType="NVarChar(204)")]
		public string Lead_ID__Name
		{
			get
			{
				return this._Lead_ID__Name;
			}
			set
			{
				if ((this._Lead_ID__Name != value))
				{
					this._Lead_ID__Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Last Visit Date/Time]", Storage="_Last_Visit_Date_Time", DbType="DateTime")]
		public System.Nullable<System.DateTime> Last_Visit_Date_Time
		{
			get
			{
				return this._Last_Visit_Date_Time;
			}
			set
			{
				if ((this._Last_Visit_Date_Time != value))
				{
					this._Last_Visit_Date_Time = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FOS", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string FOS
		{
			get
			{
				return this._FOS;
			}
			set
			{
				if ((this._FOS != value))
				{
					this._FOS = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Courtesy Call Feedback]", Storage="_Courtesy_Call_Feedback", DbType="NVarChar(200)")]
		public string Courtesy_Call_Feedback
		{
			get
			{
				return this._Courtesy_Call_Feedback;
			}
			set
			{
				if ((this._Courtesy_Call_Feedback != value))
				{
					this._Courtesy_Call_Feedback = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Visit Outcome(Courtesy Call)]", Storage="_Visit_Outcome_Courtesy_Call_", DbType="NVarChar(200)")]
		public string Visit_Outcome_Courtesy_Call_
		{
			get
			{
				return this._Visit_Outcome_Courtesy_Call_;
			}
			set
			{
				if ((this._Visit_Outcome_Courtesy_Call_ != value))
				{
					this._Visit_Outcome_Courtesy_Call_ = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Visit Outcome(Sales Rep.)]", Storage="_Visit_Outcome_Sales_Rep__", DbType="NVarChar(200)")]
		public string Visit_Outcome_Sales_Rep__
		{
			get
			{
				return this._Visit_Outcome_Sales_Rep__;
			}
			set
			{
				if ((this._Visit_Outcome_Sales_Rep__ != value))
				{
					this._Visit_Outcome_Sales_Rep__ = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TelleCallerId", DbType="Decimal(10,0) NOT NULL")]
		public decimal TelleCallerId
		{
			get
			{
				return this._TelleCallerId;
			}
			set
			{
				if ((this._TelleCallerId != value))
				{
					this._TelleCallerId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_assignedByid", DbType="Decimal(10,0) NOT NULL")]
		public decimal assignedByid
		{
			get
			{
				return this._assignedByid;
			}
			set
			{
				if ((this._assignedByid != value))
				{
					this._assignedByid = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Assign By]", Storage="_Assign_By", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Assign_By
		{
			get
			{
				return this._Assign_By;
			}
			set
			{
				if ((this._Assign_By != value))
				{
					this._Assign_By = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.V_ProjectList")]
	public partial class V_ProjectList
	{
		
		private long _Proj_Id;
		
		private string _Proj_Name;
		
		private string _Proj_Code;
		
		private string _Customer;
		
		private string _CustId;
		
		private System.Nullable<bool> _IsInUse;
		
		private string _ProjectStatus;
		
		private System.Nullable<long> _ProjBracnchid;
		
		private long _Hierarchy_ID;
		
		private string _HIERARCHY_NAME;
		
		public V_ProjectList()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Proj_Id", DbType="BigInt NOT NULL")]
		public long Proj_Id
		{
			get
			{
				return this._Proj_Id;
			}
			set
			{
				if ((this._Proj_Id != value))
				{
					this._Proj_Id = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Proj_Name", DbType="NVarChar(100)")]
		public string Proj_Name
		{
			get
			{
				return this._Proj_Name;
			}
			set
			{
				if ((this._Proj_Name != value))
				{
					this._Proj_Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Proj_Code", DbType="NVarChar(200)")]
		public string Proj_Code
		{
			get
			{
				return this._Proj_Code;
			}
			set
			{
				if ((this._Proj_Code != value))
				{
					this._Proj_Code = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Customer", DbType="VarChar(252) NOT NULL", CanBeNull=false)]
		public string Customer
		{
			get
			{
				return this._Customer;
			}
			set
			{
				if ((this._Customer != value))
				{
					this._Customer = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CustId", DbType="NVarChar(50)")]
		public string CustId
		{
			get
			{
				return this._CustId;
			}
			set
			{
				if ((this._CustId != value))
				{
					this._CustId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsInUse", DbType="Bit")]
		public System.Nullable<bool> IsInUse
		{
			get
			{
				return this._IsInUse;
			}
			set
			{
				if ((this._IsInUse != value))
				{
					this._IsInUse = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjectStatus", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string ProjectStatus
		{
			get
			{
				return this._ProjectStatus;
			}
			set
			{
				if ((this._ProjectStatus != value))
				{
					this._ProjectStatus = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProjBracnchid", DbType="BigInt")]
		public System.Nullable<long> ProjBracnchid
		{
			get
			{
				return this._ProjBracnchid;
			}
			set
			{
				if ((this._ProjBracnchid != value))
				{
					this._ProjBracnchid = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Hierarchy_ID", DbType="BigInt NOT NULL")]
		public long Hierarchy_ID
		{
			get
			{
				return this._Hierarchy_ID;
			}
			set
			{
				if ((this._Hierarchy_ID != value))
				{
					this._Hierarchy_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HIERARCHY_NAME", DbType="VarChar(MAX)")]
		public string HIERARCHY_NAME
		{
			get
			{
				return this._HIERARCHY_NAME;
			}
			set
			{
				if ((this._HIERARCHY_NAME != value))
				{
					this._HIERARCHY_NAME = value;
				}
			}
		}
	}
}
#pragma warning restore 1591