using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;

namespace BCW.PK10
{
    public class ModelHelper
    {
        #region 从DataTable、DataRow返回Model
        public static List<T> DataTableToModel<T>(DataTable dt) where T : class, new()
        {
            List<T> ts = new List<T>();
            // 
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                t = DataRowToModel<T>(dr);
                ts.Add(t);
            }
            //
            return ts;
        }
        public static T DataRowToModel<T>(DataRow row) where T : class, new()
        {
            var model = new T();
            //PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(model.GetType());
            PropertyInfo[] properties = model.GetType().GetProperties();

            foreach (PropertyInfo p in properties)
            {
                // we can't update value type properties that are read-only
                if (p.PropertyType.IsValueType && !p.CanWrite)
                {
                    continue;
                }
                if (row.Table.Columns.Contains(p.Name))
                {
                    try
                    {
                        object value = ConvertSimpleType(CultureInfo.CurrentCulture, row[p.Name], p.PropertyType);
                        if (value == null)
                        {
                            if (!TypeAllowsNullValue(p.PropertyType))
                            {
                                //break;
                            }
                        }
                        p.SetValue(model, value, null);
                    }
                    catch { }; //忽略生成不了的字段
                }
            }
            return model;
        }
        #region 数据类型转换
        /// <summary>
        /// Types the allows null value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool TypeAllowsNullValue(Type type)
        {
            // Only reference types and Nullable<> types allow null values
            return (!type.IsValueType ||
                    (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
        /// <summary>
        /// Converts the type of the simple.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="value">The value.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public static object ConvertSimpleType(CultureInfo culture, object value, Type destinationType)
        {
            if (value == null || destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            if (value == DBNull.Value)
            {
                return null;
            }
            // if this is a user-input value but the user didn't type anything, return no value
            var valueAsString = value as string;
            if (valueAsString != null && valueAsString.Length == 0)
            {
                return null;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))
                && value.GetType() != typeof(string))
            {
                var nulla = new NullableConverter(destinationType);
                destinationType = nulla.UnderlyingType;
                if (destinationType.IsEnum)
                {
                    return Enum.ToObject(destinationType, value);
                }
            }

            bool canConvertFrom = converter.CanConvertFrom(value.GetType());
            if (!canConvertFrom)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }

            if (!(canConvertFrom || converter.CanConvertTo(destinationType)))
            {
                if (destinationType.IsEnum)
                {
                    return Enum.ToObject(destinationType, value);
                }
            }

            try
            {
                object convertedValue = (canConvertFrom)
                                            ? converter.ConvertFrom(null, culture, value)
                                            : converter.ConvertTo(null, culture, value, destinationType);
                return convertedValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string message = String.Format(CultureInfo.CurrentUICulture, value.GetType().FullName,
                                               destinationType.FullName);
                throw new InvalidOperationException(message, ex);
            }
        }
        #endregion
        #endregion

        #region Model的数据操作函数
        public static T GetModelBySql<T>(string cSQL) where T : class, new()
        {
            T t = new T();
            try
            {
                DataTable dt = MySqlHelper.GetTable(cSQL);
                if (dt != null && dt.Rows.Count > 0)
                    t = ModelHelper.DataRowToModel<T>(dt.Rows[0]);
                else
                    t = null;
            }
            catch
            {
                t = null;
            }
            return t;
        }
        public static T GetModelBySql<T>(SqlConnection conn,SqlTransaction trans, string cSQL) where T : class, new()
        {
            T t = new T();
            try
            {
                DataTable dt = MySqlHelper.GetTable(conn,trans,cSQL);
                if (dt != null && dt.Rows.Count > 0)
                    t = ModelHelper.DataRowToModel<T>(dt.Rows[0]);
                else
                    t = null;
            }
            catch
            {
                t = null;
            }
            return t;
        }
        #endregion
    }
}
