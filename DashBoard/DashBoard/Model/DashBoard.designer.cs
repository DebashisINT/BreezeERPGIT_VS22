﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashBoard.DashBoard.Model
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PK07082018")]
	public partial class DashBoardDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DashBoardDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["PK07082018ConnectionString"].ConnectionString, mappingSource)
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
}
#pragma warning restore 1591
