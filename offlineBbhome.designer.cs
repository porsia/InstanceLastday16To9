﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5448
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApplication2
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
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="offLineBBHome")]
	public partial class offlineBbhomeDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public offlineBbhomeDataContext() : 
				base(global::ConsoleApplication2.Properties.Settings.Default.offLineBBHomeConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public offlineBbhomeDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public offlineBbhomeDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public offlineBbhomeDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public offlineBbhomeDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Ga_guidUserID> Ga_guidUserIDs
		{
			get
			{
				return this.GetTable<Ga_guidUserID>();
			}
		}
		
		public System.Data.Linq.Table<ob_v_visitreport> ob_v_visitreports
		{
			get
			{
				return this.GetTable<ob_v_visitreport>();
			}
		}
	}
	
	[Table(Name="dbo.Ga_guidUserID")]
	public partial class Ga_guidUserID
	{
		
		private int _uid;
		
		private string _guid;
		
		public Ga_guidUserID()
		{
		}
		
		[Column(Storage="_uid", DbType="Int NOT NULL")]
		public int uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				if ((this._uid != value))
				{
					this._uid = value;
				}
			}
		}
		
		[Column(Storage="_guid", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string guid
		{
			get
			{
				return this._guid;
			}
			set
			{
				if ((this._guid != value))
				{
					this._guid = value;
				}
			}
		}
	}
	
	[Table(Name="dbo.ob_v_visitreport")]
	public partial class ob_v_visitreport
	{
		
		private string _category;
		
		private int _productid;
		
		private string _productName;
		
		public ob_v_visitreport()
		{
		}
		
		[Column(Storage="_category", DbType="VarChar(50)")]
		public string category
		{
			get
			{
				return this._category;
			}
			set
			{
				if ((this._category != value))
				{
					this._category = value;
				}
			}
		}
		
		[Column(Storage="_productid", DbType="Int NOT NULL")]
		public int productid
		{
			get
			{
				return this._productid;
			}
			set
			{
				if ((this._productid != value))
				{
					this._productid = value;
				}
			}
		}
		
		[Column(Storage="_productName", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string productName
		{
			get
			{
				return this._productName;
			}
			set
			{
				if ((this._productName != value))
				{
					this._productName = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
