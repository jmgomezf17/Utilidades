
using Interop.DTS;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Utilidades.Datos
{
  public sealed class CapaDatos : global::b, IDisposable
  {
    private Database a;
    private DbTransaction b;
    private DbTransaction c;
    private DbConnection d;
    private string _proyecto;
    private string _modulo;
    private string _opcion;
    private bool e;

    public DbTransaction DbTran
    {
      get
      {
        return this.c;
      }
    }

    public CapaDatos(string A_0, string A_1, string A_2)
    {
      this.e = false;
      this._proyecto = A_0;
      this._modulo = A_1;
      this._opcion = A_2;
      try
      {
        string str = new global::a().b(A_0, A_1);
        if (string.IsNullOrEmpty(str))
          throw new Exception("cadena de conexion no encontrada con los parametros suministrados");
        this.a = (Database) new SqlDatabase(str);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw;
      }
    }

    public void CreateConnection()
    {
      this.d = this.a.CreateConnection();
      this.d.Open();
    }

    public void CloseConnection()
    {
      if (this.d == null)
        return;
      if (this.d.State == ConnectionState.Open)
      {
        try
        {
          this.d.Close();
          this.d.Dispose();
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          throw;
        }
      }
      this.d = (DbConnection) null;
    }

    private string b()
    {
      string str = "";
      if (this.d != null)
        str = this.d.DataSource;
      return str;
    }

    private string a()
    {
      string str = "";
      if (this.d != null)
        str = this.d.Database;
      return str;
    }

    public void BeginTransaction()
    {
      this.b = this.d.BeginTransaction();
    }

    public void CommitTransaction()
    {
      this.b.Commit();
      if (this.b == null)
        return;
      this.b.Dispose();
    }

    public void RollBackTransaction()
    {
      this.b.Rollback();
      if (this.b == null)
        return;
      this.b.Dispose();
    }

    public bool ExecuteDTS(
      string UNCFile,
      string DTSPassword,
      string DTSPackageID,
      string DTSVersionID,
      string DTSname,
      CapaDatos.DTSGlobalVariables[] DTSVariables)
    {
      object obj = (object) null;
      PackageClass A_0 = new PackageClass();
      A_0.LoadFromStorageFile(UNCFile, DTSPassword, DTSPackageID, DTSVersionID, DTSname, ref obj);
      return this.a(A_0, DTSVariables);
    }

    private bool a(PackageClass A_0, CapaDatos.DTSGlobalVariables[] A_1)
    {
      bool flag = false;
      try
      {
        IEnumerator enumerator1;
        try
        {
          enumerator1 = A_0.get_GlobalVariables().GetEnumerator();
          while (enumerator1.MoveNext())
          {
            GlobalVariable current = (GlobalVariable) enumerator1.Current;
            int num = checked (A_1.GetLength(0) - 1);
            int index = 0;
            while (index <= num)
            {
              if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(current.get_Name(), A_1[index].Name, false) == 0)
                A_0.get_GlobalVariables().Remove((object) current.get_Name());
              checked { ++index; }
            }
          }
        }
        finally
        {
          if (enumerator1 is IDisposable)
            (enumerator1 as IDisposable).Dispose();
        }
        int num1 = checked (A_1.GetLength(0) - 1);
        int index1 = 0;
        while (index1 <= num1)
        {
          this.CreateConnection();
          if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(A_1[index1].Name, "DataSource", false) == 0)
            A_0.get_GlobalVariables().AddGlobalVariable(A_1[index1].Name, (object) this.b());
          else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(A_1[index1].Name, "Catalog", false) == 0)
            A_0.get_GlobalVariables().AddGlobalVariable(A_1[index1].Name, (object) this.a());
          else
            A_0.get_GlobalVariables().AddGlobalVariable(A_1[index1].Name, (object) A_1[index1].Value);
          this.CloseConnection();
          checked { ++index1; }
        }
        A_0.Execute();
        IEnumerator enumerator2;
        try
        {
          enumerator2 = A_0.get_Steps().GetEnumerator();
          while (enumerator2.MoveNext())
          {
            if (((Step) enumerator2.Current).get_ExecutionResult() == 1)
              flag = true;
          }
        }
        finally
        {
          if (enumerator2 is IDisposable)
            (enumerator2 as IDisposable).Dispose();
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception exception = ex;
        flag = true;
        throw exception;
      }
      finally
      {
        A_0.UnInitialize();
      }
      return !flag;
    }

    public void AddInParameter(DbCommand command, string name, DbType dbType)
    {
      this.a.AddInParameter(command, name, dbType);
    }

    public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
    {
      this.a.AddInParameter(command, name, dbType, RuntimeHelpers.GetObjectValue(value));
    }

    public void AddInParameter(
      DbCommand command,
      string name,
      DbType dbType,
      string sourceColumn,
      DataRowVersion sourceVersion)
    {
      this.a.AddInParameter(command, name, dbType, sourceColumn, sourceVersion);
    }

    public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
    {
      this.a.AddOutParameter(command, name, dbType, size);
    }

    public void AddParameter(
      DbCommand command,
      string name,
      DbType dbType,
      int size,
      ParameterDirection direction,
      bool nullable,
      byte precision,
      byte scale,
      string sourceColumn,
      DataRowVersion sourceVersion,
      object value)
    {
      this.a.AddParameter(command, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, RuntimeHelpers.GetObjectValue(value));
    }

    public void AddParameter(
      DbCommand command,
      string name,
      DbType dbType,
      ParameterDirection direction,
      string sourceColumn,
      DataRowVersion sourceVersion,
      object value)
    {
      this.a.AddParameter(command, name, dbType, direction, sourceColumn, sourceVersion, RuntimeHelpers.GetObjectValue(value));
    }

    public string BuildParameterName(string name)
    {
      return this.a.BuildParameterName(name);
    }

    public void DiscoverParameters(DbCommand command)
    {
      this.a.DiscoverParameters(command);
    }

    public DataSet ExecuteDataSet(
      string storedProcedureName,
      params object[] parameterValues)
    {
      return this.a.ExecuteDataSet(storedProcedureName, parameterValues);
    }

    public DataSet ExecuteDataSet(CommandType commandType, string commandText)
    {
      return this.a.ExecuteDataSet(commandType, commandText);
    }

    public DataSet ExecuteDataSet(DbCommand command)
    {
      return this.a.ExecuteDataSet(command);
    }

    public DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
    {
      transaction = this.b;
      return this.a.ExecuteDataSet(command, transaction);
    }

    public DataSet ExecuteDataSet(
      DbTransaction transaction,
      string storedProcedureName,
      params object[] parameterValues)
    {
      transaction = this.b;
      return this.a.ExecuteDataSet(transaction, storedProcedureName, parameterValues);
    }

    public DataSet ExecuteDataSet(
      DbTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      transaction = this.b;
      return this.a.ExecuteDataSet(transaction, commandType, commandText);
    }

    public int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
    {
      return this.a.ExecuteNonQuery(storedProcedureName, parameterValues);
    }

    public int ExecuteNonQuery(CommandType commandType, string commandText)
    {
      return this.a.ExecuteNonQuery(commandType, commandText);
    }

    public int ExecuteNonQuery(DbCommand command)
    {
      return this.a.ExecuteNonQuery(command);
    }

    public int ExecuteNonQuery(DbCommand command, DbTransaction transaction)
    {
      transaction = this.b;
      return this.a.ExecuteNonQuery(command, transaction);
    }

    public int ExecuteNonQuery(
      DbTransaction transaction,
      string storedProcedureName,
      params object[] parameterValues)
    {
      transaction = this.b;
      return this.a.ExecuteNonQuery(transaction, storedProcedureName, parameterValues);
    }

    public int ExecuteNonQuery(
      DbTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      transaction = this.b;
      return this.a.ExecuteNonQuery(transaction, commandType, commandText);
    }

    public IDataReader ExecuteReader(
      string storedProcedureName,
      params object[] parameterValues)
    {
      return this.a.ExecuteReader(storedProcedureName, parameterValues);
    }

    public IDataReader ExecuteReader(CommandType commandType, string commandText)
    {
      return this.a.ExecuteReader(commandType, commandText);
    }

    public IDataReader ExecuteReader(DbCommand command)
    {
      return this.a.ExecuteReader(command);
    }

    public IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
    {
      transaction = this.b;
      return this.a.ExecuteReader(command, transaction);
    }

    public IDataReader ExecuteReader(
      DbTransaction transaction,
      string storedProcedureName,
      params object[] parameterValues)
    {
      transaction = this.b;
      return this.a.ExecuteReader(transaction, storedProcedureName, parameterValues);
    }

    public IDataReader ExecuteReader(
      DbTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      transaction = this.b;
      return this.a.ExecuteReader(transaction, commandType, commandText);
    }

    public object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
    {
      return this.a.ExecuteScalar(storedProcedureName, parameterValues);
    }

    public object ExecuteScalar(CommandType commandType, string commandText)
    {
      return this.a.ExecuteScalar(commandType, commandText);
    }

    public object ExecuteScalar(DbCommand command)
    {
      return this.a.ExecuteScalar(command);
    }

    public object ExecuteScalar(DbCommand command, DbTransaction transaction)
    {
      transaction = this.b;
      return this.a.ExecuteScalar(command, transaction);
    }

    public object ExecuteScalar(
      DbTransaction transaction,
      string storedProcedureName,
      params object[] parameterValues)
    {
      transaction = this.b;
      return this.a.ExecuteScalar(transaction, storedProcedureName, parameterValues);
    }

    public object ExecuteScalar(
      DbTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      transaction = this.b;
      return this.a.ExecuteScalar(transaction, commandType, commandText);
    }

    public DbDataAdapter GetDataAdapter()
    {
      return this.a.GetDataAdapter();
    }

    public object GetInstrumentationEventProvider()
    {
      return this.a.GetInstrumentationEventProvider();
    }

    public object GetParameterValue(DbCommand command, string name)
    {
      return this.a.GetParameterValue(command, name);
    }

    public DbCommand GetSqlStringCommand(string query)
    {
      return this.a.GetSqlStringCommand(query);
    }

    public DbCommand GetStoredProcCommand(string storedProcedureName)
    {
      return this.a.GetStoredProcCommand(storedProcedureName);
    }

    public DbCommand GetStoredProcCommand(
      string storedProcedureName,
      params object[] parameterValues)
    {
      return this.a.GetStoredProcCommand(storedProcedureName, parameterValues);
    }

    public DbCommand GetStoredProcCommandWithSourceColumns(
      string storedProcedureName,
      params string[] sourceColumns)
    {
      return this.a.GetStoredProcCommandWithSourceColumns(storedProcedureName, sourceColumns);
    }

    public void LoadDataSet(
      string storedProcedureName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      this.a.LoadDataSet(storedProcedureName, dataSet, tableNames, parameterValues);
    }

    public void LoadDataSet(
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      this.a.LoadDataSet(commandType, commandText, dataSet, tableNames);
    }

    public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName)
    {
      this.a.LoadDataSet(command, dataSet, tableName);
    }

    public void LoadDataSet(
      DbCommand command,
      DataSet dataSet,
      string tableName,
      DbTransaction transaction)
    {
      transaction = this.b;
      this.a.LoadDataSet(command, dataSet, tableName, transaction);
    }

    public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
    {
      this.a.LoadDataSet(command, dataSet, tableNames);
    }

    public void LoadDataSet(
      DbCommand command,
      DataSet dataSet,
      string[] tableNames,
      DbTransaction transaction)
    {
      transaction = this.b;
      this.a.LoadDataSet(command, dataSet, tableNames, transaction);
    }

    public void LoadDataSet(
      DbTransaction transaction,
      string storedProcedureName,
      DataSet dataSet,
      string[] tableNames,
      params object[] parameterValues)
    {
      transaction = this.b;
      this.a.LoadDataSet(transaction, storedProcedureName, dataSet, tableNames, parameterValues);
    }

    public void LoadDataSet(
      DbTransaction transaction,
      CommandType commandType,
      string commandText,
      DataSet dataSet,
      string[] tableNames)
    {
      transaction = this.b;
      this.a.LoadDataSet(transaction, commandType, commandText, dataSet, tableNames);
    }

    public void SetParameterValue(DbCommand command, string parameterName, object value)
    {
      this.a.SetParameterValue(command, parameterName, RuntimeHelpers.GetObjectValue(value));
    }

    public int UpdateDataSet(
      DataSet dataSet,
      string tableName,
      DbCommand insertCommand,
      DbCommand updateCommand,
      DbCommand deleteCommand,
      UpdateBehavior updateBehavior)
    {
      return this.a.UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, updateBehavior);
    }

    public int UpdateDataSet(
      DataSet dataSet,
      string tableName,
      DbCommand insertCommand,
      DbCommand updateCommand,
      DbCommand deleteCommand,
      UpdateBehavior updateBehavior,
      int? updateBatchSize)
    {
      return this.a.UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, updateBehavior, updateBatchSize);
    }

    public int UpdateDataSet(
      DataSet dataSet,
      string tableName,
      DbCommand insertCommand,
      DbCommand updateCommand,
      DbCommand deleteCommand,
      DbTransaction transaction)
    {
      transaction = this.b;
      return this.a.UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, transaction);
    }

    public int UpdateDataSet(
      DataSet dataSet,
      string tableName,
      DbCommand insertCommand,
      DbCommand updateCommand,
      DbCommand deleteCommand,
      DbTransaction transaction,
      int? updateBatchSize)
    {
      transaction = this.b;
      return this.a.UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, transaction, updateBatchSize);
    }

    protected void a(bool A_0)
    {
      if (!this.e && A_0)
      {
        this.CloseConnection();
        this.a = (Database) null;
      }
      this.e = true;
    }

    public void Dispose()
    {
      this.a(true);
      GC.SuppressFinalize((object) this);
    }

    public struct DTSGlobalVariables
    {
      public string Name;
      public string Value;
    }
  }
}
