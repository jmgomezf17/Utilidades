using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Utilidades.Dinamico
{
  public class AsignacionDinamica
  {
    [DebuggerNonUserCode]
    public AsignacionDinamica()
    {
    }

    public void AsignacionDinamica<T>(IDataReader reader, List<T> list, bool liberaReader = true)
    {
      try
      {
        while (reader.Read())
        {
          T instance = Activator.CreateInstance<T>();
          this._LRAsignDinamica<T>(reader, instance);
          list.Add(instance);
        }
        if (!liberaReader)
          return;
        reader.Close();
        reader = (IDataReader) null;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    public List<T> AsignacionDinamica<T>(IDataReader reader, bool liberaReader = true)
    {
      try
      {
        List<T> objList = new List<T>();
        while (reader.Read())
        {
          T instance = Activator.CreateInstance<T>();
          this._LRAsignDinamica<T>(reader, instance);
          objList.Add(instance);
        }
        if (liberaReader)
        {
          reader.Close();
          reader = (IDataReader) null;
        }
        return objList;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    public void AsignacionDinamica<T>(IDataReader reader, T item, bool liberaReader = true)
    {
      string name = "";
      try
      {
        int num = checked (reader.FieldCount - 1);
        int i = 0;
        while (i <= num)
        {
          name = reader.GetName(i);
          PropertyInfo property = item.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
          if (property != null && reader[name] != DBNull.Value)
            property.SetValue((object) item, RuntimeHelpers.GetObjectValue(reader[name]), (object[]) null);
          checked { ++i; }
        }
        if (!liberaReader)
          return;
        reader.Close();
        reader = (IDataReader) null;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception exception = ex;
        throw new Exception(exception.Message + " en propiedad : " + name, exception.InnerException);
      }
    }

    public T AsignacionDinamicaObjeto<T>(IDataReader reader, bool liberaReader = true)
    {
      try
      {
        T obj = default (T);
        if (reader.Read())
        {
          obj = Activator.CreateInstance<T>();
          this._LRAsignDinamica<T>(reader, obj);
        }
        if (liberaReader)
        {
          reader.Close();
          reader = (IDataReader) null;
        }
        return obj;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    public void AsignacionDinamica<T, Y>(
      IDataReader reader,
      List<T> lstT,
      List<Y> lstY,
      bool liberaReader = true)
    {
      try
      {
        this._LRAsignDinamica<T>(reader, lstT);
        reader.NextResult();
        this._LRAsignDinamica<Y>(reader, lstY);
        if (!liberaReader)
          return;
        reader.Close();
        reader = (IDataReader) null;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    public List<T> AsignacionDinamicaConUnDetalle<T, Y>(
      IDataReader reader,
      string nombrePropiedadDetalle,
      string campoDetalleMaestro,
      string campoDetalle,
      bool liberaReader = true)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AsignacionDinamica._Closure\u0024__1<T, Y> closure1 = new AsignacionDinamica._Closure\u0024__1<T, Y>();
      // ISSUE: reference to a compiler-generated field
      closure1.\u0024VB\u0024Local_campoDetalle = campoDetalle;
      // ISSUE: reference to a compiler-generated field
      closure1.\u0024VB\u0024Me = this;
      try
      {
        List<T> lstT = new List<T>();
        List<Y> yList = new List<Y>();
        this._LRAsignDinamica<T, Y>(reader, lstT, yList);
        if (liberaReader)
        {
          reader.Close();
          reader = (IDataReader) null;
        }
        List<T>.Enumerator enumerator;
        try
        {
          enumerator = lstT.GetEnumerator();
          while (enumerator.MoveNext())
          {
            T current = enumerator.Current;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            AsignacionDinamica._Closure\u0024__1<T, Y>._Closure\u0024__2 other = new AsignacionDinamica._Closure\u0024__1<T, Y>._Closure\u0024__2(other);
            // ISSUE: reference to a compiler-generated field
            other.\u0024VB\u0024NonLocal_\u0024VB\u0024Closure_ClosureVariable_194_8 = closure1;
            // ISSUE: reference to a compiler-generated field
            other.\u0024VB\u0024Local_valorActualCampoMaestro = RuntimeHelpers.GetObjectValue(this.GetPropertyValue((object) current, campoDetalleMaestro));
            // ISSUE: reference to a compiler-generated method
            List<Y> list = yList.Where<Y>(new Func<Y, bool>(other._Lambda\u0024__1)).ToList<Y>();
            if (list.Count > 0)
              current.GetType().GetProperty(nombrePropiedadDetalle, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)?.SetValue((object) current, (object) list, (object[]) null);
          }
        }
        finally
        {
          enumerator.Dispose();
        }
        yList.Clear();
        yList.TrimExcess();
        return lstT;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    public List<T> AsignacionDinamicaConUnDetalle<T>(
      IDataReader reader,
      string nombrePropiedadDetalle,
      string campoMaestro,
      object valorCampoMaestro,
      string campoDetalleMaestro,
      string campoDetalle,
      bool liberaReader = true)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AsignacionDinamica._Closure\u0024__3<T> closure3 = new AsignacionDinamica._Closure\u0024__3<T>();
      // ISSUE: reference to a compiler-generated field
      closure3.\u0024VB\u0024Local_campoMaestro = campoMaestro;
      // ISSUE: reference to a compiler-generated field
      closure3.\u0024VB\u0024Local_valorCampoMaestro = RuntimeHelpers.GetObjectValue(valorCampoMaestro);
      // ISSUE: reference to a compiler-generated field
      closure3.\u0024VB\u0024Local_campoDetalle = campoDetalle;
      // ISSUE: reference to a compiler-generated field
      closure3.\u0024VB\u0024Me = this;
      try
      {
        List<T> objList = new List<T>();
        this._LRAsignDinamica<T>(reader, objList);
        if (liberaReader)
        {
          reader.Close();
          reader = (IDataReader) null;
        }
        // ISSUE: reference to a compiler-generated method
        List<T> list1 = objList.Where<T>(new Func<T, bool>(closure3._Lambda\u0024__2)).ToList<T>();
        List<T>.Enumerator enumerator;
        try
        {
          enumerator = list1.GetEnumerator();
          while (enumerator.MoveNext())
          {
            T current = enumerator.Current;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            AsignacionDinamica._Closure\u0024__3<T>._Closure\u0024__4 other = new AsignacionDinamica._Closure\u0024__3<T>._Closure\u0024__4(other);
            // ISSUE: reference to a compiler-generated field
            other.\u0024VB\u0024NonLocal_\u0024VB\u0024Closure_ClosureVariable_1CF_8 = closure3;
            // ISSUE: reference to a compiler-generated field
            other.\u0024VB\u0024Local_valorActualCampoMaestro = RuntimeHelpers.GetObjectValue(this.GetPropertyValue((object) current, campoDetalleMaestro));
            // ISSUE: reference to a compiler-generated method
            List<T> list2 = objList.Where<T>(new Func<T, bool>(other._Lambda\u0024__3)).ToList<T>();
            if (list2.Count > 0)
              current.GetType().GetProperty(nombrePropiedadDetalle, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)?.SetValue((object) current, (object) list2, (object[]) null);
          }
        }
        finally
        {
          enumerator.Dispose();
        }
        objList.Clear();
        objList.TrimExcess();
        return list1;
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    private void _LRAsignDinamica<T>(IDataReader reader, T item)
    {
      string name = "";
      try
      {
        int num = checked (reader.FieldCount - 1);
        int i = 0;
        while (i <= num)
        {
          name = reader.GetName(i);
          PropertyInfo property = item.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
          if (property != null && reader[name] != DBNull.Value)
            property.SetValue((object) item, RuntimeHelpers.GetObjectValue(reader[name]), (object[]) null);
          checked { ++i; }
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception exception = ex;
        throw new Exception(exception.Message + " en propiedad : " + name, exception.InnerException);
      }
    }

    private void _LRAsignDinamica<T>(IDataReader reader, List<T> list)
    {
      try
      {
        while (reader.Read())
        {
          T instance = Activator.CreateInstance<T>();
          this._LRAsignDinamica<T>(reader, instance);
          list.Add(instance);
        }
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    private void _LRAsignDinamica<T, Y>(IDataReader reader, List<T> lstT, List<Y> lstY)
    {
      try
      {
        this._LRAsignDinamica<T>(reader, lstT);
        reader.NextResult();
        this._LRAsignDinamica<Y>(reader, lstY);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        throw ex;
      }
    }

    private object GetPropertyValue(object objeto, string nombrePropiedad)
    {
      try
      {
        return objeto.GetType().GetProperty(nombrePropiedad, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)?.GetValue(RuntimeHelpers.GetObjectValue(objeto), (object[]) null);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        Exception exception = ex;
        throw new Exception(exception.Message + " en propiedad : " + nombrePropiedad, exception.InnerException);
      }
    }
  }
}
