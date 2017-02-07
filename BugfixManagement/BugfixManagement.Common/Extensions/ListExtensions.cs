using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;

namespace BugfixManagement.Common.Extensions
{
    /// <summary>
    /// Description of ListExtensions.
    /// </summary>
    public static class ListExtensions
    {
        #region [ List To DataTable ]

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection properties =
                    TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable();

                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                DataRow row;

                foreach (T item in data)
                {
                    row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
                return table;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static DataTable ToDataTableV2<T>(this IList<T> data)
        {
            DataTable table = new DataTable();

            try
            {
                PropertyInfo[] propsArr = typeof(T).GetProperties();

                if (propsArr == null || propsArr.Length == 0)
                    return table;

                foreach (PropertyInfo prpInf in propsArr)
                {
                    if (prpInf.CanRead)
                    {
                        table.Columns.Add(prpInf.Name,
                            Nullable.GetUnderlyingType(prpInf.PropertyType) ?? prpInf.PropertyType);
                    }
                }

                DataRow row;

                foreach (T item in data)
                {
                    row = table.NewRow();

                    foreach (PropertyInfo prpInf in propsArr)
                        row[prpInf.Name] = prpInf.GetValue(item) ?? DBNull.Value;

                    table.Rows.Add(row);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return table;
        }

        #endregion [ List To DataTable ]

        #region [ Create DataSet From Generic List ]

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataSet CreateDataSet<T>(this List<T> list)
        {
            try
            {
                var properties = list[0].GetType().GetProperties();
                var dataSet = new DataSet();
                var dataTable = new DataTable();

                DataColumn[] columns = new DataColumn[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    columns[i] = new DataColumn(properties[i].Name, properties[i].PropertyType);
                }

                dataTable.Columns.AddRange(columns);

                foreach (var item in list)
                {
                    var dataRow = dataTable.NewRow();

                    var itemProperties = item.GetType().GetProperties();

                    for (int i = 0; i < itemProperties.Length; i++)
                    {
                        dataRow[i] = itemProperties[i].GetValue(item, null) ?? DBNull.Value;
                    }

                    dataTable.Rows.Add(dataRow);
                }

                dataSet.Tables.Add(dataTable);

                return dataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion [ Create DataSet From Generic List ]

        #region [ Export List As Excel ]

        public static void ExportListAsExcel<T>(this IList<T> list, string fileName)
        {
            try
            {
                using (StreamWriter sWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
                {
                    PropertyInfo[] propInfos = typeof(T).GetProperties();

                    sWriter.AutoFlush = true;

                    foreach (PropertyInfo inf in propInfos)
                    {
                        sWriter.Write("{0}\t", inf.Name);
                    }
                    sWriter.Write("\n");

                    foreach (T _t in list)
                    {
                        foreach (PropertyInfo inf in propInfos)
                        {
                            sWriter.Write("{0}\t", inf.GetValue(_t, null).ToStr().Replace("\n", " "));
                        }
                        sWriter.Write("\n");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion [ Export List As Excel ]

        #region [ Export List As Excel ]

        public static void ExportListAsExcelWithIncludeCols<T>(this IList<T> list, string fileName, params object[] includedProperties)
        {
            try
            {
                if (includedProperties.IsNull())
                {
                    return;
                }
                else
                {
                    List<PropertyInfo> propInfos = new List<PropertyInfo>();
                    PropertyInfo prpInf;
                    foreach (object obj in includedProperties)
                    {
                        prpInf = typeof(T).GetProperty(obj.ToStr());
                        propInfos.Add(prpInf);
                    }

                    using (StreamWriter sWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
                    {
                        sWriter.AutoFlush = true;

                        foreach (PropertyInfo prp in propInfos)
                        {
                            sWriter.Write("{0}\t", prp);
                        }
                        sWriter.Write("\n");

                        foreach (T _t in list)
                        {
                            foreach (PropertyInfo prp in propInfos)
                            {
                                sWriter.Write("{0}\t", prp.GetValue(_t, null).ToStr().Replace("\n", " "));
                            }
                            sWriter.Write("\n");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion [ Export List As Excel ]

        #region [ Export As Excel With Exclude Columns ]

        public static void ExportAsExcelWithExcludeColumns<T>(this IList<T> list, string fileName, params object[] excludeColumns)
        {
            try
            {
                PropertyInfo[] propInfos = typeof(T).GetProperties();
                if (excludeColumns.IsNull())
                {
                    using (StreamWriter sWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
                    {
                        sWriter.AutoFlush = true;

                        foreach (PropertyInfo inf in propInfos)
                        {
                            sWriter.Write("{0}\t", inf.Name);
                        }
                        sWriter.Write("\n");

                        foreach (T _t in list)
                        {
                            foreach (PropertyInfo inf in propInfos)
                            {
                                sWriter.Write("{0}\t", inf.GetValue(_t, null).ToStr().Replace("\n", " "));
                            }
                            sWriter.Write("\n");
                        }
                    }
                }
                else
                {
                    List<PropertyInfo> propList = new List<PropertyInfo>();
                    bool contain = false;
                    foreach (PropertyInfo inf in propInfos)
                    {
                        contain = false;
                        foreach (object obj in excludeColumns)
                        {
                            contain = inf.Name.Equals(obj.ToStr());
                            if (contain == true)
                                break;
                        }

                        if (contain == false)
                            propList.Add(inf);
                    }

                    using (StreamWriter sWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate)))
                    {
                        sWriter.AutoFlush = true;

                        foreach (PropertyInfo inf in propList)
                        {
                            sWriter.Write("{0}\t", inf.Name);
                        }
                        sWriter.Write("\n");

                        foreach (T _t in list)
                        {
                            foreach (PropertyInfo prpInf in propList)
                            {
                                sWriter.Write("{0}\t", prpInf.GetValue(_t, null).ToStr().Replace("\n", " "));
                            }
                            sWriter.Write("\n");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion [ Export As Excel With Exclude Columns ]

        #region [ Implode - List ]

        public static string Implode<T>(this IList<T> lst, string str = ",") where T : new()
        {
            string result = string.Empty;

            if (lst == null)
                return result;

            if (lst.Count == 0)
                return result;

            if (lst.Count == 1)
                return string.Format("{0}", lst[0]);

            string s = str ?? string.Empty;

            for (int counter = 0; counter < lst.Count - 1; counter++)
            {
                result = string.Format("{0}{1}{2}", result, lst[counter], s);
            }

            result = string.Format("{0}{1}", result, lst[lst.Count - 1]);

            return result;
        }

        #endregion [ Implode - List ]

        #region [ Implode - Array ]

        public static string Implode<T>(this T[] lst, string str) where T : new()
        {
            string result = string.Empty;

            if (lst == null)
                return result;

            if (lst.Length == 0)
                return result;

            if (lst.Length == 1)
            {
                result = string.Format("{0}", lst[0]);
                return result;
            }

            string s = str ?? string.Empty;

            for (int counter = 0; counter < lst.Length - 1; counter++)
            {
                result = string.Format("{0}{1}{2}", result, lst[counter], s);
            }

            result = string.Format("{0}{1}", result, lst[lst.Length - 1]);

            return result;
        }

        #endregion [ Implode - Array ]

        #region [ IsNullOrEmpty ]

        public static bool IsNullOrEmpty<T>(this IList<T> lst) where T : new()
        {
            bool result = true;

            if (lst == null)
                return result;

            result = lst.Count < 1;
            return result;
        }

        public static bool IsNullOrEmpty<T>(this T[] lst) where T : new()
        {
            bool result = true;

            if (lst == null)
                return result;

            result = lst.Length < 1;
            return result;
        }

        public static bool IsNullOrEmpty(this ArrayList arr)
        {
            bool result = true;

            if (arr == null)
                return result;

            result = arr.Count < 1;
            return result;
        }

        #endregion [ IsNullOrEmpty ]
    }
}
